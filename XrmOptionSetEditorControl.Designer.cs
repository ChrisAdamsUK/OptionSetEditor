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
            this.ExportMenu = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.ValueText = new System.Windows.Forms.TextBox();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.LabelsGroup = new System.Windows.Forms.GroupBox();
            this.DescriptionsGroup = new System.Windows.Forms.GroupBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.AddButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.GlobalLabel = new System.Windows.Forms.Label();
            this.OptionsList = new System.Windows.Forms.ListBox();
            this.EntitiesList = new System.Windows.Forms.ListBox();
            this.OptionSetList = new System.Windows.Forms.ListBox();
            this.MainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
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
            this.MainToolStrip.Size = new System.Drawing.Size(1003, 25);
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
            this.AllEntitiesMenu.Click += new System.EventHandler(this.AllEntitiesMenu_Click);
            // 
            // SolutionEntitiesMenu
            // 
            this.SolutionEntitiesMenu.Name = "SolutionEntitiesMenu";
            this.SolutionEntitiesMenu.Size = new System.Drawing.Size(149, 22);
            this.SolutionEntitiesMenu.Text = "From Solution";
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
            this.ExportMenu.Enabled = false;
            this.ExportMenu.Image = ((System.Drawing.Image)(resources.GetObject("ExportMenu.Image")));
            this.ExportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportMenu.Name = "ExportMenu";
            this.ExportMenu.Size = new System.Drawing.Size(98, 22);
            this.ExportMenu.Text = "Export to CSV";
            this.ExportMenu.ToolTipText = "Export the selected items to CSV";
            this.ExportMenu.Click += new System.EventHandler(this.ExportMenu_Click);
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
            this.UpButton.Location = new System.Drawing.Point(588, 63);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(38, 37);
            this.UpButton.TabIndex = 6;
            this.UpButton.UseVisualStyleBackColor = false;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
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
            this.DownButton.Location = new System.Drawing.Point(588, 145);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(38, 37);
            this.DownButton.TabIndex = 7;
            this.DownButton.UseVisualStyleBackColor = false;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // ValueText
            // 
            this.ValueText.Location = new System.Drawing.Point(697, 44);
            this.ValueText.Name = "ValueText";
            this.ValueText.Size = new System.Drawing.Size(100, 20);
            this.ValueText.TabIndex = 8;
            this.ValueText.Visible = false;
            this.ValueText.TextChanged += new System.EventHandler(this.ValueText_TextChanged);
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(657, 47);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(34, 13);
            this.ValueLabel.TabIndex = 9;
            this.ValueLabel.Text = "Value";
            this.ValueLabel.Visible = false;
            // 
            // LabelsGroup
            // 
            this.LabelsGroup.Location = new System.Drawing.Point(660, 76);
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
            this.DescriptionsGroup.Location = new System.Drawing.Point(660, 106);
            this.DescriptionsGroup.Name = "DescriptionsGroup";
            this.DescriptionsGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DescriptionsGroup.Size = new System.Drawing.Size(281, 23);
            this.DescriptionsGroup.TabIndex = 12;
            this.DescriptionsGroup.TabStop = false;
            this.DescriptionsGroup.Text = "Language Descriptions";
            this.DescriptionsGroup.Visible = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog_FileOk);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
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
            this.AddButton.Location = new System.Drawing.Point(588, 201);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(38, 37);
            this.AddButton.TabIndex = 15;
            this.AddButton.UseVisualStyleBackColor = false;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
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
            this.DeleteButton.Location = new System.Drawing.Point(588, 255);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(38, 37);
            this.DeleteButton.TabIndex = 16;
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // GlobalLabel
            // 
            this.GlobalLabel.AutoSize = true;
            this.GlobalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalLabel.Location = new System.Drawing.Point(222, 368);
            this.GlobalLabel.Name = "GlobalLabel";
            this.GlobalLabel.Size = new System.Drawing.Size(145, 20);
            this.GlobalLabel.TabIndex = 17;
            this.GlobalLabel.Text = "* Global Option Set";
            this.GlobalLabel.Visible = false;
            // 
            // OptionsList
            // 
            this.OptionsList.FormattingEnabled = true;
            this.OptionsList.Location = new System.Drawing.Point(398, 38);
            this.OptionsList.Name = "OptionsList";
            this.OptionsList.Size = new System.Drawing.Size(154, 316);
            this.OptionsList.TabIndex = 3;
            this.OptionsList.SelectedValueChanged += new System.EventHandler(this.OptionsList_SelectedValueChanged);
            // 
            // EntitiesList
            // 
            this.EntitiesList.FormattingEnabled = true;
            this.EntitiesList.Location = new System.Drawing.Point(36, 38);
            this.EntitiesList.Name = "EntitiesList";
            this.EntitiesList.Size = new System.Drawing.Size(151, 316);
            this.EntitiesList.Sorted = true;
            this.EntitiesList.TabIndex = 18;
            this.EntitiesList.SelectedValueChanged += new System.EventHandler(this.EntitiesList_SelectedValueChanged);
            this.EntitiesList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.List_MouseMove);
            // 
            // OptionSetList
            // 
            this.OptionSetList.FormattingEnabled = true;
            this.OptionSetList.Location = new System.Drawing.Point(207, 38);
            this.OptionSetList.Name = "OptionSetList";
            this.OptionSetList.Size = new System.Drawing.Size(174, 316);
            this.OptionSetList.Sorted = true;
            this.OptionSetList.TabIndex = 19;
            this.OptionSetList.SelectedValueChanged += new System.EventHandler(this.OptionSetList_SelectedValueChanged);
            // 
            // XrmOptionSetEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OptionSetList);
            this.Controls.Add(this.EntitiesList);
            this.Controls.Add(this.GlobalLabel);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.DescriptionsGroup);
            this.Controls.Add(this.LabelsGroup);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.ValueText);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.OptionsList);
            this.Controls.Add(this.MainToolStrip);
            this.Name = "XrmOptionSetEditorControl";
            this.Size = new System.Drawing.Size(1003, 527);
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton CloseMenu;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.TextBox ValueText;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.GroupBox LabelsGroup;
        private System.Windows.Forms.GroupBox DescriptionsGroup;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label GlobalLabel;
        private System.Windows.Forms.ToolStripDropDownButton LoadEntitiesMenu;
        private System.Windows.Forms.ToolStripMenuItem AllEntitiesMenu;
        private System.Windows.Forms.ToolStripMenuItem SolutionEntitiesMenu;
        private System.Windows.Forms.ListBox OptionsList;
        private System.Windows.Forms.ToolStripButton PublishMenu;
        private System.Windows.Forms.ToolStripButton ImportMenu;
        private System.Windows.Forms.ToolStripButton ExportMenu;
        private System.Windows.Forms.ListBox EntitiesList;
        private System.Windows.Forms.ListBox OptionSetList;
    }
}
