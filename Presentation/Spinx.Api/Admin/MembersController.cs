using Spinx.Api.Infrastructure;
using Spinx.Services.Members;
using Spinx.Services.Members.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class MembersController : BaseApiController
    {
        private readonly IMemberService _memberService;

        public MembersController(
                    IMemberService memberService)
        {
            _memberService = memberService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "Members" })]
        public IHttpActionResult Get([FromUri]MemberDetailListDto dto)
        {
            return Result(_memberService.DetailList(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Members.Create"})]
        public IHttpActionResult Post([FromBody]MemberDto dto)
        {
            return Result(_memberService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Members.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_memberService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Members.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]MemberDto dto)
        {
            return Result(_memberService.Edit(id, dto));
        }

        [AuthorizeApiAdminUser(permissions: new[] { "Members.Edit" })]
        [System.Web.Http.Route("api/admin/members/GetMemberDashboard/{Id}")]
        public IHttpActionResult GetMemberDashboard(int id)
        {
            return Result(_memberService.GetMemberDashboard(id));
        }
    }
}