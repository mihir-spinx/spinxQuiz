using Spinx.Services.Pages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class PagesController : BaseController
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService, IAppSettings appSettings)
        {
            _pageService = pageService;
        }

        public ActionResult Index(string slug)
        {
            var page = _pageService.GetPageBySlug(slug);

            return View(page);
        }

        public ActionResult Terms()
        {
            var page = _pageService.GetPageBySlug("terms");

            return View(page);
        }

        public ActionResult Privacy()
        {
            var page = _pageService.GetPageBySlug("privacy");

            return View(page);
        }
    }
}