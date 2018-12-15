using Z.EntityFramework.Plus;

namespace Spinx.Services.EmailTemplates
{
    public static class EmailTemplateCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("EmailTemplates");
        }
    }
}
