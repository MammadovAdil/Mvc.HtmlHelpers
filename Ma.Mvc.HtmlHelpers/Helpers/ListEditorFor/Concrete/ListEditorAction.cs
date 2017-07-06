using Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract;
using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Concrete
{
    /// <summary>
    /// List editor action.
    /// </summary>
    public class ListEditorAction<TModel>
        : IListEditorAction<TModel>,
        IListEditorListAction<TModel>,
        IListEditorItemAction<TModel>,
        IListEditorActionCore<TModel>
    {
        public string DefaultLinkCssClassFormat
        {
            get { return "list-data-action-link__{0}"; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="actionType">Type of action.</param>
        /// <param name="name">Action name.</param>
        public ListEditorAction(ListEditorActionType actionType, string name)
        {
            if (actionType == ListEditorActionType.NotSpecified)
                throw new InvalidOperationException(
                    "Type of action must be specified");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            ActionType = actionType;
            Name = name;
        }

        #region IListEditorActionCore members

        /// <summary>
        /// Type of action.
        /// </summary>
        public ListEditorActionType ActionType { get; private set; }

        /// <summary>
        /// Name of action.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Url to action.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Url generator to generate url for model.
        /// </summary>
        public Func<TModel, string> UrlGenerator { get; private set; }

        /// <summary>
        /// Function to decide if link should be generated for model.
        /// </summary>
        public Func<TModel, bool> RenderCondition { get; private set; }

        /// <summary>
        /// Label for action.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Html attributes for action.
        /// </summary>
        public RouteValueDictionary HtmlAttributes { get; private set; }

        /// <summary>
        /// Determine if should be rendered according to value of model.
        /// </summary>
        /// <param name="model">Model to check according to.</param>
        /// <returns>If action should be rendered according to value of model.</returns>
        public bool ShouldBeRendered(TModel model)
        {
            return RenderCondition == null
                || RenderCondition(model);
        }

        /// <summary>
        /// Generate URL according to model.
        /// </summary>
        /// <param name="model">Model to generate URL for.</param>
        /// <returns>Generated URL.</returns>
        public string GenerateUrl(TModel model)
        {
            return UrlGenerator(model);
        }

        /// <summary>
        /// Generate link.
        /// </summary>
        /// <returns>Generated link TagBuilder.</returns>
        public FluentTagBuilder GenerateLink()
        {
            if (string.IsNullOrEmpty(Url))
                throw new NullReferenceException(string.Format(
                    "{0} must be set to be able to use parameterless overload of {1} method.",
                    nameof(Url),
                    nameof(GenerateLink)));

            FluentTagBuilder linkTag = new FluentTagBuilder("a")
                .AddCssClass(string.Format(DefaultLinkCssClassFormat, Name))
                .MergeAttributes(HtmlAttributes)
                .MergeAttribute("href", Url)
                .SetInnerText(Label);

            return linkTag;
        }

        /// <summary>
        /// Generate link if for model if it should be rendered.
        /// </summary>
        /// <param name="model">Model to generate link for.</param>
        /// <returns>Generated link TagBuilder.</returns>
        public FluentTagBuilder GenerateLink(TModel model)
        {
            if (UrlGenerator == null)
                throw new NullReferenceException(string.Format(
                    "{0} must be set to be able to use parameterless overload of {1} method.",
                    nameof(UrlGenerator),
                    nameof(GenerateLink)));

            // If model is null or link should not be generated then return null.
            if (model == null
                || !ShouldBeRendered(model))
                return null;

            FluentTagBuilder linkTag = new FluentTagBuilder("a")
                .AddCssClass(string.Format(DefaultLinkCssClassFormat, Name))
                .MergeAttributes(HtmlAttributes)
                .MergeAttribute("href", GenerateUrl(model))
                .SetInnerText(Label);

            return linkTag;
        }

        #endregion

        #region IListEditorAction members

        /// <summary>
        /// Set the label for the action.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When label is null or empty.
        /// </exception>
        /// <param name="label">Label for the action.</param>
        /// <returns>Instance of IListEditorAction.</returns>
        public IListEditorAction<TModel> WithLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            Label = label;
            return this;
        }        

        /// <summary>
        /// Provide attributes for action element.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When htmlAttributes is null.
        /// </exception>
        /// <param name="htmlAttributes">Html attributes for action element.</param>
        /// <returns>Isntance of IListEditorAction</returns>
        public IListEditorAction<TModel> WithAttributes(object htmlAttributes)
        {
            if (htmlAttributes == null)
                throw new ArgumentNullException(nameof(htmlAttributes));

            HtmlAttributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);
            return this;
        }

        #endregion

        #region IListeEditorItemAction members

        /// <summary>
        /// Set url generator for action.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When urlGenerator is null.
        /// </exception>
        /// <param name="urlGenerator">Url generator for action.</param>
        /// <returns>Isntance of IListEditorAction.</returns>
        public IListEditorItemAction<TModel> WithUrl(Func<TModel, string> urlGenerator)
        {
            if (urlGenerator == null)
                throw new ArgumentNullException(nameof(urlGenerator));

            UrlGenerator = urlGenerator;
            return this;
        }

        /// <summary>
        /// Render action only when condition is met.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When condition is null.
        /// </exception>
        /// <param name="condition">Condition to chek to render the action.</param>
        /// <returns>Isntance of IListEditorAction.</returns>
        public IListEditorItemAction<TModel> When(Func<TModel, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            RenderCondition = condition;
            return this;
        }

        #endregion

        #region IListEditorListAction members

        /// <summary>
        /// Set url for action.
        /// </summary>
        /// <param name="url">Url to action.</param>
        /// <returns>Isntance of IListEditorListAction.</returns>
        public IListEditorListAction<TModel> WithUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            Url = url;
            return this;
        }

        #endregion
    }
}
