using Spinx.Web.Core.Authentication;
using System.Web;

namespace Spinx.Web.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static UserPrincipal GetUser(this HttpContextBase context)
        {
            return (UserPrincipal)context.Items["User"];
        }

        public static UserPrincipal GetAdminUser(this HttpContextBase context)
        {
            return (UserPrincipal)context.Items["AdminUser"];
        }
    }
}