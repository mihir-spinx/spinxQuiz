using FluentValidation;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberFrontUpdateValidator : AbstractValidator<MemberFrontDto>
    {
        public MemberFrontUpdateValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Password).MinimumLength(6).MaximumLength(20);

            RuleFor(v => v.Phone).NotEmpty().MaximumLength(20);
            RuleFor(v => v.AddressLine1).MaximumLength(250);
            RuleFor(v => v.AddressLine2).MaximumLength(250);
            RuleFor(v => v.City).NotEmpty().MaximumLength(100);
            RuleFor(v => v.State).MaximumLength(100);
            RuleFor(v => v.Degree).MaximumLength(100);
            RuleFor(v => v.College).MaximumLength(100);
            RuleFor(v => v.LastSemMark).MaximumLength(100);
            RuleFor(v => v.Experience).MaximumLength(100);
        }
    }
}