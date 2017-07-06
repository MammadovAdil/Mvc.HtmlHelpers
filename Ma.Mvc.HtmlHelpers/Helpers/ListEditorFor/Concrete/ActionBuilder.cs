using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using System;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    /// <summary>
    /// Action builder for list editor for.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public class ActionBuilder<TModel> 
        : IActionBuilder<TModel>
        where TModel : class
    {
        private ListEditorBuilder<TModel> ListEditorBuilder { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When listEditorBuilder is null.
        /// </exception>
        /// <param name="listEditorBuilder">ListEditorBuilder to build actions for.</param>
        public ActionBuilder(ListEditorBuilder<TModel> listEditorBuilder)
        {
            if (listEditorBuilder == null)
                throw new ArgumentNullException(nameof(listEditorBuilder));

            ListEditorBuilder = listEditorBuilder;
        }

        /// <summary>
        /// Add action to the list editor.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>IListEditorListAction instance.</returns>
        public IListEditorListAction<TModel> AddListAction(string name)
        {
            return ListEditorBuilder.AddListAction(name);
        }

        /// <summary>
        /// Add action for the items of the list editor.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>IListEditorItemAction instance.</returns>
        public IListEditorItemAction<TModel> AddItemAction(string name)
        {
            return ListEditorBuilder.AddItemAction(name);
        }
    }
}
