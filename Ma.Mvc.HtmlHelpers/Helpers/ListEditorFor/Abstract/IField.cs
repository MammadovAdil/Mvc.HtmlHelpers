using System;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Fields of list editor for.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    public interface IField<TModel>
    {
        /// <summary>
        /// Set the header or label for the field.
        /// </summary>
        /// <param name="label">Label for the field.</param>
        /// <returns>Instance of IField.</returns>
        IField<TModel> WithLabel(string label);

        /// <summary>
        /// Render field without label.
        /// </summary>
        /// <returns>Instance of IField.</returns>
        IField<TModel> WithoutLabel();

        /// <summary>
        /// Render field as a caption.
        /// </summary>
        /// <returns>Instance of IField.</returns>
        IField<TModel> AsCaption();

        /// <summary>
        /// Render field as an image.
        /// </summary>
        /// <returns>Instance of IField.</returns>
        IField<TModel> AsImage();

        /// <summary>
        /// Render field only when condition is met.
        /// </summary>
        /// <param name="condition">Condition to chek to render the field.</param>
        /// <returns>Instance of IField.</returns>
        IField<TModel> When(Func<TModel, bool> condition);

        /// <summary>
        /// Render select editor for this field.
        /// </summary>
        /// <returns>Instance of IField as ISelectField.</returns>
        ISelectField<TModel> AsSelect();

        /// <summary>
        /// Render this field as hidden.
        /// </summary>
        /// <returns>Instance of IField as IHiddenField.</returns>
        IHiddenField<TModel> AsHidden();
    }
}
