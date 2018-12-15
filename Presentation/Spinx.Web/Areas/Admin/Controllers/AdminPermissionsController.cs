using Spinx.Services.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.Infrastructure;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class AdminPermissionsController : BaseAdminController
    {
        private readonly IAdminPermissionService _adminPermissionService;

        public AdminPermissionsController(IAdminPermissionService adminPermissionService)
        {
            _adminPermissionService = adminPermissionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(AdminPermissionFilterDto model)
        {
            return ToJsonResult(_adminPermissionService.List(model));
        }

        public ActionResult Create()
        {
            ViewBag.AdminPermissions = _adminPermissionService.GetAdminPermissions();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Create(AdminPermissionDto dto)
        {
            var result = _adminPermissionService.Create(dto);
            if (result.Success)
                result.Redirect = Url.Action("Edit", new { id = result.Id });

            return ToJsonResult(result);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.AdminPermissions = _adminPermissionService.GetAdminPermissions();
            var dto = _adminPermissionService.GetById(id);
            if (dto == null)
                return PageNotFound();

            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Edit(AdminPermissionDto model)
        {
            var result = _adminPermissionService.Edit(model);
            if (result.IsRedirect)
                result.Redirect = Url.Action("Index");

            if (!result.Success) return ToJsonResult(result);

            Flash.Success(result.Message);
            result.Redirect = Url.Action("Edit", "AdminPermissions", new {model.Id});

            return ToJsonResult(result);
        }

        public ActionResult Sequence()
        {
            ViewBag.AdminPermissions = _adminPermissionService.GetSequenceData();

            return View();
        }

        [HttpPost]
        public void Sequence(string data)
        {
            _adminPermissionService.SaveSequenceData(data);
        }
    }
}