using Humanizer;
using Spinx.Web.Core.Authentication;
using Spinx.Web.Core.HtmlHelpers;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Spinx.Web.Core.Extensions
{
    public static partial class HtmlHelperExtensions
    {
        public static ListPageBodyBuilder BkndBeginList(this HtmlHelper html)
        {
            return new ListPageBodyBuilder(html.ViewContext);
        }

        public static IHtmlString BkndThSearchIdColumn(this HtmlHelper html, bool addForce = false)
        {
            var result = new HtmlString("<th></th>");
            
            if (addForce)
                return result;

            return ValidEditDeletePermissionForCheckboxColumn(html)
                ? result
                : new HtmlString("");
        }

        public static IHtmlString BkndThSearchBlankColumn(this HtmlHelper html)
        {
            return new HtmlString("<th></th>");
        }

        public static IHtmlString BkndThSearchTextbox(this HtmlHelper html, string fieldName)
        {
            return new HtmlString($@"
                <th class='hasinput' style='vertical-align: middle;'>
                    <input type='text' class='form-control' placeholder='{("Filter " + fieldName).Humanize()}' name='{fieldName}'>
                </th>");
        }

        public static IHtmlString BkndThSearchTrueFalse(this HtmlHelper html, string fieldName)
        {
            return new HtmlString($@"
                <th class='hasinput' style='vertical-align: middle;'>
                    <select class='form-control' autocomplete='off' name='{fieldName}'>
                        <option value=''>[{fieldName.Humanize()}]</option>
                        <option value='True'>True</option>
                        <option value='False'>False</option>
                    </select>
                </th>");
        }

        public static IHtmlString BkndThSearchIsActive(this HtmlHelper html)
        {
            return new HtmlString(@"
                <th class='hasinput' style='vertical-align: middle;'>
                    <select class='form-control' autocomplete='off' name='IsActive'>
                        <option value=''>[Status]</option>
                        <option value='True'>Active</option>
                        <option value='False'>Inactive</option>
                    </select>
                </th>");
        }

        public static IHtmlString BkndThSearchDate(this HtmlHelper html, string fieldName)
        {
            return new HtmlString($@"
                <th class='hasinput' rowspan='1' colspan='1'>
	                <div class='input-group mgb-5'>
		                <input type='text' name='From{fieldName}' placeholder='From date'
			                   class='form-control datepicker' data-dateformat='mm/dd/yy'>
		                <span class='input-group-addon'><i class='fa fa-calendar'></i></span>
	                </div>
	                <div class='input-group'>
		                <input type='text' name='To{fieldName}' placeholder='To date'
			                   class='form-control datepicker' data-dateformat='mm/dd/yy'>
		                <span class='input-group-addon'><i class='fa fa-calendar'></i></span>
	                </div>
                </th>");
        }

        public static IHtmlString BkndThSearchActionColumn(this HtmlHelper html, bool addForce = false)
        {
            var result = new HtmlString("<th></th>");
            
            if (addForce)
                return result;

            return ValidEditDeletePermissionForActionColumn(html)
                ? result
                : new HtmlString("");
        }

        public static IHtmlString BkndTrNoRecords(this HtmlHelper html)
        {
            return new HtmlString(@"
                <tr>
                    <td class='text-center' colspan='99'>No records</td>
                </tr>");
        }

        //public static IHtmlString BkndThSearchDropdown(this HtmlHelper html, string fieldName, IDictionary<int, string> list)
        //{
        //    var fieldLable = ("Filter" + fieldName.TrimEnd("Id")).Humanize();
        //    var options = list.Aggregate("",
        //        (current, singleItem) => current + $"<option value='{singleItem.Key}'>{singleItem.Value}</option>");

        //    return new HtmlString($@"
        //        <th class='hasinput' style='vertical-align: middle;'>
        //            <select class='form-control' autocomplete='off' name='{fieldName}'>
        //                <option value=''>[{fieldLable}]</option>
        //                {options}
        //            </select>
        //        </th>");
        //}

        #region List search section

        public static IHtmlString BkndThHeaderCheckbox(this HtmlHelper html, bool addForce = false)
        {
            var result = new HtmlString("<th style='width: 25px;' class='text-center hasinput'><input type='checkbox' /></th>");
            
            if (addForce)
                return result;

            return ValidEditDeletePermissionForCheckboxColumn(html)
                ? result
                : new HtmlString("");
        }

        public static IHtmlString BkndThHeader(this HtmlHelper html, string fieldName, string sort = "", int? width = null, bool center = false)
        {
            var cssWidth = width != null ? $"width: {width}px;" : "";
            var classCenter = center ? "text-center" : "";
            var sortField = !string.IsNullOrEmpty(sort) ? $"data-name='{sort}'" : "";

            return new HtmlString($@"
                <th style='{cssWidth}' class='{classCenter}' {sortField}>
                    {fieldName.Humanize(LetterCasing.Title)}
                </th>");
        }

        public static IHtmlString BkndThHeaderIsActive(this HtmlHelper html, string sort = "IsActive")
        {
            var sortField = !string.IsNullOrEmpty(sort) ? $"data-name='{sort}'" : "";

            return new HtmlString($"<th style='width: 80px;' class='text-center' {sortField}>Status</th>");
        }

        public static IHtmlString BkndThHeaderDatetime(this HtmlHelper html, string label, string sort = "")
        {
            var sortField = !string.IsNullOrEmpty(sort) ? $"data-name='{sort}'" : "";

            return new HtmlString($"<th style='width: 120px;' class='text-center' {sortField}>{label.Humanize(LetterCasing.Title)}</th>");
        }

        public static IHtmlString BkndThHeaderActionColumn(this HtmlHelper html, bool addForce = false)
        {
            var result = new HtmlString("<th style='width: 80px;' class='text-center'>Action</th>");
            
            if (addForce)
                return result;

            return ValidEditDeletePermissionForActionColumn(html)
                ? result
                : new HtmlString("");
        }

        #endregion

        #region Table Body Columns

        public static IHtmlString BkndTdCheckboxId(this HtmlHelper html, bool addForce = false)
        {
            var result = new HtmlString("<td class='text-center'><input type='checkbox' value='{{Id}}' /></td>");
            
            if (addForce)
                return result;

            return ValidEditDeletePermissionForCheckboxColumn(html)
                ? result
                : new HtmlString("");
        }

        public static IHtmlString BkndTd(this HtmlHelper html, string field, bool center = false)
        {
            var classCenter = center ? "text-center" : "";

            return new HtmlString($"<td class='{classCenter}'>{{{{ {field} }}}}</td>");
        }

        public static IHtmlString BkndTdHumanize(this HtmlHelper html, string field, bool center = false)
        {
            var classCenter = center ? "text-center" : "";

            return new HtmlString($"<td class='{classCenter}'>{{{{ humanize {field} }}}}</td>");
        }

        public static IHtmlString BkndTdIfZeroThen(this HtmlHelper html, string field, string ifNullThen = "-")
        {
            return new HtmlString($@"
                <td class='text-center'>
                    {{{{#ifCond {field} '!=' 0}}}}
                        {{{{{field}}}}}
                    {{{{else}}}}
                        {ifNullThen}
                    {{{{/ ifCond}}}}
                </td>");
        }

        public static IHtmlString BkndTdTrue(this HtmlHelper html, string field)
        {
            return new HtmlString($@"
                <td class='text-center'>
                    {{{{#ifCond {field} '==' true}}}}
                        <strong class='txt-color-blue'>Yes</strong>
                    {{{{else}}}}
                        -
                    {{{{/ ifCond}}}}
                </td>");
        }

        public static IHtmlString BkndTdIsActive(this HtmlHelper html)
        {
            return new HtmlString(@"
                <td class='text-center'>
                    {{#ifCond IsActive '==' true}}
                        <span class='label label-success'>Active</span>
                    {{else}}
                        <span class='label label-default'>Inactive</span>
                    {{/ ifCond}}
                </td>");
        }

        public static IHtmlString BkndTdDatetime(this HtmlHelper html, string field)
        {
            return new HtmlString($"<td class='text-center'>{{{{ datetime {field} }}}}</td>");
        }

        public static IHtmlString BkndTdDate(this HtmlHelper html, string field)
        {
            return new HtmlString($"<td class='text-center'>{{{{ date {field} }}}}</td>");
        }

        public static IHtmlString BkndTdDocument(this HtmlHelper html, string field)
        {
            return new HtmlString($"<td class='text-center'><a href='{{{{ {field} }}}}' target='_blank'><i class='fa fa-file-pdf-o'></i></a></td>");
        }

        public static IHtmlString BkndTdImage(this HtmlHelper html, string field, int height = 50, string suffixPath = "")
        {
            return new HtmlString($"<td class='text-center'><img src='{suffixPath}{{{{ {field} }}}}' alt='' height='{height}' /></td>");
        }

        public static IHtmlString BkndTdUrl(this HtmlHelper html, string field)
        {
            return new HtmlString($"<td class='text-center'><a href='{{{{ {field} }}}}' target='_blank'><i class='fa fa-globe'></i></a></td>");
        }

        public static IHtmlString BkndTdAction(this HtmlHelper html, string editUrl = "", bool delete = true)
        {
            var editHtml = !string.IsNullOrEmpty(editUrl) ? html.BkndEditBtn(editUrl).ToString() : "";
            var deleteHtml = delete ? html.BkndDeleteBtn().ToString() : "";

            return ValidEditDeletePermissionForActionColumn(html)
                ? new HtmlString($@"<td class='text-center'>{editHtml}{deleteHtml}</td>")
                : new HtmlString("");
        }

        public static IHtmlString BkndTdActionRemove(this HtmlHelper html)
        {
            return new HtmlString(@"
                <td class='text-center'>
                    <a href='javascript:;' class='text-danger mglr-2' data-remove='true' data-id='{{ Id }}' rel='tooltip' data-original-title='Remove' data-placement='bottom'>
                        <i class='glyphicon glyphicon-remove'></i>
                    </a>
                </td>");
        }

        #endregion

        private static bool ValidEditDeletePermissionForCheckboxColumn(HtmlHelper html)
        {
            var actions = (IList<string>)html.ViewContext.TempData["actions"];
            if (actions == null)
                return false;

            var controller = html.ViewContext.RouteData.Values["controller"].ToString();

            // Remove: remove from associeated records.
            var editFlag = (actions.Contains("Active") || actions.Contains("Inactive") || actions.Contains("Remove")) &&
                           UserAuth.AdminUser.HasPermission(controller + ".Edit");

            var deleteFlag = actions.Contains("Delete") && UserAuth.AdminUser.HasPermission(controller + ".Delete");

            return editFlag || deleteFlag;
        }

        private static bool ValidEditDeletePermissionForActionColumn(HtmlHelper html)
        {
            var controller = html.ViewContext.RouteData.Values["controller"].ToString();

            return UserAuth.AdminUser.HasPermission(controller + ".Edit") ||
                   UserAuth.AdminUser.HasPermission(controller + ".Delete");
        }
    }
}