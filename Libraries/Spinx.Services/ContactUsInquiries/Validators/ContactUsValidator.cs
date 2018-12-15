using FluentValidation;
using Spinx.Services.ContactUsInquiries.DTOs;

namespace Spinx.Services.ContactUsInquiries.Validators
{
    public class ContactUsValidator : AbstractValidator<ContactUsDto>
    {
        public ContactUsValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Email).NotEmpty().WithMessage("Invalid email address.")
                        .EmailAddress().WithMessage("Invalid email address.").MaximumLength(254);
            RuleFor(v => v.Phone).MaximumLength(20);
            RuleFor(v => v.Details).NotEmpty().WithMessage("'Message' should not be empty.").MaximumLength(2000);
        }
    }
}