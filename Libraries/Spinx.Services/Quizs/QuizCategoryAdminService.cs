using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Quizs;
using Spinx.Domain.QuizCategories;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.QuizCategories.Actions;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Services.QuizCategories.Filters;
using Spinx.Services.QuizCategories.ListOrders;
using Spinx.Services.QuizCategories.Mappers;
using Spinx.Services.QuizCategories.Validators;
using System.Linq;

namespace Spinx.Services.QuizCategories
{
    public interface IQuizCategoryAdminService
    {
        Result List(QuizCategoryAdminFilterDto dto);
        Result Create(QuizCategoryCreateAdminDto dto);
        QuizCategoryEditAdminDto GetById(int id);
        Result Edit(int id, QuizCategoryEditAdminDto dto);
    }

    public class QuizCategoryAdminService : IQuizCategoryAdminService
    {
        private readonly IQuizCategoryRepository _quizCategoryRepository;
        private readonly QuizCategoryAdminActionFactory _actionFactory;
        private readonly QuizCategoryCreateAdminValidator _validatorCreate;
        private readonly QuizCategoryEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public QuizCategoryAdminService(
            IQuizCategoryRepository quizCategoryRepository,
            QuizCategoryAdminActionFactory actionFactory,
            QuizCategoryCreateAdminValidator validatorCreate,
            QuizCategoryEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _quizCategoryRepository = quizCategoryRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            QuizCategoryAdminMapper.Init();
        }

        public Result List(QuizCategoryAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _quizCategoryRepository.AsNoTracking;
            query = new QuizCategoryAdminFilter(query, dto).FilteredQuery();
            query = new QuizCategoryAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.IsActive,
                    QuizCount = s.Quizs.Where(w => w.IsActive && w.QuizCategoryId == s.Id).ToList().Count
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(QuizCategoryCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<QuizCategory>(dto);
            entity.IsActive = true;
            entity.Slug = _quizCategoryRepository.GenerateUniqueSlug(entity.Name);
            _quizCategoryRepository.Insert(entity);

            _unitOfWork.Commit();

            QuizCategoryCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public QuizCategoryEditAdminDto GetById(int id)
        {
            var entity = _quizCategoryRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<QuizCategoryEditAdminDto>(entity);
        }

        public Result Edit(int id, QuizCategoryEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _quizCategoryRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");

                Mapper.Map<QuizCategory>(dto, entity);
                _quizCategoryRepository.Update(entity);

                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<QuizCategory>(dto);
                _quizCategoryRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            QuizCategoryCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}