using Z.EntityFramework.Plus;

namespace Spinx.Services.ContactUsInquiries
{
    public static class ContactUsInquiryCacheManager
    {
        public static string Name = "ContactUsInquiries";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag(Name);
        }
    }
}