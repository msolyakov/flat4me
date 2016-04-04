using Duke.Owin.VkontakteMiddleware;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Data.Repository.MsSql;
using Flat4Me.Identity;
using Flat4Me.Web.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flat4Me.Web.Boss
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<F4MeUserStore>(F4MeUserStore.Create);
            app.CreatePerOwinContext<F4MeBossUserManager>(F4MeBossUserManager.Create);
            app.CreatePerOwinContext<F4MeBossSignInManager>(F4MeBossSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            var cookieProvider = new CookieAuthenticationProvider
            {
                OnException = (cookieExceptionContext) =>
                {

                },
                // Enables the application to validate the security stamp when the user logs in.
                // This is a security feature which is used when you change a password or add an external login to your account.  
                OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<F4MeBossUserManager, F4MeUser, int>(
                    // User's roles and claims cookie valid save interval
                    validateInterval: TimeSpan.FromMinutes(0),
                    regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (claim) => claim.GetUserId<int>())
            };

            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = cookieProvider,
                // When expired user should login again
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            };
            app.UseCookieAuthentication(cookieOptions);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // LIVE ID
            var microsoftAuth = new MicrosoftAccountAuthenticationOptions
            {
                ClientId = "0000000048152AD2",
                ClientSecret = "7IQOyA30-Q4hd8QJ6dpPe-O2nxRUBMzP"
            };

            microsoftAuth.Scope.Add("wl.emails");
            app.UseMicrosoftAccountAuthentication(microsoftAuth);

            // TWITTER
            var twitterAuth = new TwitterAuthenticationOptions
            {
                ConsumerKey = "zh0X3SjpDGDumxeE5jWqpXRgS",
                ConsumerSecret = "NA2uoGvtymAbSULoNqzlLLss9vR2Ec8CCKbm7QOsTa3QYL139b"
            };
            app.UseTwitterAuthentication(twitterAuth);

            // FACEBOOK
            var facebookAuth = new FacebookAuthenticationOptions
            {
                AppId = "1412435339074437",
                AppSecret = "8bb8dafa713a40e8eacca18b6bdde630"
            };
            facebookAuth.Scope.Add("email");
            app.UseFacebookAuthentication(facebookAuth);

            // GOOGLE
            var googleAuth = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "274037637904-dauic051ikvtul7dn2ggtstjsnj8k0mh.apps.googleusercontent.com",
                ClientSecret = "yKzzjBZfLJnl-Fr3lI8NTOOi"
            };
            googleAuth.Scope.Add(@"https://www.googleapis.com/auth/userinfo.email");
            app.UseGoogleAuthentication(googleAuth);

            // VKONTAKTE
            var vkAuth = new VkAuthenticationOptions
            {
                AppId = "4880937",
                AppSecret = "2Wyemj3BNg07xV7siEmC",
                Scope = "email"
            };
            app.UseVkontakteAuthentication(vkAuth);
        }
    }
}