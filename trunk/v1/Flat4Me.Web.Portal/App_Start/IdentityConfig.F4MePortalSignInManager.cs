using Flat4Me.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flat4Me.Web.Portal.App_Start
{
    // Configure the application sign-in manager which is used in this application.
    public class F4MePortalSignInManager : SignInManager<F4MeUser, int>
    {
        public F4MePortalSignInManager(F4MePortalUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(F4MeUser user)
        {           
            return user.GenerateUserIdentityAsync((F4MePortalUserManager)UserManager);
        }

        public static F4MePortalSignInManager Create(IdentityFactoryOptions<F4MePortalSignInManager> options, IOwinContext context)
        {            
            return new F4MePortalSignInManager(context.GetUserManager<F4MePortalUserManager>(), context.Authentication);
        }       
    }
}