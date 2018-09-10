// <copyright file="ImportExport.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>Imports or exports the options.</summary>
namespace OptionSetEditor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Class to import and export the changes to a file.
    /// </summary>
    public partial class XrmOptionSetEditorControl
    {
        /// <summary>
        /// Exports the changed items to a comma separated file.
        /// </summary>
        /// <param name="exportItems">The items that have changed.</param>
        /// <param name="installedLanguages">The list of languages installed.</param>
        /// <returns>The comma separated file.</returns>
        internal string Export(IEnumerable<EntityItem> exportItems, Dictionary<int, string> installedLanguages)
        {
            StringBuilder csv = new StringBuilder("Entity,Attribute,Global Name,Value,");
            foreach (var item in installedLanguages.OrderBy(l => l.Key))
            {
                csv.Append("Label-");
                csv.Append(item.Value);
                csv.Append(',');
            }

            foreach (var item in installedLanguages.OrderBy(l => l.Key))
            {
                csv.Append("Description-");
                csv.Append(item.Value);
                csv.Append(',');
            }

            csv.Length = csv.Length - 1;
            csv.AppendLine();

            List<EntityItem> export = new List<EntityItem>();
            for (int i = 0; i < exportItems.Count(); i++)
            {
                EntityItem exportItem = exportItems.Skip(i).First();
                if (!exportItem.Loaded)
                {
                    this.GetOptions(exportItem);
                }

                export.Add(exportItem);
            }

            foreach (var item in export)
            {
                csv.Append(item.ToCSV(installedLanguages.OrderBy(l => l.Key).Select(l => l.Key)));
            }

            return csv.ToString();
        }

        /// <summary>
        /// Imports a comma separated file in to the app.
        /// </summary>
        /// <param name="fileName">The file name to import.</param>
        /// <param name="entityItem">The item to import.</param>
        /// <param name="installedLanguages">The installed languages.</param>
        internal void Import(string fileName, EntityItem entityItem, Dictionary<int, string> installedLanguages)
        {
            var file = File.ReadAllLines(fileName);
            file = file.Skip(1).ToArray();
            var fileEntities = file.GroupBy(f => f.Substring(0, f.IndexOf(',')));
            foreach (var fileEntity in fileEntities)
            {
                Dictionary<string, EntityItem> attributes = new Dictionary<string, EntityItem>();
                if (entityItem.LogicalName.ToLower() == fileEntity.Key.ToLower())
                {
                    var fileAttributes = fileEntity.GroupBy(f => f.Substring(f.IndexOf(',') + 1, f.IndexOf(',', f.IndexOf(',') + 1) - f.IndexOf(',') - 1));
                    foreach (var fileAttribute in fileAttributes)
                    {
                        var attributeItem = entityItem.Children.Where(c => c.LogicalName.ToLower() == fileAttribute.Key.ToLower()).FirstOrDefault();
                        if (attributeItem != null)
                        {
                            if (!attributeItem.Loaded)
                            {
                                this.GetOptions(attributeItem);
                            }
                            var children = attributeItem.Children.ToArray();
                            attributeItem.Children.Clear();
                            foreach (var line in fileAttribute)
                            {
                                var columns = line.Split(',');
                                int maxColumns = (installedLanguages.Count() * 2) + 4;

                                if (columns.Count() > maxColumns)
                                {
                                    List<int> columnsToDelete = new List<int>();
                                    for (int i = 0; i < columns.Length - 1; i++)
                                    {
                                        if (columns[i].StartsWith("\"") && columns[i + 1].EndsWith("\""))
                                        {
                                            columns[i] = columns[i] + "," + columns[i + 1];
                                            columns[i] = columns[i].Trim('"');
                                            columnsToDelete.Add(i + 1);
                                        }
                                    }
                                    if (columnsToDelete.Count > 0)
                                    {
                                        var cols = new List<string>(columns);
                                        for (int i = columnsToDelete.Count(); i > 0; i--)
                                        {
                                            cols.RemoveAt(columnsToDelete[i - 1]);
                                        }
                                        columns = cols.ToArray();

                                    }
                                    if (columns.Length != (installedLanguages.Count() * 2) + 4)
                                    {
                                        MessageBox.Show("Invalid number of columns in the record.");
                                    }
                                }
                                Microsoft.Xrm.Sdk.LocalizedLabel[] labels = new Microsoft.Xrm.Sdk.LocalizedLabel[installedLanguages.Count()];
                                Microsoft.Xrm.Sdk.LocalizedLabel[] descriptions = new Microsoft.Xrm.Sdk.LocalizedLabel[installedLanguages.Count()];

                                for (int i = 0; i < installedLanguages.Count; i++)
                                {
                                    labels[i] = new Microsoft.Xrm.Sdk.LocalizedLabel(columns[i + 4], installedLanguages.Keys.OrderBy(l => l).ToArray()[i]);
                                    descriptions[i] = new Microsoft.Xrm.Sdk.LocalizedLabel(columns[i + 4 + installedLanguages.Count], installedLanguages.Keys.OrderBy(l => l).ToArray()[i]);
                                }

                                Microsoft.Xrm.Sdk.Label label = new Microsoft.Xrm.Sdk.Label();
                                label.LocalizedLabels.AddRange(labels);
                                label.UserLocalizedLabel = label.LocalizedLabels.Where(l => l.LanguageCode == installedLanguages.Keys.First()).First();
                                Microsoft.Xrm.Sdk.Label description = new Microsoft.Xrm.Sdk.Label();
                                description.LocalizedLabels.AddRange(descriptions);
                                description.UserLocalizedLabel = description.LocalizedLabels.Where(l => l.LanguageCode == installedLanguages.Keys.First()).First();
                                int value = 0;
                                if (int.TryParse(columns[3], out value) && value != 2147483647)
                                {
                                    EntityItem child = new EntityItem(value, label, description, attributeItem);
                                    var found = children.Where(c => c.Equals(child)).FirstOrDefault();

                                    child.Changed = found==null;
                                    attributeItem.GlobalName = columns[2];
                                    attributeItem.Children.Add(child);
                                    attributeItem.Loaded = true;
                                    child.Parent.Changed = true;
                                }
                                else
                                {
                                    MessageBox.Show($"Invalid value - {columns[3]}");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Cannot find the attribute - {fileAttribute.Key} on entity {fileEntity.Key}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"You can only import to the currently selected entity\r\nImport entity = {fileEntity.Key}");
                }
            }
        }
    }
}