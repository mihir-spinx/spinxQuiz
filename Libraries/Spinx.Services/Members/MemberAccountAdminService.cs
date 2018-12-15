using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Member;
using Spinx.Domain.Members;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.Validators;

namespace Spinx.Services.Members
{
    public interface IMemberAccountAdminService
    {
        MemberEditProfileDto GetMemberEditProfile(int userId);
        Result SaveMemberEditProfile(MemberEditProfileDto model);
    }

    public class MemberAccountAdminService : IMemberAccountAdminService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MemberEditProfileValidator _memberEditProfileValidator;

        public MemberAccountAdminService(
            IMemberRepository memberRepository,
            IUnitOfWork unitOfWork,
            MemberEditProfileValidator memberEditProfileValidator)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
            _memberEditProfileValidator = memberEditProfileValidator;
        }

        public MemberEditProfileDto GetMemberEditProfile(int userId)
        {
            var member = _memberRepository.Find(userId);

            return member == null
                ? null
                : Mapper.Map<MemberEditProfileDto>(member);
        }

        public Result SaveMemberEditProfile(MemberEditProfileDto dto)
        {
            var result = _memberEditProfileValidator.ValidateResult(dto);
            if (!result.Success) return result;

            var dbMember = _memberRepository.Find(dto.Id);
            if (dbMember == null) return null;

            MemberEditProfileSave(dto, dbMember);

            return new Result().SetSuccess(Messages.ProfileUpdated);
        }

        private void MemberEditProfileSave(MemberEditProfileDto model, Member member)
        {
            member.Name = model.Name;
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.AddressLine1 = model.AddressLine1;
            member.AddressLine2 = model.AddressLine2;
            member.City = model.City;
            member.State = model.State;
            member.College = model.College;
            member.Degree = model.Degree;
            member.LastSemMark = model.LastSemMark;
            member.Experience = model.Experience;

            _memberRepository.Update(member);
            _unitOfWork.Commit();
        }
    }
}