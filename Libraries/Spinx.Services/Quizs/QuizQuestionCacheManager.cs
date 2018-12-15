using Z.EntityFramework.Plus;

namespace Spinx.Services.QuizQuestions
{
    public static class QuizQuestionCacheManager
    {
        public static string Name = "QuizQuestions";

        public static void ClearCache()
        {
            QueryCacheManager.ExpireTag("QuizQuestions");
        }
    }
}
