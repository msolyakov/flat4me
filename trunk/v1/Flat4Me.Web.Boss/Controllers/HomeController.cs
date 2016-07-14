using AutoMapper;
using Flat4Me.Core.Auth;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.Repository.Interfaces.Short;
using Flat4Me.Identity;
using Flat4Me.Web.Boss.Models.Short;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Web.Boss.Controllers
{
    [Authorize(Roles = UserRoleList.Admin)]
    public class HomeController : BaseController
    {
        [Inject]
        public IHotelierProfileRepository HotelierProfileRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Hoteliers()
        {
            var dbList = await HotelierProfileRepository.GetUnapprovedList();
            var viewList = dbList.Select(p => Mapper.Map<HotelierProfileUnapprovedViewModel>(p));

            return View(viewList);
        }

        public async Task<ActionResult> ApproveHotelier(int userId)
        {
            await HotelierProfileRepository.Approve(userId, true);            
            await UserManager.AddClaimAsync(userId, F4MeClaims.GetHotelierApproved());

            await NotifyHotelierAboutProfileApproved(userId);

            return RedirectToAction("Hoteliers");
        }

        private async Task NotifyHotelierAboutProfileApproved(int userId)
        {
            var url = @"http://portal-f4me0dev.azurewebsites.net";
            var subject = "Аккаунт Хотельера подвержден";
            var body = new StringBuilder();
            body.Append("Ваш аккаут подтвержден");
            body.Append("<br /><br />");
            body.AppendFormat("Чтобы сдать жилье перейдите по <a href='{0}'>ссылке</a>!", url);
            body.Append("<br /><br />");
            body.Append("Спасибо!");

            await UserManager.SendEmailAsync(
                userId,
                subject,
                body.ToString());
        }
    }
}