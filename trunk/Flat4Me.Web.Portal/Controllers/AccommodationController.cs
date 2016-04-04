using Flat4Me.Core.Auth;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
using System.Threading.Tasks;
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
    public class AccommodationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult ReservationList()
        {
            return View();
        }

        public async Task SendMail()
        {
            await UserManager.SendEmailAsync(UserId, "Тест", "Тест");
        }
    }
}