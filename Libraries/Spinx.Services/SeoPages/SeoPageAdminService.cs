using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.SeoPages;
using Spinx.Domain.SeoPages;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.SeoPages.Actions;
using Spinx.Services.SeoPages.DTOs;
using Spinx.Services.SeoPages.Filters;
using Spinx.Services.SeoPages.ListOrders;
using Spinx.Services.SeoPages.Mappers;
using Spinx.Services.SeoPages.Validators;
using System.Linq;

namespace Spinx.Services.SeoPages
{
    public interface ISeoPageAdminService
    {
        Result List(SeoPageAdminFilterDto dto);
        Result Create(SeoPageCreateAdminDto dto);
        SeoPageEditAdminDto GetById(int id);
        Result Edit(int id, SeoPageEditAdminDto dto);
    }

    public class SeoPageAdminService : ISeoPageAdminService
    {
        private readonly ISeoPageRepository _seoPageRepository;
        private readonly SeoPageAdminActionFactory _actionFactory;
        private readonly SeoPageCreateAdminValidator _validatorCreate;
        private readonly SeoPageEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public SeoPageAdminService(
            ISeoPageRepository seoPageRepository,
            SeoPageAdminActionFactory actionFactory,
            SeoPageCreateAdminValidator validatorCreate,
            SeoPageEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _seoPageRepository = seoPageRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            SeoPageAdminMapper.Init();
        }

        public Result List(SeoPageAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _seoPageRepository.AsNoTracking;
            query = new SeoPageAdminFilter(query, dto).FilteredQuery();
            query = new SeoPageAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Name
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(SeoPageCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<SeoPage>(dto);

            _seoPageRepository.Insert(entity);
            _unitOfWork.Commit();

            SeoPageCacheManager.ClearCache();
            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public SeoPageEditAdminDto GetById(int id)
        {
            var entity = _seoPageRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<SeoPageEditAdminDto>(entity);
        }

        public Result Edit(int id, SeoPageEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);

            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _seoPageRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");

                Mapper.Map<SeoPage>(dto, entity);
                _seoPageRepository.Update(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<SeoPage>(dto);
                _seoPageRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            SeoPageCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}