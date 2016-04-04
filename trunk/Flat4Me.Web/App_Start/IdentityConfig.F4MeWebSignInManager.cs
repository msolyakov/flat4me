using Flat4Me.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flat4Me.Web.App_Start
{
    // Configure the application sign-in manager which is used in this application.
    public class F4MeWebSignInManager : SignInManager<F4MeUser, int>
    {
        public F4MeWebSignInManager(F4MeWebUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(F4MeUser user)
        {
            return user.GenerateUserIdentityAsync((F4MeWebUserManager)UserManager);
        }

        public static F4MeWebSignInManager Create(IdentityFactoryOptions<F4MeWebSignInManager> options, IOwinContext context)
        {
            var manager = new F4MeWebSignInManager(context.GetUserManager<F4MeWebUserManager>(), context.Authentication);
            
            return manager;
        }
    }
}