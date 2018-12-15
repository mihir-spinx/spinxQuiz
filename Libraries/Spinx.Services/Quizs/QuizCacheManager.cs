using Z.EntityFramework.Plus;

namespace Spinx.Services.Quizs
{
    public static class QuizCacheManager
    {
        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("Quizs");
        }

        public static string Name = "Quizs";
    }
}
