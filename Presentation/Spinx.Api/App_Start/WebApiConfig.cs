using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Spinx.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "AdminApiControllerWihtId",
            //    routeTemplate: "api/admin/{controller}/{id}",
            //    defaults: new { namespaces = new[] { "Spinx.Api.Admin" } },
            //    constraints: new { id = @"^[0-9]+$", httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put) }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "AdminApiControllerForPost",
            //    routeTemplate: "api/admin/{controller}",
            //    defaults: new { action = "Post", namespaces = new[] { "Spinx.Api.Admin" } },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "AdminApiControllerSearch",
            //    routeTemplate: "api/admin/{controller}/search",
            //    defaults: new { action = "Search", namespaces = new[] { "Spinx.Api.Admin" } },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            //);


            //config.Routes.MapHttpRoute(
            //    name: "ListApiForAreaLocations",
            //    routeTemplate: "api/list/arealocationslist/{siteId}/{areaId}",
            //    defaults: new { namespaces = new[] { "Spinx.Api.List" }, controller = "AreaLocationsList" },
            //    constraints: new { siteId = @"^[0-9]+$", areaId = @"^[0-9]+$", httpMethod = new HttpMethodConstraint(HttpMethod.Get)}
            //);



            
            config.Routes.MapHttpRoute(
                name: "ListApi",
                routeTemplate: "api/list/{controller}",
                defaults: new { namespaces = new[] { "Spinx.Api.List" }, httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            //config.Routes.MapHttpRoute(
            //    name: "ListApiBySiteId",
            //    routeTemplate: "api/list/{controller}/{siteId}",
            //    defaults: new { namespaces = new[] { "Spinx.Api.List" }, httpMethod = new HttpMethodConstraint(HttpMethod.Get) },
            //    constraints: new { siteId = @"^[0-9]+$", httpMethod = new HttpMethodConstraint(HttpMethod.Get)}
            //);

            //config.Routes.MapHttpRoute(
            //    name: "UpdateWith_SiteId_SiblingId",
            //    routeTemplate: "api/admin/{controller}/{siteId}/{siblingId}",
            //    defaults: new { namespaces = new[] { "Spinx.Api.Admin" } },
            //    constraints: new { siteId = @"^[0-9]+$", siblingId = @"^[0-9]+$", httpMethod = new HttpMethodConstraint(HttpMethod.Put, HttpMethod.Get)}
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/admin/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, namespaces = new[] { "Spinx.Api.Admin" } }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional, namespaces = new[] { "Spinx.Api" } }
            //);
        }
    }
}
