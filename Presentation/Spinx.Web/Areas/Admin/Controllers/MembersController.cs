using Spinx.Services.Members;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class MembersController : BaseAdminController
    {
        private readonly IMemberService _memberService;
        private readonly IAppSettings _appSettings;

        public MembersController(
            IAppSettings appSettings,
            IMemberService memberService)
        {
            _memberService = memberService;
            _appSettings = appSettings;
        }

        [AuthorizeAdminUser(permissions: new [] {"Members"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Members.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Members.Edit"})]
        public ActionResult Edit(int id)
        {
            ViewBag.Member = _memberService.GetById(id);
            ViewBag.filepath = _appSettings.ResumeUploads;
            return View();
        }

        [AuthorizeApiAdminUser(permissions: new[] { "Members.Edit" })]
        [System.Web.Http.Route("api/admin/members/GetMemberDetail/{Id}")]
        public JsonNetResult GetMemberDetail(int id)
        {
            var result = _memberService.GetMemberDashboard(id);
            return new JsonNetResult(result);
        }
    }
}