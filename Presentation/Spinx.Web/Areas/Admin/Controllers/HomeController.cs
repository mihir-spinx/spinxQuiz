using Spinx.Services.Members;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class HomeController : BaseAdminController
    {
        private readonly IMemberService _memberService;

        public HomeController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult Index()
        {
            var entity = _memberService.GetAdminDashboard();

            return View(entity);
        }
    }
}