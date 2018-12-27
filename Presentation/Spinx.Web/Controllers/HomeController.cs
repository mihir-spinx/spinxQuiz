using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;
using Spinx.Web.Core.Authentication;

namespace Spinx.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISeoPageService _seoPageService;

        public HomeController(ISeoPageService seoPageService)
        {
            _seoPageService = seoPageService;
        }

        public ActionResult Index()
        {
            if (UserAuth.IsLogedIn())
                return RedirectToAction("Index", "Quizzes");
            else
                return RedirectToAction("Login", "Member");

            return View();
        }
    }
}