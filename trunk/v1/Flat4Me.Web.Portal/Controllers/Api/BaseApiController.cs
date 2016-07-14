using Flat4Me.Web.Portal.App_Start;
using Flat4Me.Data.Repository.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Flat4Me.Identity;

namespace Flat4Me.Web.Portal.Controllers.Api
{
    public abstract class BaseApiController : ApiController
    {
        private ILogRepository _logger = null;
        private F4MePortalSignInManager _signInManager;
        private F4MePortalUserManager _userManager;

        public ILogRepository Logger
        {
            get
            {
                // Resolving only when called
                return _logger ?? (_logger = DependencyResolver.Current.GetService<ILogRepository>());
            }
        }
        public F4MePortalSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<F4MePortalSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public F4MePortalUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<F4MePortalUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public int UserId
        {
            get
            {
                return User.Identity.GetUserId<int>();
            }
        }
        public Task<F4MeUser> CurrentUser()
        {
            return UserManager.FindByIdAsync(UserId);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            // Set curren culture to RU
            var langCode = "ru";

            var ci = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            base.Initialize(controllerContext);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
