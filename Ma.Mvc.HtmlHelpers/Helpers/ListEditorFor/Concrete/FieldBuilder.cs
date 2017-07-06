using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using System;
using System.Linq.Expressions;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    /// <summary>
    /// Creates instances of ListItems.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public class FieldBuilder<TModel> : IFieldBuilder<TModel>
        where TModel : class
    {
        private ListEditorBuilder<TModel> ListEditorBuilder { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listEditorBuilder">Instance of ListEditorBuilder.</param>
        public FieldBuilder(ListEditorBuilder<TModel> listEditorBuilder)
        {
            if (listEditorBuilder == null)
                throw new ArgumentNullException(nameof(listEditorBuilder));

            ListEditorBuilder = listEditorBuilder;
        }

        /// <summary>
        /// Add lambda expression as a field to the ListEditorBuilder.
        /// </summary>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>
        /// <returns>An instance of Field.</returns>
        public IField<TModel> Property<TProperty>(
            Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ListEditorBuilder.AddField(expression);
        }
    }
}