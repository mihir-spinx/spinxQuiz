using Spinx.Api.Infrastructure;
using Spinx.Services.EmailTemplates;
using Spinx.Services.EmailTemplates.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class EmailTemplatesController : BaseApiController
    {
        private readonly IEmailTemplateAdminService _emailTemplateAdminService;

        public EmailTemplatesController(
            IEmailTemplateAdminService emailTemplateAdminService)
        {
            _emailTemplateAdminService = emailTemplateAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "EmailTemplates" })]
        public IHttpActionResult Get([FromUri]EmailTemplateAdminFilterDto dto)
        {
            return Result(_emailTemplateAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"EmailTemplates.Create"})]
        public IHttpActionResult Post([FromBody]EmailTemplateCreateAdminDto dto)
        {
            return Result(_emailTemplateAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"EmailTemplates.Edit"})]
        public IHttpActionResult Get(int Id)
        {
            return Result(_emailTemplateAdminService.GetById(Id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"EmailTemplates.Edit"})]
        public IHttpActionResult Put(int Id, [FromBody]EmailTemplateEditAdminDto dto)
        {
            return Result(_emailTemplateAdminService.Edit(Id, dto));
        }
    }
}