using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

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
            var entity = _seoPageService.GetPageMeta("HomePage");

            //ViewBag.Title = entity.MetaTitle;
            //ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }
    }
}