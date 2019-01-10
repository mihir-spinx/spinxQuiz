using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class GeneralSettingsController : BaseAdminController
    {
       // [AuthorizeAdminUser(permissions: new [] { "GeneralSettings" })]
        public ActionResult Index()
        {
            return View();
        }

       // [AuthorizeAdminUser(permissions: new [] { "GeneralSettings.Create" })]
        //public ActionResult Create()
        //{
        //    return View();
        //}

       // [AuthorizeAdminUser(permissions: new [] { "GeneralSettings.Edit" })]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}