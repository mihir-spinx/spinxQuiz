using Spinx.Data.Repository.EmailTemplates;
using Spinx.Domain.EmailTemplates;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.EmailTemplates
{
    public interface IEmailTemplateService
    {
        IEnumerable<EmailTemplate> GetCachedEmailTemplates();
        EmailTemplate GetEmailTemplateBySlug(string slug);
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public IEnumerable<EmailTemplate> GetCachedEmailTemplates()
        {
            return _emailTemplateRepository.AsNoTracking
                .FromCache("EmailTemplates")
                .ToList();
        }

        private EmailTemplate EmailTemplateBySlug(string slug)
        {
            var emailTemplate = GetCachedEmailTemplates()
                .FirstOrDefault(w => w.Slug == slug);

            return emailTemplate;
        }

        public EmailTemplate GetEmailTemplateBySlug(string slug)
        {
            var header = EmailTemplateBySlug("header").Content;
            var body = EmailTemplateBySlug(slug);
            var footer = EmailTemplateBySlug("footer").Content;

            return new EmailTemplate
            {
                Subject = body.Subject,
                Content = header + body.Content + footer
            };
        }
    }
}