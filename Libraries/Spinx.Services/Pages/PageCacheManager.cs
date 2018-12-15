using Z.EntityFramework.Plus;

namespace Spinx.Services.Pages
{
    public static class PageCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("Pages");
        }
    }
}
