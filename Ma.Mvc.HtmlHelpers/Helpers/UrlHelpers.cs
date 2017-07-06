using System.Web.Mvc;
using System.Web.Routing;

namespace Ma.Mvc.HtmlHelpers.Helpers
{
    public static class UrlHelpers
    {
        /// <summary>
        /// Construct action with persistent query string.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instanse.</param>
        /// <param name="actionName">Name of action.</param>
        /// <param name="routeValues">Route values to add to action.</param>
        /// <returns>Constructed action.</returns>
        public static string PersistentQueryStringAction(
            this UrlHelper urlHelper,
            string actionName,
            object routeValues)
        {
            string controllerName = urlHelper
                .RequestContext
                .RouteData
                .Values["controller"]
                .ToString();

            return urlHelper.PersistentQueryStringAction(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Construct action with persistent query string.
        /// </summary>
        /// <param name="urlHelper">UrlHelper instanse.</param>
        /// <param name="actionName">Name of action.</param>
        /// <param name="controllerName">Name of controller.</param>
        /// <param name="routeValues">Route values to add to action.</param>
        /// <returns>Constructed action.</returns>
        public static string PersistentQueryStringAction(
            this UrlHelper urlHelper,
            string actionName,
            string controllerName,
            object routeValues)
        {
            var request = urlHelper.RequestContext.HttpContext.Request;            

            RouteValueDictionary mergedRouteValues = 
                new RouteValueDictionary(routeValues);

            // Add values from query string to persist them.
            foreach (string key in request.QueryString.Keys)
            {
                mergedRouteValues.Add(key, request.QueryString[key]);
            }

            string action = urlHelper
                .Action(actionName, controllerName, mergedRouteValues);

            return action;
        }
    }
}
