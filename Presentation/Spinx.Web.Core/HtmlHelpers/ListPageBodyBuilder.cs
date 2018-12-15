using System;
using System.IO;
using System.Web.Mvc;

namespace Spinx.Web.Core.HtmlHelpers
{
    public class ListPageBodyBuilder : IDisposable
    {
        private bool _disposed;
        private readonly FormContext _originalFormContext;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;

        public ListPageBodyBuilder(ViewContext viewContext)
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
            _writer.Write(@"
                <div class='jarviswidget well'>
                    <div>
                        <div class='widget-body no-padding'>
                            <div class='table-responsive dataTables_wrapper' data-table='true'>

                            <div data-msg></div>");
        }

        private void End()
        {
            _writer.Write(@"
                            </div>
                        </div>
                    </div>
                </div>");
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