using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Pages;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.Pages.Actions
{
    public class PageAdminInactiveAction : BaseAction
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PageAdminInactiveAction(
            IPageRepository pageRepository, 
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _pageRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            var result = new Result().SetSuccess(string.Format(Messages.RecordInactivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = false;
                _pageRepository.Update(entity);
            }

            _unitOfWork.Commit();
            PageCacheManager.ClearCache();

            return result;
        }
    }
}
