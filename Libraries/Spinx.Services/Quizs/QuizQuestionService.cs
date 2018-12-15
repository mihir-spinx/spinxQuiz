using Spinx.Data.Repository.QuizQuestions;
using Spinx.Domain.QuizQuestions;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;
using Spinx.Services.QuizQuestionsList.DTOs;
using Spinx.Data.Repository.QuizAnswers;

namespace Spinx.Services.QuizQuestions
{
    public interface IQuizQuestionService
    {
        IEnumerable<QuizQuestion> GetCachedQuizQuestions();
        List<QuizQuestionListDto> GetQuizQuestionsByQuizId(int quizId);
    }

    public class QuizQuestionService : IQuizQuestionService
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IQuizAnswerRepository _quizAnswerRepository;

        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository, IQuizAnswerRepository quizAnswerRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _quizAnswerRepository = quizAnswerRepository;
        }

        public IEnumerable<QuizQuestion> GetCachedQuizQuestions()
        {
            return _quizQuestionRepository.AsNoTracking
                .FromCache("QuizQuestions")
                .ToList();
        }

        public List<QuizQuestionListDto> GetQuizQuestionsByQuizId(int quizId)
        {
            var query = _quizQuestionRepository.AsNoTracking;
            query = query.Where(w => w.QuizId == quizId);
            query = query.Where(w => w.IsActive);
            query = query.OrderBy(s => s.Question);

            var result = query.Where(w => w.IsActive)
              .OrderBy(o => o.SortOrder)
              .Select(s => new QuizQuestionListDto
              {
                  Question = s.Question,
                  QuizAnswers = _quizAnswerRepository.AsNoTracking.Where(x => x.QuizQuestionId == s.Id).OrderBy(y=>y.SortOrder).ToList()
              })
              .ToList();
            return result;
        }
    }
}