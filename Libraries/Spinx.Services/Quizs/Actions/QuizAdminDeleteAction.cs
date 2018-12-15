using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Quizs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.Quizs.Actions
{
    public class QuizAdminDeleteAction : BaseAction
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuizAdminDeleteAction(
            IQuizRepository quizRepository,
            IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _quizRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _quizRepository.Delete(entity);

            _unitOfWork.Commit();
            QuizCacheManager.ClearCache();

            return result;
        }
    }
}
