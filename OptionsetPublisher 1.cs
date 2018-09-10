// <copyright file="OptionSetPu.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>class to publish option set changes to CRM.</summary>
namespace OptionSetEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml;
    using XrmToolBox.Extensibility;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Metadata;

    internal class OptionsetPublisher : PluginControlBase
    {
        /// <summary>
        /// The enumeration of operation.
        /// </summary>
        private enum Operation
        {
            /// <summary>
            /// Insert an option set item.
            /// </summary>
            Insert,
            /// <summary>
            /// Update an option set item.
            /// </summary>
            Update,
            /// <summary>
            /// Delete an option set item.
            /// </summary>
            Delete
        }

        /// <summary>
        /// Processes the fee type records to update the option set
        /// </summary>
        /// <param name="service">The CRM service.</param>
        /// <param name="entities">The entity collection.</param>
        public void PublishOptions(IOrganizationService service, IEnumerable<EntityItem> entities)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Please wait...",
                Work = (w, e) =>
                {
                    string message = string.Empty;
                    string caption = "Success";
                    w.WorkerReportsProgress = true;

                    foreach (var entity in entities)
                    {
                        foreach (var attribute in entity.Children.Where(c => c.Changed))
                        {
                            if (attribute.Children.Any(i => i.Label == null || string.IsNullOrWhiteSpace(i.Label.UserLocalizedLabel.Label)))
                            {
                                message = "All items require a label";
                                caption = "  Error";
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        message = "Publish Successful";
                        XmlDocument publishXml = new XmlDocument();
                        publishXml.LoadXml("<importexportxml></importexportxml>");
                        int count = entities.Count() + 1;

                        ExecuteMultipleRequest batchRequest = new ExecuteMultipleRequest
                        {
                            Settings = new ExecuteMultipleSettings
                            {
                                ContinueOnError = true,
                                ReturnResponses = true
                            },
                            Requests = new OrganizationRequestCollection()

                        };

                        foreach (var entity in entities)
                        {
                            foreach (var attribute in entity.Children.Where(c => c.Changed))
                            {
                                OptionMetadata[] optionList;
                                w.ReportProgress(100 / count, $"Creating {attribute.DisplayName}...");
                                count--;
                                if (attribute.Global)
                                {
                                    AddNode(publishXml, attribute.GlobalName, true);

                                    RetrieveOptionSetRequest request = new RetrieveOptionSetRequest
                                    {
                                        Name = attribute.GlobalName,
                                        RetrieveAsIfPublished = true
                                    };

                                    RetrieveOptionSetResponse response = (RetrieveOptionSetResponse)service.Execute(request);
                                    OptionSetMetadata optionSet = (OptionSetMetadata)response.OptionSetMetadata;

                                    optionList = optionSet.Options.ToArray();
                                }
                                else
                                {
                                    AddNode(publishXml, entity.LogicalName, false);

                                    var attributeRequest = new RetrieveAttributeRequest
                                    {
                                        EntityLogicalName = entity.LogicalName,
                                        LogicalName = attribute.LogicalName,
                                        RetrieveAsIfPublished = true
                                    };

                                    var attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
                                    var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

                                    optionList = attributeMetadata.OptionSet.Options.ToArray();
                                }

                                var currentValues = optionList.Select(o => o.Value.Value);

                                var removedOptions = currentValues.Where(o => !attribute.Children.Any(i => i.Value == o));


                                foreach (var removedOption in removedOptions)
                                {
                                    EntityItem deletion = new EntityItem(string.Empty, attribute.LogicalName)
                                    {
                                        GlobalName = attribute.GlobalName,
                                        Value = removedOption,
                                        Parent = entity
                                    };

                                    batchRequest.Requests.Add(GetRequest(deletion, Operation.Delete));
                                }


                                foreach (EntityItem option in attribute.Children.Where(o => o.Changed))
                                {
                                    OptionMetadata currentOption = optionList.Where(o => o.Value.Value == option.Value).FirstOrDefault();

                                    if (currentOption != null)
                                    {
                                        batchRequest.Requests.Add(GetRequest(option, Operation.Update));
                                    }
                                    else
                                    {
                                        batchRequest.Requests.Add(GetRequest(option, Operation.Insert));
                                    }
                                }


                            }
                        }
                        PublishXmlRequest publish = new PublishXmlRequest { ParameterXml = publishXml.InnerXml };
                        batchRequest.Requests.Add(publish);

                        ExecuteMultipleResponse batchResults = (ExecuteMultipleResponse)service.Execute(batchRequest);
                        w.ReportProgress(100 / count, $"Publishing...");

                        foreach (var responseItem in batchResults.Responses)
                        {
                            if (responseItem.Fault != null)
                            {
                                message += "\r\n" + batchRequest.Requests[responseItem.RequestIndex].RequestName + ": " + responseItem.Fault.Message;
                                caption = "  Error";
                            }
                        }
                    }
                    e.Result = message + caption;
                },
                PostWorkCallBack = e =>
                {
                    string result = e.Result.ToString();
                    string message = result.Substring(0, result.Length - 7);
                    string caption = result.Substring(result.Length - 7).Trim();
                    MessageBox.Show(message, caption);
                },
                AsyncArgument = null,
                IsCancelable = true,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        private static void AddNode(XmlDocument publishXml, string item, bool isOptionSet)
        {
            XmlNode itemElem = null;
            XmlNode subItemElem = null;
            if (isOptionSet)
            {
                var itemElems = publishXml.GetElementsByTagName("optionsets");
                if (itemElems.Count == 0)
                {
                    itemElem = publishXml.CreateNode(XmlNodeType.Element, "optionsets", string.Empty);
                }
                else
                {
                    itemElem = itemElems[0];
                }
                subItemElem = publishXml.CreateNode(XmlNodeType.Element, "optionset", string.Empty);
            }
            else
            {
                var itemElems = publishXml.GetElementsByTagName("entities");
                if (itemElems.Count == 0)
                {
                    itemElem = publishXml.CreateNode(XmlNodeType.Element, "entities", string.Empty);
                }
                else
                {
                    itemElem = itemElems[0];
                }
                subItemElem = publishXml.CreateNode(XmlNodeType.Element, "entity", string.Empty);
            }

            subItemElem.InnerText = item;
            itemElem.AppendChild(subItemElem);
            XmlElement root = publishXml.DocumentElement;
            root.AppendChild(itemElem);
        }

        private static OrganizationRequest GetRequest(EntityItem option, Operation operation)
        {
            OrganizationRequest request = null;

            switch (operation)
            {
                case Operation.Insert:
                    request = new InsertOptionValueRequest
                    {
                        OptionSetName = option.Parent.GlobalName,
                        EntityLogicalName = option.Parent.Global ? null : option.Parent.Parent.LogicalName,
                        AttributeLogicalName = option.Parent.Global ? null : option.Parent.LogicalName,
                        Value = option.Value,
                        Label = option.Label,
                        Description = option.Description
                    };
                    break;
                case Operation.Update:
                    request = new UpdateOptionValueRequest
                    {
                        OptionSetName = option.Parent.GlobalName,
                        EntityLogicalName = option.Parent.Global ? null : option.Parent.Parent.LogicalName,
                        AttributeLogicalName = option.Parent.Global ? null : option.Parent.LogicalName,
                        Value = option.Value,
                        Label = option.Label,
                        Description = option.Description
                    };

                    break;
                case Operation.Delete:
                    request = new DeleteOptionValueRequest
                    {
                        OptionSetName = option.GlobalName,
                        EntityLogicalName = option.Global ? null : option.Parent.LogicalName,
                        AttributeLogicalName = option.Global ? null : option.LogicalName,
                        Value = option.Value
                    };

                    break;
                default:
                    break;
            }

            return request;
        }
    }
}
