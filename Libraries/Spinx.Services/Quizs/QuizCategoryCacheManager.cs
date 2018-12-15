using Z.EntityFramework.Plus;

namespace Spinx.Services.QuizCategories
{
    public static class QuizCategoryCacheManager
    {
        public static string Name = "QuizCategories";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("QuizCategories");
        }
    }
}
