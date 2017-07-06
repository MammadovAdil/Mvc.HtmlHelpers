using Ma.Mvc.HtmlHelpers.Infrastructure;
using Ma.Mvc.HtmlHelpers.Models;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class TreeViewHelpers
    {
        /// <summary>
        /// Construct tree view for selected property.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of selected property.</typeparam>
        /// <param name="helper">Html helper.</param>
        /// <param name="expression">Expression which selects property.</param>
        /// <param name="source">Source for tree view.</param>
        /// <param name="sourceUrl">Url to get child items using Ajax.</param>
        /// <param name="searchExpression">Search expression to filter leaves.</param>
        /// <returns>Constructed Tree view.</returns>
        public static MvcHtmlString TreeViewFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,            
            TreeViewSource<TProperty> source,
            string sourceUrl,
            string searchExpression)
            where TProperty : struct
        {
            // CSS classes
            string treeViewCssClass = "tree-view";
            string levelCssClassFormat = "level-{0}";
            string errorCssClass = string.Format("{0}__error", treeViewCssClass);
            string rootListCssClass = "root-list";

            // Attribute keys
            string levelAttribute = "data-tv-level";
            string forAttribute = "data-tv-for";
            string sourceUrlAttribute = "data-tv-source-url";
            string searchExpresionAttribute = "data-tv-search-exp";

            // Get model name from expression
            string modelName = ExpressionHelper.GetExpressionText(expression);

            // Construct root list
            FluentTagBuilder rootList = helper.ConstructList(expression, source)
                .AddCssClass(string.Format(levelCssClassFormat, "0"))
                .AddCssClass(rootListCssClass)
                .MergeAttribute(levelAttribute, "0");

            // Construct tree view container
            FluentTagBuilder treeViewContainer = new FluentTagBuilder("div")
                .AppendChild(rootList)
                .MergeAttribute(forAttribute, modelName)
                .MergeAttribute(sourceUrlAttribute, sourceUrl)
                .MergeAttribute(searchExpresionAttribute, searchExpression)
                .AddCssClass(treeViewCssClass);

            // Add error
            MvcHtmlString validationMessage = helper.ValidationMessageFor(expression);
            FluentTagBuilder errorSpan = new FluentTagBuilder("span")
                .AddCssClass(errorCssClass)
                .AppendChild(validationMessage);
            treeViewContainer.AppendChild(errorSpan);

            return new MvcHtmlString(treeViewContainer.Render());
        }

        /// <summary>
        /// Construct list for tree view.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of selected property.</typeparam>
        /// <param name="helper">Html helper.</param>
        /// <param name="expression">Expression which selects property.</param>
        /// <param name="source">Soruce of tree view.</param>
        /// <returns>Tree view list as FluentTagBuilder.</returns>
        private static FluentTagBuilder ConstructList<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            TreeViewSource<TProperty> source)
            where TProperty : struct
        {
            // CSS classes
            string listCssClass = "tree-view-list";
            string loadedCssClass = "tv-loaded";

            // Construct tree view list
            FluentTagBuilder treeViewList = new FluentTagBuilder("ul")
                .AddCssClass(listCssClass);

            // If there is not any item return
            if (source == null 
                || source.ItemList == null 
                || source.ItemList.Count == 0)
                return treeViewList;

            // If source is not empty add class
            treeViewList.AddCssClass(loadedCssClass);

            // Construct list items and add them to treeViewList
            foreach (TreeViewItem<TProperty> listItemSource in source.ItemList)
            {
                FluentTagBuilder listItem = helper
                    .ConstructListItem(expression, listItemSource);
                treeViewList.AppendChild(listItem);
            }

            return treeViewList;
        }

        /// <summary>
        /// Construct list item for tree view.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When listItemSource is null.
        /// </exception>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="helper">Html helper.</param>
        /// <param name="expression">Expression which selects property.</param>
        /// <param name="listItemSource">Source for list item.</param>
        /// <returns>Tree view list item as FluentTagBuilder.</returns>
        private static FluentTagBuilder ConstructListItem<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            TreeViewItem<TProperty> listItemSource)
            where TProperty : struct
        {
            if (listItemSource == null)
                throw new ArgumentNullException(nameof(FluentTagBuilder));

            // CSS classes
            string listItemCssClass = "tree-view-list-item";
            string levelCssClassFormat = "level-{0}";
            string contentContainerCssClass = "content-container";

            // Attribute keys
            string levelAttribute = "data-tv-level";

            // Construct icon
            FluentTagBuilder icon = new FluentTagBuilder("i")
                .AddCssClass("default-icons")
                .SetInnerHtml("keyboard_arrow_right");

            // Construct span
            FluentTagBuilder span = new FluentTagBuilder("span")
                .SetInnerText(listItemSource.Value);

            // Construct radio button
            MvcHtmlString radioButton = helper
                .RadioButtonFor(expression, listItemSource.Id, new { hidden = "hidden" });

            // Construct content container
            FluentTagBuilder contentContainer = new FluentTagBuilder("div")
                .AddCssClass(contentContainerCssClass)
                .AppendChild(icon)
                .AppendChild(span)
                .AppendChild(radioButton);

            // Construct child view, add level class and attribute
            FluentTagBuilder childView = helper
                .ConstructList(expression, listItemSource.ChildViewSource)
                .AddCssClass(string.Format(levelCssClassFormat, listItemSource.Level))
                .MergeAttribute(levelAttribute, listItemSource.Level.ToString())
                .MergeAttribute("hidden", "hidden");

            // Construct list item
            FluentTagBuilder listItem = new FluentTagBuilder("li")
                .AppendChild(contentContainer)
                .AppendChild(childView)
                .AddCssClass(listItemCssClass);

            return listItem;
        }
    }
}
