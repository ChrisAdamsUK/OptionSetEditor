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
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;

    /// <summary>
    /// Class to host the option set editor control.
    /// </summary>
    public partial class XrmOptionSetEditorControl : PluginControlBase, IPayPalPlugin, IGitHubPlugin
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
        /// Gets the repository name.
        /// </summary>
        public string RepositoryName
        {
            get
            {
                return "OptionSetEditor";
            }
        }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public string UserName
        {
            get
            {
                return "ChrisAdamsUK";
            }
        }

        /// <summary>
        /// Gets the current value of the option.
        /// </summary>
        public string CurrentValue { get; private set; }

        /// <summary>
        /// Gets or sets the default language of the current CRM user.
        /// </summary>
        private int DefaultLanguage { get; set; }

        /// <summary>
        /// Displays an error dialog.
        /// </summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="e">The exception.</param>
        /// <returns>The dialog response.</returns>
        private static DialogResult ShowThreadExceptionDialog(string title, Exception e)
        {
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:\n\n";
            errorMsg = errorMsg + e.Message + "\n\nStack Trace:\n" + e.StackTrace;
            return System.Windows.Forms.MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
        }

        /// <summary>
        /// Event when the user selects to open all entities.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void AllEntitiesMenu_Click(object sender, EventArgs e)
        {
            this.GetSolutionEntities("default");
            this.PublishMenu.Enabled = false;
        }

        /// <summary>
        /// Event when an item is selected from the entities list.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void EntitiesList_SelectedValueChanged(object sender, EventArgs e)
        {
            AttributesList.DataSource = null;
            EntityItem currentItem = (EntityItem)EntitiesList.SelectedItem;

            if (!currentItem.Loaded)
            {
                this.ExecuteMethod<EntityItem>(this.GetAttributes, currentItem);
            }
            else
            {
                AttributesList.DataSource = new BindingList<EntityItem>(currentItem.Children.ToArray());
                ImportMenu.Enabled = true;
                ExportMenu.Enabled = true;
            }

            if (OptionsList.SelectedItem == null)
            {
                this.ShowLabels(false);
            }
        }

        /// <summary>
        /// Event when an option set attribute is selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void AttributesList_SelectedValueChanged(object sender, EventArgs e)
        {
            OptionsList.DataSource = null;
            EntityItem currentEntity = (EntityItem)EntitiesList.SelectedItem;
            EntityItem currentAttribute = (EntityItem)AttributesList.SelectedItem;
            if (currentAttribute != null)
            {
                if (!currentAttribute.Loaded)
                {
                    this.ExecuteMethod<EntityItem>(this.GetOptions, currentAttribute);
                }
                else
                {
                    OptionsList.DataSource = new BindingList<EntityItem>(currentAttribute.Children);
                }

                if (OptionsList.SelectedItem == null)
                {
                    if (AttributesList.SelectedItem != null)
                    {
                        this.ShowLabels(false);
                    }
                    else
                    {
                        this.ShowLabels(false);
                    }
                }

                AddButton.Enabled = AttributesList.SelectedIndex > -1 && OptionsList.Items.Count < 300;
            }
        }

        /// <summary>
        /// Shows/Hides the label elements.
        /// </summary>
        /// <param name="display">Whether the controls are displayed.</param>
        private void ShowLabels(bool display)
        {
            LabelsGroup.Visible = display;
            DescriptionsGroup.Visible = display;
            ValueText.Visible = display;
            ValueLabel.Visible = display;
            UpButton.Visible = display;
            DownButton.Visible = display;
            AddButton.Visible = display || AttributesList.SelectedItem != null;
            DeleteButton.Visible = display;
        }

        /// <summary>
        /// Sets up the application.
        /// </summary>
        private void Setup()
        {
            this.WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving languages...",
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
                this.Loading = true;
                this.ValueText.DataBindings.Add(
                    "Text",
                    currentOption,
                    "Value",
                    false,
                    DataSourceUpdateMode.OnPropertyChanged);
                this.LoadLabels(this.LabelsGroup, currentOption.Label);
                DescriptionsGroup.Top = LabelsGroup.Top + LabelsGroup.Height + 30;
                this.LoadLabels(this.DescriptionsGroup, currentOption.Description);
                this.CurrentValue = ValueText.Text;
                this.Loading = false;
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
                text.MaxLength = 255;
                text.Tag = optionLabel;
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(text, $"{label.Text} translation for the option.");
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
                        text.MaxLength = 255;
                        text.Tag = optionLabel;
                        text.Name = " " + item.Key.ToString();
                        text.TextChanged += this.LabelChanged;
                        toolTip = new ToolTip();
                        toolTip.SetToolTip(text, $"{label.Text} translation for the option.");
                        group.Controls.Add(text);
                        top += 48;
                    }
                }

                group.Height = top;
            }

            this.ShowLabels(true);
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

            ((EntityItem)OptionsList.SelectedItem).Changed = true;
            ExportMenu.Enabled = true;
            PublishMenu.Enabled = true;
        }

        /// <summary>
        /// Event when import menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportMenu_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "CSV|*.csv";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
        }

        /// <summary>
        /// Event raised when the file save editor has a valid file selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="ev">Cancel event arguments.</param>
        private void SaveFileDialog_FileOk(object sender, CancelEventArgs ev)
        {
            IEnumerable<EntityItem> items = null;
            if (saveFileDialog.Tag.ToString() == "ALL")
            {
                items = AttributesList.Items.Cast<EntityItem>();
            }
            else
            {
                items = AttributesList.Items.Cast<EntityItem>().Where(e => e.Changed);
            }

            if (items.Count() != 0)
            {
                File.WriteAllText(saveFileDialog.FileName, this.Export(items, this.installedLanguages));
            }
            else
            {
                MessageBox.Show("There a no items to export", "Export");
            }
        }

        /// <summary>
        /// Event raised when the file open editor has a valid file selected.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            this.Import(openFileDialog.FileName, (EntityItem)EntitiesList.SelectedItem, this.installedLanguages);
            PublishMenu.Enabled = true;
            if (AttributesList.SelectedItem != null)
            {
                OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)AttributesList.SelectedItem).Children);
            }
        }

        /// <summary>
        /// Event when the delete button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ((EntityItem)OptionsList.SelectedItem).Parent.Changed = true;
            ((BindingList<EntityItem>)OptionsList.DataSource).Remove((EntityItem)OptionsList.SelectedItem);
            PublishMenu.Enabled = true;         
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
            ((EntityItem)AttributesList.SelectedItem).Children.RemoveAt(OptionsList.SelectedIndex);
            ((EntityItem)AttributesList.SelectedItem).Children.Insert(newIndex, selected);
            ((EntityItem)AttributesList.SelectedItem).Changed = true;

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
            var currentItem = (EntityItem)AttributesList.SelectedItem;
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
            OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)AttributesList.SelectedItem).Children);
            OptionsList.SelectedItem = item;
            AddButton.Enabled = AttributesList.SelectedIndex > -1 && OptionsList.Items.Count < 300;
            item.Changed = true;
            item.Parent.Changed = true;
            PublishMenu.Enabled = true;
        }

        /// <summary>
        /// Event when focus leaves a label.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void Label_Leave(object sender, EventArgs e)
        {
            var selected = OptionsList.SelectedItem;
            OptionsList.DataSource = new BindingList<EntityItem>(((EntityItem)AttributesList.SelectedItem).Children);
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
            string toolTipText = string.Empty;
            System.Windows.Forms.ListBox list = (System.Windows.Forms.ListBox)sender;
            int index = list.IndexFromPoint(e.Location);
            if ((index >= 0) && (index < list.Items.Count))
            {
                EntityItem item = (EntityItem)list.Items[index];

                if (!string.IsNullOrWhiteSpace(item?.Parent?.GlobalName))
                {
                    toolTipText += $"Global optionset\t= {item.Parent.GlobalName}\r\n";
                }

                if (!string.IsNullOrWhiteSpace(item?.GlobalName))
                {
                    toolTipText += $"Global optionset\t= {item.GlobalName}\r\n";
                }

                toolTipText += $"Disaplay Name\t= {item.DisplayName}\r\n";

                if (item.Value == 0)
                {
                    toolTipText += $"Logical Name\t= {item.LogicalName}\r\n";
                }
                else
                {
                    toolTipText += $"Description\t= {item?.Description?.LocalizedLabels.Where(l => l.LanguageCode == DefaultLanguage).FirstOrDefault()?.Label}\r\n";
                    toolTipText += $"Value\t\t= {item.Value}";
                }

                if (this.toolTip.GetToolTip(list) != toolTipText)
                {
                    toolTip.SetToolTip(list, toolTipText);
                }
            }
        }

        /// <summary>
        /// Event when the publish button is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void PublishButton_Click(object sender, EventArgs e)
        {
            this.PublishOptions(EntitiesList.Items.Cast<EntityItem>().Where(c => c.Changed));
            this.PublishMenu.Enabled = false;
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
            this.PublishMenu.Enabled = false;
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
            int value = 0;
            if (int.TryParse(ValueText.Text, out value) && value != 2147483647)
            {
                if (!this.Loading)
                {
                    ((EntityItem)OptionsList.SelectedItem).Changed = true;
                }

                this.CurrentValue = ValueText.Text;
            }
            else
            {
                MessageBox.Show("Only whole numbers up to 2,147,483,646 are allowed");
                ValueText.Text = this.CurrentValue;
            }
        }

        /// <summary>
        /// Event on key press of value text box to ensure only digits are entered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ValueText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Sets up the error handling on load.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void XrmOptionSetEditorControl_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(this.Form_UIThreadException);
        }

        /// <summary>
        /// Event when the close button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseMenu_Click(object sender, EventArgs e)
        {
            this.CloseTool();
        }

        /// <summary>
        /// Handle the UI exceptions by showing a dialog box, and asking the user whether
        /// or not they wish to abort execution
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="t">The event arguments.</param>
        private void Form_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
            }
            catch
            {
                try
                {
                    System.Windows.Forms.MessageBox.Show("Fatal Windows Forms Error", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    this.CloseTool();
                }
            }

            if (result == DialogResult.Abort)
            {
                this.CloseTool();
            }
        }

        /// <summary>
        /// Event when the export all menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ExportAllMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV|*.csv";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Tag = "ALL";
            saveFileDialog.ShowDialog();
        }

        /// <summary>
        /// Event when the export changed menu is clicked.
        /// </summary>
        /// <param name="sender">Sender argument.</param>
        /// <param name="e">Event arguments.</param>
        private void ExportChangedMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV|*.csv";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Tag = "CHG";
            saveFileDialog.ShowDialog();
        }
    }
}
