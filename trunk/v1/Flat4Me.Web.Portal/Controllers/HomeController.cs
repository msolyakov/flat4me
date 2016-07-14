using Flat4Me.Core.Auth;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
using System.Web.Mvc;

namespace Flat4Me.Web.Portal.Controllers
{
    [F4MeAuthorize(Roles = UserRoleList.Hotelier)]
    // EmailConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.EmailConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // PhoneConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.PhoneConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // HotelierApproved
    [F4MeAuthorize(ClaimType = F4MeClaims.HotelierApproved, ClaimValue = F4MeClaims.EmptyValue)]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}