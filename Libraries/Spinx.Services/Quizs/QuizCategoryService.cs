using Spinx.Core;
using Spinx.Data.Repository.Quizs;
using Spinx.Domain.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.QuizCategories
{
    public interface IQuizCategoryService
    {
        IEnumerable<QuizCategory> GetCachedQuizCategories();
        List<QuizCategoryListDto> Get();
        Result List(QuizCategory dto);
        QuizCategory GetQuizCategoryById(int id);

    }

    public class QuizCategoryService : IQuizCategoryService
    {
        private readonly IQuizCategoryRepository _quizCategoryRepository;

        public QuizCategoryService(IQuizCategoryRepository quizCategoryRepository)
        {
            _quizCategoryRepository = quizCategoryRepository;
        }

        public IEnumerable<QuizCategory> GetCachedQuizCategories()
        {
            return _quizCategoryRepository.AsNoTracking
                .FromCache("QuizCategories")
                .ToList();
        }
        public List<QuizCategoryListDto> Get()
        {
            return _quizCategoryRepository.AsNoTracking
                .Where(w => w.IsActive)
                .OrderBy(o => o.Name)
                .Select(s => new QuizCategoryListDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .FromCache(QuizCategoryCacheManager.Name)
                .ToList();
        }
        public Result List(QuizCategory dto)
        {
            var result = new Result();
            var query = _quizCategoryRepository.Table
             .Include(s => s.Quizs);

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                query = query.Where(w => w.Name.Contains(dto.Name) || w.Quizs.Count > 1);
            }
            else
                dto.Name = string.Empty;
            query = query.Where(w => w.IsActive);
            query = query.OrderBy(s => s.Name);

            result.Data = query.Select(s => new
            {
                s.Name,
                s.CategoryIcon,
                Quizs = s.Quizs.Where(w => w.IsActive && w.Title.Contains(dto.Name)).OrderBy(o => o.Title)
                            .Select(x => new { x.Title, x.Slug }).ToList()

            }).ToList();

            return result;
        }
        public QuizCategory GetQuizCategoryById(int id)
        {
            return _quizCategoryRepository.AsNoTracking
                .Where(w => w.Id == id && w.IsActive)
                .DeferredFirstOrDefault()
                .FromCache(QuizCategoryCacheManager.Name);
        }
    }
}