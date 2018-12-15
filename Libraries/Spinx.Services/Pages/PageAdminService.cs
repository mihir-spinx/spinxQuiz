using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Pages;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Pages.Actions;
using Spinx.Services.Pages.DTOs;
using Spinx.Services.Pages.Filters;
using Spinx.Services.Pages.ListOrders;
using Spinx.Services.Pages.Mappers;
using Spinx.Services.Pages.Validators;
using System.Collections.Generic;
using Spinx.Domain.Pages;
using System.Linq;

namespace Spinx.Services.Pages
{
    public interface IPageAdminService
    {
        Result List(PageAdminFilterDto dto);
        Result Create(PageCreateAdminDto dto);
        PageEditAdminDto GetById(int Id);
        Result Edit(int Id, PageEditAdminDto dto);
    }

    public class PageAdminService : BaseService, IPageAdminService
    {
        private readonly IPageRepository _pageRepository;        
        private readonly PageAdminActionFactory _actionFactory;
        private readonly PageCreateAdminValidator _validatorCreate;
        private readonly PageEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public PageAdminService(
            IPageRepository pageRepository,        
            PageAdminActionFactory actionFactory,
            PageCreateAdminValidator validatorCreate,
            PageEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;            
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;

            PageAdminMapper.Init();
        }

        public Result List(PageAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _pageRepository.AsNoTracking;
            query = new PageAdminFilter(query, dto).FilteredQuery();
            query = new PageAdminListOrder(query, dto).OrderByQuery();
            result.SetPaging(dto.Page, dto.Size, query.Count());

            result.Data = query.Select(s => new
            {
                s.Id,
                s.Title,                
                s.IsActive                
            })
            .ToPaged(result.Paging.Page, result.Paging.Size)
            .ToList();

            return result;
        }

        public Result Create(PageCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;


            var entity = Mapper.Map<Page>(dto);
            _pageRepository.Insert(entity);
            _unitOfWork.Commit();

            result.Id = entity.Id;
            PageCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }

        public PageEditAdminDto GetById(int Id)
        {
            var entity = _pageRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == Id);

            return entity == null ? null : Mapper.Map<PageEditAdminDto>(entity);
        }

        public Result Edit(int Id, PageEditAdminDto dto)
        {
            dto.Id = Id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;            

            var entity = _pageRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

            if (entity == null)
                return result.SetError("There are error for update record. Please try again with refresh.");

            Mapper.Map<Page>(dto, entity);
            _pageRepository.Update(entity);
            _unitOfWork.Commit();

            result.Id = entity.Id;

            PageCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}
