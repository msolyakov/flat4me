using Flat4Me.Web.Portal.Exceptions;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Web.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
