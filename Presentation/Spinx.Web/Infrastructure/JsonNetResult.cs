using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Spinx.Web.Infrastructure
{
    /// <summary> 
    /// Simple Json Result that implements the Json.NET serialiser offering more versatile serialisation 
    /// </summary> 
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
        {
        }

        public JsonNetResult(object responseBody)
        {
            ResponseBody = responseBody;
        }

        public JsonNetResult(object responseBody, JsonSerializerSettings settings)
        {
            Settings = settings;
        }

        /// <summary>Gets or sets the serialiser settings</summary> 
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>Gets or sets the encoding of the response</summary> 
        public Encoding ContentEncoding { get; set; }

        /// <summary>Gets or sets the content type for the response</summary> 
        public string ContentType { get; set; }

        /// <summary>Gets or sets the body of the response</summary> 
        public object ResponseBody { get; set; }

        /// <summary>Gets the formatting types depending on whether we are in debug mode</summary> 
        private Formatting Formatting
        {
            get
            {
                return Debugger.IsAttached ? Formatting.Indented : Formatting.None;
            }
        }

        /// <summary> 
        /// Serializes the response and writes it out to the response object 
        /// </summary> 
        /// <param name="context">The execution context</param> 
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            // set content type 
            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            // set content encoding 
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            Settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (ResponseBody != null)
            {
                response.Write(JsonConvert.SerializeObject(ResponseBody, Formatting, Settings));
            }
        }
    }

    public static class ControllerExtensions 
    { 
        public static JsonNetResult JsonEx(this Controller controller, object responseBody) 
        { 
            return new JsonNetResult(responseBody); 
        }
        public static string RenderView(this Controller controller, string viewName, object model)
        {
            return RenderView(controller, viewName, new ViewDataDictionary(model));
        }

        public static string RenderView(this Controller controller, string viewName, ViewDataDictionary viewData)
        {
            var controllerContext = controller.ControllerContext;
            var viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
            StringWriter stringWriter;

            using (stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(
                    controllerContext,
                    viewResult.View,
                    viewData,
                    controllerContext.Controller.TempData,
                    stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
            }
            return stringWriter.ToString();
        }
    }
}