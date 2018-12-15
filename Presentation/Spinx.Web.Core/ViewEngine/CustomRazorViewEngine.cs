using System.Collections.Generic;
using System.Web.Mvc;

namespace Spinx.Web.Core.ViewEngine
{
    public class CustomRazorViewEngine : CustomVirtualPathProviderViewEngine
    {
        public CustomRazorViewEngine()
        {
            var areaLocationFormats = new[]
            {
                //Front
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
            };

            AreaMasterLocationFormats = areaLocationFormats;
            AreaPartialViewLocationFormats = areaLocationFormats;
            AreaViewLocationFormats = areaLocationFormats;

            var locationFormats = new[]
            {
                //default
                "~/Views/{1}/{0}.cshtml", 
                "~/Views/Shared/{0}.cshtml",
            };

            MasterLocationFormats = locationFormats;
            ViewLocationFormats = locationFormats;
            PartialViewLocationFormats = locationFormats;

            FileExtensions = new[] { "cshtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            const bool runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var layoutPath = masterPath;
            const bool runViewStartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions);
        }
    }
}
