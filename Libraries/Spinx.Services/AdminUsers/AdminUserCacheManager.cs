using Z.EntityFramework.Plus;

namespace Spinx.Services.AdminUsers
{
    public static class AdminUserCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("AdminRoles");
        }
    }
}