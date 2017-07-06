using System;
using System.Linq.Expressions;

namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract
{
    public interface ISortingFieldBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Add lambda expression as a sorting field.
        /// </summary>
        /// <typeparam name="TProperty">Type of selected property.</typeparam>
        /// <param name="expression">Expression to select property.</param>
        /// <returns>Instance of ISortingField.</returns>
        ISortingField<TModel> Property<TProperty>(
            Expression<Func<TModel, TProperty>> expression);
    }
}
