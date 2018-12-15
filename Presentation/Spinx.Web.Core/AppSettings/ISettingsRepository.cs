namespace Spinx.Web.Core.AppSettings
{
    public interface ISettingsRepository
    {
        string Get(string settingName);
    }
}