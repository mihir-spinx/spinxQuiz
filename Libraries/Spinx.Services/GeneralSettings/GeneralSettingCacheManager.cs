using Z.EntityFramework.Plus;

namespace Spinx.Services.GeneralSettings
{
    public static class GeneralSettingCacheManager
    {
        public static string Name = "GeneralSettings";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag(Name);
        }
    }
}