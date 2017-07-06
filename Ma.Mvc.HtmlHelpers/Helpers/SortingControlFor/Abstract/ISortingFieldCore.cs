namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract
{
    public interface ISortingFieldCore<TModel>
        where TModel : class
    {
        /// <summary>
        /// Label for sorting field.
        /// </summary>
        string FieldLabel { get; set; }
        /// <summary>
        /// Explicitly set name of selected property.
        /// </summary>
        string ExplicitPropertyName { get; set; }

        /// <summary>
        /// Get name of selected property.
        /// </summary>
        string GetPropertyName();
    }
}
