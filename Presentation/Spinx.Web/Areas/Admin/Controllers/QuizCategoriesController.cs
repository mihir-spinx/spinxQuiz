using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class QuizCategoriesController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"QuizCategories"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizCategories.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizCategories.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}