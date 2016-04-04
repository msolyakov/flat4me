using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Identity
{
    /// <summary>
    /// Supports Claim auth
    /// </summary>
    public class F4MeAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {            
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            var user = HttpContext.Current.User as ClaimsPrincipal;
            var result = true;

            if (string.IsNullOrEmpty(ClaimType) == false && string.IsNullOrEmpty(ClaimValue) == false)
            {
                result = user.HasClaim(ClaimType, ClaimValue);
            }

            return result;
        }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
