using Spinx.Api.Infrastructure;
using Spinx.Services.Members;
using System.Web.Http;

namespace Spinx.Api.List
{
    public class MembersListController : BaseApiController
    {
        private readonly IMemberService _memberService;

        public MembersListController(
            IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IHttpActionResult Get()
        {
            return Ok(_memberService.Get());
        }
    }
}