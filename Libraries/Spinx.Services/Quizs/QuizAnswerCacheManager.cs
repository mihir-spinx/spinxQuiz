using Z.EntityFramework.Plus;

namespace Spinx.Services.QuizAnswers
{
    public static class QuizAnswerCacheManager
    {
        public static string Name = "QuizAnswers";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("QuizAnswers");
        }
    }
}
