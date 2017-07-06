using System;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Action for items of list.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IListEditorItemAction<TModel>
        : IListEditorAction<TModel>
    {
        /// <summary>
        /// Set url generator for action.
        /// </summary>
        /// <param name="urlGenerator">Url generator for action.</param>
        /// <returns>Isntance of IListEditorItemAction.</returns>
        IListEditorItemAction<TModel> WithUrl(Func<TModel, string> urlGenerator);

        /// <summary>
        /// Render action only when condition is met.
        /// </summary>
        /// <param name="condition">Condition to chek to render the action.</param>
        /// <returns>Isntance of IListEditorItemAction.</returns>
        IListEditorItemAction<TModel> When(Func<TModel, bool> condition);
    }
}
