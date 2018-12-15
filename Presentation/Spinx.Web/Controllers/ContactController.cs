using Spinx.Services.ContactUsInquiries;
using Spinx.Services.ContactUsInquiries.DTOs;
using Spinx.Services.EmailTemplates;
using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactUsInquiryService _contactUsService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IAppSettings _appSettings;
        private readonly ISeoPageService _seoPageService;

        public ContactController(
            IContactUsInquiryService contactUsService,
            IEmailTemplateService emailTemplateService,
            IAppSettings appSettings,
            ISeoPageService seoPageService)
        {
            _contactUsService = contactUsService;
            _emailTemplateService = emailTemplateService;
            _appSettings = appSettings;
            _seoPageService = seoPageService;
        }

        public ActionResult Index()
        {
            ViewBag.RecaptchaEnable = _appSettings.RecaptchaEnable;
            ViewBag.RecaptchaPublicKey = _appSettings.RecaptchaPublicKey;

            var entity = _seoPageService.GetPageMeta("ContactUS");
            if (entity == null) return View();

            ViewBag.Title = entity.MetaTitle;
            ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }

        [HttpPost]
        public JsonNetResult Index(ContactUsDto dto)
        {
            var result = _contactUsService.SaveValidation(dto);

            if (_appSettings.RecaptchaEnable)
            {
                if (string.IsNullOrEmpty(dto.gRecaptchaResponse))
                {
                    result.Errors.Add("gRecaptchaResponse", "The captcha verification required.");
                    result.Success = false;
                }
                else if (ReCaptcha.Validate(dto.gRecaptchaResponse, _appSettings.RecaptchaSecretKey) != "true")
                {
                    result.Errors.Add("gRecaptchaResponse", "Invalid captcha verification. Please try again or refresh page.");
                    result.Success = false;
                }
            }

            if (!result.Success) return new JsonNetResult(result);

            result = _contactUsService.Save(dto);

            if (!result.Success) return new JsonNetResult(result);

            var emailService = new EmailService(_emailTemplateService, _appSettings);
            emailService.SendMailToAdminForContactUs(dto);
            emailService.SendMailToEndUserForContactUs(dto.Email, dto.Name);

            result.Data = null;

            return new JsonNetResult(result);
        }
    }
}