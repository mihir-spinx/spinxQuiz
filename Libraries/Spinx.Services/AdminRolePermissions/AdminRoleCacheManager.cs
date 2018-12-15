using Z.EntityFramework.Plus;

namespace Spinx.Services.AdminRolePermissions
{
    public static class AdminRoleCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("AdminRoles");
        }
    }
}