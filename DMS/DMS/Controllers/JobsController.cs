using DMS.Bussiness;
using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DMS.Controllers
{
    public class JobsController : Controller
    {
        /// <summary>
        /// JMS entities
        /// </summary>
        JMSEntities _EntityModel = new JMSEntities();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult JobsOverdue()
        {
            return View();
        }

        [Authorize]
        public ActionResult JobsSent()
        {
            return View();
        }

        [Authorize]
        public ActionResult JobsReceived()
        {
            return View();
        }

        [Authorize]
        public ActionResult JobsStatc()
        {
            return View();
        }

        public JsonResult JobsGrid()
        {
            JobBussiness _jobBuzz = new JobBussiness();
            List<JobModels> model = _jobBuzz.GetAll();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NotesGrid()
        {
            JobBussiness _jobBuzz = new JobBussiness();
            List<NoteModels> model = _jobBuzz.GetNotes(Guid.NewGuid());

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AttachmentsGrid(string apk)
        {
            Guid id = Guid.Empty;
            if(!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            List<Attachment> model = _EntityModel.Attachments.ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HistoriesGrid()
        {
            JobBussiness _jobBuzz = new JobBussiness();
            List<HistoryModels> model = _jobBuzz.GetHistories(Guid.NewGuid());

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JobDelete()
        {
             return new JsonResult();
        }

        public ActionResult JobInsert()
        {
            return new JsonResult();
        }

        public ActionResult Update()
        {
            return new JsonResult();
        }

        [Authorize]
        public ActionResult JobDialog()
        {
            JobModels model = new JobModels();
            return PartialView(model);
        }

        [Authorize]
        public ActionResult NoteDialog()
        {
            NoteModels model = new NoteModels();
            return PartialView(model);
        }

        [Authorize]
        public ActionResult AttachmentDialog()
        {
            AttachmentModels model = new AttachmentModels();
            return PartialView(model);
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult AddAttachments(List<AttachmentModels> models)
        {
            var attachments = models ?? new List<AttachmentModels>();

            foreach (var item in attachments)
            {
                Attachment att = new Attachment
                {
                    APK = Guid.NewGuid(),
                    AttachmentComment = item.AttachmentComment,
                    AttachmentFileName = item.AttachmentFileName,
                    AttachmentFileExtension = item.AttachmentFileExtension,
                    AttachmentFileSize = item.AttachmentFileSize,
                    AttachmentFileType = item.AttachmentFileType,
                    AttachmentOwner = User.Identity.Name,
                    JobAPK = item.JobAPK ?? Guid.Empty,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = User.Identity.Name,
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = User.Identity.Name
                };

                _EntityModel.Attachments.AddObject(att);
                _EntityModel.SaveChanges();
            }

            return Json(new { result = true });
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult RemoveAttachments(List<AttachmentModels> models)
        {
            var attachments = models ?? new List<AttachmentModels>();

            foreach (var item in attachments)
            {
                var finder = _EntityModel.Attachments.Where(x => 
                    x.AttachmentFileName == item.AttachmentFileName &&
                    x.AttachmentOwner == User.Identity.Name).FirstOrDefault();

                if (finder != null)
                {
                    _EntityModel.Attachments.DeleteObject(finder);
                    _EntityModel.SaveChanges();
                }
            }

            return Json(new { result = true });
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult RemoveAttachment(AttachmentModels model)
        {
            var item = model ?? new AttachmentModels();

            var finder = _EntityModel.Attachments.Where(x =>
                    x.APK == item.APK).FirstOrDefault();

            if (finder != null)
            {
                _EntityModel.Attachments.DeleteObject(finder);
                _EntityModel.SaveChanges();

                var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + User.Identity.Name), finder.AttachmentFileName);

                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed 
                    System.IO.File.Delete(physicalPath);
                }
            }

            return Json(new { result = true });
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult UpdateAttachment(AttachmentModels model)
        {
            var item = model ?? new AttachmentModels();

            var finder = _EntityModel.Attachments.Where(x =>
                    x.APK == item.APK).FirstOrDefault();

            if (finder != null)
            {
                finder.AttachmentFileName = item.AttachmentFileName;
                finder.AttachmentComment = item.AttachmentComment;

                _EntityModel.SaveChanges();
            }

            return Json(new { result = true });
        }

        [Authorize]
        public ActionResult UpdateAttachmentDialog(string apk)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            Attachment att = _EntityModel.Attachments.FirstOrDefault(x => x.APK == id);

            AttachmentModels model = new AttachmentModels
            {
                APK = att.APK,
                AttachmentComment = att.AttachmentComment,
                AttachmentFileExtension = att.AttachmentFileExtension,
                AttachmentFileName = att.AttachmentFileName,
                AttachmentFileSize = att.AttachmentFileSize,
                AttachmentFileType = att.AttachmentFileType,
                AttachmentOwner = att.AttachmentOwner,
                CreatedDate = att.CreatedDate,
                CreatedUserID = att.CreatedUserID
            };

            return PartialView(model);
        }

        [Authorize]
        public ActionResult JobView(string id)
        {
            JobModels model = new JobModels();
            JobBussiness _jobBuzz = new JobBussiness();
            model = _jobBuzz.GetAll().FirstOrDefault();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
