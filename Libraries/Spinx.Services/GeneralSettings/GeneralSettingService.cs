using System.Linq;
using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.GeneralSettings;
using Spinx.Data.Repository.Member;
using Spinx.Domain.Members;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members;
using Spinx.Services.Members.Validators;

namespace Spinx.Services.GeneralSettings
{
    public interface IGeneralSettingService
    {
        string GetGeneralSetting(string slug);
    }

    public class GeneralSettingService : IGeneralSettingService
    {
        private readonly IGeneralSettingRepository _generalSettingRepository;

        public GeneralSettingService(
            IGeneralSettingRepository generalSettingRepository)
        {
            _generalSettingRepository = generalSettingRepository;
        }

        public string GetGeneralSetting(string slug)
        {
            string strValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(slug))
            {
                var generalSetting = _generalSettingRepository.AsNoTracking.FirstOrDefault(f => f.Slug == slug);
                if (generalSetting != null)
                    strValue = generalSetting.Value;
            }
            return strValue;
        }
    }
}