using Spinx.Services.EmailTemplates;
using Spinx.Services.Members;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class DashBoardsController : BaseController
    {
        public DashBoardsController(
            IMemberService memberService,
            IEmailTemplateService emailTemplateService,
            IAppSettings appSettings)
        {
        }

        public ActionResult Index()
        {
            if (!UserAuth.IsLogedIn()) return RedirectToAction("Login", "Member");

            return View();
        }

        public ActionResult AdvertisementDashBoard()
        {
            if (!UserAuth.IsLogedIn()) return RedirectToAction("Login", "Member");

            return View();
        }
    }
}