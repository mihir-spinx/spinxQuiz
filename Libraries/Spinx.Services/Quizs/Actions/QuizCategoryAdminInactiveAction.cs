using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Quizs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.QuizCategories.Actions
{
    public class QuizCategoryAdminInactiveAction : BaseAction
    {
        private readonly IQuizCategoryRepository _quizCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuizCategoryAdminInactiveAction(
            IQuizCategoryRepository quizCategoryRepository, 
            IUnitOfWork unitOfWork)
        {
            _quizCategoryRepository = quizCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _quizCategoryRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordInactivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = false;
                _quizCategoryRepository.Update(entity);
            }

            _unitOfWork.Commit();
            QuizCategoryCacheManager.ClearCache();

            return result;
        }
    }
}