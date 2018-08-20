// <copyright file="EntityItem.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>Holds details of an entity and t's associated option attributes.</summary>
namespace OptionSetEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Class to hold the entity item and it associated child option set attributes.
    /// </summary>
    internal class EntityItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItem"/> class. 
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="logicalName">The logical name.</param>
        /// <param name="parent">The parent entity item.</param>
        public EntityItem(string displayName, string logicalName, EntityItem parent = null)
        {
            this.Children = new List<EntityItem>();
            this.Parent = parent; 
            this.DisplayName = displayName;
            this.LogicalName = logicalName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItem"/> class.
        /// </summary>
        /// <param name="value">The items value.</param>
        /// <param name="label">The items label.</param>
        /// <param name="description">The description label.</param>
        /// <param name="parent">The parent item.</param>
        public EntityItem(int value, Label label, Label description, EntityItem parent) :
            this(label.UserLocalizedLabel == null ? string.Empty : label.UserLocalizedLabel.Label, description.UserLocalizedLabel == null ? string.Empty : description.UserLocalizedLabel.Label)
        {
            this.Children = new List<EntityItem>();
            this.Parent = parent;
            this.Value = value;
            this.Label = label;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the parent item.
        /// </summary>
        public EntityItem Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item has changed.
        /// </summary>
        public bool Changed { get; set; }

        /// <summary>
        /// Gets or sets the child items of this item.
        /// </summary>
        public List<EntityItem> Children { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the logical name.
        /// </summary>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the global option set name.
        /// </summary>
        public string GlobalName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the attribute uses a global option set.
        /// </summary>
        public bool Global
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.GlobalName) || (this.Parent?.Global ?? false);
            }
        }

        /// <summary>
        /// Gets or sets the value of the option.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the users default language.
        /// </summary>
        public int DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item has been loaded from CRM.
        /// </summary>
        public bool Loaded { get; set; }

        /// <summary>
        /// Gets or sets the label for the option.
        /// </summary>
        public Label Label { get; set; }

        /// <summary>
        /// Gets or sets the description for the option.
        /// </summary>
        public Label Description { get; set; }

        /// <summary>
        /// Override of the base method to convert to a string.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            string display = this.Label != null && this.Label.UserLocalizedLabel != null ? this.Label.UserLocalizedLabel.Label : this.DisplayName;
            display = (!string.IsNullOrWhiteSpace(this.Parent?.GlobalName) || !string.IsNullOrWhiteSpace(this.GlobalName) ? "*" : string.Empty) + display;
            return display;
        }

        /// <summary>
        /// Converts an option to comma separated values for exporting.
        /// </summary>
        /// <param name="languages">The list of languages to export.</param>
        /// <returns>The comma separated version of the item.</returns>
        internal string ToCSV(IEnumerable<int> languages)
        {
            StringBuilder csv = new StringBuilder();
            foreach (var child in this.Children)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(child.Parent.Parent.LogicalName);
                builder.AppendLine(child.Parent.LogicalName);
                builder.AppendLine(child.Value.ToString());
                foreach (var item in languages)
                {
                    builder.AppendLine(child.Label.LocalizedLabels.Select(l => l.LanguageCode).Contains(item) ? child.Label.LocalizedLabels.Where(l => l.LanguageCode == item).FirstOrDefault().Label : string.Empty);
                }

                foreach (var item in languages)
                {
                    builder.AppendLine(child.Description.LocalizedLabels.Select(l => l.LanguageCode).Contains(item) ? child.Description.LocalizedLabels.Where(l => l.LanguageCode == item).FirstOrDefault().Label : string.Empty);
                }

                csv.Append(string.Join(",", builder.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None)));
                csv.Length = csv.Length - 1;
                csv.AppendLine();
            }

            return csv.ToString();
        }
    }
}
