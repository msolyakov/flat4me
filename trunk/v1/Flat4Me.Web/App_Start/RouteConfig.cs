using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Flat4Me.Web
{
    public class RouteConfig
    {
        /// <summary>
        /// Значение - "{city}"
        /// </summary>
        public const string URL_PATTERN_CITY = "{city}";
        
        /// <summary>
        /// Значение - "{city}/flat/{id}"
        /// </summary>
        public const string URL_PATTERN_FLAT = "{city}/flat/{id}";
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "defaultManage",
                url: "manage",
                defaults: new { controller = "manage", action = "index" });

            routes.MapRoute(
                name: "defaultSitemap",
                url: "sitemap",
                defaults: new { controller = "sitemap", action = "index" });
            
            routes.MapRoute(
                name: "defaultCity",
                url: URL_PATTERN_CITY,
                defaults: new { controller = "home", action = "city" });

            routes.MapRoute(
                name: "defaultFlat",
                url: URL_PATTERN_FLAT,
                defaults: new { controller = "home", action = "flat" });
            
            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);            
        }
    }
}
