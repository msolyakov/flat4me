using Flat4Me.Core.Exceptions;
using Flat4Me.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Web.Exceptions
{
    /// <summary>
    /// Global handler for mvc excepitions 
    /// </summary>
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public ILogRepository Logger
        {
            get
            {
                return DependencyResolver.Current.GetService<ILogRepository>();
            }
        }        

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            var isUserException = filterContext.Exception is UserException;

            var userMessage = isUserException ? filterContext.Exception.Message : "Произошла системная ошибка. Пожалуйста, обратитесь к администратору..";

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentEncoding = Encoding.Unicode,
                    Data = new
                    {
                        hasError = true,
                        message = userMessage
                    }
                };
            }
            else
            {
                var viewData = new ViewDataDictionary();
                viewData.Add("ErrorMessage", userMessage);

                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = viewData,
                };
            }

            // Handle only system exceptions
            if (!isUserException)            
                Logger.LogException(userMessage, filterContext.Exception);
            

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}