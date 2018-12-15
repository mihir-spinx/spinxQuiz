using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.ContactUsInquiries;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.ContactUsInquiries.Actions
{
    public class ContactUsInquiryAdminDeleteAction : BaseAction
    {
        private readonly IContactUsInquiryRepository _contactUsInquiryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsInquiryAdminDeleteAction(
                    IContactUsInquiryRepository contactUsInquiryRepository,
                    IUnitOfWork unitOfWork)
        {
            _contactUsInquiryRepository = contactUsInquiryRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _contactUsInquiryRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _contactUsInquiryRepository.Delete(entity);

            _unitOfWork.Commit();
            ContactUsInquiryCacheManager.ClearCache();

            return result;
        }
    }
}
