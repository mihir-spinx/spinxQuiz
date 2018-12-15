using MoreLinq;
using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.QuizAnswers;
using Spinx.Domain.QuizAnswers;
using Spinx.Services.Content;
using Spinx.Services.QuizAnswers.Actions;
using Spinx.Services.QuizAnswers.DTOs;
using Spinx.Services.QuizAnswers.Filters;
using Spinx.Services.QuizAnswers.ListOrders;
using Spinx.Services.QuizAnswers.Validators;
using Spinx.Services.QuizAnswers.Mappers;
using Spinx.Services.Infrastructure;
using System.Linq;
using Z.EntityFramework.Plus;
using System;

namespace Spinx.Services.QuizAnswers
{
    public interface IQuizAnswerAdminService
    {
        Result List(QuizAnswerAdminFilterDto dto);
        Result Create(QuizAnswerCreateAdminDto dto);
        QuizAnswerEditAdminDto GetById(int id);
        Result Edit(int id, QuizAnswerEditAdminDto dto);
        void SaveSortOrder(int[] ids);
    }

    public class QuizAnswerAdminService : IQuizAnswerAdminService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly QuizAnswerAdminActionFactory _actionFactory;
        private readonly QuizAnswerCreateAdminValidator _validatorCreate;
        private readonly QuizAnswerEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public QuizAnswerAdminService(
            IQuizAnswerRepository quizAnswerRepository,
            QuizAnswerAdminActionFactory actionFactory,
            QuizAnswerCreateAdminValidator validatorCreate,
            QuizAnswerEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            QuizAnswerAdminMapper.Init();
        }

        public Result List(QuizAnswerAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            if (dto.Action == "markanaswer")
            {
                var questionId = dto.Ids[1];
                var queryAllAnswer = _quizAnswerRepository.AsNoTracking.Where(x => x.QuizQuestionId == questionId);
                queryAllAnswer.Update(u => new QuizAnswer { IsCorrectAnswer = false });

                var answerId = dto.Ids[0];
                var queryAnswer = _quizAnswerRepository.AsNoTracking.Where(x => x.Id == answerId);
                queryAnswer.Update(u => new QuizAnswer { IsCorrectAnswer = true });
            }

            var query = _quizAnswerRepository.AsNoTracking;
            query = new QuizAnswerAdminFilter(query, dto).FilteredQuery();
            query = new QuizAnswerAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Answer,
                    s.IsCorrectAnswer,
                    s.QuizQuestionId
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(QuizAnswerCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<QuizAnswer>(dto);

            _quizAnswerRepository.Insert(entity);
            _unitOfWork.Commit();

            QuizAnswerCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public QuizAnswerEditAdminDto GetById(int id)
        {
            var entity = _quizAnswerRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<QuizAnswerEditAdminDto>(entity);
        }

        public Result Edit(int id, QuizAnswerEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _quizAnswerRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");
                var query = _quizAnswerRepository.AsNoTracking.Where(x => x.QuizQuestionId == entity.QuizQuestionId);

                if (dto.IsCorrectAnswer)
                    query.Update(u => new QuizAnswer { IsCorrectAnswer = false });

                Mapper.Map<QuizAnswer>(dto, entity);
                _quizAnswerRepository.Update(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<QuizAnswer>(dto);
                _quizAnswerRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            QuizAnswerCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
        public void SaveSortOrder(int[] ids)
        {
            var entities = _quizAnswerRepository.AsNoTracking
                .Where(w => ids.Contains(w.Id))
                .ToList();

            for (var index = 0; index < ids.Length; index++)
            {
                var id = ids[index];
                var sortOrder = index + 1;
                entities.Where(w => w.Id == id).ForEach(ac => ac.SortOrder = sortOrder);
            }

            _quizAnswerRepository.Update(entities);
            _unitOfWork.Commit();

            QuizAnswerCacheManager.ClearCache();
        }
    }
}