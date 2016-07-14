using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Linq;

namespace Flat4Me.Identity
{
    /// <summary>
    /// Supports Claim auth
    /// </summary>
    public class F4MeWebApiAuthorizeAttribute : AuthorizeAttribute
    {
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>())
            {
                return true;
            }
            return actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>();
        }

        public async override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
            if (actionContext.Response != null)
            {
                return;
            }

            var user = System.Web.HttpContext.Current.User as ClaimsPrincipal;

            if (user != null && string.IsNullOrEmpty(ClaimType) == false && string.IsNullOrEmpty(ClaimValue) == false)
            {
                if (user.HasClaim(ClaimType, ClaimValue) == false)
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                }
            }
        }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
