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
    using System.Windows.Forms;

    /// <summary>
    /// Class to import and export the changes to a file.
    /// </summary>
    public static class ImportExport
    {
        /// <summary>
        /// Exports the changed items to a comma separated file.
        /// </summary>
        /// <param name="changedItems">The items that have changed.</param>
        /// <param name="installedLanguages">The list of languages installed.</param>
        /// <returns>The comma separated file.</returns>
        internal static string Export(IEnumerable<EntityItem> changedItems, Dictionary<int, string> installedLanguages)
        {
            StringBuilder csv = new StringBuilder("Entity,Attribute,Value,");
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
            foreach (var item in changedItems)
            {
                foreach (var child in item.Children.Where(c => c.Changed))
                {
                    csv.Append(child.ToCSV(installedLanguages.OrderBy(l => l.Key).Select(l => l.Key)));
                }
            }

            return csv.ToString();
        }

        /// <summary>
        /// Imports a comma separated file in to the app.
        /// </summary>
        /// <param name="fileName">The file name to import.</param>
        /// <param name="items">The items to import.</param>
        /// <param name="installedLanguages">The installed languages.</param>
        internal static void Import(string fileName, IEnumerable<EntityItem> items, Dictionary<int, string> installedLanguages)
        {
            Dictionary<string, EntityItem> attributes = new Dictionary<string, EntityItem>();
            var file = File.ReadAllLines(fileName);
            file = file.Skip(1).ToArray();
            var entities = file.Select(f => f.Substring(0, f.IndexOf(','))).Distinct();
            file = file.Select(f => f.Substring(f.IndexOf(',') + 1)).ToArray();
            foreach (var entity in entities)
            {
                var entityItem = items.Where(i => i.LogicalName == entity).FirstOrDefault();
                if (entityItem != null)
                {
                    entityItem.Children.All(e => { attributes.Add(e.LogicalName, e); return true; });

                    var groups = file.GroupBy(f => f.Substring(0, f.IndexOf(',')));
                    entityItem.Children.All(c => { c.Children.Clear(); return true; });
                    foreach (var line in file)
                    {
                        var columns = line.Split(',');
                        if (columns.Length != (installedLanguages.Count() * 2) + 2)
                        {
                            MessageBox.Show("Invalid number of columns in the file.");
                            break;
                        }
                        else
                        {
                            if (attributes.ContainsKey(columns[0]))
                            {
                                var attribute = entityItem.Children.Where(e => e.LogicalName == columns[0]).FirstOrDefault();
                                Microsoft.Xrm.Sdk.LocalizedLabel[] labels = new Microsoft.Xrm.Sdk.LocalizedLabel[installedLanguages.Count()];
                                Microsoft.Xrm.Sdk.LocalizedLabel[] descriptions = new Microsoft.Xrm.Sdk.LocalizedLabel[installedLanguages.Count()];

                                for (int i = 0; i < installedLanguages.Count; i++)
                                {
                                    labels[i] = new Microsoft.Xrm.Sdk.LocalizedLabel(columns[i + 2], installedLanguages.Keys.OrderBy(l => l).ToArray()[i]);
                                    descriptions[i] = new Microsoft.Xrm.Sdk.LocalizedLabel(columns[i + 5], installedLanguages.Keys.OrderBy(l => l).ToArray()[i]);
                                }

                                Microsoft.Xrm.Sdk.Label label = new Microsoft.Xrm.Sdk.Label();
                                label.LocalizedLabels.AddRange(labels);
                                label.UserLocalizedLabel = label.LocalizedLabels.Where(l => l.LanguageCode == installedLanguages.Keys.First()).First();
                                Microsoft.Xrm.Sdk.Label description = new Microsoft.Xrm.Sdk.Label();
                                description.LocalizedLabels.AddRange(descriptions);
                                description.UserLocalizedLabel = description.LocalizedLabels.Where(l => l.LanguageCode == installedLanguages.Keys.First()).First();

                                attribute.Children.Add(new EntityItem(int.Parse(columns[1]), label, description, attribute));
                                attribute.Loaded = true;
                            }
                            else
                            {
                                MessageBox.Show($"Cannot find the attribute {columns[0]}");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Cannot find the entity {entity}");
                }
            }
        }
    }
}