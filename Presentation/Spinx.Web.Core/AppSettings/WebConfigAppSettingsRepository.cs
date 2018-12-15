using System.Configuration;

namespace Spinx.Web.Core.AppSettings
{
    public class WebConfigAppSettingsRepository : ISettingsRepository
    {
        public string Get(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}