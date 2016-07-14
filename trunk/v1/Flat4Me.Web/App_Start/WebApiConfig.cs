using Flat4Me.Web.App_Start;
using Flat4Me.Web.Exceptions;
using Newtonsoft.Json.Converters;
using Ninject;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Flat4Me.Web
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
                name: DefaultRoute,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
