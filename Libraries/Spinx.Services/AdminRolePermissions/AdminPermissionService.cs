using Newtonsoft.Json.Linq;
using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.Actions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Services.AdminRolePermissions.Filters;
using Spinx.Services.AdminRolePermissions.Mappers;
using Spinx.Services.AdminRolePermissions.Validators;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions
{
    public interface IAdminPermissionService
    {
        Result List(AdminPermissionFilterDto dto);
        Result Create(AdminPermissionDto dto);
        AdminPermissionDto GetById(int id);
        Result Edit(AdminPermissionDto dto);

        IEnumerable<AdminPermissionDropdownDto> GetAdminPermissions();

        IEnumerable<AdminPermission> GetSequenceData();
        void SaveSequenceData(string data);
    }

    public class AdminPermissionService : IAdminPermissionService
    {
        private readonly IAdminPermissionRepository _adminPermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminPermissionService(
            IAdminPermissionRepository adminPermissionRepository,
            IUnitOfWork unitOfWork)
        {
            _adminPermissionRepository = adminPermissionRepository;
            _unitOfWork = unitOfWork;

            AdminPermissionMapper.Init();
        }

        public Result List(AdminPermissionFilterDto dto)
        {
            var result =
                new AdminPermissionActionFactory(_adminPermissionRepository, _unitOfWork).ExecuteAction(dto);

            if (!result.Success)
                return result;

            var query = _adminPermissionRepository.AsNoTracking;
            query = new AdminPermissionFilter(query, dto).FilteredQuery();
            query = query.OrderBy(o => o.Left);
            result.SetPaging(dto.Page, dto.Size, query.Count());

            result.Data = query.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DisplayName,
                    Depth = _adminPermissionRepository.AsNoTracking.Count(w => w.Left < x.Left && w.Right > x.Right),
                })
                .ToPaged(result.Paging.Page, result.Paging.Size)
                .ToList();

            return result;
        }

        public Result Create(AdminPermissionDto dto)
        {
            var validator = new AdminPermissionValidator(_adminPermissionRepository);
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<AdminPermission>(dto);

            _adminPermissionRepository.Insert(entity);
            
            _unitOfWork.Commit();
            AdminRoleCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public AdminPermissionDto GetById(int id)
        {
            var entity = _adminPermissionRepository.Find(id);

            return entity == null ? null : Mapper.Map<AdminPermissionDto>(entity);
        }

        public Result Edit(AdminPermissionDto dto)
        {
            var validator = new AdminPermissionValidator(_adminPermissionRepository);
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = _adminPermissionRepository.Find(dto.Id);

            if (entity == null) return new Result().SetBlankRedirect();

            Mapper.Map<AdminPermission>(dto, entity);

            _adminPermissionRepository.Update(entity);

            _unitOfWork.Commit();

            _adminPermissionRepository.MoveToParentNode("AdminPermissions", dto.Id,
                dto.IsParentSelected ? dto.ParentId : null);

            AdminRoleCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }

        public IEnumerable<AdminPermissionDropdownDto> GetAdminPermissions()
        {
            return _adminPermissionRepository.AsNoTracking
                .OrderBy(o => o.Left)
                .Select(s => new AdminPermissionDropdownDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DisplayName = s.DisplayName,
                    Depth = _adminPermissionRepository.AsNoTracking.Count(w => w.Left < s.Left && w.Right > s.Right),
                    Left = s.Left,
                    Right = s.Right
                }).ToList();
        }

        #region Sequence

        public IEnumerable<AdminPermission> GetSequenceData()
        {
            return _adminPermissionRepository.AsNoTracking
                .Where(w => w.ParentId == null)
                .OrderBy(o => o.Left)
                .ToList();
        }

        public void SaveSequenceData(string data)
        {
            var adminPermissions = _adminPermissionRepository.AsNoTracking.ToList();

            var jsonData = JArray.Parse(data);

            var sequance = 1;
            SetPermissionTree(ref adminPermissions, jsonData, null, ref sequance);
            _unitOfWork.Commit();
            AdminPermissionCacheManager.ClearCache();
        }

        private void SetPermissionTree(ref List<AdminPermission> adminPermissions, JToken jArray, int? parentId, ref int sequance)
        {
            foreach (var single in jArray)
            {
                var adminPermission = adminPermissions.First(w => w.Id == (int) single["id"]);
                adminPermission.Left = sequance++;
                adminPermission.ParentId = parentId;

                if (single["children"] != null)
                {
                    SetPermissionTree(ref adminPermissions, single["children"], adminPermission.Id, ref sequance);
                }

                adminPermission.Right = sequance++;
                _adminPermissionRepository.Update(adminPermission);
            }
        }

        #endregion
    }
}