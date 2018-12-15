using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.QuizQuestions;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.QuizQuestions.Actions
{
    public class QuizQuestionAdminActiveAction : BaseAction
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionAdminActiveAction(
            IQuizQuestionRepository quizQuestionRepository, 
            IUnitOfWork unitOfWork)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _quizQuestionRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordActivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = true;
                _quizQuestionRepository.Update(entity);
            }

            _unitOfWork.Commit();
            QuizQuestionCacheManager.ClearCache();

            return result;
        }
    }
}