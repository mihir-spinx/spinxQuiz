using Spinx.Services.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class AccountController : BaseAdminController
    {
        private readonly IAccountAdminService _accountAdminService;

        public AccountController(IAccountAdminService accountAdminService)
        {
            _accountAdminService = accountAdminService;
        }

        public ActionResult EditProfile()
        {
            var model = _accountAdminService.GetEditProfile(UserAuth.AdminUser.UserId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult EditProfile(EditProfileDto dto)
        {
            dto.Id = UserAuth.AdminUser.UserId;
            return ToJsonResult(_accountAdminService.SaveEditProfile(dto));
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordDto model)
        {
            model.Id = UserAuth.AdminUser.UserId;
            return ToJsonResult(_accountAdminService.ChangePassword(model));
        }
    }
}