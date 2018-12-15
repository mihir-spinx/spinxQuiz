using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.EmailTemplates;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.EmailTemplates.Actions
{
    public class EmailTemplateAdminDeleteAction : BaseAction
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailTemplateAdminDeleteAction(
            IEmailTemplateRepository emailTemplateRepository,
            IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _emailTemplateRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _emailTemplateRepository.Delete(entity);

            _unitOfWork.Commit();
            EmailTemplateCacheManager.ClearCache();

            return result;
        }
    }
}
