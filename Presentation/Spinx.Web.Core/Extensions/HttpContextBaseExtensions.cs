using Spinx.Web.Core.Authentication;
using System.Web;

namespace Spinx.Web.Core.Extensions
{
    public static class HttpContextBaseExtensions
    {
        public static UserPrincipal GetUser(this HttpContext context)
        {
            return (UserPrincipal)context.Items["User"];
        }

        public static UserPrincipal GetAdminUser(this HttpContext context)
        {
            return (UserPrincipal)context.Items["AdminUser"];
        }
    }
}