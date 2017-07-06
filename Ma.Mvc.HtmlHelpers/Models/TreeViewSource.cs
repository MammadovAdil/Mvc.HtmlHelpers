using System;
using System.Collections.Generic;

namespace Ma.Mvc.HtmlHelpers.Models
{
    /// <summary>
    /// Source for tree view.
    /// </summary>
    /// <typeparam name="TKey">Type of key in items.</typeparam>
    public class TreeViewSource<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Information about if ItemList is loaded.
        /// </summary>
        public bool IsLooaded { get; set; }

        /// <summary>
        /// List of items.
        /// </summary>
        public List<TreeViewItem<TKey>> ItemList { get; set; }
    }

    /// <summary>
    /// Model for one tree view item.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    public class TreeViewItem<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Identification number.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Id of parent.
        /// </summary>
        public Nullable<TKey> ParentId { get; set; }

        /// <summary>
        /// Level of item in hiereachy.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Value of item.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Optinal child list.
        /// </summary>
        public TreeViewSource<TKey> ChildViewSource { get; set; }
    }
}
