using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.Actions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Services.AdminRolePermissions.Filters;
using Spinx.Services.AdminRolePermissions.ListOrders;
using Spinx.Services.AdminRolePermissions.Mappers;
using Spinx.Services.AdminRolePermissions.Validators;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.AdminRolePermissions
{
    public interface IAdminRoleService
    {
        Result List(AdminRoleFilterDto dto);
        Result Create(AdminRoleDto dto);
        AdminRoleDto GetById(int id);
        Result Edit(AdminRoleDto dto);

        List<AdminRoleListDto> GetAll();
    }

    public class AdminRoleService : IAdminRoleService
    {
        private readonly IAdminRoleRepository _adminRoleRepository;
        private readonly IAdminPermissionRepository _adminPermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminRoleService(
            IAdminRoleRepository adminRoleRepository,
            IAdminPermissionRepository adminPermissionRepository,
            IUnitOfWork unitOfWork)
        {
            _adminRoleRepository = adminRoleRepository;
            _adminPermissionRepository = adminPermissionRepository;
            _unitOfWork = unitOfWork;

            AdminRoleMapper.Init();
        }

        public Result List(AdminRoleFilterDto dto)
        {
            var result =
                new AdminRoleActionFactory(_adminRoleRepository, _unitOfWork).ExecuteAction(dto);

            if (!result.Success)
                return result;

            var query = _adminRoleRepository.AsNoTracking;
            query = new AdminRoleFilter(query, dto).FilteredQuery();
            query = new AdminRoleListOrder(query, dto).OrderByQuery();
            result.SetPaging(dto.Page, dto.Size, query.Count());

            result.Data = query.Select(s => new
            {
                s.Id,
                s.Name,
                UsersCount = s.Users.Count
            })
                .ToPaged(result.Paging.Page, result.Paging.Size)
                .ToList();

            return result;
        }

        public Result Create(AdminRoleDto dto)
        {
            var validator = new AdminRoleValidator();
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<AdminRole>(dto);
            entity.SystemName = _adminRoleRepository.GenerateUniqueSlug(entity.Name, slugFieldName: "SystemName");

            _adminRoleRepository.Insert(entity);

            foreach (var permissionId in dto.Permissions)
                entity.Permissionses.Add(_adminPermissionRepository.Find(permissionId));

            _unitOfWork.Commit();
            AdminRoleCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess("Record has been successfully saved.");
        }

        public AdminRoleDto GetById(int id)
        {
            var entity = _adminRoleRepository.AsNoTracking
                .Include(i => i.Permissionses)
                .FirstOrDefault(s => s.Id == id);

            return entity == null ? null : Mapper.Map<AdminRoleDto>(entity);
        }

        public Result Edit(AdminRoleDto dto)
        {
            var validator = new AdminRoleValidator();
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = _adminRoleRepository.AsNoTracking
                .Include(i => i.Permissionses)
                .FirstOrDefault(s => s.Id == dto.Id);

            if (entity == null)
                return new Result().SetBlankRedirect();

            Mapper.Map<AdminRole>(dto, entity);
            entity.SystemName = _adminRoleRepository.GenerateUniqueSlug(entity.Name, slugFieldName: "SystemName", id: dto.Id);

            _adminRoleRepository.Update(entity);

            ChildRoleUpdate(entity, dto);

            _unitOfWork.Commit();
            AdminRoleCacheManager.ClearCache();

            return result.SetSuccess("Record has been successfully saved.");
        }

        private void ChildRoleUpdate(AdminRole entity, AdminRoleDto dto)
        {
            var currentRecords = entity.Permissionses.Select(s => s.Id).ToList();

            var addedRecords = dto.Permissions.Except(currentRecords).ToList();
            foreach (var record in addedRecords)
                entity.Permissionses.Add(_adminPermissionRepository.Find(record));

            var deletedRecords = currentRecords.Except(dto.Permissions).ToList();
            foreach (var record in deletedRecords)
                entity.Permissionses.Remove(entity.Permissionses.First(w => w.Id == record));
        }

        public List<AdminRoleListDto> GetAll()
        {
            return _adminRoleRepository.AsNoTracking
                .OrderBy(o => o.Name)
                .Select(s => new AdminRoleListDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .FromCache("AdminRoles")
                .ToList();
        }
    }
}