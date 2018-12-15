using FluentValidation;
using Spinx.Data.Repository.EmailTemplates;
using Spinx.Services.EmailTemplates.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.EmailTemplates.Validators
{
    public class EmailTemplateCreateAdminValidator : AbstractValidator<EmailTemplateCreateAdminDto>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        

        public EmailTemplateCreateAdminValidator(
            IEmailTemplateRepository emailTemplateRepository
           )
        {
            _emailTemplateRepository = emailTemplateRepository;
           

            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Slug).NotEmpty().MaximumLength(100)
                .Must(UniqueSlug).WithMessage("{PropertyName} already used with other emailTemplate.");
            
            RuleFor(v => v.Subject).NotEmpty().MaximumLength(100);
        }

        private bool UniqueSlug(EmailTemplateCreateAdminDto dto, string slug)
        {
            return !_emailTemplateRepository.AsNoTracking.Any(w => w.Slug == slug);
        }        
    }
}
