using Flat4Me.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Security.Claims;

namespace Flat4Me.Web.Portal.App_Start
{
    public class F4MePortalUserManager : F4MeUserManager
    {
        public F4MePortalUserManager(F4MeUserStore store)
            : base(store)
        {
        }

        public static F4MePortalUserManager Create(IdentityFactoryOptions<F4MePortalUserManager> options, IOwinContext context)
        {
            var manager = new F4MePortalUserManager(context.Get<F4MeUserStore>());
            // Configure validation logic for usernames
            manager.UserValidator = new F4MeUserValidator<F4MeUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(1);
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser, int>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser, int>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            manager.EmailService = new F4MeEmailService();
            manager.SmsService = new F4MeSmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                var protector = new DataProtectorTokenProvider<F4MeUser, int>(dataProtectionProvider.Create("F4MeWebPortal"));
                // Email token and Phone PIN valid lifespan
                protector.TokenLifespan = TimeSpan.FromDays(1);
                manager.UserTokenProvider = protector;
            }
            return manager;
        }
    }
}