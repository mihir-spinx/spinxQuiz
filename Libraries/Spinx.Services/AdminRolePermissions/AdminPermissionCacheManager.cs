using Z.EntityFramework.Plus;

namespace Spinx.Services.AdminRolePermissions
{
    public static class AdminPermissionCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("AdminPermissions", "AdminRoles", "AdminUsers");
        }
    }
}