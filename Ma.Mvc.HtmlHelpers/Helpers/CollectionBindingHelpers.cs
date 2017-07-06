using Ma.Mvc.HtmlHelpers.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    /// <summary>
    /// Helpers for MVC collection binding.
    /// </summary>
    /// <remarks>
    /// Hidden indexer helpers was created by Adil Mammadov,
    /// according to following web post:
    /// http://haacked.com/archive/2008/10/23/model-binding-to-a-list.aspx/.
    /// 
    /// Begin collection item helpers created by Adil Mammadov,
    /// according to following web post:
    /// http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/.
    /// </remarks>
    public static class CollectionBindingHelpers
    {
        #region HiddenIndexer helpers

        /// <summary>
        /// Collection group name for regex.
        /// </summary>
        private static string CollectionGroupName
        {
            get { return "collectionName"; }
        }

        /// <summary>
        /// Group name of index for regex.
        /// </summary>
        private static string CollectionIndexGroupName
        {
            get { return "collectionIndex"; }
        }

        /// <summary>
        /// Regex for html prefix.
        /// </summary>
        private static Regex htmlCollectionPrefixRegex = new Regex(string.Format(
            "^(?<{0}>.*)\\[(?<{1}>.*)\\]$",
            CollectionGroupName,
            CollectionIndexGroupName), RegexOptions.Compiled);

        /// <summary>
        /// Get collection name from TemplateInfo.
        /// </summary>
        /// <param name="templateInfo">TemplateInfo to get collection name from.</param>
        /// <returns>Name of html collection prefix.</returns>
        private static string GetCollectionName(this TemplateInfo templateInfo)
        {
            Match match = htmlCollectionPrefixRegex.Match(templateInfo.HtmlFieldPrefix);
            string collectionName = null;

            if (match.Success)
                collectionName = match.Groups[CollectionGroupName].Value;

            return collectionName;
        }

        /// <summary>
        /// Get collection index from TemplateInfo.
        /// </summary>
        /// <param name="templateInfo">TemplateInfo to get collection name from.</param>
        /// <returns>Index of html collection prefix.</returns>
        private static string GetCollectionIndex(this TemplateInfo templateInfo)
        {
            Match match = htmlCollectionPrefixRegex.Match(templateInfo.HtmlFieldPrefix);
            string collectionIndex = null;

            if (match.Success)
                collectionIndex = match.Groups[CollectionIndexGroupName].Value;

            return collectionIndex;
        }

        /// <summary>
        /// Generates hidden indexer input for model.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>        
        /// <returns>Indexer input for model.</returns>
        public static MvcHtmlString HiddenIndexerForModel<TModel>(
            this HtmlHelper<TModel> helper)
        {
            return helper.HiddenIndexerForModel(null);
        }

        /// <summary>
        /// Generates hidden indexer input for model according to template info.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>        
        /// <param name="htmlAttributes">Html attributes to attach to input.</param>
        /// <returns>Indexer input for model.</returns>
        public static MvcHtmlString HiddenIndexerForModel<TModel>(
            this HtmlHelper<TModel> helper,
            object htmlAttributes)
        {
            // Get collection name and index from template info
            TemplateInfo templateInfo = helper.ViewData.TemplateInfo;
            string collectionName = templateInfo.GetCollectionName();
            string collectionIndex = templateInfo.GetCollectionIndex();

            return helper.HiddenIndexerForModel(
                collectionName, 
                collectionIndex, 
                htmlAttributes);
        }

        /// <summary>
        /// Generates hidden indexer input for model.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>        
        /// <param name="collectionName">Name of collection.</param>
        /// <param name="collectionIndex">Value of index.</param>   
        /// <param name="htmlAttributes">Html attributes to attach to input.</param>
        /// <returns>Indexer input for model.</returns>
        public static MvcHtmlString HiddenIndexerForModel<TModel>(
            this HtmlHelper<TModel> helper,
            string collectionName,
            string collectionIndex,
            object htmlAttributes)
        {
            // Convert html attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            // Construct name for input
            string inputName = string.Format("{0}.index", collectionName);

            // Generate input element
            FluentTagBuilder hiddenIndexer = new FluentTagBuilder("input")
                .MergeAttribute("name", inputName)
                .MergeAttribute("type", "hidden")
                .MergeAttribute("value", helper.Encode(collectionIndex))
                .MergeAttribute("autocomplete", "off")
                .MergeAttributes(attributes);

            return MvcHtmlString.Create(hiddenIndexer.Render());
        }

        #endregion

        #region BeginCollectionItem helpers

        /// <summary>
        /// Get currently used Ids to reuse if there is any.
        /// </summary>
        /// <remarks>
        /// We need to use the same sequence of IDs following a server-side validation failure,
        /// otherwise the framework won't render the validation error messages next to each item.
        /// </remarks>
        /// <param name="httpContext">Current http context.</param>
        /// <param name="colletionName">Name of collection.</param>
        /// <returns>Queue of currently used Ids.</returns>
        private static Queue<string> GetCurrentIds(
            HttpContextBase httpContext,
            string colletionName)
        {
            // Construct key
            string key = string.Format(
                "_collectionBindingHelpers_IdsToReuse_{0}",
                colletionName);

            // Get queue from http context
            Queue<string> queue = httpContext.Items[key] as Queue<string>;
            if (queue == null)
            {
                // If queue does not exist in the context
                // create new one and add to context
                httpContext.Items[key] = queue = new Queue<string>();

                // Try to get currently used Ids from form
                string currentlyUsedIds = httpContext
                    .Request
                    .Form[colletionName + ".index"];

                // Split Ids and add them to queue
                if (!string.IsNullOrEmpty(currentlyUsedIds))
                    foreach (string currentlyUsedId in currentlyUsedIds.Split(','))
                        queue.Enqueue(currentlyUsedId);
            }

            return queue;
        }

        /// <summary>
        /// Generate index and add it to input elements.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="collectionName">Name of collection, must be same at posted action method.</param>
        /// <returns>IDisposable.</returns>
        public static IDisposable BeginCollectionItem<TModel>(
            this HtmlHelper<TModel> helper,
            string collectionName)
        {
            return helper.BeginCollectionItem(collectionName, true);
        }

        /// <summary>
        /// Generate index and add it to input elements.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="collectionName">Name of collection, must be same at posted action method.</param>
        /// <param name="generateHiddenIndexer">Generate hidden indexer input and write it to html if set to true.</param>
        /// <returns>IDisposable.</returns>
        internal static IDisposable BeginCollectionItem<TModel>(
            this HtmlHelper<TModel> helper,
            string collectionName,
            bool generateHiddenIndexer)
        {
            Queue<string> currentIds = GetCurrentIds(
                helper.ViewContext.HttpContext, collectionName);

            // If any id exist in the http context use it,
            // otherwise create new id.
            string itemIndex = currentIds.Count > 0
                ? currentIds.Dequeue()
                : Guid.NewGuid().ToString();

            // Generate and write hidden indexer to view if needed            
            if (generateHiddenIndexer)
            {
                MvcHtmlString hiddenIndexer = helper.HiddenIndexerForModel(
                    collectionName, 
                    itemIndex, 
                    null);

                TextWriter writer = helper.ViewContext.Writer;
                writer.WriteLine(hiddenIndexer.ToHtmlString());
            }

            // Construct html prefix
            string htmlFieldPrefix = string.Format("{0}[{1}]", collectionName, itemIndex);

            // Begin field prefix scope
            return helper.BeginHtmlFieldPrefixScope(htmlFieldPrefix);
        }

        /// <summary>
        /// Begin scope of HtmlFieldPrefix.
        /// </summary>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="htmlFieldPrefix">Prefix to add input fields.</param>
        /// <returns>IDisposable.</returns>
        internal static IDisposable BeginHtmlFieldPrefixScope<TModel>(
            this HtmlHelper<TModel> helper,
            string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(helper.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        internal class HtmlFieldPrefixScope : IDisposable
        {
            internal TemplateInfo TemplateInfo { get; set; }
            internal string PreviousHtmlFieldPrefix { get; set; }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="templateInfo">TemplateInfo.</param>
            /// <param name="htmlFieldPrefix">Prefix to add to input elements.</param>
            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                TemplateInfo = templateInfo;

                // Store current prefix for use in the future.
                PreviousHtmlFieldPrefix = TemplateInfo.HtmlFieldPrefix;
                TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            /// <summary>
            /// Reset HtmlFieldPrefix to previous value.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    // Free managed resources
                    TemplateInfo.HtmlFieldPrefix = PreviousHtmlFieldPrefix;
                }
            }
        }

        #endregion
    }
}