using Spinx.Api.Infrastructure;
using Spinx.Services.GeneralSettings;
using Spinx.Services.GeneralSettings.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    //[AuthorizeApiAdminUser]
    public class GeneralSettingsController : BaseApiController
    {
        private readonly IGeneralSettingAdminService _GeneralSettingRepository;

        public GeneralSettingsController(
                    IGeneralSettingAdminService GeneralSettingAdminService)
        {
            _GeneralSettingRepository = GeneralSettingAdminService;
        }

       // [AuthorizeApiAdminUser(permissions: new[] { "GeneralSettings" })]
        public IHttpActionResult Get([FromUri]GeneralSettingAdminFilterDto dto)
        {
            return Result(_GeneralSettingRepository.List(dto));
        }

      //  [AuthorizeApiAdminUser(permissions: new [] {"GeneralSettings.Create"})]
        public IHttpActionResult Post([FromBody]GeneralSettingCreateAdminDto dto)
        {
            return Result(_GeneralSettingRepository.Create(dto));
        }

       // [AuthorizeApiAdminUser(permissions: new [] {"GeneralSettings.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_GeneralSettingRepository.GetById(id));
        }

       // [AuthorizeApiAdminUser(permissions: new [] {"GeneralSettings.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]GeneralSettingEditAdminDto dto)
        {
            return Result(_GeneralSettingRepository.Edit(id, dto));
        }
    }
}