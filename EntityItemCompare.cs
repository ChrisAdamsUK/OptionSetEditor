// <copyright file="EntityItemCompare.cs" company="Almad Solutions.">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>Chris Adams</author>
// <date>12/7/2018</date>
// <summary>Compares two entity items.</summary>
namespace OptionSetEditor
{
    using System.Collections.Generic;

    /// <summary>
    /// Class to compare two entity items.
    /// </summary>
    internal class EntityItemCompare : IEqualityComparer<EntityItem>
    {
        /// <summary>
        /// Equals function.
        /// </summary>
        /// <param name="x">The first item to compare.</param>
        /// <param name="y">The second item to compare.</param>
        /// <returns>Whether the items are equal.</returns>
        bool IEqualityComparer<EntityItem>.Equals(EntityItem x, EntityItem y)
        {
            return x.LogicalName == y.LogicalName && x.DisplayName == y.DisplayName;
        }

        /// <summary>
        /// Override of the get hash code function.
        /// </summary>
        /// <param name="obj">the item.</param>
        /// <returns>The hash code.</returns>
        int IEqualityComparer<EntityItem>.GetHashCode(EntityItem obj)
        {
            return obj.LogicalName.GetHashCode();
        }
    }
}
