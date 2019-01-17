using System.Web.Optimization;

namespace Spinx.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/js/jquery.min.js",
                "~/Content/js/custom.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css/bundles").Include(
                "~/Content/css/fontawesome.min.css",
                "~/Content/css/reset.css",
                "~/Content/css/style.css",
                "~/Content/css/media.css"
                ));
        }
    }
}