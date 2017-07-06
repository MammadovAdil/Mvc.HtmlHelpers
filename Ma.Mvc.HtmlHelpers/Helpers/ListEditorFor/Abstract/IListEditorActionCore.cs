using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models.Enums;
using System;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers.ListEditorFor.Abstract
{
    /// <summary>
    /// Data container for list editor action.
    /// </summary>
    /// <typeparam name="TModel">Type of view model.</typeparam>
    internal interface IListEditorActionCore<TModel>
    {
        /// <summary>
        /// Type of action.
        /// </summary>
        ListEditorActionType ActionType { get; }

        /// <summary>
        /// Name of action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Label for action.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Html attributes for action.
        /// </summary>
        RouteValueDictionary HtmlAttributes { get; }

        /// <summary>
        /// Url to action.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Url generator to generate url for model.
        /// </summary>
        Func<TModel, string> UrlGenerator { get; }

        /// <summary>
        /// Function to decide if link should be generated for model.
        /// </summary>
        Func<TModel, bool> RenderCondition { get; }

        /// <summary>
        /// Determine if should be rendered according to value of model.
        /// </summary>
        /// <param name="model">Model to check according to.</param>
        /// <returns>If action should be rendered according to value of model.</returns>
        bool ShouldBeRendered(TModel model);

        /// <summary>
        /// Generate URL according to model.
        /// </summary>
        /// <param name="model">Model to generate URL for.</param>
        /// <returns>Generated URL.</returns>
        string GenerateUrl(TModel model);

        /// <summary>
        /// Generate link.
        /// </summary>
        /// <returns>Generated link TagBuilder.</returns>
        FluentTagBuilder GenerateLink();

        /// <summary>
        /// Generate link for model if it should be rendered.
        /// </summary>
        /// <param name="model">Model to generate link for.</param>
        /// <returns>Generated link TagBuilder.</returns>
        FluentTagBuilder GenerateLink(TModel model);
    }
}
