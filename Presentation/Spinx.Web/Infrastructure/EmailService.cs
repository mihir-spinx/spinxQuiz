using Spinx.Core.Helpers;
using Spinx.Domain.AdminUsers;
using Spinx.Domain.Members;
using Spinx.Services.ContactUsInquiries.DTOs;
using Spinx.Services.EmailTemplates;
using Spinx.Services.Members.DTOs;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Spinx.Web.Infrastructure
{
    public class EmailService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IAppSettings _appSettings;
        private readonly RequestContext _requestContext;
        private readonly string _adminEmail = "mihir.spinx@gmail.com";

        public EmailService(IEmailTemplateService emailTemplateService, IAppSettings appSettings)
        {
            _emailTemplateService = emailTemplateService;
            _appSettings = appSettings;
            _requestContext = HttpContext.Current.Request.RequestContext;
        }

        #region Welcome

        public void SendWelcomEmail(MemberFrontDto member)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplateBySlug("welcome-user");

            new MailHelper()
                .To(member.Name, member.Email)
                .Subject(emailTemplate.Subject)
                .Body(emailTemplate.Content)
                .Variables(new Dictionary<string, object>()
                {
                    {"FullName", member.Name},
                    {"WebsiteUrl", _appSettings.WebsiteUrl},
                    {"WebsiteName", _appSettings.WebsiteName},
                    {"Year", DateTime.Today.Year}
                })
                .Send();
        }

        #endregion

        #region Password

        public void SendAdminForgotPasswordEmail(AdminUser adminUser)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplateBySlug("reset-password-admin");

            new MailHelper()
                .To(adminUser.Name, adminUser.Email)
                .Subject(emailTemplate.Subject)
                .Body(emailTemplate.Content)
                .Variables(new Dictionary<string, object>()
                {
                    {
                        "Link", new UrlHelper(_requestContext).Action("ResetPassword", "Auth",
                            routeValues: new {token = adminUser.ForgotPasswordToken},
                            protocol: HttpContext.Current.Request.Url.Scheme)
                    },
                    {"WebsiteUrl", _appSettings.WebsiteUrl},
                    {"WebsiteName", _appSettings.WebsiteName},
                    {"Year", DateTime.Today.Year}
                })
                .Send();
        }

        public void SendMemberForgotPasswordEmail(Member member)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplateBySlug("reset-password-member");

            new MailHelper()
                .To(member.Name, member.Email)
                .Subject(emailTemplate.Subject)
                .Body(emailTemplate.Content)
                .Variables(new Dictionary<string, object>()
                {
                    {
                        "Link", new UrlHelper(_requestContext).Action("ResetPassword", "Member",
                            routeValues: new {token = member.ForgotPasswordToken},
                            protocol: HttpContext.Current.Request.Url.Scheme)
                    },
                    {"WebsiteUrl", _appSettings.WebsiteUrl},
                    {"WebsiteName", _appSettings.WebsiteName},
                    {"Year", DateTime.Today.Year}
                })
                .Send();
        }

        #endregion

        #region Contact Us

        public void SendMailToAdminForContactUs(ContactUsDto model)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplateBySlug("contact-us-admin");

            new MailHelper()
                .To(_adminEmail)
                .Subject(emailTemplate.Subject)
                .Body(emailTemplate.Content)
                .Variables(new Dictionary<string, object>
                {
                    {"Name", model.Name},
                    {"Email", model.Email},
                    {"Phone", model.Phone},
                    {"Message", model.Details},
                    {"WebsiteUrl", _appSettings.WebsiteUrl},
                    {"WebsiteName", _appSettings.WebsiteName},
                    {"Year", DateTime.Today.Year}
                })
                .Send();
        }

        public void SendMailToEndUserForContactUs(string email, string name)
        {
            var emailTemplate = _emailTemplateService.GetEmailTemplateBySlug("contact-us");

            new MailHelper()
                .To(name, email)
                .Subject(emailTemplate.Subject)
                .Body(emailTemplate.Content)
                .Variables(new Dictionary<string, object>
                {
                    {"WebsiteUrl", _appSettings.WebsiteUrl},
                    {"WebsiteName", _appSettings.WebsiteName},
                    {"Year", DateTime.Today.Year}
                })
                .Send();
        }

        #endregion
    }
}