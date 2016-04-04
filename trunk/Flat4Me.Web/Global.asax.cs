using Flat4Me.Web.App_Start;
using Flat4Me.Web.Models;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Flat4Me.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // AreaRegistration.RegisterAllAreas(); // Became halted on IISExpress/Win7
            
            // Code that runs on application startup 
            // Should be first before all
            GlobalConfiguration.Configure(WebApiConfig.Register); // Became halted on IISExpress/Win7
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelMapperInitializer.Init();
        }
    }
}