using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Pages;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.Pages.Actions
{
    public class PageAdminActiveAction : BaseAction
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PageAdminActiveAction(
            IPageRepository pageRepository, 
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _pageRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            var result = new Result().SetSuccess(string.Format(Messages.RecordActivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = true;
                _pageRepository.Update(entity);
            }

            _unitOfWork.Commit();
            PageCacheManager.ClearCache();

            return result;
        }
    }
}
