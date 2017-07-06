namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Action for list.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IListEditorListAction<TModel>
        : IListEditorAction<TModel>
    {
        /// <summary>
        /// Set url for action.
        /// </summary>
        /// <param name="url">Url to action.</param>
        /// <returns>Isntance of IListEditorListAction.</returns>
        IListEditorListAction<TModel> WithUrl(string url);
    }
}
