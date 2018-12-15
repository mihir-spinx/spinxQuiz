using System.Web;

namespace Spinx.Web.Core.Infrastructure
{
    public static class BrowserHelper
    {
        public static void BackButtonClearCache()
        {
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");
        }
    }
}