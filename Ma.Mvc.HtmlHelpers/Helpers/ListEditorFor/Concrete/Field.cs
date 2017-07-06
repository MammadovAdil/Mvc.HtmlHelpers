using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class Field<TModel, TProperty>
        : IFieldCore<TModel>,
        IField<TModel>,
        ISelectField<TModel>,
        IHiddenField<TModel>
    {
        /// <summary>
        /// Type of editor to render.
        /// </summary>
        public EditorType EditorType { get; set; }

        /// <summary>
        /// Header or label of item.
        /// </summary>
        public string FieldLabel { get; set; }

        /// <summary>
        /// Should be rendered without label.
        /// </summary>
        public bool IsWithoutLabel { get; set; }

        /// <summary>
        /// Should be rendered as a caption.
        /// </summary>
        public bool IsAsCaption { get; set; }

        /// <summary>
        /// Should be rendered as image.
        /// </summary>
        public bool IsAsImage { get; set; }

        /// <summary>
        /// Data source for field to select from while editing.
        /// </summary>
        public IEnumerable<SelectListItem> FieldSource { get; set; }

        /// <summary>
        /// Html helper to generate Display, Editor and Hidden for field.
        /// </summary>
        public HtmlHelper<TModel> Helper { get; set; }

        /// <summary>
        /// Lambda expression to get data according to property.
        /// </summary>
        public Expression<Func<TModel, TProperty>> Expression { get; set; }

        /// <summary>
        /// Condition to check while rendering field.
        /// </summary>
        private Func<TModel, bool> RenderCondition { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="helper">Html helper to generate elements for field.</param>
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>
        public Field(
            HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            /// Store html helper and expressin. Compile expression
            /// beforhand for perfermance.
            Helper = helper;
            Expression = expression;
        }


        /// <summary>
        /// Set the header or label for the field.
        /// </summary>
        /// <param name="label">Label for the field.</param>
        /// <returns>Instance of Field.</returns>
        public IField<TModel> WithLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            FieldLabel = label;
            return this;
        }

        /// <summary>
        /// Render field without label.
        /// </summary>
        /// <returns>Instance of </returns>
        public IField<TModel> WithoutLabel()
        {
            IsWithoutLabel = true;
            return this;
        }

        /// <summary>
        /// Render field as a caption.
        /// </summary>
        /// <returns>nstance of IField.</returns>
        public IField<TModel> AsCaption()
        {
            IsAsCaption = true;
            return this;
        }

        /// <summary>
        /// Render field as an image.
        /// </summary>
        /// <returns>Instance of IField.</returns>
        public IField<TModel> AsImage()
        {
            IsAsImage = true;
            return this;
        }

        /// <summary>
        /// Render field only when condition is met.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When condition is null.
        /// </exception>
        /// <param name="condition">Condition to chek to render the field.</param>
        /// <returns>Instance of IField.</returns>
        public IField<TModel> When(Func<TModel, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            RenderCondition = condition;
            return this;
        }

        /// <summary>
        /// Render select editor for this field.
        /// </summary>
        /// <returns>Instance of ListItem as ISelectField.</returns>
        public ISelectField<TModel> AsSelect()
        {
            EditorType = EditorType.Select;
            return this;
        }

        /// <summary>
        /// Set data source for select field.
        /// </summary>
        /// <param name="dataSource">Data as source for field.</param>
        /// <returns>Instance of Field as ISelectField.</returns>
        public ISelectField<TModel> WithDataSource(
            IEnumerable<SelectListItem> dataSource)
        {
            if (dataSource == null
                || dataSource.Count() == 0)
                throw new ArgumentNullException(nameof(dataSource));

            FieldSource = dataSource;
            return this;
        }

        /// <summary>
        /// Render this field as hidden.
        /// </summary>
        /// <returns>Instance of IField as IHiddenField.</returns>
        public IHiddenField<TModel> AsHidden()
        {
            EditorType = EditorType.Hidden;
            return this;
        }

        /// <summary>
        /// Check if field should be rendered.
        /// </summary>
        /// <param name="model">Model to check against.</param>
        /// <returns>If field should be rendered.</returns>
        public bool ShouldBeRendered(TModel model)
        {
            return RenderCondition == null
                || RenderCondition(model);
        }

        /// <summary>
        /// Get the property value from the model object.
        /// </summary>
        /// <param name="model">Model to get value of property from.</param>
        /// <returns>Property value from the model.</returns>
        public string Evaluate(TModel model)
        {
            Func<TModel, TProperty> compiledExpression = Expression.Compile();
            TProperty result = compiledExpression(model);

            return result == null ? string.Empty : result.ToString();
        }

        /// <summary>
        /// Get label for current field.
        /// </summary>
        /// <returns>Label for current field.</returns>
        public MvcHtmlString LabelFor()
        {
            if (!string.IsNullOrEmpty(FieldLabel))
                return Helper.LabelFor(Expression, FieldLabel);
            else
                return Helper.LabelFor(Expression);
        }

        /// <summary>
        /// Get display for current field.
        /// </summary>
        /// <returns>Display for current field.</returns>
        public MvcHtmlString DisplayFor()
        {
            return Helper.DisplayFor(Expression);
        }

        /// <summary>
        /// Get editor for current field.
        /// </summary>
        /// <returns>Editor for current field.</returns>
        public MvcHtmlString EditorFor()
        {
            MvcHtmlString editor = null;

            var defaultModel = Helper.ViewData.Model;

            switch(EditorType)
            {
                case EditorType.TextBox:
                    editor = Helper.TextBoxFor(Expression);
                    break;
                case EditorType.Select:
                    editor = Helper.DropDownListFor(Expression, FieldSource);
                    break;
                case EditorType.Hidden:
                    editor = Helper.HiddenFor(Expression);
                    break;
                default:
                    editor = Helper.EditorFor(Expression);
                    break;
            }            

            return editor;
        }

        /// <summary>
        /// Get hidden for current field.
        /// </summary>
        /// <returns>Hidden for current field.</returns>
        public MvcHtmlString HiddenFor()
        {
            var defaultModel = Helper.ViewData.Model;

            return Helper.HiddenFor(Expression);
        }
    }
}