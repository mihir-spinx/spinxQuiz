using MoreLinq;
using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.QuizQuestions;
using Spinx.Domain.QuizQuestions;
using Spinx.Services.Content;
using Spinx.Services.QuizQuestions.Actions;
using Spinx.Services.QuizQuestions.DTOs;
using Spinx.Services.QuizQuestions.Filters;
using Spinx.Services.QuizQuestions.ListOrders;
using Spinx.Services.QuizQuestions.Validators;
using Spinx.Services.QuizQuestions.Mappers;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizQuestions
{
    public interface IQuizQuestionAdminService
    {
        Result List(QuizQuestionAdminFilterDto dto);
        Result Create(QuizQuestionCreateAdminDto dto);
        QuizQuestionEditAdminDto GetById(int id);
        Result Edit(int id, QuizQuestionEditAdminDto dto);
        void SaveSortOrder(int[] ids);
    }

    public class QuizQuestionAdminService : IQuizQuestionAdminService
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionAdminActionFactory _actionFactory;
        private readonly QuizQuestionCreateAdminValidator _validatorCreate;
        private readonly QuizQuestionEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionAdminService(
            IQuizQuestionRepository quizQuestionRepository,
            QuizQuestionAdminActionFactory actionFactory,
            QuizQuestionCreateAdminValidator validatorCreate,
            QuizQuestionEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            QuizQuestionAdminMapper.Init();
        }

        public Result List(QuizQuestionAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _quizQuestionRepository.AsNoTracking;
            query = new QuizQuestionAdminFilter(query, dto).FilteredQuery();
            query = new QuizQuestionAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Question,
                    s.QuizId,
                    QuizTitle = s.Quiz.Title,
                    s.IsActive
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(QuizQuestionCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<QuizQuestion>(dto);
            entity.IsActive = true;

            _quizQuestionRepository.Insert(entity);
            _unitOfWork.Commit();

            QuizQuestionCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public QuizQuestionEditAdminDto GetById(int id)
        {
            var entity = _quizQuestionRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<QuizQuestionEditAdminDto>(entity);
        }

        public Result Edit(int id, QuizQuestionEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _quizQuestionRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");

                Mapper.Map<QuizQuestion>(dto, entity);
                _quizQuestionRepository.Update(entity);

                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<QuizQuestion>(dto);
                _quizQuestionRepository.Insert(entity);

                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            QuizQuestionCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
        public void SaveSortOrder(int[] ids)
        {
            var entities = _quizQuestionRepository.AsNoTracking
                .Where(w => ids.Contains(w.Id))
                .ToList();

            for (var index = 0; index < ids.Length; index++)
            {
                var id = ids[index];
                var sortOrder = index + 1;
                entities.Where(w => w.Id == id).ForEach(ac => ac.SortOrder = sortOrder);
            }

            _quizQuestionRepository.Update(entities);
            _unitOfWork.Commit();

            QuizQuestionCacheManager.ClearCache();
        }
    }
}