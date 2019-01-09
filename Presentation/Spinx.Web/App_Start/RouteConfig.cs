using System.Web.Mvc;
using System.Web.Routing;

namespace Spinx.Web
{
    public class RouteConfig
    {
        private static readonly string[] FrontNamespaces = { "Spinx.Web.Controllers" };

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            // CMS Pages
            routes.MapRoute(name: "Privacy", url: "privacy", defaults: new { controller = "Pages", action = "Index", slug = "privacy" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "Terms", url: "terms", defaults: new { controller = "Pages", action = "Index", slug = "terms" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "Error", url: "Error", defaults: new { controller = "Error", action = "PageNotFound" }, namespaces: FrontNamespaces);

            routes.MapRoute(name: "Contact", url: "Contact", defaults: new { controller = "Contact", action = "Index" }, namespaces: FrontNamespaces);

            // Resume
            routes.MapRoute(name: "resume-board", url: "resumes", defaults: new { controller = "PostAResumes", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "post-a-resume", url: "post-a-resume", defaults: new { controller = "PostAResumes", action = "create" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "resume-board-detail", url: "resumes/{slug}", defaults: new { controller = "PostAResumes", action = "detail", slug = "" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "resume-access-plans", url: "resume-access-plans", defaults: new { controller = "ResumeAccessPlans", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "ResumeAccessPurchaseList", url: "resume-access-purchase-list", defaults: new { controller = "DashBoards", action = "ResumeAccessPurchaseList" }, namespaces: FrontNamespaces);

            // Job a post 
            routes.MapRoute(name: "thankyoumember", url: "thank-you-member", defaults: new { controller = "PostAJobs", action = "thank-you-member" }, namespaces: FrontNamespaces);

            //Grammar Lessons
            routes.MapRoute(name: "grammar-lessons-categories", url: "resources/grammar-lessons", defaults: new { controller = "GrammarLessons", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "grammar-lessons", url: "resources/grammar-lessons/{CategorySlug}", defaults: new { controller = "GrammarLessons", action = "LessonsList" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "grammar-lessons-detail", url: "resources/grammar-lessons/{CategorySlug}/{Slug}", defaults: new { controller = "GrammarLessons", action = "LessonsDetail" }, namespaces: FrontNamespaces);

            //Lesson Plan 
            routes.MapRoute(name: "lesson-plans-categories", url: "resources/lesson-plans", defaults: new { controller = "LessonPlan", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "add-lesson-plan", url: "resources/lesson-plans/create", defaults: new { controller = "LessonPlan", action = "Create" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "lesson-plans", url: "resources/lesson-plans/{CategorySlug}", defaults: new { controller = "LessonPlan", action = "PlansList" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "lesson-plans-detail", url: "resources/lesson-plans/{CategorySlug}/{Slug}", defaults: new { controller = "LessonPlan", action = "PlansDetail" }, namespaces: FrontNamespaces);

            //idioms
            routes.MapRoute(name: "idioms", url: "resources/idioms", defaults: new { controller = "Idioms", action = "Index" }, namespaces: FrontNamespaces);

            //slangs
            routes.MapRoute(name: "slangs", url: "resources/slangs", defaults: new { controller = "Slangs", action = "Index" }, namespaces: FrontNamespaces);

            //phrasal verbs
            routes.MapRoute(name: "phrasal-verbs", url: "resources/phrasal-verbs", defaults: new { controller = "PhrasalVerbs", action = "Index" }, namespaces: FrontNamespaces);

            //quizzes
            routes.MapRoute(name: "quizzes", url: "quiz", defaults: new { controller = "Quizzes", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "quiz", url: "resources/quiz/{Slug}", defaults: new { controller = "Quizzes", action = "Detail" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "question", url: "resources/question/{Slug}", defaults: new { controller = "Quizzes", action = "Question" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "getquestion", url: "api/quiz/getQuestion/", defaults: new { controller = "Quizzes", action = "GetQuestion" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "getAnswer", url: "api/quiz/getAnswer/", defaults: new { controller = "Quizzes", action = "getAnswer" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "submitquiz", url: "api/quiz/submitquiz/", defaults: new { controller = "Quizzes", action = "submitquiz" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "quizthankyou", url: "resources/thankyou", defaults: new { controller = "Quizzes", action = "ThankYou" }, namespaces: FrontNamespaces);

            //Member
            routes.MapRoute(name: "create-account", url: "create-account", defaults: new { controller = "Member", action = "Index" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "login", url: "login", defaults: new { controller = "Member", action = "Login" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "checkoutlogin", url: "checkout-login", defaults: new { controller = "Member", action = "CheckoutLogin" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "forgot-password", url: "forgot-password", defaults: new { controller = "Member", action = "ForgotPassword" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "reset-password", url: "reset-password", defaults: new { controller = "Member", action = "ResetPassword" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "edit-profile", url: "edit-profile", defaults: new { controller = "Member", action = "EditProfile" }, namespaces: FrontNamespaces);
            routes.MapRoute(name: "change-password", url: "change-password", defaults: new { controller = "Member", action = "ChangePassword" }, namespaces: FrontNamespaces);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Quizzes", action = "Index", id = UrlParameter.Optional },
                namespaces: FrontNamespaces
            );
        }
    }
}