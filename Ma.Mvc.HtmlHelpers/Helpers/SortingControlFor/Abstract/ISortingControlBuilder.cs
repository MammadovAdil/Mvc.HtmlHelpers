using Ma.Mvc.HtmlHelpers.Models;
using System;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.SortingControlFor.Abstract
{
    public interface ISortingControlBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Specify sorting enabled fields and their properties.
        /// </summary>
        /// <param name="fieldBuilder">Field builder to add fields.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> Fields(
            Action<ISortingFieldBuilder<TModel>> fieldBuilder);

        /// <summary>
        /// Provide current sorting info.
        /// </summary>
        /// <param name="info">Current sorting information.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> WithSortingInfo(SortingInfo info);

        /// <summary>
        /// Specify url generator for sort field.
        /// </summary>
        /// <param name="sortFieldUrlGenerator">Url generator for sort field.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> WithSortFieldAction(
            Func<string, string> sortFieldUrlGenerator);

        /// <summary>
        /// Specify url generator for sort order.
        /// </summary>
        /// <param name="sortOrderUrlGenerator">Url generator for sort order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> WithSortOrderAction(
            Func<SortOrder, string> sortOrderUrlGenerator);

        /// <summary>
        /// Set label for ascending order.
        /// </summary>
        /// <param name="label">Label to set for ascending order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> WithAscendingLabel(string label);

        /// <summary>
        /// Set label for descending order.
        /// </summary>
        /// <param name="label">Label to set for descending order.</param>
        /// <returns>Instance of ISortingControlBuilder.</returns>
        ISortingControlBuilder<TModel> WithDescendingLabel(string label);

        /// <summary>
        /// Render sorting controller.
        /// </summary>        
        /// <returns>MvcHtmlString.</returns>
        MvcHtmlString Render();

        /// <summary>
        /// Render sorting controller.
        /// </summary>
        /// <param name="htmlAttributes">HtmlAttributes to add to control.</param>
        /// <returns>MvcHtmlString.</returns>
        MvcHtmlString Render(object htmlAttributes);
    }
}
