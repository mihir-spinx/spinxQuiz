using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.QuizAnswers;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.QuizAnswers.Actions
{
    public class QuizAnswerAdminDeleteAction : BaseAction
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuizAnswerAdminDeleteAction(
            IQuizAnswerRepository quizAnswerRepository,
            IUnitOfWork unitOfWork)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _quizAnswerRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _quizAnswerRepository.Delete(entity);

            _unitOfWork.Commit();
            QuizAnswerCacheManager.ClearCache();

            return result;
        }
    }
}