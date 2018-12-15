using Spinx.Api.Infrastructure;
using Spinx.Services.SeoPages;
using Spinx.Services.SeoPages.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class SeoPagesController : BaseApiController
    {
        private readonly ISeoPageAdminService _seoPageRepository;

        public SeoPagesController(
                    ISeoPageAdminService seoPageAdminService)
        {
            _seoPageRepository = seoPageAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "SeoPages" })]
        public IHttpActionResult Get([FromUri]SeoPageAdminFilterDto dto)
        {
            return Result(_seoPageRepository.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"SeoPages.Create"})]
        public IHttpActionResult Post([FromBody]SeoPageCreateAdminDto dto)
        {
            return Result(_seoPageRepository.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"SeoPages.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_seoPageRepository.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"SeoPages.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]SeoPageEditAdminDto dto)
        {
            return Result(_seoPageRepository.Edit(id, dto));
        }
    }
}