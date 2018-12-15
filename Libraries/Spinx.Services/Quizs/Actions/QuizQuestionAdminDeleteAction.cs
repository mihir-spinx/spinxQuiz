using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.QuizQuestions;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.QuizQuestions.Actions
{
    public class QuizQuestionAdminDeleteAction : BaseAction
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionAdminDeleteAction(
            IQuizQuestionRepository quizQuestionRepository,
            IUnitOfWork unitOfWork)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _quizQuestionRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _quizQuestionRepository.Delete(entity);

            _unitOfWork.Commit();
            QuizQuestionCacheManager.ClearCache();

            return result;
        }
    }
}