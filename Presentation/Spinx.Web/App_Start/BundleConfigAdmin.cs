using System.Collections.Generic;
using System.Web.Optimization;

namespace Spinx.Web
{
    public class BundleConfigAdmin
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Admin Area
            bundles.Add(new StyleBundle("~/Content/Admin/css/bundle").Include(
                "~/Content/Admin/css/bootstrap.min.css",
                "~/Content/Admin/css/font-awesome.min.css",
                "~/Content/Admin/css/smartadmin-production-plugins.css",
                "~/Content/Admin/css/smartadmin-production.min.css",
                "~/Content/Admin/css/smartadmin-skins.min.css",
                "~/Content/Admin/js/myplugin/period_picker/jquery.periodpicker.min.css",
                "~/Content/Admin/js/myplugin/period_picker/jquery.timepicker.min.css",
                "~/bower_components/angular-ui-select/dist/select.min.css",
                "~/Content/Admin/css/style.css"));

            bundles.Add(new ScriptBundle("~/Content/Admin/js/bundles").Include(
                "~/Content/Admin/js/libs/jquery-2.1.1.min.js",
                "~/Content/Admin/js/libs/jquery-ui-1.10.3.min.js",
                "~/Content/Admin/js/app.config.js",
                "~/Content/Admin/js/bootstrap/bootstrap.min.js",
                "~/Content/Admin/js/notification/SmartNotification.min.js",
                "~/Content/Admin/js/plugin/select2/select2.full.min.js",
                "~/Content/Admin/js/smartwidgets/jarvis.widget.min.js",
                "~/Content/Admin/js/myplugin/bootstrap-maxlength.js",
                "~/Content/Admin/js/myplugin/handlebars/handlebars-v4.0.5.js",
                "~/Content/Admin/js/myplugin/history/history.js",
                "~/Content/Admin/js/myplugin/jquery.confirm/jquery.confirm.js",
                "~/Content/Admin/js/myplugin/fancyBox/jquery.fancybox.js",
                "~/Content/Admin/js/myplugin/moment.min.js",
                "~/Content/Admin/js/myplugin/period_picker/jquery.periodpicker.full.min.js",
                "~/Content/Admin/js/myplugin/period_picker/jquery.timepicker.min.js",
                "~/Content/Admin/js/myplugin/md5.js",
                "~/Content/Admin/js/plugin/jquery-nestable/jquery.nestable.min.js",
                "~/Content/Admin/js/myplugin/jquery.cookie.js",
                "~/Content/Admin/js/myplugin/jQuery.flashMessage.js",
                "~/Content/Admin/js/app.js",
                "~/Content/Admin/js/admin-list.js",
                "~/Content/Admin/js/web.js"));
        }
    }

    public class PassthruBundleOrderer: IBundleOrderer 
    {  
        public IEnumerable < BundleFile > OrderFiles(BundleContext context, IEnumerable < BundleFile > files) 
        {
            return files;
        }
    }
}