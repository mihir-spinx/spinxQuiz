using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Spinx.Data.Repository.SeoPages;

namespace Spinx.Services.SeoPages.Actions
{
    public class SeoPageAdminDeleteAction : BaseAction
    {
        private readonly ISeoPageRepository _seoPageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SeoPageAdminDeleteAction(
                    ISeoPageRepository seoPageRepository,
                    IUnitOfWork unitOfWork)
        {
            _seoPageRepository = seoPageRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _seoPageRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _seoPageRepository.Delete(entity);

            _unitOfWork.Commit();
            SeoPageCacheManager.ClearCache();

            return result;
        }
    }
}
