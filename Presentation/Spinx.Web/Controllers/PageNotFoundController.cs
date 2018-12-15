using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class PageNotFoundController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}