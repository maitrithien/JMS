using DMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    public class UploadController : Controller
    {
        /// <summary>
        /// JMS entities
        /// </summary>
        JMSEntities _EntityModel = new JMSEntities();

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var directory = Server.MapPath("~/App_Data/" + User.Identity.Name);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var physicalPath = Path.Combine(directory, fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("0");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + User.Identity.Name), fileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("1");
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileStreamResult Download(string id)
        {
            Guid apk = Guid.Empty;
            if(!string.IsNullOrEmpty(id))
            {
                Guid.TryParse(id, out apk);
            }

            var currentFile = _EntityModel.Attachments
                .FirstOrDefault(x => x.APK == apk);

            var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + User.Identity.Name), currentFile.AttachmentFileName);
            MemoryStream streamFile = new MemoryStream();

            if (System.IO.File.Exists(physicalPath))
            {
                streamFile = new MemoryStream(System.IO.File.ReadAllBytes(physicalPath));
            }

            return File(streamFile, "application/force-download", currentFile.AttachmentFileName); 
        }
    }
}
