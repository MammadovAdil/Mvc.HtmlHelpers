using System.Web.Mvc;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// List editor builder which is managed at client side.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public interface IClientListEditorBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Enable adding new items to the list.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableAdd();
        /// <summary>
        /// Enable or disable adding new items to the list.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableAdd(bool enable);

        /// <summary>
        /// Enable editing items.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableEdit();
        /// <summary>
        /// Enable or disable editing items.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableEdit(bool enable);

        /// <summary>
        /// Enable deleting items.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableDelete();
        /// <summary>
        /// Disable or disable deleting items.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableDelete(bool enable);

        /// <summary>
        /// Enable viewing more information.
        /// </summary>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableMoreInfo();
        /// <summary>
        /// Enable or disable viewing more information.
        /// </summary>
        /// <param name="enable">Set true to enable, false to disable.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> EnableMoreInfo(bool enable);

        /// <summary>
        /// Set label of add button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithAddButtonLabel(string label);
        /// <summary>
        /// Set label of edit button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithEditButtonLabel(string label);
        /// <summary>
        /// Set label of delete button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithDeleteButtonLabel(string label);
        /// <summary>
        /// Set label of more info button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithMoreInfoButtonLabel(string label);


        /// <summary>
        /// Set label of save button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithSaveButtonLabel(string label);
        /// <summary>
        /// Set label of cancel button.
        /// </summary>
        /// <param name="label">Label to set for button.</param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithCancelButtonLabel(string label);

        /// <summary>
        /// Set the name of bound collection.
        /// </summary>
        /// <param name="collectionName">
        /// Name of collection. Must be same with posted action parameter name.
        /// </param>
        /// <returns>An instance of IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> WithCollectionName(string collectionName);

        /// <summary>
        /// Render client list editor builder as list.
        /// </summary>
        /// <returns>IClientListEditorBuilder as MvcHtmlString format.</returns>
        MvcHtmlString RenderList();

        /// <summary>
        /// Render client list editor builder as list.
        /// </summary>
        /// <param name="htmlAttributes">Html attributes to add to list.</param>
        /// <returns>IClientListEditorBuilder as MvcHtmlString format.</returns>
        MvcHtmlString RenderList(object htmlAttributes);
    }
}
