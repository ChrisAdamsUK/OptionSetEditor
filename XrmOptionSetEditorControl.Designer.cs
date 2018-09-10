// <copyright file="XrmOptionSetEditorControl.Designer.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>Control for the option set editor.</summary>
namespace OptionSetEditor
{
    /// <summary>
    /// Control for editing option sets.
    /// </summary>
    partial class XrmOptionSetEditorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XrmOptionSetEditorControl));
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.CloseMenu = new System.Windows.Forms.ToolStripButton();
            this.LoadEntitiesMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.AllEntitiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SolutionEntitiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PublishMenu = new System.Windows.Forms.ToolStripButton();
            this.ImportMenu = new System.Windows.Forms.ToolStripButton();
            this.ExportMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.ExportAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportChangedMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ValueText = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.LabelsGroup = new System.Windows.Forms.GroupBox();
            this.DescriptionsGroup = new System.Windows.Forms.GroupBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.OptionsList = new System.Windows.Forms.ListBox();
            this.EntitiesList = new System.Windows.Forms.ListBox();
            this.AttributesList = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.MainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseMenu,
            this.LoadEntitiesMenu,
            this.PublishMenu,
            this.ImportMenu,
            this.ExportMenu});
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(1302, 25);
            this.MainToolStrip.TabIndex = 0;
            this.MainToolStrip.Text = "toolStrip1";
            // 
            // CloseMenu
            // 
            this.CloseMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CloseMenu.Image = ((System.Drawing.Image)(resources.GetObject("CloseMenu.Image")));
            this.CloseMenu.ImageTransparentColor = System.Drawing.Color.Black;
            this.CloseMenu.Name = "CloseMenu";
            this.CloseMenu.Size = new System.Drawing.Size(23, 22);
            this.CloseMenu.Text = "Close";
            this.CloseMenu.Click += new System.EventHandler(this.CloseMenu_Click);
            // 
            // LoadEntitiesMenu
            // 
            this.LoadEntitiesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllEntitiesMenu,
            this.SolutionEntitiesMenu});
            this.LoadEntitiesMenu.Image = ((System.Drawing.Image)(resources.GetObject("LoadEntitiesMenu.Image")));
            this.LoadEntitiesMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadEntitiesMenu.Name = "LoadEntitiesMenu";
            this.LoadEntitiesMenu.Size = new System.Drawing.Size(103, 22);
            this.LoadEntitiesMenu.Text = "Load Entities";
            this.LoadEntitiesMenu.Click += new System.EventHandler(this.LoadEntitiesMenu_Click);
            // 
            // AllEntitiesMenu
            // 
            this.AllEntitiesMenu.Name = "AllEntitiesMenu";
            this.AllEntitiesMenu.Size = new System.Drawing.Size(149, 22);
            this.AllEntitiesMenu.Text = "All Entities";
            this.AllEntitiesMenu.ToolTipText = "Load all the entities in the default solution";
            this.AllEntitiesMenu.Click += new System.EventHandler(this.AllEntitiesMenu_Click);
            // 
            // SolutionEntitiesMenu
            // 
            this.SolutionEntitiesMenu.Name = "SolutionEntitiesMenu";
            this.SolutionEntitiesMenu.Size = new System.Drawing.Size(149, 22);
            this.SolutionEntitiesMenu.Text = "From Solution";
            this.SolutionEntitiesMenu.ToolTipText = "Load all the entities from a solution";
            // 
            // PublishMenu
            // 
            this.PublishMenu.Enabled = false;
            this.PublishMenu.Image = ((System.Drawing.Image)(resources.GetObject("PublishMenu.Image")));
            this.PublishMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PublishMenu.Name = "PublishMenu";
            this.PublishMenu.Size = new System.Drawing.Size(66, 22);
            this.PublishMenu.Text = "Publish";
            this.PublishMenu.ToolTipText = "Publish the selected items to CRM";
            this.PublishMenu.Click += new System.EventHandler(this.PublishButton_Click);
            // 
            // ImportMenu
            // 
            this.ImportMenu.Enabled = false;
            this.ImportMenu.Image = ((System.Drawing.Image)(resources.GetObject("ImportMenu.Image")));
            this.ImportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImportMenu.Name = "ImportMenu";
            this.ImportMenu.Size = new System.Drawing.Size(116, 22);
            this.ImportMenu.Text = "Import from CSV";
            this.ImportMenu.ToolTipText = "Import data from a CSV file";
            this.ImportMenu.Click += new System.EventHandler(this.ImportMenu_Click);
            // 
            // ExportMenu
            // 
            this.ExportMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportAllMenu,
            this.ExportChangedMenu});
            this.ExportMenu.Enabled = false;
            this.ExportMenu.Image = ((System.Drawing.Image)(resources.GetObject("ExportMenu.Image")));
            this.ExportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportMenu.Name = "ExportMenu";
            this.ExportMenu.Size = new System.Drawing.Size(107, 22);
            this.ExportMenu.Text = "Export to CSV";
            this.ExportMenu.ToolTipText = "Export items to CSV";
            // 
            // ExportAllMenu
            // 
            this.ExportAllMenu.Image = ((System.Drawing.Image)(resources.GetObject("ExportAllMenu.Image")));
            this.ExportAllMenu.Name = "ExportAllMenu";
            this.ExportAllMenu.Size = new System.Drawing.Size(158, 22);
            this.ExportAllMenu.Text = "All Attributes";
            this.ExportAllMenu.ToolTipText = "Export all the attributes for the selected entity";
            this.ExportAllMenu.Click += new System.EventHandler(this.ExportAllMenu_Click);
            // 
            // ExportChangedMenu
            // 
            this.ExportChangedMenu.Image = ((System.Drawing.Image)(resources.GetObject("ExportChangedMenu.Image")));
            this.ExportChangedMenu.Name = "ExportChangedMenu";
            this.ExportChangedMenu.Size = new System.Drawing.Size(158, 22);
            this.ExportChangedMenu.Text = "Export Changed";
            this.ExportChangedMenu.ToolTipText = "Export changed attributes for the selected entity";
            this.ExportChangedMenu.Click += new System.EventHandler(this.ExportChangedMenu_Click);
            // 
            // ValueText
            // 
            this.ValueText.Location = new System.Drawing.Point(1048, 44);
            this.ValueText.MaxLength = 10;
            this.ValueText.Name = "ValueText";
            this.ValueText.Size = new System.Drawing.Size(100, 20);
            this.ValueText.TabIndex = 8;
            this.toolTip.SetToolTip(this.ValueText, "Value of the selected option");
            this.ValueText.Visible = false;
            this.ValueText.TextChanged += new System.EventHandler(this.ValueText_TextChanged);
            this.ValueText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueText_KeyPress);
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackColor = System.Drawing.Color.Transparent;
            this.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DeleteButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.DeleteButton.Enabled = false;
            this.DeleteButton.FlatAppearance.BorderSize = 0;
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.Location = new System.Drawing.Point(919, 81);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(38, 37);
            this.DeleteButton.TabIndex = 20;
            this.toolTip.SetToolTip(this.DeleteButton, "Delete the selected option");
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Visible = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.BackColor = System.Drawing.Color.Transparent;
            this.DownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DownButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.DownButton.Enabled = false;
            this.DownButton.FlatAppearance.BorderSize = 0;
            this.DownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownButton.Image = ((System.Drawing.Image)(resources.GetObject("DownButton.Image")));
            this.DownButton.Location = new System.Drawing.Point(919, 161);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(38, 37);
            this.DownButton.TabIndex = 18;
            this.toolTip.SetToolTip(this.DownButton, "Move the selected option down");
            this.DownButton.UseVisualStyleBackColor = false;
            this.DownButton.Visible = false;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.BackColor = System.Drawing.Color.Transparent;
            this.UpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UpButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.UpButton.Enabled = false;
            this.UpButton.FlatAppearance.BorderSize = 0;
            this.UpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpButton.Image = ((System.Drawing.Image)(resources.GetObject("UpButton.Image")));
            this.UpButton.Location = new System.Drawing.Point(919, 121);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(38, 37);
            this.UpButton.TabIndex = 17;
            this.toolTip.SetToolTip(this.UpButton, "Move the selected option up");
            this.UpButton.UseVisualStyleBackColor = false;
            this.UpButton.Visible = false;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(1008, 47);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(34, 13);
            this.ValueLabel.TabIndex = 9;
            this.ValueLabel.Text = "Value";
            this.ValueLabel.Visible = false;
            // 
            // LabelsGroup
            // 
            this.LabelsGroup.Location = new System.Drawing.Point(1011, 76);
            this.LabelsGroup.Name = "LabelsGroup";
            this.LabelsGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelsGroup.Size = new System.Drawing.Size(281, 24);
            this.LabelsGroup.TabIndex = 11;
            this.LabelsGroup.TabStop = false;
            this.LabelsGroup.Text = "Language Labels";
            this.LabelsGroup.Visible = false;
            // 
            // DescriptionsGroup
            // 
            this.DescriptionsGroup.Location = new System.Drawing.Point(1011, 106);
            this.DescriptionsGroup.Name = "DescriptionsGroup";
            this.DescriptionsGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DescriptionsGroup.Size = new System.Drawing.Size(281, 23);
            this.DescriptionsGroup.TabIndex = 12;
            this.DescriptionsGroup.TabStop = false;
            this.DescriptionsGroup.Text = "Language Descriptions";
            this.DescriptionsGroup.Visible = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog_FileOk);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog_FileOk);
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.SynchronizingObject = this;
            // 
            // OptionsList
            // 
            this.OptionsList.FormattingEnabled = true;
            this.OptionsList.Location = new System.Drawing.Point(649, 38);
            this.OptionsList.Name = "OptionsList";
            this.OptionsList.Size = new System.Drawing.Size(270, 550);
            this.OptionsList.TabIndex = 3;
            this.OptionsList.SelectedValueChanged += new System.EventHandler(this.OptionsList_SelectedValueChanged);
            this.OptionsList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.List_MouseMove);
            // 
            // EntitiesList
            // 
            this.EntitiesList.FormattingEnabled = true;
            this.EntitiesList.Location = new System.Drawing.Point(7, 38);
            this.EntitiesList.Name = "EntitiesList";
            this.EntitiesList.Size = new System.Drawing.Size(270, 550);
            this.EntitiesList.Sorted = true;
            this.EntitiesList.TabIndex = 18;
            this.EntitiesList.SelectedValueChanged += new System.EventHandler(this.EntitiesList_SelectedValueChanged);
            this.EntitiesList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.List_MouseMove);
            // 
            // AttributesList
            // 
            this.AttributesList.FormattingEnabled = true;
            this.AttributesList.Location = new System.Drawing.Point(328, 38);
            this.AttributesList.Name = "AttributesList";
            this.AttributesList.Size = new System.Drawing.Size(270, 550);
            this.AttributesList.Sorted = true;
            this.AttributesList.TabIndex = 19;
            this.AttributesList.SelectedValueChanged += new System.EventHandler(this.AttributesList_SelectedValueChanged);
            this.AttributesList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.List_MouseMove);
            // 
            // AddButton
            // 
            this.AddButton.BackColor = System.Drawing.Color.Transparent;
            this.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AddButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.AddButton.Enabled = false;
            this.AddButton.FlatAppearance.BorderSize = 0;
            this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.Location = new System.Drawing.Point(920, 41);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(38, 37);
            this.AddButton.TabIndex = 21;
            this.toolTip.SetToolTip(this.AddButton, "Add a new option");
            this.AddButton.UseVisualStyleBackColor = false;
            this.AddButton.Visible = false;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // XrmOptionSetEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.AttributesList);
            this.Controls.Add(this.EntitiesList);
            this.Controls.Add(this.DescriptionsGroup);
            this.Controls.Add(this.LabelsGroup);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.ValueText);
            this.Controls.Add(this.OptionsList);
            this.Controls.Add(this.MainToolStrip);
            this.Name = "XrmOptionSetEditorControl";
            this.Size = new System.Drawing.Size(1302, 604);
            this.Load += new System.EventHandler(this.XrmOptionSetEditorControl_Load);
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton CloseMenu;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox ValueText;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.GroupBox LabelsGroup;
        private System.Windows.Forms.GroupBox DescriptionsGroup;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.ToolStripDropDownButton LoadEntitiesMenu;
        private System.Windows.Forms.ToolStripMenuItem AllEntitiesMenu;
        private System.Windows.Forms.ToolStripMenuItem SolutionEntitiesMenu;
        private System.Windows.Forms.ListBox OptionsList;
        private System.Windows.Forms.ToolStripButton PublishMenu;
        private System.Windows.Forms.ToolStripButton ImportMenu;
        private System.Windows.Forms.ListBox EntitiesList;
        private System.Windows.Forms.ListBox AttributesList;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.ToolStripDropDownButton ExportMenu;
        private System.Windows.Forms.ToolStripMenuItem ExportAllMenu;
        private System.Windows.Forms.ToolStripMenuItem ExportChangedMenu;
        private System.Windows.Forms.Button AddButton;
    }
}
