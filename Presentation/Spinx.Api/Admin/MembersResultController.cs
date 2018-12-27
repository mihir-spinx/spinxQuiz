using Spinx.Api.Infrastructure;
using Spinx.Services.Members;
using Spinx.Services.Members.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class MembersResultController : BaseApiController
    {
        private readonly IMemberResultService _memberResultService;

        public MembersResultController(
            IMemberResultService memberResultService)
        {
            _memberResultService = memberResultService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "MembersResult" })]
        public IHttpActionResult Get([FromUri]MemberResultListDto dto)
        {
            return Result(_memberResultService.DetailList(dto));
        }


    }
}