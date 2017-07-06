using System;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Configurator for ListEditorBuilder.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public interface IListEditorBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Create instance of the IFieldBuilder and add fields.
        /// </summary>
        /// <param name="fieldInitializer">Action to create instance of IFieldBuilder.</param>
        /// <returns>An instance of ILisEditorBuilder.</returns>
        IListEditorBuilder<TModel> Fields(
            Action<IFieldBuilder<TModel>> fieldBuilder);     

        /// <summary>
        /// Provide html attributes for list data rows.
        /// </summary>
        /// <param name="htmlAttributes">Html attributes to add to list data row.</param>
        /// <returns>An instance of ILisEditorBuilder.</returns>
        IListEditorBuilder<TModel> WithDataRowAttributes(object htmlAttributes);

        /// <summary>
        /// Rendered list editor builder which will be managed at client side.
        /// </summary>
        /// <returns>ILisEditorBuilder as IClientListEditorBuilder.</returns>
        IClientListEditorBuilder<TModel> ManageAtClientSide();

        /// <summary>
        /// Render list editor builder which will be managed at server side.
        /// </summary>
        /// <returns>ILisEditorBuilder as IServerListEditorBuilder.</returns>
        IServerListEditorBuilder<TModel> ManageAtServerSide();
    }
}
