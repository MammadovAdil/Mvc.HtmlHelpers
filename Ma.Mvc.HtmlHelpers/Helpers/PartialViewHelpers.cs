using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class PartialViewHelpers
    {
        /// <summary>
        /// Render partial view by keeping html prefix.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="expression">Expression to select model for partial view..</param>
        /// <param name="partialViewName">Name of partial view.</param>
        /// <returns>MvcHtmlString of partial view.</returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string partialViewName)
        {
            return helper.PartialFor(expression, partialViewName, null);
        }

        /// <summary>
        /// Render partial view by keeping html prefix.
        /// </summary>
        /// <remarks>
        /// According to answer at StackOVerflow, with few alterations.
        /// http://stackoverflow.com/a/6292180/1380428
        /// </remarks>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="helper">HtmlHelper.</param>
        /// <param name="expression">Expression to select model for partial view..</param>
        /// <param name="partialViewName">Name of partial view.</param>
        /// <param name="fieldPrefix">Field prefix to use.</param>
        /// <returns>MvcHtmlString of partial view.</returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string partialViewName,
            string fieldPrefix)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object model = ModelMetadata
                .FromLambdaExpression(expression, helper.ViewData)
                .Model;

            string newPrefix;

            // If prexit has been suppleid use it as new prefix,
            // otherwise initialize new prefix as {oldPrefix}.{name}
            if(!string.IsNullOrEmpty(fieldPrefix))
            {
                newPrefix = fieldPrefix;
            }
            else
            {
                // Get old prefix
                string oldPrefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
                newPrefix = !string.IsNullOrEmpty(oldPrefix)
                    ? string.Format("{0}.{1}", oldPrefix, name)
                    : name;
            }
            

            // Initialize new ViewData
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = newPrefix
                }
            };

            return helper.Partial(partialViewName, model, viewData);
        }
    }
}
