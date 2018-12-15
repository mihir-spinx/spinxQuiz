using Spinx.Core;
using Spinx.Data.Repository.ContactUsInquiries;
using Spinx.Services.ContactUsInquiries.Actions;
using Spinx.Services.ContactUsInquiries.DTOs;
using Spinx.Services.ContactUsInquiries.Filters;
using Spinx.Services.ContactUsInquiries.ListOrders;
using System.Linq;

namespace Spinx.Services.ContactUsInquiries
{
    public interface IContactUsInquiryAdminService
    {
        Result List(ContactUsInquiryAdminFilterDto dto);
    }

    public class ContactUsInquiryAdminService : IContactUsInquiryAdminService
    {
        private readonly IContactUsInquiryRepository _contactUsInquiryRepository;
        private readonly ContactUsInquiryAdminActionFactory _actionFactory;

        public ContactUsInquiryAdminService(
            IContactUsInquiryRepository contactUsInquiryRepository,
            ContactUsInquiryAdminActionFactory actionFactory)
        {
            _contactUsInquiryRepository = contactUsInquiryRepository;
            _actionFactory = actionFactory;
        }

        public Result List(ContactUsInquiryAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();

            if (!result.Success) return result;

            var query = _contactUsInquiryRepository.AsNoTracking;
            query = new ContactUsInquiryAdminFilter(query, dto).FilteredQuery();
            query = new ContactUsInquiryAdminListOrder(query, dto).OrderByQuery();
            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.Phone,
                    s.Details,
                    s.CreatedAt,
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }
    }
}