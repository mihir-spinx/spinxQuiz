using Spinx.Data.Repository.Quizs;
using Spinx.Domain.Quizs;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.Quizs
{
    public interface IQuizService
    {
        IEnumerable<Quiz> GetCachedQuizs();
        Quiz GetQuizBySlug(string slug);
    }

    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public IEnumerable<Quiz> GetCachedQuizs()
        {
            return _quizRepository.AsNoTracking
                .FromCache("Quizs")
                .ToList();
        }
        public Quiz GetQuizBySlug(string slug)
        {
            return _quizRepository.AsNoTracking
                .Where(w => w.Slug == slug && w.IsActive)
                .DeferredFirstOrDefault()
                .FromCache(QuizCacheManager.Name);
        }
    }
}