using Humanizer;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spinx.Web.Core.Extensions
{
    public static partial class HtmlHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controllerName = "", string cssClass = "active")
        {
            var viewContext = html.ViewContext;
            var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            var routeValues = viewContext.RouteData.Values;
            var currentController = routeValues["controller"].ToString().ToLower();

            if (string.IsNullOrEmpty(controllerName))
                controllerName = currentController;

            var acceptedControllers = controllerName.Trim().ToLower().Split(',').Distinct().ToArray();

            return acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }

        public static IHtmlString BkndHeader(this HtmlHelper html, IList<string> labels, string icon = "file",
            string backUrl = "",
            string addBtnText = "", string addBtnUrl = "",
            string sequenceUrl = "")
        {
            return new HeaderBuilder(html.ViewContext, labels, icon, backUrl, addBtnText, addBtnUrl, sequenceUrl).Render();
        }

        public static FormTag BkndBeginForm(this HtmlHelper html)
        {
            return new FormTag(html.ViewContext);
        }

        public static IHtmlString BkndToolBar(this HtmlHelper html, IList<string> actions, bool paging = true)
        {
            return new ListPageToolbarBuilder(html.ViewContext, actions, paging).Render();
        }

        public static IHtmlString BkndFormButtonSave(this HtmlHelper html)
        {
            return new HtmlString(@"
                <div class='form-actions'>
                    <div class='row text-left'>
                        <div class='col-md-offset-2 col-md-6'>
                            <button class='btn btn-primary' type='submit'>
                                <i class='fa fa-save'></i> Save
                            </button>
                        </div>
                    </div>
                </div>");
        }

        public static IHtmlString BkndTextBox(this HtmlHelper html,
            string property,
            object value = null,
            string label = "",
            string placeholder = "",
            int? maxlength = null,
            bool required = false,
            string note = "",
            string type = "text")
        {
            if (string.IsNullOrEmpty(label))
                label = property.Humanize();

            if (string.IsNullOrEmpty(placeholder) && !string.IsNullOrEmpty(label))
                placeholder = label.Humanize();
            else if (string.IsNullOrEmpty(placeholder) && string.IsNullOrEmpty(label))
                placeholder = property.Humanize();

            var strMaxLength = "";
            if (maxlength != null)
                strMaxLength = $"maxlength='{maxlength}'";

            var requiredText = required ? "<span class='text-danger'>*</span>" : "";

            if (!string.IsNullOrEmpty(note))
                note = $"<p class='note'>{note}</p>";

            return new HtmlString($@"
                <div class='form-group'>
                    <label class='col-md-2 control-label'>{label} {requiredText}</label>
                    <div class='col-md-6'>
                        <input class='form-control' placeholder='{placeholder}' type='{type}' id='{property}'
                            name='{property}' {strMaxLength} value='{value}'>
                        {note}
                    </div>
                </div>");
        }

        public static IHtmlString BkndDropdownActiveInactive(this HtmlHelper html,
            string property = "IsActive",
            bool value = true,
            string label = "Status",
            string note = "")
        {
            if (string.IsNullOrEmpty(label))
                label = property.Humanize();

            return new HtmlString($@"
                <div class='form-group'>
                    <label class='col-md-2 control-label'>{label}</label>
                    <div class='col-md-6'>
                        <select class='form-control' id='{property}' name='{property}'>
                            <option value='true' {(value ? "selected" : "")}>Active</option>
                            <option value='false' {(value ? "" : "selected")}>Inactive</option>
                        </select>
                    </div>
                </div>");
        }

        public static IHtmlString BkndLegend(this HtmlHelper html, string label)
        {
            return new HtmlString($"<legend>{label}</legend>");
        }

        public static IHtmlString BkndEditBtn(this HtmlHelper html, string url)
        {
            var controller = html.ViewContext.RouteData.Values["controller"].ToString();

            if (!UserAuth.AdminUser.HasPermission(controller + ".Edit"))
                return new HtmlString("");

            return new HtmlString($@"
                <a href='{url}/{{{{ Id }}}}' rel='tooltip' data-original-title='Edit' data-placement='bottom' class='mglr-2'>
                    <i class='glyphicon glyphicon-edit'></i>
                </a>");
        }

        public static IHtmlString BkndDeleteBtn(this HtmlHelper html)
        {
            var controller = html.ViewContext.RouteData.Values["controller"].ToString();

            if (!UserAuth.AdminUser.HasPermission(controller + ".Delete"))
                return new HtmlString("");

            return new HtmlString(@"
                <a href='javascript:;' class='text-danger mglr-2' data-delete='true' data-id='{{ Id }}' rel='tooltip' data-original-title='Delete' data-placement='bottom'>
                    <i class='glyphicon glyphicon-trash'></i>
                </a>");
        }

        public static IHtmlString BkndMenuItem(this HtmlHelper html, string controller, string action = "Index", string label = null, string permission = null)
        {
            permission = permission ?? controller;
            if (UserAuth.AdminUser.UserId != 1 && !UserAuth.AdminUser.HasPermission(permission))
                return null;

            var itemName = label ?? controller.Humanize(LetterCasing.Title);
            var selectedClass = ""; //html.IsSelected(controller);
            var requestContext = HttpContext.Current.Request.RequestContext;
            var url = new UrlHelper(requestContext).Action(action, controller);

            return new HtmlString($@"<li class='{selectedClass}'><a href='{url}' data-controller='{controller.ToLower()}' data-action='{action.ToLower()}'>{itemName}</a></li>");
        }

        public static string BkndIsAction(this HtmlHelper html, string action, string controller, string trueResult)
        {
            var actionName = html.ViewContext.RouteData.Values["action"].ToString();
            var controllerName = html.ViewContext.RouteData.Values["controller"].ToString();

            return actionName == action && controllerName == controller ? trueResult : "";
        }

        public static string BkndIsNotAction(this HtmlHelper html, string action, string controller, string trueResult)
        {
            var actionName = html.ViewContext.RouteData.Values["action"].ToString();
            var controllerName = html.ViewContext.RouteData.Values["controller"].ToString();

            return actionName != action || controllerName != controller ? trueResult : "";
        }

        public static ImageUploadBuilder BkndImageUpload(this HtmlHelper html, string name)
        {
            return new ImageUploadBuilder(name);
        }

        public static IHtmlString BkndImageUploadScript(this HtmlHelper html, string name)
        {
            return new HtmlString($"<script>funSetImageUploadClickEvents('{name}');</script>");
        }

        public static IHtmlString BkndEditor(this HtmlHelper html,
            string property,
            object value = null,
            string label = "",
            bool required = false)
        {
            if (string.IsNullOrEmpty(label))
                label = property.Humanize();

            var requiredText = required ? "<span class='text-danger'>*</span>" : "";

            return new HtmlString($@"
                <div class='form-group'>
                    <label class='col-md-2 control-label'>{label} {requiredText}</label>
                    <div class='col-md-10'>
                        <textarea data-content-editor name='{property}' id='{property}'>{value}</textarea>
                    </div>
                </div>");
        }
    }
}