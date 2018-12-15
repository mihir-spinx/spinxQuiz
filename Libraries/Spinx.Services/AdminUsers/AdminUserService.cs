using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminUsers.Actions;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.AdminUsers.Filters;
using Spinx.Services.AdminUsers.ListOrders;
using Spinx.Services.AdminUsers.Mappers;
using Spinx.Services.AdminUsers.Validators;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.AdminUsers
{
    public interface IAdminUserService
    {
        Result List(AdminUserFilterDto dto, int loggedInUserId);
        Result Create(AdminUserDto dto);
        AdminUserDto GetById(int id);
        Result Edit(int id, AdminUserDto dto);

        IList<string> GetPermissions(int userId);
        IList<string> GetRoles(int userId);
    }

    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IAdminRoleRepository _adminRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserService(
            IAdminUserRepository adminUserRepository,
            IAdminRoleRepository adminRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _adminRoleRepository = adminRoleRepository;
            _unitOfWork = unitOfWork;

            AdminUserMapper.Init();
        }

        public Result List(AdminUserFilterDto dto, int loggedInUserId)
        {
            var result =
                new AdminUserActionFactory(_adminUserRepository, _unitOfWork).ExecuteAction(dto, loggedInUserId);

            if (!result.Success)
                return result;

            var query = _adminUserRepository.AsNoTracking;
            query = new AdminUserFilter(query, dto).FilteredQuery();
            query = new AdminUserListOrder(query, dto).OrderByQuery();
            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query.Select(s => new
            {
                s.Id,
                s.Name,
                s.Email,
                s.IsActive,
                s.LastLoginAt,
                s.CreatedAt,
                AdminRoles = s.Roles.Select(x => new { x.Name }).ToList()
            })
                .ToPaged(result.Paging.Page, result.Paging.Size)
                .ToList();

            return result;
        }

        public Result Create(AdminUserDto dto)
        {
            var validator = new AdminUserValidator(_adminUserRepository);
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<AdminUser>(dto);
            entity.Salt = SecurityHelper.GenerateSalt();
            entity.Password = SecurityHelper.GenerateHash(dto.Password, entity.Salt);

            _adminUserRepository.Insert(entity);

            foreach (var roleId in dto.Roles)
                entity.Roles.Add(_adminRoleRepository.Find(roleId));

            _unitOfWork.Commit();
            AdminUserCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public AdminUserDto GetById(int id)
        {
            var entity = _adminUserRepository.AsNoTracking
                .Include(i => i.Roles)
                .FirstOrDefault(s => s.Id == id);

            return entity == null ? null : Mapper.Map<AdminUserDto>(entity);
        }

        public Result Edit(int id, AdminUserDto dto)
        {
            dto.Id = id;

            var validator = new AdminUserValidator(_adminUserRepository);
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = _adminUserRepository.AsNoTracking.Include(i => i.Roles)
                .FirstOrDefault(s => s.Id == dto.Id);

            if (entity == null)
                return new Result().SetBlankRedirect();

            Mapper.Map<AdminUser>(dto, entity);

            if (!string.IsNullOrEmpty(dto.Password))
            {
                entity.Salt = SecurityHelper.GenerateSalt();
                entity.Password = SecurityHelper.GenerateHash(dto.Password, entity.Salt);
            }

            _adminUserRepository.Update(entity);

            ChildRoleUpdate(entity, dto);

            _unitOfWork.Commit();
            AdminUserCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }

        private void ChildRoleUpdate(AdminUser entity, AdminUserDto dto)
        {
            var currentRecords = entity.Roles.Select(s => s.Id).ToList();

            var addedRecords = dto.Roles.Except(currentRecords).ToList();
            foreach (var record in addedRecords)
                entity.Roles.Add(_adminRoleRepository.Find(record));

            var deletedRecords = currentRecords.Except(dto.Roles).ToList();
            foreach (var record in deletedRecords)
                entity.Roles.Remove(entity.Roles.First(w => w.Id == record));
        }

        public IList<string> GetPermissions(int userId)
        {
            var permissions = _adminUserRepository.AsNoTracking
                .Include(i => i.Roles.Select(s => s.Permissionses))
                .DeferredFirst(w => w.Id == userId)
                .FromCache("AdminPermissions", "AdminRoles", "AdminUsers")
                .Roles.SelectMany(s => s.Permissionses)
                .OrderBy(o => o.Left)
                .Select(s => s.Name.ToLower())
                .ToList();

            return permissions;
        }

        public IList<string> GetRoles(int userId)
        {
            var roles = _adminUserRepository.AsNoTracking
                .Include(i => i.Roles)
                .DeferredFirst(w => w.Id == userId)
                .FromCache("AdminRoles", "AdminUsers")
                .Roles.Select(s => s.SystemName.ToLower())
                .ToList();

            return roles;
        }
    }
}