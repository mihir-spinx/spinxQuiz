using Spinx.Data.Repository.QuizAnswers;
using Spinx.Domain.QuizAnswers;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.Quizs
{
    public interface IQuizAnswerService
    {
        IEnumerable<QuizAnswer> GetCachedQuizAnswers();
    }

    public class QuizAnswerService : IQuizAnswerService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository)
        {
            _quizAnswerRepository = quizAnswerRepository;
        }

        public IEnumerable<QuizAnswer> GetCachedQuizAnswers()
        {
            return _quizAnswerRepository.AsNoTracking
                .FromCache("QuizAnswers")
                .ToList();
        }
    }
}