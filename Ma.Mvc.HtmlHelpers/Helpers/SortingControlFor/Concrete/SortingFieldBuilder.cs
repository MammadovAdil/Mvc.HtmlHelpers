using Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract;
using System;
using System.Linq.Expressions;

namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Concrete
{
    public class SortingFieldBuilder<TModel>
        : ISortingFieldBuilder<TModel>
        where TModel : class
    {
        private SortingControlBuilder<TModel> ControllerBuilder { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="controllerBuilder">Sorting controller builder.</param>
        public SortingFieldBuilder(SortingControlBuilder<TModel> controllerBuilder)
        {
            if (controllerBuilder == null)
                throw new ArgumentNullException(nameof(controllerBuilder));

            ControllerBuilder = controllerBuilder;
        }

        /// <summary>
        /// Add lambda expression as a sorting field.
        /// </summary>
        /// <typeparam name="TProperty">Type of selected property.</typeparam>
        /// <param name="expression">Expression to select property.</param>
        /// <returns>Instance of ISortingField.</returns>
        public ISortingField<TModel> Property<TProperty>(
            Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ControllerBuilder.AddField(expression);
        }
    }
}
