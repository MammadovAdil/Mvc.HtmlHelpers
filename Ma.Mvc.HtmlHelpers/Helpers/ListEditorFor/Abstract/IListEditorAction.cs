namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Action for list.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IListEditorAction<TModel>
    {
        /// <summary>
        /// Set the label for the action.
        /// </summary>
        /// <param name="label">Label for the action.</param>
        /// <returns>Instance of IListEditorAction.</returns>
        IListEditorAction<TModel> WithLabel(string label);

        /// <summary>
        /// Provide attributes for action element.
        /// </summary>
        /// <param name="htmlAttributes">Html attributes for action element.</param>
        /// <returns>Isntance of IListEditorAction</returns>
        IListEditorAction<TModel> WithAttributes(object htmlAttributes);
    }
}
