// <copyright file="XrmOptionSetEditorControl.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>Control for the option set editor.</summary>
namespace OptionSetEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Metadata;
    using Microsoft.Xrm.Sdk.Query;
    using Newtonsoft.Json;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;

    /// <summary>
    /// Class to host the option set editor control.
    /// </summary>
    public partial class XrmOptionSetEditorControl : PluginControlBase, IPayPalPlugin
    {
        /// <summary>
        /// List of installed languages on the CRM.
        /// </summary>
        private Dictionary<int, string> installedLanguages;

        /// <summary>
        /// Initializes a new instance of the <see cref="XrmOptionSetEditorControl"/> class. 
        /// </summary>
        public XrmOptionSetEditorControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is changing because of loading.
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        /// Gets or sets the default publisher for the solution.
        /// </summary>
        public Guid DefaultPublisher { get; set; }

        /// <summary>
        /// Gets or sets the option set prefix.
        /// </summary>
        public int OptionSetPrefix { get; set; }

        /// <summary>
        /// Gets the text for pay pal donation
        /// </summary>
        public string DonationDescription
        {
            get
            {
                return "Please help to continue development of this project.";
            }
        }

        /// <summary>
        /// Gets the email account for pay pal.
        /// </summary>
        public string EmailAccount
        {
            get
            {
                return "chrisa_uk20@hotmail.com";
            }
        }

        /// <summary>
        /// Gets or sets the default language of the current CRM user.
        /// </summary>
        private int DefaultLanguage { get; set; }

        /// <summary>
        /// Event when the user selects to open all entities.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void AllEntitiesMenu_Click(object sender, EventArgs e)
        {
            this.GetSolutionEntities("default");
        }

        /// <summary>
        /// Event when an item is selected from the entities list.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void EntitiesList_SelectedValueChanged(object sender, EventArgs e)
        {
            OptionSetList.DataSource = null;
            EntityItem currentItem = (EntityItem)EntitiesList.SelectedItem;
            if (!currentItem.Loaded)
            {
                this.ExecuteMethod<EntityItem>(this.GetAttributes, currentItem);
            }
            else
            {
                OptionSetList.DataSource = new BindingList<EntityItem>(currentItem.Children.ToArray());
                ImportMenu.Enabled = true;
            }
        }

        /// <summary>
        /// Gets the attributes for the selected entity from CRM.
        /// </summary>
        /// <param name="entity">The entity whose attributes are to be retrieved.</param>
        private void GetAttributes(EntityItem entity)
        {
            this.WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving option attributes...",
                Work = (w, e) =>
                {
                    var request = new RetrieveEntityRequest
                    {
                        LogicalName = entity.LogicalName,
                        EntityFilters = EntityFilters.Attributes
                    };
                    var response = (RetrieveEntityResponse)Service.Execute(request);

                    e.Result = response.EntityMetadata;
                },
                PostWorkCallBack = e =>
                {
                    AttributeMetadata[] results = ((EntityMetadata)e.Result).Attributes;
                    entity.Children = new List<EntityItem>();
                    entity.Children.AddRange(results.Where(r => r.AttributeType == AttributeTypeCode.Picklist && r.DisplayName.UserLocalizedLabel != null).Select(r => new EntityItem(r.DisplayName.UserLocalizedLabel.Label, r.LogicalName, entity)));
                    OptionSetList.DataSource = new BindingList<EntityItem>(entity.Children.ToArray());

                    entity.Loaded = true;
                    ImportMenu.Enabled = true;
                },
                AsyncArgument = null,
                IsCancelable = true,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        /// <summary>
        /// Event when an option set attribute is selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void OptionSetList_SelectedValueChanged(object sender, EventArgs e)
        {
            OptionsList.DataSource = null;
            EntityItem currentEntity = (EntityItem)EntitiesList.SelectedItem;
            EntityItem currentAttribute = (EntityItem)OptionSetList.SelectedItem;
            if (currentAttribute != null)
            {
                if (!currentAttribute.Loaded)
                {
                    this.ExecuteMethod<EntityItem>(this.GetOptions, currentAttribute);
                }
                else
                {
                    OptionsList.DataSource = new BindingList<EntityItem>(currentAttribute.Children);
                    GlobalLabel.Visible = currentAttribute.Global;
                }

                AddButton.Enabled = OptionSetList.SelectedIndex > -1;
            }
        }

        /// <summary>
        /// Retrieves the options from CRM for the selected attribute.
        /// </summary>
        /// <param name="attribute">The attribute whose options are to be retrieved.</param>
        private void GetOptions(EntityItem attribute)
        {
            this.WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving options...",
                Work = (w, e) =>
                {
                    var request = new RetrieveAttributeRequest
                    {
                        EntityLogicalName = attribute.Parent.LogicalName,
                        LogicalName = attribute.LogicalName,
                        RetrieveAsIfPublished = true
                    };
                    var response = (RetrieveAttributeResponse)Service.Execute(request);

                    var attributeMetadata = (EnumAttributeMetadata)response.AttributeMetadata;

                    e.Result = attributeMetadata.OptionSet;
                },
                PostWorkCallBack = e =>
                {
                    OptionSetMetadata optionset = (OptionSetMetadata)e.Result;
                    attribute.GlobalName = optionset.IsGlobal.GetValueOrDefault() ? optionset.Name : null;

                    if (attribute.Global)
                    {
                        GlobalLabel.Text = "* Global Option Set = " + attribute.GlobalName;
                        foreach (var item in EntitiesList.Items.Cast<EntityItem>())
                        {
                            var existing = item.Children.Where(i => i.GlobalName == attribute.GlobalName && i.LogicalName != attribute.LogicalName && i.Parent.LogicalName != attribute.Parent.LogicalName);
                            if (existing.Count() > 0)
                            {
                                attribute.Children.AddRange(existing.First().Children);
                                attribute.Loaded = true;
                                OptionsList.DataSource = new BindingList<EntityItem>(attribute.Children);
                                OptionSetList.DataSource = null;
                                OptionSetList.DataSource = new BindingList<EntityItem>(attribute.Parent.Children.ToArray());
                                return;
                            }
                        }
                    }

                    attribute.Children = new List<EntityItem>();
                    attribute.Children.AddRange(optionset.Options.Select(r => new EntityItem(r.Value.GetValueOrDefault(), r.Label, r.Description, attribute)));

                    Loading = true;
                    OptionsList.DataSource = new BindingList<EntityItem>(attribute.Children);
                    Loading = false;

                    attribute.Loaded = true;

                    if (attribute.Global)
                    {
                        OptionSetList.DataSource = null;
                        OptionSetList.DataSource = new BindingList<EntityItem>(attribute.Parent.Children.ToArray());
                        GlobalLabel.Visible = true;
                    }
                    else
                    {
                        GlobalLabel.Visible = false;
                    }
                },
                AsyncArgument = null,
                IsCancelable = true,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        /// <summary>
        /// Sets up the application.
        /// </summary>
        private void Setup()
        {
            this.WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving ...",
                Work = (w, e) =>
                {
                    if (installedLanguages == null)
                    {
                        RetrieveAvailableLanguagesRequest request = new RetrieveAvailableLanguagesRequest();
                        RetrieveAvailableLanguagesResponse response = (RetrieveAvailableLanguagesResponse)Service.Execute(request);
                        e.Result = response;
                    }
                },
                PostWorkCallBack = e =>
                {
                    if (e.Result != null)
                    {
                        Dictionary<int, string> languages = JsonConvert.DeserializeObject<Dictionary<int, string>>(Properties.Resources.Countries);
                        installedLanguages = new Dictionary<int, string>();
                        foreach (var language in ((RetrieveAvailableLanguagesResponse)e.Result).LocaleIds)
                        {
                            if (languages.ContainsKey(language))
                            {
                                installedLanguages.Add(language, languages[language]);
                            }
                        }

                        GetUser();
                        GetSolutions();
                    }
                },
                AsyncArgument = null,
                IsCancelable = true,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        /// <summary>
        /// Event when an option is selected from the list.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void OptionsList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (OptionsList.SelectedItem != null)
            {
                EntityItem currentOption = (EntityItem)OptionsList.SelectedItem;
                ValueText.DataBindings.Clear();
                this.ValueText.DataBindings.Add(
                    "Text",
                    currentOption,
                    "Value",
                    false,
                    DataSourceUpdateMode.OnPropertyChanged);
                this.LoadLabels(this.LabelsGroup, currentOption.Label);
                DescriptionsGroup.Top = LabelsGroup.Top + LabelsGroup.Height + 30;
                this.LoadLabels(this.DescriptionsGroup, currentOption.Description);
            }

            this.ShowArrows(OptionsList.SelectedIndex, OptionsList.Items.Count);
        }

        /// <summary>
        /// Show the relevant arrows.
        /// </summary>
        /// <param name="current">The current option.</param>
        /// <param name="count">The total number of options.</param>
        private void ShowArrows(int current, int count)
        {
            UpButton.Enabled = current > -1 && current != 0;
            DownButton.Enabled = current > -1 && current != count - 1;
            DeleteButton.Enabled = current > -1;
        }

        /// <summary>
        /// Loads the labels in to the group boxes.
        /// </summary>
        /// <param name="group">The group box to load into.</param>
        /// <param name="optionLabel">The CRM label to put in the box.</param>
        private void LoadLabels(GroupBox group, Microsoft.Xrm.Sdk.Label optionLabel)
        {
            group.Controls.Clear();
            if (this.installedLanguages.ContainsKey(this.DefaultLanguage))
            {
                int left = 20;
                int top = 20;
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Text = this.installedLanguages[this.DefaultLanguage];
                label.Left = left;
                label.AutoSize = true;
                label.Top = top;

                group.Controls.Add(label);

                TextBox text = new System.Windows.Forms.TextBox();
                var localOption = optionLabel.LocalizedLabels.Where(l => l.LanguageCode == this.DefaultLanguage).FirstOrDefault();
                localOption = localOption == null ? optionLabel.UserLocalizedLabel : localOption;
                text.Text = localOption == null ? string.Empty : localOption.Label;
                text.Top = top + 15;
                text.Left = left;
                text.Width = 150;
                text.Tag = optionLabel;
                text.Name = "L" + this.DefaultLanguage.ToString();
                text.TextChanged += this.LabelChanged;
                if (this.DefaultLanguage != 0 && !group.Name.ToLower().Contains("description"))
                {
                    text.Leave += this.Label_Leave;
                }

                group.Controls.Add(text);

                top += 48;

                foreach (var item in this.installedLanguages)
                {
                    if (item.Key != this.DefaultLanguage)
                    {
                        label = new System.Windows.Forms.Label();
                        label.Text = item.Value;
                        label.Left = left;
                        label.Top = top;
                        label.AutoSize = true;
                        group.Controls.Add(label);

                        text = new System.Windows.Forms.TextBox();
                        var localLabel = optionLabel.LocalizedLabels.Where(l => l.LanguageCode == item.Key).FirstOrDefault();
                        text.Text = localLabel == null ? string.Empty : localLabel.Label;
                        text.Top = top + 15;
                        text.Left = left;
                        text.Width = 150;
                        text.Tag = optionLabel;
                        text.Name = " " + item.Key.ToString();
                        text.TextChanged += this.LabelChanged;
                        group.Controls.Add(text);
                        top += 48;
                    }
                }

                group.Height = top;
            }

            LabelsGroup.Visible = true;
            DescriptionsGroup.Visible = true;
            ValueText.Visible = true;
            ValueLabel.Visible = true;
        }

        /// <summary>
        /// Event when a label is changed.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void LabelChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var label = (Microsoft.Xrm.Sdk.Label)textBox.Tag;
            int language = int.Parse(textBox.Name.Substring(1));
            if (textBox.Name.Substring(0, 1) == "L")
            {
                if (label.UserLocalizedLabel == null)
                {
                    label.UserLocalizedLabel = new LocalizedLabel(textBox.Text, language);
                }
                else
                {
                    label.UserLocalizedLabel.Label = textBox.Text;
                }
            }

            var localLabel = label.LocalizedLabels.Where(l => l.LanguageCode == language).FirstOrDefault();
            if (localLabel == null)
            {
                label.LocalizedLabels.Add(new LocalizedLabel(textBox.Text, language));
            }
            else
            {
                localLabel.Label = textBox.Text;
            }

            this.SetChanged((EntityItem)OptionsList.SelectedItem);
            ExportMenu.Enabled = true;
            PublishMenu.Enabled = true;
        }

        /// <summary>
        /// Sets the current option and it's parents to changed.
        /// </summary>
        /// <param name="item">The item being changed.</param>
        private void SetChanged(EntityItem item)
        {
            item.Changed = true;
            item.Parent.Changed = true;
            item.Parent.Parent.Changed = true;
        }

        /// <summary>
        /// Event when the export menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ExportMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV|*.csv";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Event when import menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV|*.csv";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Event raised when the file save editor has a valid file selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="ev">Cancel event arguments.</param>
        private void SaveFileDialog_FileOk(object sender, CancelEventArgs ev)
        {
            var items = EntitiesList.Items.Cast<EntityItem>().Where(e => e.Changed);
            File.WriteAllText(saveFileDialog1.FileName, ImportExport.Export(items, this.installedLanguages));
        }

        /// <summary>
        /// Event raised when the file open editor has a valid file selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            ImportExport.Import(openFileDialog1.FileName, EntitiesList.Items.Cast<EntityItem>(), this.installedLanguages);
            if (OptionSetList.SelectedItem != null)
            {
                OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)OptionSetList.SelectedItem).Children);
            }
        }

        /// <summary>
        /// Event when the delete button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ((BindingList<EntityItem>)OptionsList.DataSource).Remove((EntityItem)OptionsList.SelectedItem);
        }

        /// <summary>
        /// Event when the down button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void DownButton_Click(object sender, EventArgs e)
        {
            this.MoveItem(1);
        }

        /// <summary>
        /// Moves an item in the given direction
        /// </summary>
        /// <param name="direction">positive number to move down, negative number to move down.</param>
        private void MoveItem(int direction)
        {
            int newIndex = OptionsList.SelectedIndex + direction;

            EntityItem selected = OptionsList.SelectedItem as EntityItem;
            ((EntityItem)OptionSetList.SelectedItem).Children.RemoveAt(OptionsList.SelectedIndex);
            ((EntityItem)OptionSetList.SelectedItem).Children.Insert(newIndex, selected);

            OptionsList.SetSelected(newIndex, true);
        }

        /// <summary>
        /// Event when the up button is clicked
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void UpButton_Click(object sender, EventArgs e)
        {
            this.MoveItem(-1);
        }

        /// <summary>
        /// Event when the add button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            LocalizedLabel[] localLabels = new LocalizedLabel[this.installedLanguages.Count];
            LocalizedLabel[] localDescriptions = new LocalizedLabel[this.installedLanguages.Count];
            for (int i = 0; i < this.installedLanguages.Count; i++)
            {
                localLabels[i] = new LocalizedLabel(string.Empty, this.installedLanguages.Keys.Skip(i).First());
                localDescriptions[i] = new LocalizedLabel(string.Empty, this.installedLanguages.Keys.Skip(i).First());
            }

            Microsoft.Xrm.Sdk.Label label = new Microsoft.Xrm.Sdk.Label(new LocalizedLabel(string.Empty, this.DefaultLanguage), localLabels);
            Microsoft.Xrm.Sdk.Label description = new Microsoft.Xrm.Sdk.Label(new LocalizedLabel(string.Empty, this.DefaultLanguage), localDescriptions);

            int currentValue = 0;
            var currentItem = (EntityItem)OptionSetList.SelectedItem;
            for (int i = this.OptionSetPrefix * 10000; i < (this.OptionSetPrefix * 10000) + 9999; i++)
            {
                if (!currentItem.Children.Exists(c => c.Value == i))
                {
                    currentValue = i;
                    break;
                }
            }

            EntityItem item = new EntityItem(currentValue, label, description, currentItem);

            currentItem.Children.Add(item);
            OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)OptionSetList.SelectedItem).Children);
            OptionsList.SelectedItem = item;
        }

        /// <summary>
        /// Event when focus leaves a label.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void Label_Leave(object sender, EventArgs e)
        {
            var selected = OptionsList.SelectedItem;
            OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)OptionSetList.SelectedItem).Children);
            OptionsList.SelectedItem = selected;
            var current = this.ActiveControl;
            current.Focus();
        }

        /// <summary>
        /// Event when the mouse is moved over a list.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            string toolTip = string.Empty;
            System.Windows.Forms.ListBox list = (System.Windows.Forms.ListBox)sender;
            int index = list.IndexFromPoint(e.Location);
            if ((index >= 0) && (index < list.Items.Count))
            {
                toolTip = ((EntityItem)list.Items[index]).LogicalName;
            }

            toolTip1.SetToolTip(list, toolTip);
        }

        /// <summary>
        /// Event when the publish button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void PublishButton_Click(object sender, EventArgs e)
        {
            this.PublishOptions(EntitiesList.Items.Cast<EntityItem>().Where(c => c.Changed));
        }

        /// <summary>
        /// Event when the from solution button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void SolutionMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            this.GetSolutionEntities(menu.Name, Guid.Parse(menu.Tag.ToString()));
        }

        /// <summary>
        /// Event when the load entities menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void LoadEntitiesMenu_Click(object sender, EventArgs e)
        {
            this.ExecuteMethod(this.Setup);
        }

        /// <summary>
        /// Event raised when the option set value is changed.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ValueText_TextChanged(object sender, EventArgs e)
        {
            if (!this.Loading)
            {
                this.SetChanged((EntityItem)OptionsList.SelectedItem);
            }
        }
    }
}
