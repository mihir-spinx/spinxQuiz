using Spinx.Api.Infrastructure;
using Spinx.Services.Pages;
using Spinx.Services.Pages.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class PagesController : BaseApiController
    {
        private readonly IPageAdminService _pageAdminService;      

        public PagesController(
                 IPageAdminService pageAdminService)
        {
            _pageAdminService = pageAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "Pages" })]
        public IHttpActionResult Get([FromUri]PageAdminFilterDto dto)
        {
            return Result(_pageAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Pages.Create"})]
        public IHttpActionResult Post([FromBody]PageCreateAdminDto dto)
        {
            return Result(_pageAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Pages.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_pageAdminService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Pages.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]PageEditAdminDto dto)
        {
            return Result(_pageAdminService.Edit(id, dto));
        }
    }
}