using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Site_Edit_Page",
                "admin/{controller}/{action}/{siteId}/{siblingId}",
                new { area = "Admin" },
                new { siteId = @"^[0-9]+$", siblingId = @"^[0-9]+$" },
                new[] { "Spinx.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
               "Admin_Edit_Page_By_Id",
               "admin/{controller}/{action}/{id}",
               new { area = "Admin" },
               new { id = @"^[0-9]+$" },
               new[] { "Spinx.Web.Areas.Admin.Controllers" }
           );

            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}",
                new { area = "Admin", controller = "Home", action = "Index" },
                new[] { "Spinx.Web.Areas.Admin.Controllers" }
            );
        }
    }
}