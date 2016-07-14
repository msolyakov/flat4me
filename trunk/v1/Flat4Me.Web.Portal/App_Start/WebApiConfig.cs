using Flat4Me.Web.Portal.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Flat4Me.Web.Portal
{
    public static class WebApiConfig
    {
        public const string DefaultRoute = "DefaultApi";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services            
            config.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());
            config.Filters.Add(new ValidateModelAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
