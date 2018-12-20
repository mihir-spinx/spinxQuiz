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
        List<QuizQuestionCountDto> GetQuizQuestionsByQuizId(int quizId);
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

        public List<QuizQuestionCountDto> GetQuizQuestionsByQuizId(int quizId)
        {
            var query = _quizQuestionRepository.AsNoTracking;
            query = query.Where(w => w.QuizId == quizId);
            query = query.Where(w => w.IsActive);
            query = query.OrderBy(s => s.SortOrder);

            var result = query.Select(s => new QuizQuestionCountDto
              {
                 Id = s.Id,
                 SortOrder = s.SortOrder
              })
              .ToList();
            return result;
        }
    }
}