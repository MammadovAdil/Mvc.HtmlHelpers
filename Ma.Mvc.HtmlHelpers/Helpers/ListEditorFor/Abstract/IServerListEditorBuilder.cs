using System;
using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// List editor builder which is managed at client side.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public interface IServerListEditorBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Create instance of IActionBuilder and add list editor actions.
        /// </summary>
        /// <param name="actionInitializer">Action to create instance of IActionBuilder.</param>
        /// <returns>Instance of IServerListEditorBuilder.</returns>
        IServerListEditorBuilder<TModel> Actions(
            Action<IActionBuilder<TModel>> actionInitializer);

        /// <summary>
        /// Provide html attribute generator for list data rows.
        /// </summary>
        /// <param name="attributeGenerator">Attribute generator function.</param>
        /// <returns>Instance of IServerListEditorBuilder.</returns>
        IServerListEditorBuilder<TModel> WithDataRowAttributes(Func<TModel, object> attributeGenerator);

        /// <summary>
        /// Render client list editor builder as list.
        /// </summary>
        /// <returns>IServerListEditorBuilder as MvcHtmlString format.</returns>
        MvcHtmlString RenderList();

        /// <summary>
        /// Render client list editor builder as list.
        /// </summary>
        /// <param name="htmlAttributes">Html attributes to add to list.</param>
        /// <returns>IServerListEditorBuilder as MvcHtmlString format.</returns>
        MvcHtmlString RenderList(object htmlAttributes);
    }
}
