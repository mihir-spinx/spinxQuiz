using Spinx.Services.SeoPages;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly ISeoPageService _seoPageService;

        public SiteMapController(ISeoPageService seoPageService)
        {
            _seoPageService = seoPageService;
        }

        public ActionResult Index()
        {
            var entity = _seoPageService.GetPageMeta("Sitemap");

            if (entity == null) return View();

            ViewBag.Title = entity.MetaTitle;
            ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }
    }
}