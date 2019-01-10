using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.GeneralSettings;
using Spinx.Domain.GeneralSettings;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.GeneralSettings.Actions;
using Spinx.Services.GeneralSettings.DTOs;
using Spinx.Services.GeneralSettings.Filters;
using Spinx.Services.GeneralSettings.ListOrders;
using Spinx.Services.GeneralSettings.Mappers;
using Spinx.Services.GeneralSettings.Validators;
using System.Linq;

namespace Spinx.Services.GeneralSettings
{
    public interface IGeneralSettingAdminService
    {
        Result List(GeneralSettingAdminFilterDto dto);
        Result Create(GeneralSettingCreateAdminDto dto);
        GeneralSettingEditAdminDto GetById(int id);
        Result Edit(int id, GeneralSettingEditAdminDto dto);
    }

    public class GeneralSettingAdminService : IGeneralSettingAdminService
    {
        private readonly IGeneralSettingRepository _GeneralSettingRepository;
        private readonly GeneralSettingAdminActionFactory _actionFactory;
        private readonly GeneralSettingCreateAdminValidator _validatorCreate;
        private readonly GeneralSettingEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public GeneralSettingAdminService(
            IGeneralSettingRepository GeneralSettingRepository,
            GeneralSettingAdminActionFactory actionFactory,
            GeneralSettingCreateAdminValidator validatorCreate,
            GeneralSettingEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _GeneralSettingRepository = GeneralSettingRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            GeneralSettingAdminMapper.Init();
        }

        public Result List(GeneralSettingAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _GeneralSettingRepository.AsNoTracking;
            query = new GeneralSettingAdminFilter(query, dto).FilteredQuery();
            query = new GeneralSettingAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Value
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(GeneralSettingCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<GeneralSetting>(dto);

            _GeneralSettingRepository.Insert(entity);
            _unitOfWork.Commit();

            GeneralSettingCacheManager.ClearCache();
            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public GeneralSettingEditAdminDto GetById(int id)
        {
            var entity = _GeneralSettingRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<GeneralSettingEditAdminDto>(entity);
        }

        public Result Edit(int id, GeneralSettingEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);

            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _GeneralSettingRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");

                Mapper.Map<GeneralSetting>(dto, entity);
                _GeneralSettingRepository.Update(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<GeneralSetting>(dto);
                _GeneralSettingRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            GeneralSettingCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}