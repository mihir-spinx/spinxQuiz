using FluentValidation;
using Spinx.Data.Repository.Member;
using Spinx.Services.AdminUsers.DTOs;
using System.Linq;

namespace Spinx.Services.Members.Validators
{
    public class MemberEditProfileValidator : AbstractValidator<MemberEditProfileDto>
    {
        private readonly IMemberRepository _memberRepository;

        public MemberEditProfileValidator(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;

            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);

            RuleFor(v => v.Email).NotEmpty().WithMessage("Invalid email address.")
                .EmailAddress().WithMessage("Invalid email address.").MaximumLength(100)
                .Must(UniqueEmail).WithMessage("{PropertyName} already used with other user.");

            RuleFor(v => v.Phone).MaximumLength(20);
            RuleFor(v => v.AddressLine1).MaximumLength(250);
            RuleFor(v => v.AddressLine2).MaximumLength(250);
            RuleFor(v => v.City).MaximumLength(100);
            RuleFor(v => v.State).MaximumLength(100);
            RuleFor(v => v.Degree).MaximumLength(100);
            RuleFor(v => v.College).MaximumLength(100);
            RuleFor(v => v.LastSemMark).MaximumLength(100);
            RuleFor(v => v.Experience).MaximumLength(100);
        }

        private bool UniqueEmail(MemberEditProfileDto dto, string email)
        {
            return !_memberRepository.AsNoTracking.Any(w => w.Id != dto.Id && w.Email == email);
        }
    }
}