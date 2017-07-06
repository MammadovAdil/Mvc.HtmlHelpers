using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class SecureForm
    {
        /// <summary>
        /// Begin form with Antiforgery token to the same controller and action.
        /// </summary>
        /// <remarks>
        /// Source:
        /// http://prideparrot.com/blog/archive/2012/7/securing_all_forms_using_antiforgerytoken
        /// </remarks>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <returns>MvcForm.</returns>
        public static MvcForm BeginSecureForm(
            this HtmlHelper htmlHelper)
        {
            // Get controller and action name
            string actionName = htmlHelper
                .ViewContext
                .RouteData
                .Values["action"]
                .ToString();

            string controllerName = htmlHelper
                .ViewContext
                .RouteData
                .Values["controller"]
                .ToString();

            return htmlHelper.BeginSecureForm(actionName, controllerName, null, null);
        }

        /// <summary>
        /// Begin form with Antiforgery token to the same controller.
        /// </summary>
        /// <remarks>
        /// Source:
        /// http://prideparrot.com/blog/archive/2012/7/securing_all_forms_using_antiforgerytoken
        /// </remarks>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="actionName">Name of action method to post to.</param>
        /// <returns>MvcForm.</returns>
        public static MvcForm BeginSecureForm(
            this HtmlHelper htmlHelper,
            string actionName)
        {
            // Get controller name
            string controllerName = htmlHelper
                .ViewContext
                .RouteData
                .Values["controller"]
                .ToString();

            return htmlHelper.BeginSecureForm(actionName, controllerName, null, null);
        }

        /// <summary>
        /// Begin form with Antiforgery token to the same controller.
        /// </summary>
        /// <remarks>
        /// Source:
        /// http://prideparrot.com/blog/archive/2012/7/securing_all_forms_using_antiforgerytoken
        /// </remarks>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="actionName">Name of action method to post to.</param>
        /// <param name="controllerName">Name of controller to post to.</param>
        public static MvcForm BeginSecureForm(
            this HtmlHelper htmlHelper,
            string actionName,
            string controllerName)
        {
            return htmlHelper.BeginSecureForm(actionName, controllerName, null, null);
        }

        /// <summary>
        /// Begin form with Antiforgery token to the same controller and action.
        /// </summary>
        /// <remarks>
        /// Source:
        /// http://prideparrot.com/blog/archive/2012/7/securing_all_forms_using_antiforgerytoken
        /// </remarks>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="htmlAttributes">Html attributes to add to form.</param>
        public static MvcForm BeginSecureForm(
            this HtmlHelper htmlHelper,
            object htmlAttributes)
        {
            // Get controller and action name
            string actionName = htmlHelper
                .ViewContext
                .RouteData
                .Values["action"]
                .ToString();

            string controllerName = htmlHelper
                .ViewContext
                .RouteData
                .Values["controller"]
                .ToString();

            return htmlHelper.BeginSecureForm(actionName, controllerName, null, htmlAttributes);
        }

        /// <summary>
        /// Begin form with Antiforgery token.
        /// </summary>
        /// <remarks>
        /// Source:
        /// http://prideparrot.com/blog/archive/2012/7/securing_all_forms_using_antiforgerytoken
        /// </remarks>
        /// <param name="htmlHelper">HtmlHelper.</param>
        /// <param name="actionName">Name of action to post.</param>
        /// <param name="controllerName">Name of controller to post.</param>
        /// <returns>MvcForm.</returns>
        public static MvcForm BeginSecureForm(
            this HtmlHelper htmlHelper,
            string actionName,
            string controllerName,
            object routeValues,           
            object htmlAttributes)
        {
            // Initialize route value dictionary
            RouteValueDictionary routeValueDictionary = routeValues == null
                ? new RouteValueDictionary()
                : new RouteValueDictionary(routeValues);

            // Generate Url for action
            string action = UrlHelper
                .GenerateUrl(
                    null, actionName, controllerName, routeValueDictionary,
                    htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);            

            // Get attributes
            RouteValueDictionary attributes = HtmlHelper
                .AnonymousObjectToHtmlAttributes(htmlAttributes);

            TagBuilder formBuilder = new TagBuilder("form");
            formBuilder.MergeAttribute("action", action);
            formBuilder.MergeAttribute("method", "POST", true);
            formBuilder.MergeAttributes(attributes);            

            var textWriter = htmlHelper.ViewContext.Writer;
            textWriter.Write(formBuilder.ToString(TagRenderMode.StartTag));
            textWriter.Write(htmlHelper.AntiForgeryToken().ToString());

            MvcForm form = new MvcForm(htmlHelper.ViewContext);
            return form;
        }
    }
}
