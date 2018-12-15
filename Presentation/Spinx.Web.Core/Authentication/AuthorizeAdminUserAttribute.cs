using Autofac;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace Spinx.Web.Core.Authentication
{
    public class AuthorizeApiAdminUserAttribute : AuthorizationFilterAttribute
    {
        private readonly IList<string> _permissions;
        public IComponentContext IocContext { get; set; }

        public AuthorizeApiAdminUserAttribute(string[] permissions = null)
        {
            _permissions = permissions;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!UserAuth.OnAuthorization(HttpContext.Current.Request.Cookies, UserAuth.CookieAdminUser))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            //var adminUser = UserAuth.AdminUser;

            //if (adminUser.HasRole("super-admin")) return;

            //if (_permissions != null && adminUser.HasPermission(_permissions))
            //    throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }

    public class AuthorizeAdminUserAttribute : AuthorizeAttribute
    {
        private readonly IList<string> _permissions;
        public IComponentContext IocContext { get; set; }

        public AuthorizeAdminUserAttribute(string[] permissions = null)
        {
            _permissions = permissions;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!UserAuth.OnAuthorization(filterContext.HttpContext.Request.Cookies, UserAuth.CookieAdminUser))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        area = "Admin",
                        controller = "Auth",
                        action = "Login",
                        returnUrl = HttpContext.Current.Request.Url
                    }));

                return;
            }

            //var adminUser = UserAuth.AdminUser;

            //if (adminUser.HasRole("super-admin")) return;

            //if (_permissions != null && adminUser.HasPermission(_permissions))
            //{
            //    filterContext.Result = 
            //        new RedirectToRouteResult(
            //            new RouteValueDictionary(new {
            //                area = "Admin",
            //                controller = "Errors",
            //                action = "PageNotFound"
            //            }));
            //}
        }
    }
}
