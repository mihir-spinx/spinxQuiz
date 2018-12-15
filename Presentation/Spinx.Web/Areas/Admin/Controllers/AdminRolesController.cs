using Spinx.Services.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser(permissions: new [] {"admin-roles"})]
    public class AdminRolesController : BaseAdminController
    {
        private readonly IAdminRoleService _adminRoleService;
        private readonly IAdminPermissionService _adminPermissionService;

        public AdminRolesController(IAdminRoleService adminRoleService, 
            IAdminPermissionService adminPermissionService)
        {
            _adminRoleService = adminRoleService;
            _adminPermissionService = adminPermissionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(AdminRoleFilterDto dto)
        {
            return ToJsonResult(_adminRoleService.List(dto));
        }

        public ActionResult Create()
        {
            ViewBag.AdminPermissions = _adminPermissionService.GetSequenceData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Create(AdminRoleDto dto)
        {
            var result = _adminRoleService.Create(dto);
            if (result.Success)
                result.Redirect = Url.Action("Edit", new { id = result.Id });

            return ToJsonResult(result);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.AdminPermissions = _adminPermissionService.GetSequenceData();
            var model = _adminRoleService.GetById(id);
            if (model == null)
                return PageNotFound();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Edit(AdminRoleDto dto)
        {
            var result = _adminRoleService.Edit(dto);
            if (result.IsRedirect)
                result.Redirect = Url.Action("Index");

            return ToJsonResult(result);
        }
    }
}