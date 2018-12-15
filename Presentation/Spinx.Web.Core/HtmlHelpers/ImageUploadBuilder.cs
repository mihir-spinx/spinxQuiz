using Humanizer;
using System.Web;

namespace Spinx.Web.Core.HtmlHelpers
{
    public class ImageUploadBuilder
    {
        private readonly string _name;
        private string _label;
        private string _value;
        private bool _isRequired = false;

        private int? _imageWidth;
        private int? _imageHeight;

        private int _boxHeight = 250;
        private string _boxBackgroundColor = "fff";

        private int _fileIconSize = 100;
        private string _note = "";

        public ImageUploadBuilder(string name)
        {
            _name = name;
        }

        public ImageUploadBuilder Label(string label) { _label = label; return this; }
        public ImageUploadBuilder Value(string value) { _value = value; return this; }
        public ImageUploadBuilder Required() { _isRequired = true; return this; }

        public ImageUploadBuilder Note(string note) { _note = note; return this; }
        
        public ImageUploadBuilder ImageWidth(int imageWidth) { _imageWidth = imageWidth; return this; }
        public ImageUploadBuilder ImageHeight(int imageHeight) { _imageHeight = imageHeight; return this; }
        
        public ImageUploadBuilder BoxHeight(int boxHeight) { _boxHeight = boxHeight; return this; }
        public ImageUploadBuilder BoxBackgroundColor(string boxBackgroundColor) { _boxBackgroundColor = boxBackgroundColor; return this; }

        public ImageUploadBuilder FileIconSize(int fileIconSize) { _fileIconSize = fileIconSize; return this; }

        public IHtmlString Render()
        {
            var label = string.IsNullOrEmpty(_label) ? _name.Humanize() : _label;
            var requiredTag = _isRequired ? "<span class='text-danger'>*</span>" : "";
            var note = string.IsNullOrEmpty(_note)
                ? ""
                : $"<div class='note pull-right'><strong>Note: </strong> {_note}</div>";
            var imageWidth = _imageWidth != null ? $"max-width: {_imageWidth}px;" : "";
            var imageHeight = _imageHeight != null ?  $"max-height: {_imageHeight}px;" : "" ;
            

            return new HtmlString($@"
            <div class='form-group'>
                <label class='col-md-2 control-label'>{label} {requiredTag}</label>
                <div class='col-md-6'>
                    <input class='form-control' id='{_name}' name='{_name}' type='text' readonly value='{_value}'>
                    <div class='div-img-preview' id='divPreview{_name}' style='height: {_boxHeight}px; font-size: {_fileIconSize}px; background: #{_boxBackgroundColor};'>
                        <i class='fa fa-file-image-o' data-file-icon></i>
                        <img src='' style='{imageWidth}{imageHeight}' data-img-preview />
                    </div>
                    <div style='margin-top: 10px;'>
                        {note}
                        <a href='javascript:;' class='btn btn-link' id='btn{_name}'><i class='fa fa-file-image-o'></i> Browse Files</a>
                        <a href='javascript:;' class='btn btn-link txt-color-red' id='btnRemove{_name}'><i class='fa fa-trash-o'></i> Remove</a>
                    </div>
                </div>
            </div>
            ");
        }
    }
}