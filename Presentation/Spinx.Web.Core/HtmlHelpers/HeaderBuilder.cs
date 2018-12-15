using Spinx.Web.Core.Authentication;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Spinx.Web.Core.HtmlHelpers
{
    public class HeaderBuilder
    {
        private readonly IList<string> _labels;
        private readonly string _icon;
        private readonly string _backUrl;
        private readonly string _addBtnText;
        private readonly string _addBtnUrl;
        private readonly string _sequenceUrl;
        private readonly string _controller;

        public HeaderBuilder(ViewContext viewContext, IList<string> labels, string icon = "file",
            string backUrl = "",
            string addBtnText = "", string addBtnUrl = "",
            string sequenceUrl = "")
        {
            _labels = labels;
            _icon = icon;
            _backUrl = backUrl;
            _addBtnText = addBtnText;
            _addBtnUrl = addBtnUrl;
            _sequenceUrl = sequenceUrl;
            viewContext.FormContext = new FormContext();

            _controller = viewContext.RouteData.Values["controller"].ToString();
        }

        private string BreadCrumpGenerate()
        {
            var breadcrumbBuilder = _labels[0];
            for (var i = 1; i < _labels.Count; i++)
                breadcrumbBuilder += $" <span> &gt; {_labels[i]}</span>";

            return breadcrumbBuilder;
        }

        private string BackButtonGenerate()
        {
            if (string.IsNullOrEmpty(_backUrl))
                return "";

            return $@"
                <a class='btn bg-color-blueLight txt-color-white' href='{_backUrl}'>
                    <i class='fa fa-long-arrow-left'></i> Back to List
                </a>";
        }

        private string AddButtonGenerate()
        {
            if (string.IsNullOrEmpty(_addBtnUrl))
                return "";

            if (!UserAuth.AdminUser.HasPermission(_controller + ".Create"))
                return "";

            return $@"
                <a class='btn btn-primary' href='{_addBtnUrl}'>
                    <i class='fa fa-plus'></i> Add {_addBtnText}
                </a>";
        }

        private string SequenceButtonGenerate()
        {
            if (string.IsNullOrEmpty(_sequenceUrl))
                return "";

            if (!UserAuth.AdminUser.HasPermission(_controller + ".Sequence"))
                return "";

            return $@"
                <a class='btn btn-primary bg-color-teal' href='{_sequenceUrl}'>
                    <i class='fa fa-sort'></i> Change Sequence
                </a>";
        }

        private string ButtonSectionFactory()
        {
            var backButton = BackButtonGenerate();
            var addButton = AddButtonGenerate();
            var sequenceButton = SequenceButtonGenerate();

            var rightPart = backButton + addButton + sequenceButton;

            if (string.IsNullOrEmpty(rightPart))
                return "";

            return $@"
                <div class='col-xs-12 col-sm-5 col-md-5 col-lg-8 edit-btns'>
                    {rightPart}
                </div>";
        }

        public IHtmlString Render()
        {
            return new HtmlString($@"
                <div class='row'>
                    <div class='col-xs-12 col-sm-7 col-md-7 col-lg-4'>
                        <h1 class='page-title txt-color-blueDark'>
                            <i class='fa-fw fa fa-{_icon}'></i> 
                            {BreadCrumpGenerate()}
                        </h1>
                    </div>
                    {ButtonSectionFactory()}
                </div>");
        }
    }
}