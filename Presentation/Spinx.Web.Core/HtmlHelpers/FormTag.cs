using System;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Spinx.Web.Core.HtmlHelpers
{
    public class FormTag : IDisposable
    {
        private bool _disposed;
        private readonly FormContext _originalFormContext;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;

        public FormTag(ViewContext viewContext)
        {
            _viewContext = viewContext ?? throw new ArgumentNullException("viewContext");
            _writer = viewContext.Writer;
            _originalFormContext = viewContext.FormContext;
            viewContext.FormContext = new FormContext();

            Begin();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Begin()
        {
            _writer.Write("<form action='' class='form-horizontal' data-ajax='true' method='POST'>");
            _writer.Write("<fieldset>");
            _writer.Write("<div data-msg></div>");
            _writer.Write(AntiForgery.GetHtml().ToString());
        }

        private void End()
        {
            _writer.Write("</fieldset>");
            _writer.Write("</form>");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
                return;

            _disposed = true;
            End();

            if (_viewContext == null) 
                return;

            _viewContext.OutputClientValidation();
            _viewContext.FormContext = _originalFormContext;
        }
    }
}