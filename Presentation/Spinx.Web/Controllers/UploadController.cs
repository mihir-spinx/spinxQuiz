using Spinx.Core;
using Spinx.Web.Infrastructure;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IAppSettings _appSettings;

        public UploadController(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FileUpload()
        {
            var res = new Result();
            var tempUploads = _appSettings.TempUploads;

            if (Request.Files.Count > 0)
            {
                var objFile = Request.Files[0];
                var supportedTypes = new[] {".doc", ".docx", ".pdf"};
                var fileExt = System.IO.Path.GetExtension(objFile.FileName).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    res.SetError("File Extension Is InValid - Only Upload WORD or PDF File");
                }
                else
                {
                    var originalFileName = System.IO.Path.GetFileName(objFile.FileName);
                    var fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(objFile.FileName);
                    objFile.SaveAs(Server.MapPath("~/" + tempUploads + fileName));

                    res.Data = new
                    {
                        fileName = fileName,
                        filePath = tempUploads
                    };
                    res.SetSuccess(fileName);
                }
            }

            return ToJsonResult(res);
        }

        [HttpPost]
        public JsonResult ImageUpload()
        {
            var res = new Result();
            var tempUploads = _appSettings.TempUploads;

            if (Request.Files.Count > 0)
            {
                var objFile = Request.Files[0];
                var supportedTypes = new[] {".jpg", ".jpeg", ".png", ".gif"};
                var fileExt = System.IO.Path.GetExtension(objFile.FileName).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    res.SetError("File Extension Is InValid - Only Upload JPEG,JPG, PNG or GIF File");
                }
                else
                {
                    var originalFileName = System.IO.Path.GetFileName(objFile.FileName);
                    var fileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(objFile.FileName);

                    objFile.SaveAs(Server.MapPath("~/" + tempUploads + fileName));

                    res.Data = new
                    {
                        fileName = fileName,
                        filePath = tempUploads
                    };
                    res.SetSuccess(fileName);
                }
            }

            return ToJsonResult(res);
        }
    }
}