using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class ContactUsInquiriesController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new[] { "contactUs" })]
        public ActionResult Index()
        {
            return View();
        }
    }
}