using System.Web.Mvc;

namespace Spinx.Web.Infrastructure
{
    public static class UrlHelperExtensions
    {
        public static string RootUrl(this UrlHelper helper)
        {
            return helper.RouteUrl("Default", new { controller = "Home", action = "Index"});
        }
        //ex.: public static string AboutUrl(this UrlHelper helper) { return helper.RouteUrl("About"); }
        //ex.: public static string ProjectDetailFullUrl(this UrlHelper helper, string slug) { return helper.RouteUrl("ProjectDetail", new { slug }, HttpContext.Current.Request.Url.Scheme); }

        public static string PrivacyUrl(this UrlHelper helper, string slug) { return helper.RouteUrl("Privacy", new { slug }); }

        public static string TermsUrl(this UrlHelper helper, string slug) { return helper.RouteUrl("terms", new { slug }); }
        public static string ErrorUrl(this UrlHelper helper) { return helper.RouteUrl("error"); }
        public static string ContactUrl(this UrlHelper helper) { return helper.RouteUrl("contact"); }

        public static string ThankYouMemberUrl(this UrlHelper helper) { return helper.RouteUrl("thankyoumember"); }

        public static string QuizzesUrl(this UrlHelper helper) { return helper.RouteUrl("quizzes"); }

        public static string BannersUrl(this UrlHelper helper) { return helper.RouteUrl("bannersurl"); }

        public static string LoginUrl(this UrlHelper helper) { return helper.RouteUrl("login"); }
        public static string CreateAccountUrl(this UrlHelper helper) { return helper.RouteUrl("create-account"); }
        public static string LogoutUrl(this UrlHelper helper) { return helper.RouteUrl("Logout"); }
        public static string ForgotPasswordUrl(this UrlHelper helper) { return helper.RouteUrl("forgot-password"); }
        public static string VerificationEmailUrl(this UrlHelper helper) { return helper.RouteUrl("VerificationEmail"); }
        public static string ResetPasswordUrl(this UrlHelper helper) { return helper.RouteUrl("reset-password"); }

        public static string KoreaUrl = "korea";
        public static string ChinaUrl = "china";
        public static string InternationalUrl = "international";
    }
}