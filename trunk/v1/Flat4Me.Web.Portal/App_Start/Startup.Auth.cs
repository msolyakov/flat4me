using Duke.Owin.VkontakteMiddleware;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
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

namespace Flat4Me.Web.Portal
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<F4MeUserStore>(F4MeUserStore.Create);
            app.CreatePerOwinContext<F4MePortalUserManager>(F4MePortalUserManager.Create);
            app.CreatePerOwinContext<F4MePortalSignInManager>(F4MePortalSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            var cookieProvider = new CookieAuthenticationProvider
            {
                OnException = (cookieExceptionContext) =>
                {

                },
                // Uncomment when production
                // Enables the application to validate the security stamp when the user logs in.
                // This is a security feature which is used when you change a password or add an external login to your account.  
                OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<F4MePortalUserManager, F4MeUser, int>(
                    // User's roles and claims cookie valid save interval
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (claim) => claim.GetUserId<int>())
            };
            //// Remove when production
            //cookieProvider.OnValidateIdentity = async context =>
            //{
            //    var validateInterval = TimeSpan.FromMinutes(30);

            //    DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            //    if (context.Options != null && context.Options.SystemClock != null)
            //    {
            //        utcNow = context.Options.SystemClock.UtcNow;
            //    }
            //    DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;
            //    bool hasValue = !issuedUtc.HasValue;
            //    if (issuedUtc.HasValue)
            //    {
            //        hasValue = utcNow.Subtract(issuedUtc.Value) > validateInterval;
            //    }
            //    if (hasValue)
            //    {
            //        var userManager = context.OwinContext.GetUserManager<F4MePortalUserManager>();
            //        int identity = context.Identity.GetUserId<int>();
            //        if (userManager != null && identity != null)
            //        {
            //            var tUser = await userManager.FindByIdAsync(identity);
            //            bool flag = true;
            //            if (tUser != null && userManager.SupportsUserSecurityStamp)
            //            {
            //                if (IdentityExtensions.FindFirstValue(context.Identity, "AspNet.Identity.SecurityStamp") == await userManager.GetSecurityStampAsync(identity))
            //                {
            //                    flag = false;
            //                    var claimsIdentity = await tUser.GenerateUserIdentityAsync(userManager);
            //                    if (claimsIdentity != null)
            //                    {
            //                        context.Properties.IssuedUtc  = null;
            //                        context.Properties.ExpiresUtc = null;
            //                        context.OwinContext.Authentication.SignIn(context.Properties, new ClaimsIdentity[] { claimsIdentity });
            //                    }
            //                }
            //            }
            //            if (flag)
            //            {
            //                context.RejectIdentity();
            //                context.OwinContext.Authentication.SignOut(new string[] { context.Options.AuthenticationType });
            //            }
            //        }
            //    }
            //};

            var cookieOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = cookieProvider,
                // When expired user should login again
                //ExpireTimeSpan = TimeSpan.FromSeconds(30)
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