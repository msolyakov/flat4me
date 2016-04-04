using Flat4Me.Data.Repository.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;

namespace Flat4Me.Web.Exceptions
{
    /// <summary>
    /// Global handler for Web Api exceptions
    /// </summary>
    public class ApiExceptionLogger : ExceptionLogger
    {
        public ILogRepository Logger
        {
            get
            {
                return DependencyResolver.Current.GetService<ILogRepository>();
            }
        }
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Logger.LogException(context.Exception, cancellationToken);
        }
    }
}