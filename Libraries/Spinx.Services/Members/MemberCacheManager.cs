using Z.EntityFramework.Plus;

namespace Spinx.Services.Members
{
    public static class MemberCacheManager
    {
        public static string Name = "Members";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag(Name);
        }
    }
}