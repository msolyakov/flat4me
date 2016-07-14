using Flat4Me.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flat4Me.Web.App_Start
{
    // Configure the application sign-in manager which is used in this application.
    public class F4MeBossSignInManager : SignInManager<F4MeUser, int>
    {
        public F4MeBossSignInManager(F4MeBossUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(F4MeUser user)
        {
            return user.GenerateUserIdentityAsync((F4MeBossUserManager)UserManager);
        }

        public static F4MeBossSignInManager Create(IdentityFactoryOptions<F4MeBossSignInManager> options, IOwinContext context)
        {
            var manager = new F4MeBossSignInManager(context.GetUserManager<F4MeBossUserManager>(), context.Authentication);

            return manager;
        }
    }
}