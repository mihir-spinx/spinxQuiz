using Spinx.Core;
using Spinx.Domain.Members;
using Spinx.Services.EmailTemplates;
using Spinx.Services.Members;
using Spinx.Services.Members.DTOs;
using Spinx.Services.SeoPages;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.Infrastructure;
using Spinx.Web.Infrastructure;
using System;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IAppSettings _appSettings;
        private readonly IMemberResetPasswordService _memberResetPasswordService;
        private readonly ISeoPageService _seoPageService;

        public MemberController(
            IMemberService memberService,
            IEmailTemplateService emailTemplateService,
            IAppSettings appSettings,
            IMemberResetPasswordService memberResetPasswordService,
            ISeoPageService seoPageService)
        {
            _memberService = memberService;
            _emailTemplateService = emailTemplateService;
            _appSettings = appSettings;
            _memberResetPasswordService = memberResetPasswordService;
            _seoPageService = seoPageService;
        }

        public ActionResult Index()
        {
            if (UserAuth.IsLogedIn())
                return RedirectToAction("Index", "Quizzes");

            var entity = _seoPageService.GetPageMeta("CreateAccount");

            if (entity == null) return View();

            ViewBag.Title = entity.MetaTitle;
            ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }

        [HttpPost]
        public JsonNetResult Index(MemberFrontDto dto)
        {
            var refUrl = Convert.ToString(Request["refUrl"]);
            dto.CreatedSource = (int) MemberCreatedSource.SignUp;
            var result = _memberService.SaveFront(dto);

            if (!result.Success) return new JsonNetResult(result);

            //var emailService = new EmailService(_emailTemplateService, _appSettings);
            //emailService.SendWelcomEmail(dto);

            //UserAuth.SignIn(Convert.ToInt32(result.Id), dto.Name, dto.Email, true);

            result.Data = null;
            result.IsRedirect = true;

            result.SetRedirect(Url.RouteUrl("login"));

            return new JsonNetResult(result);
        }

        public ActionResult EditProfile()
        {
            if (!UserAuth.IsLogedIn())
                return RedirectToAction("Login", "Member");

            var entity = _memberService.GetById(UserAuth.User.UserId);

            return View(entity);
        }

        [HttpPost]
        public JsonNetResult EditProfile(MemberFrontDto dto)
        {
            var result = _memberService.UpdateProfile(dto, UserAuth.User.UserId);
            if (!result.Success) return new JsonNetResult(result);

            UserAuth.SignIn(UserAuth.User.UserId, dto.Name, dto.Email, true);
            result.Data = null;

            return new JsonNetResult(result);
        }

        public ActionResult Login()
        {
            if (UserAuth.IsLogedIn())
                return RedirectToAction("Index", "Quizzes");

            var entity = _seoPageService.GetPageMeta("Login");

            if (entity == null) return View();

            ViewBag.Title = entity.MetaTitle;
            ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }

        public ActionResult CheckoutLogin()
        {
            return View();
        }

        [ActionName("logout")]
        public ActionResult Logout()
        {
            UserAuth.Signout(UserAuth.CookieUser);

            return RedirectToAction("Login", "Member");
        }

        [HttpPost]
        public JsonNetResult Login(MemberFrontDto dto)
        {
            var result = _memberService.Login(dto);

            if (!result.Success) return new JsonNetResult(result);

            UserAuth.SignIn(result.Data.Id, result.Data.Name, result.Data.Email, true);

            result.Data = null;
            result.IsRedirect = true;

            result.SetRedirect("quiz");

            return new JsonNetResult(result);
        }

        [HttpPost]
        public JsonNetResult CheckoutLogin(MemberFrontDto dto)
        {
            var refUrl = Request["refUrl"];
            var result = _memberService.Login(dto);

            if (!result.Success) return new JsonNetResult(result);

            UserAuth.SignIn(result.Data.Id, result.Data.Name, result.Data.Email, true);

            result.Data = null;
            result.IsRedirect = true;
            result.SetRedirect(refUrl);

            return new JsonNetResult(result);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonNetResult ForgotPassword(MemberForgotPasswordDto dto)
        {
            var result = _memberService.ForgotPassword(dto, out var member);

            if (member != null)
                new EmailService(_emailTemplateService, _appSettings).SendMemberForgotPasswordEmail(member);

            if (!result.Success) return new JsonNetResult(result);

            result.IsRedirect = true;
            result.SetRedirect("login");
            result.Clear();

            return new JsonNetResult(result);
        }

        public ActionResult ResetPassword(string token)
        {
            if (VerifyToken(token))
                return View();

            return Redirect("forgot-password?token=expired");
        }

        [HttpPost]
        public JsonNetResult ResetPassword(MemberChangePasswordDto dto)
        {
            if (!VerifyToken(dto.Token))
                return new JsonNetResult((new Result().SetRedirect(Url.Action("ForgotPassword"))));

            var result = _memberResetPasswordService.ResetPassword(dto);

            if (result.IsRedirect)
                result.SetRedirect(result.Success ? Url.Action("Login") : Url.Action("ForgotPassword"));

            return new JsonNetResult(result);
        }

        private bool VerifyToken(string token)
        {
            if (_memberResetPasswordService.IsValidToken(token))
                return true;

            Flash.Info("Invalid forgot password token or token already expired.");

            return false;
        }

        public ActionResult ChangePassword()
        {
            if (!UserAuth.IsLogedIn())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public JsonNetResult ChangePassword(MemberFrontChangePasswordDto dto)
        {
            dto.Id = UserAuth.User.UserId;
            var result = _memberService.MemberFrontChangePassword(dto);

            return new JsonNetResult(result);
        }
    }
}