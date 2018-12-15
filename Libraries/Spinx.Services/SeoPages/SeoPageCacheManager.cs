using Z.EntityFramework.Plus;

namespace Spinx.Services.SeoPages
{
    public static class SeoPageCacheManager
    {
        public static string Name = "SeoPages";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag(Name);
        }
    }
}