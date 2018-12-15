using Spinx.Core;
using Spinx.Services.AdminAuth;
using Spinx.Services.AdminAuth.DTOs;
using Spinx.Services.EmailTemplates;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.Infrastructure;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    public class AuthController : BaseAdminController
    {
        private readonly IAdminLoginService _adminLoginService;
        private readonly IAdminForgotPasswordService _adminForgotPasswordService;
        private readonly IAdminResetPasswordService _adminResetPasswordService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IAppSettings _appSettings;

        public AuthController(IAdminLoginService adminLoginService,
            IAdminForgotPasswordService adminForgotPasswordService,
            IAdminResetPasswordService adminResetPasswordService,
            IEmailTemplateService emailTemplateService,
            IAppSettings appSettings)
        {
            _adminLoginService = adminLoginService;
            _adminForgotPasswordService = adminForgotPasswordService;
            _adminResetPasswordService = adminResetPasswordService;
            _emailTemplateService = emailTemplateService;
            _appSettings = appSettings;
        }

        #region Login
        public ActionResult Login()
        {
            if (UserAuth.IsAdminLogedIn())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Login(AdminLoginDto dto, string returnUrl, bool remember = false)
        {
            var result = _adminLoginService.Login(dto, out var adminUser);
            if (!result.Success)
                return ToJsonResult(result);

            UserAuth.SignInAdmin(adminUser.Id, adminUser.Name, adminUser.Email, remember);
            return ToJsonResult(
                new Result().SetRedirect(string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home") : returnUrl));
        }
        #endregion

        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ForgotPassword(AdminForgotPasswordDto dto)
        {
            var result = _adminForgotPasswordService.ForgotPassword(dto, out var adminUser);
            if (adminUser != null)
                new EmailService(_emailTemplateService, _appSettings).SendAdminForgotPasswordEmail(adminUser);

            if (result.Success) result.Clear();
            return ToJsonResult(result);
        }
        #endregion

        #region Reset Password
        public ActionResult ResetPassword(string token)
        {
            if (VerifyToken(token))
                return View();

            return RedirectToAction("ForgotPassword");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ResetPassword(AdminResetPasswordDto dto)
        {
            if (!VerifyToken(dto.Token))
                return ToJsonResult(new Result().SetRedirect(Url.Action("ForgotPassword")));

            var result = _adminResetPasswordService.ResetPassword(dto);
            if (result.IsRedirect)
                result.SetRedirect(result.Success ? Url.Action("Login") : Url.Action("ForgotPassword"));

            return ToJsonResult(result);
        }

        private bool VerifyToken(string token)
        {
            if (_adminResetPasswordService.IsValidToken(token))
                return true;

            Flash.Info("Invalid forgot password token or token already expired.");
            return false;
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            UserAuth.Signout(UserAuth.CookieAdminUser);
            return RedirectToAction("Login", "Auth");
        }
        #endregion
    }
}