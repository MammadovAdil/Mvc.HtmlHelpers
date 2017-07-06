namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Action builder for list editor for.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IActionBuilder<TModel>
    {
        /// <summary>
        /// Add action to the list editor.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>IListEditorListAction instance.</returns>
        IListEditorListAction<TModel> AddListAction(string name);

        /// <summary>
        /// Add action for the items of the list editor.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>IListEditorItemAction instance.</returns>
        IListEditorItemAction<TModel> AddItemAction(string name);
    }
}
