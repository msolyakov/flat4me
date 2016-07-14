using System.Web.Mvc;

namespace Flat4Me.Web.Extensions
{
    public static class UrlHelperExtension
    {
        /// <summary>
        /// Returns a full qualified action URL
        /// </summary>
        public static string QualifiedAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}
