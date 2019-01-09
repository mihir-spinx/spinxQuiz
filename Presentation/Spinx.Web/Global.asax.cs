using Spinx.Api;
using Spinx.Core;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.Infrastructure;
using Spinx.Web.Core.ModelBinders;
using Spinx.Web.Core.ViewEngine;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Spinx.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            // Force https for release mode site
            if (!HttpContext.Current.IsDebuggingEnabled)
                GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Bundling Configuration
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleConfigAdmin.RegisterBundles(BundleTable.Bundles);

            // Custom module binders
            ModelBinders.Binders.DefaultBinder = new CommaSeparatedValuesModelBinder();

            // Custom View Engine Define
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomRazorViewEngine());

            // Auto Mapper
            Bootstrapper.Run();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            // Front UserUserAuth
            var user = UserAuth.GetSignInUser(UserAuth.CookieUser);
            if (user.IsLogedIn())
            {
                HttpContext.Current.Items.Add("User", user);
                BrowserHelper.BackButtonClearCache();
            }

            // Admin User
            var adminUser = UserAuth.GetSignInUser(UserAuth.CookieAdminUser);
            if (adminUser.IsLogedIn())
            {
                HttpContext.Current.Items.Add("AdminUser", adminUser);
                BrowserHelper.BackButtonClearCache();
            }
        }

        private void Application_Error(object sender, EventArgs e)
        {

            Response.Redirect("~/pagenotfound");
            var ex = Server.GetLastError();

            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first

                var json = new JavaScriptSerializer().Serialize(new Result()
                {
                    MessageType = MessageType.Warning,
                    Message = "Opps! Seems you couldn\'t submit form for a longtime.<br/>Please refresh page and try again.",
                });

                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(json);
                Response.End();
            }
            else if (ex is DbUpdateException)
            {
                // TODO: Log this exception
                //var sqlex = ex.InnerException.InnerException as SqlException;

                Response.Clear();
                Server.ClearError(); //make sure you log the exception first

                var json = new JavaScriptSerializer().Serialize(new Result().SetError(
                    "Opps! Seems there are some database level error. Please contact your system administrator."));

                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(json);
                Response.End();
            }
            else if (ex is DbEntityValidationException)
            {

            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Write(HttpContext.Current.Request.Url);
            //HttpContext.Current.Response.Write(Request.Url.AbsoluteUri);
            //HttpContext.Current.Response.End();

            //var currenUrl = Request.Url.AbsoluteUri;
            //if (currenUrl.EndsWith("/") && Request.HttpMethod != "POST" && Request.HttpMethod != "PUT" && !currenUrl.Contains("admin") && !currenUrl.Contains("Content")
            //    && !currenUrl.Contains("favicon") && !currenUrl.Contains("bower_components") && !currenUrl.Contains("Upload")
            //    && !currenUrl.Contains("Files") && !currenUrl.Contains("Upload") && !currenUrl.Contains("ckfinder")
            //    && !currenUrl.Contains("api") && !currenUrl.Contains("?"))
            //{
            //    Response.Status = "301 Moved Permanently";
            //    Response.AddHeader("Location", Request.Url.AbsoluteUri.TrimEnd('/'));
            //    Response.End();
            //}

            ////You don't want to redirect on posts, or images/css/js
            //var isGet = HttpContext.Current.Request.RequestType.ToLowerInvariant().Contains("get");
            //if (isGet && HttpContext.Current.Request.Url.AbsolutePath.Contains(".") == false)    
            //{
            //    var lowercaseUrl = (Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.AbsolutePath);
            //    if (Regex.IsMatch(lowercaseUrl, @"[A-Z]"))
            //    {
            //        //You don't want to change casing on query strings
            //        lowercaseUrl = lowercaseUrl.ToLower() + HttpContext.Current.Request.Url.Query;

            //        Response.Clear();
            //        Response.Status = "301 Moved Permanently";
            //        Response.AddHeader("Location", lowercaseUrl); 
            //        Response.End();
            //    }
            //}
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 404)
            {
                if (!context.Request.RawUrl.Contains("/PageNotFound"))
                    context.Response.Redirect("~/PageNotFound");
            }
            //if (Context.Response.StatusCode == 404)
            //{
            //    Response.Clear();

            //    var routeData = Request.RequestContext.RouteData;

            //    // ignore for api
            //    if ((routeData.Values.ContainsKey("controller") && Convert.ToString(routeData.Values["controller"]).NullSafeToLower() == "api") ||
            //        HttpContext.Current.Request.Url.Segments.Any(w => w.NullSafeToLower() == "api/"))
            //        return;

            //    if (Convert.ToString(routeData.Values["area"]) == "Admin")
            //        HttpContext.Current.Response.Redirect("~/admin/errors/notfound");

            //    HttpContext.Current.Response.Redirect("~/errors/notfound");
            //}
        }
    }
}
