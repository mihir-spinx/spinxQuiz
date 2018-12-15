using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Quizs;
using Spinx.Domain.Quizs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Quizs.Actions;
using Spinx.Services.Quizs.DTOs;
using Spinx.Services.Quizs.Filters;
using Spinx.Services.Quizs.ListOrders;
using Spinx.Services.Quizs.Mappers;
using Spinx.Services.Quizs.Validators;
using System.Linq;

namespace Spinx.Services.Quizs
{
    public interface IQuizAdminService
    {
        Result List(QuizAdminFilterDto dto);
        Result Create(QuizCreateAdminDto dto);
        QuizEditAdminDto GetById(int id);
        Result Edit(int id, QuizEditAdminDto dto);
    }

    public class QuizAdminService : IQuizAdminService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly QuizAdminActionFactory _actionFactory;
        private readonly QuizCreateAdminValidator _validatorCreate;
        private readonly QuizEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public QuizAdminService(
            IQuizRepository quizRepository,
            QuizAdminActionFactory actionFactory,
            QuizCreateAdminValidator validatorCreate,
            QuizEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            QuizAdminMapper.Init();
        }

        public Result List(QuizAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _quizRepository.AsNoTracking;

            query = new QuizAdminFilter(query, dto).FilteredQuery();
            query = new QuizAdminListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());            

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Title,                    
                    QuizCategoryName = s.QuizCategory.Name,
                    s.IsActive
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(QuizCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<Quiz>(dto);
            entity.Slug = _quizRepository.GenerateUniqueSlug(entity.Title);
            _quizRepository.Insert(entity);

            _unitOfWork.Commit();  

            QuizCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public QuizEditAdminDto GetById(int id)
        {
            var entity = _quizRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<QuizEditAdminDto>(entity);
        }

        public Result Edit(int id, QuizEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _quizRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh.");
                
                Mapper.Map<Quiz>(dto, entity);
                _quizRepository.Update(entity);

                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {               
                var entity = Mapper.Map<Quiz>(dto);
                _quizRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            QuizCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}