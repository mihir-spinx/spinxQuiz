using Spinx.Web.Core.Authentication;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Spinx.Web.Core.HtmlHelpers
{
    public class ListPageToolbarBuilder
    {
        private readonly IList<string> _actions;
        private readonly bool _paging;
        private readonly string _controller;

        public ListPageToolbarBuilder(ViewContext viewContext, IList<string> actions = null, bool paging = true)
        {
            _actions = actions;
            viewContext.TempData["actions"] = _actions;
            _paging = paging;
            viewContext.FormContext = new FormContext();

            _controller = viewContext.RouteData.Values["controller"].ToString();
        }

        public string GenerateOptions()
        {
            var options = "";
            foreach (var action in _actions)
            {
                switch (action.ToLower())
                {
                    case "active":
                    case "inactive":
                    case "remove":

                        if (UserAuth.AdminUser.HasPermission(_controller + ".Edit"))
                            options += $"<option value='{action.ToLower()}'>{action}</option>";

                        break;
                    case "delete":

                        if (UserAuth.AdminUser.HasPermission(_controller + ".Delete"))
                            options += $"<option value='{action.ToLower()}'>{action}</option>";

                        break;
                }
            }

            return options;
        }

        public string ActionMenu()
        {
            if (_actions == null || _actions.Count == 0)
                return "";

            var options = GenerateOptions();

            if (string.IsNullOrEmpty(options))
                return "";

            return $@"
                <label class='mgr-5'>
                    <span class='paging-view-text'>Actions&nbsp;</span>
                    <label class='select'>
                        <select id='selAction' name='dt_basic_length' aria-controls='dt_basic'
                                class='form-control input-sm' autocomplete='off'>
                            <option value=''>[Actions]</option>
                            {options}
                        </select>
                    </label>

                    <button class='btn btn-primary btn-sm mgb-3' id='btnSubmitAction'>Submit</button>
                </label>
                ";
        }

        public string Paging()
        {
            if (!_paging)
                return "";

            return @"
                <label class='mgr-5'>
                    <select name='dt_basic_length' aria-controls='dt_basic'
                            class='form-control input-sm mgt-1' data-page='true'>
                        <option value='10'>10 Items</option>
                        <option value='25'>25 Items</option>
                        <option value='50'>50 Items</option>
                        <option value='100'>100 Items</option>
                        <option value='200'>200 Items</option>
                    </select>
                </label>

                <ul class='pagination pagination-sm mg-0 pagination-list'>
                    <li class='previous disabled'>
                        <a href='javascript:;'>
                            <i class='fa fa-chevron-left'></i>
                        </a>
                    </li>
                    <li class='pagination-info'><span>Page 1 of 10</span></li>
                    <li class='next disabled'>
                        <a href='javascript:;'>
                            <i class='fa fa-chevron-right'></i>
                        </a>
                    </li>
                </ul>";
        }
        

        public IHtmlString Render()
        {
            return new HtmlString($@"
                <div class='dt-toolbar'>
                    <div class='col-xs-12 col-sm-6'>
                        {ActionMenu()}
                    </div>
                    <div class='col-sm-6 col-xs-12 text-right'>
                        <label class='mgr-5 mgt-5 mgb-15'>
                            <span class='paging-view-text'>
                                Results / <span class='text-primary'>100</span> Items&nbsp; {(_paging == false ? "" : " &nbsp; View")}
                            </span>
                        </label>

                        {Paging()}
                    </div>
                </div>");
        }
    }
}