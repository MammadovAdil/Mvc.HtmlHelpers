using System;
using System.Linq.Expressions;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Field builder for list editor for.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IFieldBuilder<TModel>
    {
        /// <summary>
        /// Add lambda expression as a field to the IListEditorBuilder.
        /// </summary>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>
        /// <returns>An instance of IField.</returns>
        IField<TModel> Property<TProperty>(
            Expression<Func<TModel, TProperty>> expression);
    }
}
