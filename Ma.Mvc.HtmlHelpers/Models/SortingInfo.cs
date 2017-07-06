namespace Ma.Mvc.HtmlHelpers.Models
{
    /// <summary>
    /// Information about sorting.
    /// </summary>
    public class SortingInfo
    {
        /// <summary>
        /// Name of property according to which data sorted.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Order of sorting.
        /// </summary>
        public SortOrder Order { get; set; }
    }

    public enum SortOrder
    {
        Asc, Desc
    }
}
