using Spinx.Api.Infrastructure;
using Spinx.Services.ContactUsInquiries;
using Spinx.Services.ContactUsInquiries.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class ContactUsInquiriesController : BaseApiController
    {
        private readonly IContactUsInquiryAdminService _contactUsInquiryAdminService;

        public ContactUsInquiriesController(
                     IContactUsInquiryAdminService contactUsInquiryAdminService)
        {
            _contactUsInquiryAdminService = contactUsInquiryAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "ContactUsInquiries" })]
        public IHttpActionResult Get([FromUri]ContactUsInquiryAdminFilterDto dto)
        {
            return Result(_contactUsInquiryAdminService.List(dto));
        }
    }
}