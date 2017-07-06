using System;

namespace Ma.Mvc.HtmlHelpers.Models
{
    /// <summary>
    /// Information needed for paging.
    /// </summary>
    public class PagingInfo
    {
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int ItemCount { get; set; }

        public int TotalPageCount
        {
            get
            {
                return (int)(Math.Ceiling((decimal)ItemCount / ItemsPerPage));
            }
        }
    }
}
