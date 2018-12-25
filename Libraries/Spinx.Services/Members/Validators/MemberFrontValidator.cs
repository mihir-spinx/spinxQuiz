using FluentValidation;
using Spinx.Data.Repository.Member;
using Spinx.Services.Members.DTOs;
using System.Linq;

namespace Spinx.Services.Members.Validators
{
    public class MemberFrontValidator : AbstractValidator<MemberFrontDto>
    {
        private readonly IMemberRepository _memberRepository;

        public MemberFrontValidator(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;

            RuleFor(v => v.Name).NotEmpty().WithMessage("'Full Name' should not be empty.").MaximumLength(100);

            RuleFor(v => v.Email).NotEmpty().WithMessage("Invalid email address.")
                .EmailAddress().WithMessage("Invalid email address.").MaximumLength(100)
                .Must(UniqueEmail).WithMessage("{PropertyName} already used with other user.");

            RuleFor(v => v.Password).NotEmpty().MaximumLength(20);

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

        private bool UniqueEmail(string email)
        {
            var query = _memberRepository.AsNoTracking.Where(w => w.Email == email);
            return !query.Any();
        }
    }
}