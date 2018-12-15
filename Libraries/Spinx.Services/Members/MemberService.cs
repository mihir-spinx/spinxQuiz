using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Core.Extensions;
using Spinx.Core.Helpers;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.ContactUsInquiries;
using Spinx.Data.Repository.Member;
using Spinx.Domain.Members;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.Actions;
using Spinx.Services.Members.DTOs;
using Spinx.Services.Members.Filters;
using Spinx.Services.Members.ListOrders;
using Spinx.Services.Members.Mappers;
using Spinx.Services.Members.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.Members
{
    public interface IMemberService
    {
        Result Create(MemberDto dto);
        Result GuestUpdatePassword(MemberGuestDto dto, int memberId);
        MemberDto GetById(int id);
        Result Edit(int id, MemberDto dto);
        List<MemberFrontDto> Get();

        Result SaveFront(MemberFrontDto dto);
        Result UpdateProfile(MemberFrontDto dto, int memberId);
        Result Login(MemberFrontDto dto);
        Result ForgotPassword(MemberForgotPasswordDto dto, out Member member);
        Result MemberChangePassword(MemberChangePasswordDto model);

        Result MemberFrontChangePassword(MemberFrontChangePasswordDto model);
        Result DetailList(MemberDetailListDto dto);
        AdminDashboardDto GetAdminDashboard();
        Result GetMemberDashboard(int memberId);
    }

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MemberActionFactory _actionFactory;
        private readonly MemberFrontValidator _memberValidator;
        private readonly MemberLoginValidator _memberLoginValidator;
        private readonly MemberFrontUpdateValidator _memberUpdateValidator;
        private readonly MemberChangePasswordValidator _memberChangePasswordValidator;
        private readonly MemberFrontChangePasswordValidator _memberFrontChangePasswordValidator;
        private readonly IContactUsInquiryRepository _contactUsInquiryRepository;

        public MemberService(
            IMemberRepository memberRepository,
            IUnitOfWork unitOfWork, MemberActionFactory actionFactory,
            MemberFrontValidator memberValidator,
            MemberChangePasswordValidator memberChangePasswordValidator,
            MemberFrontChangePasswordValidator memberFrontChangePasswordValidator,
            MemberFrontUpdateValidator memberUpdateValidator,
            MemberLoginValidator memberLoginValidator,
            IContactUsInquiryRepository contactUsInquiryRepository)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
            _actionFactory = actionFactory;

            _memberValidator = memberValidator;
            _memberChangePasswordValidator = memberChangePasswordValidator;
            _memberFrontChangePasswordValidator = memberFrontChangePasswordValidator;
            _memberUpdateValidator = memberUpdateValidator;
            _memberLoginValidator = memberLoginValidator;
            _contactUsInquiryRepository = contactUsInquiryRepository;

            MemberMapper.Init();
        }

        public Result DetailList(MemberDetailListDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();

            if (!result.Success)
                return result;

            var query = _memberRepository.AsNoTracking;

            query = new MemberDetailListFilter(query, dto).FilteredQuery();
            query = new MemberListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            var test = query.Select(s => new MemberDetailListDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                CreatedSource = s.CreatedSource,
            });

            result.Data = test.ToPaged(result.Paging.Page, result.Paging.Size).ToList();
            return result;
        }

        public Result GetMemberDashboard(int memberId)
        {
            var result = new Result();
            var query = _memberRepository.AsNoTracking.Where(w => w.Id == memberId);

            result.Data = query.Select(s => new MemberDetailListDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                LastLoginAt = s.LastLoginAt,
                CreatedSource = s.CreatedSource,
            }).FirstOrDefault();

            return result;
        }

        public Result Create(MemberDto dto)
        {
            var validator = new MemberValidator(_memberRepository);
            var result = validator.ValidateResult(dto);

            if (!result.Success) return result;
            dto.CreatedSource = (int) MemberCreatedSource.Admin;
            var entity = Mapper.Map<Member>(dto);
            entity.Salt = SecurityHelper.GenerateSalt();
            entity.Password = SecurityHelper.GenerateHash(dto.Password, entity.Salt);
            entity.IsActive = true;
            entity.LastLoginAt = DateTime.Now;
            _memberRepository.Insert(entity);
            _unitOfWork.Commit();

            MemberCacheManager.ClearCache();
            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public Result GuestUpdatePassword(MemberGuestDto dto, int memberId)
        {
            var validator = new MemberGuestValidator();
            var result = validator.ValidateResult(dto);

            if (!result.Success) return result;

            var strSalt = SecurityHelper.GenerateSalt();
            var strPassword = SecurityHelper.GenerateHash(dto.Password, strSalt);

            var query = _memberRepository.AsNoTracking.Where(x => x.Id == memberId);
            query.Update(u => new Member { Password = strPassword, Salt = strSalt, IsActive = true });

            MemberCacheManager.ClearCache();
            result.Id = memberId;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public MemberDto GetById(int id)
        {
            var entity = _memberRepository.AsNoTracking
                .FirstOrDefault(s => s.Id == id);

            return entity == null ? null : Mapper.Map<MemberDto>(entity);
        }

        public Result Edit(int id, MemberDto dto)
        {
            dto.Id = id;

            var validator = new MemberValidator(_memberRepository);
            var result = validator.ValidateResult(dto);

            if (!result.Success) return result;

            var entity = _memberRepository.AsNoTracking
                .FirstOrDefault(s => s.Id == dto.Id);

            if (entity == null)
                return new Result().SetBlankRedirect();

            Mapper.Map<Member>(dto, entity);

            if (!string.IsNullOrEmpty(dto.Password))
            {
                entity.Salt = SecurityHelper.GenerateSalt();
                entity.Password = SecurityHelper.GenerateHash(dto.Password, entity.Salt);
            }

            _memberRepository.Update(entity);

            _unitOfWork.Commit();
            MemberCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }

        public Result SaveFront(MemberFrontDto dto)
        {
            var result = _memberValidator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<Member>(dto);
            entity.Salt = SecurityHelper.GenerateSalt();
            entity.Password = SecurityHelper.GenerateHash(dto.Password, entity.Salt);
            entity.IsActive = true;
            entity.LastLoginAt = DateTime.Now;
            _memberRepository.Insert(entity);
            _unitOfWork.Commit();

            result.Data = dto.Email;
            result.Id = entity.Id;
            result.SetSuccess("Your account was created successfully.");

            return result;
        }

        public Result UpdateProfile(MemberFrontDto dto, int memberId)
        {
            var result = _memberUpdateValidator.ValidateResult(dto);
            if (!result.Success) return result;

            var query = _memberRepository.AsNoTracking.Where(x => x.Id == memberId);
            query.Update(u => new Member
            {
                Name = dto.Name,
                Phone = dto.Phone,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Degree = dto.Degree,
                College = dto.College,
                LastSemMark = dto.LastSemMark,
                Experience = dto.Experience
            });

            MemberCacheManager.ClearCache();
            result.SetSuccess("Your account updated successfully.");

            return result;
        }

        public Result Login(MemberFrontDto dto)
        {
            var result = _memberLoginValidator.ValidateResult(dto);
            if (!result.Success) return result;

            var salt = SecurityHelper.GenerateSalt();
            var password = SecurityHelper.GenerateHash(dto.Password, salt);

            var member = _memberRepository.AsNoTracking
                .FirstOrDefault(s => s.Email == dto.Email && s.IsActive);

            if (member == null || !SecurityHelper.VerifyHash(dto.Password, member.Password, member.Salt))
            {
                result.SetError("You did not sign in correctly or your account is temporarily disabled.");
                result.Success = false;
            }
            else
            {
                SaveLastLogin(member);
                result.Data = member;
                result.SetSuccess("Logged in successfully.");
                result.Success = true;
            }

            return result;
        }

        public Result ForgotPassword(MemberForgotPasswordDto dto, out Member member)
        {
            member = null;

            var validator = new MemberForgotPasswordValidator();
            var result = validator.ValidateResult(dto);

            if (!result.Success) return result;

            result = ForgotPasswordResponse(dto.Email);

            member = _memberRepository.AsNoTracking
                .FirstOrDefault(w => w.IsActive && w.Email == dto.Email);

            if (member != null)
            {
                member = new Member()
                {
                    Name = member.Name,
                    Email = member.Email,
                    ForgotPasswordToken = GenerateAndSaveForgotPasswordToken(member)
                };
            }

            return result;
        }

        private string GenerateAndSaveForgotPasswordToken(Member member)
        {
            var passwordResetToken = StringHelper.RandomString(12);

            member.ForgotPasswordToken = passwordResetToken;
            _memberRepository.Update(member);
            _unitOfWork.Commit();

            return passwordResetToken;
        }

        private static Result ForgotPasswordResponse(string email)
        {
            return new Result()
            {
                Success = true,
                MessageType = MessageType.Success,
                Message = $"If there is an account associated with {email} you will receive an email with a link to reset your password."
            };
        }

        public Result MemberChangePassword(MemberChangePasswordDto dto)
        {
            var result = _memberChangePasswordValidator.ValidateResult(dto);

            if (!result.Success)
                return result;

            Member member = _memberRepository.AsNoTracking
                .FirstOrDefault(s => s.ForgotPasswordToken == dto.Token && s.IsActive);

            MemberChangePasswordSave(dto.NewPassword, member);

            return new Result().SetSuccess(Messages.PasswordUpdated).Clear();
        }

        public Result MemberFrontChangePassword(MemberFrontChangePasswordDto dto)
        {
            var result = _memberFrontChangePasswordValidator.ValidateResult(dto);

            if (!result.Success)
                return result;

            Member member = _memberRepository.AsNoTracking
                .FirstOrDefault(s => s.Id == dto.Id && s.IsActive);

            var salt = SecurityHelper.GenerateSalt();
            var encryptedPassword = SecurityHelper.GenerateHash(dto.NewPassword, salt);

            var query = _memberRepository.AsNoTracking.Where(x => x.Id == dto.Id);
            query.Update(u => new Member { Salt = salt, Password = encryptedPassword });

            return new Result().SetSuccess(Messages.PasswordUpdated).Clear();
        }

        private void MemberChangePasswordSave(string newPassword, Member member)
        {
            var salt = SecurityHelper.GenerateSalt();
            var encryptedPassword = SecurityHelper.GenerateHash(newPassword, salt);

            member.Salt = salt;
            member.Password = encryptedPassword;

            _memberRepository.Update(member);
            _unitOfWork.Commit();
        }

        private void SaveLastLogin(Member member)
        {
            member.LastLoginAt = DateTime.Now;

            _memberRepository.Update(member);
            _unitOfWork.Commit();
        }

        public List<MemberFrontDto> Get()
        {
            return _memberRepository.AsNoTracking
                .OrderBy(o => o.Name)
                .Select(s => new MemberFrontDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                })
                .FromCache(MemberCacheManager.Name)
                .ToList();
        }

        public AdminDashboardDto GetAdminDashboard()
        {
            var dto = new AdminDashboardDto
            {
                TotalMembers = _memberRepository.AsNoTracking.Count(),
                TotalContactUsInquiries = _contactUsInquiryRepository.AsNoTracking.Count(),
            };
            return dto;
        }
    }
}