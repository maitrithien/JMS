using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using System.Threading;

namespace DMS.Controllers
{
    public class JobsController : Controller
    {
        /// <summary>
        /// JMS entities
        /// </summary>
        JMSEntities _EntityModel = new JMSEntities();

        /// <summary>
        /// Current employee login
        /// </summary>
        private string _EmployeeID {
            get
            {
                return (_EntityModel.Employees.FirstOrDefault(
                    x => User.Identity.Name.Equals(x.UserName)) ?? new Employee()).EmployeeID;
            }
        }
        /// <summary>
        /// Current manager of this account
        /// </summary>
        private string _ManagerID
        {
            get
            {
                var departmentId = (_EntityModel.Employees.FirstOrDefault(
                    x => User.Identity.Name.Equals(x.UserName)) ?? new Employee()).DepartmentID;
                return (_EntityModel.Departments.FirstOrDefault(x => x.DepartmentID == departmentId) ?? new Department()).ManagerID;
            }
        }

        #region ---- Jobs ----

        /// <summary>
        /// Counter
        /// </summary>
        /// <returns></returns>
        public ActionResult CountJobEmps()
        {
            // Đếm số lượng hồ sơ (Notify menu)
            var counter = _EntityModel.GetCounterJobs(User.Identity.Name).FirstOrDefault();
            var feeds = _EntityModel.Feeds
                .Where(x => x.Reader == _EmployeeID && !x.Read)
                .Count();

            return Json(new
            {
                countE = string.Format("{0:00}", counter.CountE ?? 0),
                countS = string.Format("{0:00}", counter.CountS ?? 0),
                countR = string.Format("{0:00}", counter.CountR ?? 0),
                countP = string.Format("{0:00}", counter.CountP ?? 0),
                countO = string.Format("{0:00}", counter.CountO ?? 0),
                countM = string.Format("{0:00}", feeds)
            });
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewData["MenuSelected"] = "Index";

            return View();
        }

        [Authorize]
        public ActionResult JobsOverdue()
        {
            ViewData["MenuSelected"] = "JobsOverdue";
            return View();
        }

        [Authorize]
        public ActionResult JobsSent()
        {
            ViewData["MenuSelected"] = "JobsSent";
            return View();
        }

        [Authorize]
        public ActionResult JobsReceived()
        {
            ViewData["MenuSelected"] = "JobsReceived";
            return View();
        }

        [Authorize]
        public ActionResult JobsEmployee()
        {
            ViewData["MenuSelected"] = "JobsEmployee";
            return View();
        }

        [Authorize]
        public ActionResult JobsStatc()
        {
            ViewData["MenuSelected"] = "JobsStatc";
            return View();
        }

        public JsonResult GridReceived(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            #region --- Load data ----

            var result = _EntityModel.GetJobs(model.IsFilter,
                model.JobIDFilter, model.JobNameFilter, model.StatusFilter, model.PosterFilter,
                model.RecipientFilter, model.ConfirmerFilter, model.DeadlineFilter, model.PriorityFilter,
                model.RateFilter, model.ComplexFilter, model.DepartmentIDFilter, User.Identity.Name, JobModels.JOBS_R_TYPE).ToList();

            if (result != null && result.Count() > 0)
            {
                lst = result.Select(x => new JobModels()
                {
                    APK = x.APK,
                    Complex = x.Complex,
                    ComplexName = x.ComplexName,
                    Confirmer = x.Confirmer,
                    ConfirmerName = x.ConfirmerName,
                    CreatedDate = x.CreatedDate,
                    CreatedUserID = x.CreatedUserID,
                    Deadline = x.Deadline,
                    DepartmentID = x.DepartmentID,
                    DepartmentName = x.DepartmentName,
                    JobID = x.JobID,
                    JobName = x.JobName,
                    LastModifyDate = x.LastModifyDate,
                    LastModifyUserID = x.LastModifyUserID,
                    Note = x.Note,
                    OverDeadlineNumber = (double)(x.OverDeadlineNumber ?? 0),
                    Poster = x.Poster,
                    PosterName = x.PosterName,
                    Priority = x.Priority,
                    PriorityName = x.PriorityName,
                    Rate = x.Rate,
                    RateComment = x.RateComment,
                    RateName = x.RateName,
                    Recipient = x.Recipient,
                    RecipientName = x.RecipientName,
                    Status = x.Status,
                    StatusConfirm = x.StatusConfirm,
                    StatusConfirmName = x.StatusConfirmName,
                    StatusName = x.StatusName,
                    Sender = x.Sender,
                    SenderName = x.SenderName
                }).ToList();
            }

            #endregion --- Load data ----

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GridSent(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            #region --- Load data ----

            List<GetJobs_Result> result = _EntityModel.GetJobs(model.IsFilter,
                model.JobIDFilter, model.JobNameFilter, model.StatusFilter, model.PosterFilter,
                model.RecipientFilter, model.ConfirmerFilter, model.DeadlineFilter, model.PriorityFilter,
                model.RateFilter, model.ComplexFilter, model.DepartmentIDFilter, User.Identity.Name, JobModels.JOBS_S_TYPE).ToList();

            if (result != null && result.Count() > 0)
            {
                lst = result.Select(x => new JobModels()
                {
                    APK = x.APK,
                    Complex = x.Complex,
                    ComplexName = x.ComplexName,
                    Confirmer = x.Confirmer,
                    ConfirmerName = x.ConfirmerName,
                    CreatedDate = x.CreatedDate,
                    CreatedUserID = x.CreatedUserID,
                    Deadline = x.Deadline,
                    DepartmentID = x.DepartmentID,
                    DepartmentName = x.DepartmentName,
                    JobID = x.JobID,
                    JobName = x.JobName,
                    LastModifyDate = x.LastModifyDate,
                    LastModifyUserID = x.LastModifyUserID,
                    Note = x.Note,
                    OverDeadlineNumber = (double)(x.OverDeadlineNumber ?? 0),
                    Poster = x.Poster,
                    PosterName = x.PosterName,
                    Priority = x.Priority,
                    PriorityName = x.PriorityName,
                    Rate = x.Rate,
                    RateComment = x.RateComment,
                    RateName = x.RateName,
                    Recipient = x.Recipient,
                    RecipientName = x.RecipientName,
                    Status = x.Status,
                    StatusConfirm = x.StatusConfirm,
                    StatusConfirmName = x.StatusConfirmName,
                    StatusName = x.StatusName,
                    Sender = x.Sender,
                    SenderName = x.SenderName
                }).ToList();
            }

            #endregion --- Load data ----

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GridOverdue(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            #region --- Load data ----

            var result = _EntityModel.GetJobs(model.IsFilter,
                model.JobIDFilter, model.JobNameFilter, model.StatusFilter, model.PosterFilter,
                model.RecipientFilter, model.ConfirmerFilter, model.DeadlineFilter, model.PriorityFilter,
                model.RateFilter, model.ComplexFilter, model.DepartmentIDFilter, User.Identity.Name, JobModels.JOBS_O_TYPE).ToList();

            if (result != null && result.Count() > 0)
            {
                lst = result.Select(x => new JobModels()
                {
                    APK = x.APK,
                    Complex = x.Complex,
                    ComplexName = x.ComplexName,
                    Confirmer = x.Confirmer,
                    ConfirmerName = x.ConfirmerName,
                    CreatedDate = x.CreatedDate,
                    CreatedUserID = x.CreatedUserID,
                    Deadline = x.Deadline,
                    DepartmentID = x.DepartmentID,
                    DepartmentName = x.DepartmentName,
                    JobID = x.JobID,
                    JobName = x.JobName,
                    LastModifyDate = x.LastModifyDate,
                    LastModifyUserID = x.LastModifyUserID,
                    Note = x.Note,
                    OverDeadlineNumber = (double)(x.OverDeadlineNumber ?? 0),
                    Poster = x.Poster,
                    PosterName = x.PosterName,
                    Priority = x.Priority,
                    PriorityName = x.PriorityName,
                    Rate = x.Rate,
                    RateComment = x.RateComment,
                    RateName = x.RateName,
                    Recipient = x.Recipient,
                    RecipientName = x.RecipientName,
                    Status = x.Status,
                    StatusConfirm = x.StatusConfirm,
                    StatusConfirmName = x.StatusConfirmName,
                    StatusName = x.StatusName,
                    Sender = x.Sender,
                    SenderName = x.SenderName
                }).ToList();
            }

            #endregion --- Load data ----

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GridPersonal(JobModels model)
        {
            // Xử lý đa ngôn ngữ
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            #region --- Load data ----

            var result = _EntityModel.GetJobs(model.IsFilter,
                model.JobIDFilter, model.JobNameFilter, model.StatusFilter, model.PosterFilter,
                model.RecipientFilter, model.ConfirmerFilter, model.DeadlineFilter, model.PriorityFilter,
                model.RateFilter, model.ComplexFilter, model.DepartmentIDFilter, User.Identity.Name, JobModels.JOBS_P_TYPE).ToList();

            if (result != null && result.Count() > 0)
            {
                lst = result.Select(x => new JobModels()
                {
                    APK = x.APK,
                    Complex = x.Complex,
                    ComplexName = x.ComplexName,
                    Confirmer = x.Confirmer,
                    ConfirmerName = x.ConfirmerName,
                    CreatedDate = x.CreatedDate,
                    CreatedUserID = x.CreatedUserID,
                    Deadline = x.Deadline,
                    DepartmentID = x.DepartmentID,
                    DepartmentName = x.DepartmentName,
                    JobID = x.JobID,
                    JobName = x.JobName,
                    LastModifyDate = x.LastModifyDate,
                    LastModifyUserID = x.LastModifyUserID,
                    Note = x.Note,
                    OverDeadlineNumber = (double)(x.OverDeadlineNumber ?? 0),
                    Poster = x.Poster,
                    PosterName = x.PosterName,
                    Priority = x.Priority,
                    PriorityName = x.PriorityName,
                    Rate = x.Rate,
                    RateComment = x.RateComment,
                    RateName = x.RateName,
                    Recipient = x.Recipient,
                    RecipientName = x.RecipientName,
                    Status = x.Status,
                    StatusConfirm = x.StatusConfirm,
                    StatusConfirmName = x.StatusConfirmName,
                    StatusName = x.StatusName,
                    Sender = x.Sender,
                    SenderName = x.SenderName
                }).ToList();
            }

            #endregion --- Load data ----

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GridEmployee(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            #region --- Load data ----

            var result = _EntityModel.GetJobs(model.IsFilter,
                model.JobIDFilter, model.JobNameFilter, model.StatusFilter, model.PosterFilter,
                model.RecipientFilter, model.ConfirmerFilter, model.DeadlineFilter, model.PriorityFilter,
                model.RateFilter, model.ComplexFilter, model.DepartmentIDFilter, User.Identity.Name, JobModels.JOBS_E_TYPE).ToList();

            if (result != null && result.Count() > 0)
            {
                lst = result.Select(x => new JobModels()
                {
                    APK = x.APK,
                    Complex = x.Complex,
                    ComplexName = x.ComplexName,
                    Confirmer = x.Confirmer,
                    ConfirmerName = x.ConfirmerName,
                    CreatedDate = x.CreatedDate,
                    CreatedUserID = x.CreatedUserID,
                    Deadline = x.Deadline,
                    DepartmentID = x.DepartmentID,
                    DepartmentName = x.DepartmentName,
                    JobID = x.JobID,
                    JobName = x.JobName,
                    LastModifyDate = x.LastModifyDate,
                    LastModifyUserID = x.LastModifyUserID,
                    Note = x.Note,
                    OverDeadlineNumber = (double)(x.OverDeadlineNumber ?? 0),
                    Poster = x.Poster,
                    PosterName = x.PosterName,
                    Priority = x.Priority,
                    PriorityName = x.PriorityName,
                    Rate = x.Rate,
                    RateComment = x.RateComment,
                    RateName = x.RateName,
                    Recipient = x.Recipient,
                    RecipientName = x.RecipientName,
                    Status = x.Status,
                    StatusConfirm = x.StatusConfirm,
                    StatusConfirmName = x.StatusConfirmName,
                    StatusName = x.StatusName,
                    Sender = x.Sender,
                    SenderName = x.SenderName
                }).ToList();
            }

            #endregion --- Load data ----

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEdit(JobModels model)
        {
            var item = model ?? new JobModels();

            bool allowEdit = true;
            string message = string.Empty;

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                if (finder.Status == "2")
                {
                    if (!_EmployeeID.Equals(finder.Recipient))
                    {
                        allowEdit = false;
                        message = string.Format("Hồ sơ {0} đã hoàn tất. Bạn không có quyền sửa hồ sơ này.", finder.JobID);
                    }
                }
            }

            return Json(new { result = allowEdit, message = message });
        }

        public ActionResult CheckDelete(JobModels model)
        {
            var item = model ?? new JobModels();

            bool allowEdit = false;
            string message = string.Empty;

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                if (!string.IsNullOrEmpty(finder.Status) && finder.Status.Equals("0")
                    && (_EmployeeID.Equals(finder.Poster) || _EmployeeID.Equals(finder.CreatedUserID)))
                {
                    // Xóa hồ sơ
                    allowEdit = true;
                }
                else
                {
                    message = string.Format("Bạn không có quyền xóa hồ sơ {0}", finder.JobID);
                }
            }

            return Json(new { result = allowEdit, message = message });
        }

        public ActionResult Delete(JobModels model)
        {
            var item = model ?? new JobModels();

            var finder = _EntityModel.Jobs.Where(x =>
                    x.APK == item.APK).FirstOrDefault();

            if (finder != null)
            {
                _EntityModel.Jobs.DeleteObject(finder);

                var attachments = _EntityModel.Attachments.Where(x => x.JobAPK == finder.APK).ToList() ?? new List<Attachment>();
                var notes = _EntityModel.Notes.Where(x => x.JobAPK == finder.APK).ToList() ?? new List<Note>();
                var histories = _EntityModel.Histories.Where(x => x.JobAPK == finder.APK).ToList() ?? new List<History>();

                // Xóa đính kèm
                foreach (var att in attachments)
                {
                    _EntityModel.Attachments.DeleteObject(att);
                }

                // Xóa ghi chú
                foreach (var note in notes)
                {
                    _EntityModel.Notes.DeleteObject(note);
                }

                // Xóa lịch sử
                foreach (var his in histories)
                {
                    _EntityModel.Histories.DeleteObject(his);
                }

                // Lưu thay đổi
                _EntityModel.SaveChanges();

                // Xóa file vật lý đính kèm sau khi xóa JOB hoàn tất
                foreach (var att in attachments)
                {
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + _EmployeeID), att.AttachmentFileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed 
                        System.IO.File.Delete(physicalPath);
                    }
                }

            }

            return Json(new { result = true });
        }

        public ActionResult Update(JobModels model)
        {
            JobModels item = model ?? new JobModels();

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                // Do Update
                finder.Complex = item.Complex;
                finder.Confirmer = item.Confirmer;
                finder.Deadline = item.Deadline;
                finder.DepartmentID = item.DepartmentID;
                finder.JobName = item.JobName;
                finder.Note = item.Note;
                finder.Poster = item.Poster;
                finder.Priority = item.Priority;
                finder.Rate = string.IsNullOrEmpty(item.Rate) ? finder.Rate : item.Rate;
                finder.StatusConfirm = string.IsNullOrEmpty(item.StatusConfirm) ? finder.StatusConfirm : item.StatusConfirm;
                finder.RateComment = string.IsNullOrEmpty(item.RateComment) ? finder.RateComment : item.RateComment;
                finder.Recipient = item.Recipient;
                finder.Status = item.Status == "0"
                    ? (item.StatusConfirm == "1")
                        ? "1" : item.Status
                    : item.Status;
                finder.LastModifyDate = DateTime.Now;
                finder.LastModifyUserID = User.Identity.Name;

                _EntityModel.SaveChanges();
            }
            else
            {
                // Do Insert
                Job job = new Job
                {
                    APK = Guid.NewGuid(),
                    Complex = item.Complex,
                    Confirmer = item.Confirmer,
                    Deadline = item.Deadline,
                    DepartmentID = item.DepartmentID,
                    JobID = item.JobID,
                    JobName = item.JobName,
                    Note = item.Note,
                    Poster = item.Poster,
                    Priority = item.Priority,
                    Rate = item.Rate,
                    Recipient = item.Recipient,
                    Status = item.Status,
                    StatusConfirm = item.StatusConfirm ?? "0",
                    RateComment = item.RateComment,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = User.Identity.Name,
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = User.Identity.Name,
                };

                _EntityModel.Jobs.AddObject(job);
                _EntityModel.SaveChanges();

                item.APK = job.APK;
            }

            return Json(new { result = true, id = item.APK });
        }

        [Authorize]
        public ActionResult JobDialog(JobModels model)
        {
            JobModels item = model ?? new JobModels();

            item.Poster = _EmployeeID;
            item.Status = "0";
            item.Complex = "1";
            item.Priority = "1";
            item.Deadline = DateTime.Now.Date;
            item.Poster = _EmployeeID;
            item.Recipient = _EmployeeID;
            item.Confirmer = _ManagerID;

            bool isUpdate = false;
            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == (item.APK ?? Guid.Empty));
            if (finder != null)
            {
                isUpdate = true;

                item.Complex = finder.Complex;
                item.Confirmer = finder.Confirmer;
                item.CreatedDate = finder.CreatedDate;
                item.CreatedUserID = finder.CreatedUserID;
                item.Deadline = finder.Deadline ?? DateTime.Now;
                item.DepartmentID = finder.DepartmentID;
                item.JobID = finder.JobID;
                item.JobName = finder.JobName;
                item.LastModifyDate = finder.LastModifyDate;
                item.LastModifyUserID = finder.LastModifyUserID;
                item.Note = finder.Note;
                item.Poster = finder.Poster;
                item.Priority = finder.Priority;
                item.Rate = finder.Rate;
                item.Recipient = finder.Recipient;
                item.Status = finder.Status;
                item.StatusConfirm = finder.StatusConfirm;
                item.RateComment = finder.RateComment;
            }

            if (!isUpdate)
            {
                GetNextJobID_Result id = _EntityModel.GetNextJobID(User.Identity.Name).FirstOrDefault()
                    ?? new GetNextJobID_Result();
                item.JobID = string.Format("{0}-{1}-{2:0000}{3:00}{4:000}", 
                    id.DepartmentID, id.EmployeeID, id.Year, id.Month, id.Next);
            }

            return PartialView(item);
        }

        [Authorize]
        public ActionResult JobView(string id)
        {
            ViewData["MenuSelected"] = "Index";

            JobModels model = new JobModels() { APK = new Guid(id) };
            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == model.APK);

            if (finder != null)
            {
                model.Complex = finder.Complex;
                model.Confirmer = finder.Confirmer;
                model.CreatedDate = finder.CreatedDate;
                model.CreatedUserID = finder.CreatedUserID;
                model.Deadline = finder.Deadline ?? DateTime.Now;
                model.DepartmentID = finder.DepartmentID;
                model.JobID = finder.JobID;
                model.JobName = finder.JobName;
                model.LastModifyDate = finder.LastModifyDate;
                model.LastModifyUserID = finder.LastModifyUserID;
                model.Note = finder.Note;
                model.Poster = finder.Poster;
                model.Priority = finder.Priority;
                model.Rate = finder.Rate;
                model.Recipient = finder.Recipient;
                model.Status = finder.Status;
                model.StatusConfirm = finder.StatusConfirm;
                model.RateComment = finder.RateComment;
                model.ReAPK = finder.ReAPK;
                model.Sender = finder.Sender;
                model.SentMessage = finder.SentMessage;
                model.ReJobID = finder.ReJobID;
                model.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                model.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                model.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                model.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                model.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                model.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Poster) ?? new Employee()).FullName;
                model.SenderName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Sender) ?? new Employee()).FullName;
                model.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Recipient) ?? new Employee()).FullName;
                model.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Confirmer) ?? new Employee()).FullName;
                model.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == finder.DepartmentID) ?? new Department()).DepartmentName;

                // Update feeds
                var feeds = _EntityModel.Feeds
                    .Where(x => x.JobAPK == finder.APK
                        && x.Reader == _EmployeeID).ToList();
                if (feeds != null)
                {
                    foreach (var f in feeds)
                    {
                        f.Read = true;
                    }
                    _EntityModel.SaveChanges();
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult JobViewPartial(string id)
        {
            JobModels model = new JobModels() { APK = new Guid(id) };

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == model.APK);
            if (finder != null)
            {
                model.Complex = finder.Complex;
                model.Confirmer = finder.Confirmer;
                model.CreatedDate = finder.CreatedDate;
                model.CreatedUserID = finder.CreatedUserID;
                model.Deadline = finder.Deadline ?? DateTime.Now;
                model.DepartmentID = finder.DepartmentID;
                model.JobID = finder.JobID;
                model.JobName = finder.JobName;
                model.LastModifyDate = finder.LastModifyDate;
                model.LastModifyUserID = finder.LastModifyUserID;
                model.Note = finder.Note;
                model.Poster = finder.Poster;
                model.Priority = finder.Priority;
                model.Rate = finder.Rate;
                model.Recipient = finder.Recipient;
                model.Status = finder.Status;
                model.StatusConfirm = finder.StatusConfirm;
                model.RateComment = finder.RateComment;
                model.ReAPK = finder.ReAPK;
                model.SentMessage = finder.SentMessage;
                model.ReJobID = finder.ReJobID;
                model.Sender = finder.Sender;
                model.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                model.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                model.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                model.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                model.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                    s.CodeID == finder.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                model.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Poster) ?? new Employee()).FullName;
                model.SenderName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Sender) ?? new Employee()).FullName;
                model.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Recipient) ?? new Employee()).FullName;
                model.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Confirmer) ?? new Employee()).FullName;
                model.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == finder.DepartmentID) ?? new Department()).DepartmentName;
            }

            return PartialView(model);
        }

        [Authorize]
        public ActionResult SentDialog(JobModels model)
        {
            JobModels item = model ?? new JobModels();

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                item.ReAPK = finder.APK;
                item.ReJobID = finder.JobID;
                item.Sender = _EmployeeID;
            }

            return PartialView(model);
        }

        public ActionResult SentJob(JobModels model)
        {
            JobModels item = model ?? new JobModels();

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                string empUserName = (_EntityModel.Employees.FirstOrDefault(x => x.EmployeeID == item.Recipient)
                    ?? new Employee()).UserName;

                GetNextJobID_Result id = _EntityModel.GetNextJobID(empUserName).FirstOrDefault()
                                                                                        ?? new GetNextJobID_Result();
                finder.JobID = string.Format("{0}-{1}-{2:0000}{3:00}{4:000}",
                                    id.DepartmentID, id.EmployeeID, id.Year, id.Month, id.Next);

                // Do Insert
                Job job = new Job
                {
                    APK = Guid.NewGuid(),
                    Complex = finder.Complex,
                    Confirmer = item.Confirmer,
                    Deadline = finder.Deadline,
                    DepartmentID = item.DepartmentID,
                    JobID = finder.JobID,
                    JobName = finder.JobName,
                    Note = finder.Note,
                    Poster = item.Sender,
                    Priority = finder.Priority,
                    Rate = finder.Rate,
                    Sender = item.Sender,
                    ReAPK = finder.APK,
                    ReJobID = item.ReJobID,
                    Recipient = item.Recipient,
                    Status = finder.Status,
                    StatusConfirm = "0",
                    RateComment = finder.RateComment,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = User.Identity.Name,
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = User.Identity.Name,
                };

                _EntityModel.Jobs.AddObject(job);
                _EntityModel.SaveChanges();

                item.APK = finder.APK;
            }

            return Json(new { result = true, id = item.APK });
        }

        #endregion ---- Jobs ----

        #region ---- Attachments ----

        public JsonResult GridAttachments(string apk)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            List<Attachment> model = _EntityModel.Attachments
                                                .Where(x => x.JobAPK == id).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
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
                    AttachmentOwner = _EmployeeID,
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
                    x.AttachmentOwner == _EmployeeID).FirstOrDefault();

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

                var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + _EmployeeID), finder.AttachmentFileName);

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

        #endregion ---- Attachments ----

        #region ---- Notes ----

        public JsonResult GridNotes(string apk)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            List<Note> lst = _EntityModel.Notes.Where(x => x.JobAPK == id).ToList();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEditNote(NoteModels model)
        {
            var item = model ?? new NoteModels();

            bool allowEdit = false;
            string message = string.Empty;

            var finder = _EntityModel.Notes.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                if (_EmployeeID.Equals(finder.CreatedUserID))
                {
                    // Xóa hồ sơ
                    allowEdit = true;
                }
                else
                {
                    message = string.Format("Bạn không có quyền sửa/xóa ghi chú này.");
                }
            }

            return Json(new { result = allowEdit, message = message });
        }

        [Authorize]
        public ActionResult NoteDialog(NoteModels model)
        {
            NoteModels item = model ?? new NoteModels();

            var finder = _EntityModel.Notes.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                item.Title = finder.Title;
                item.Description = finder.Description;
                item.CreatedDate = finder.CreatedDate;
                item.CreatedUserID = finder.CreatedUserID;
                item.LastModifyDate = finder.LastModifyDate;
                item.LastModifyUserID = finder.LastModifyUserID;

                // Update feeds
                var feeds = _EntityModel.Feeds
                    .Where(x => x.NoteAPK == item.APK 
                        && x.Reader == _EmployeeID).ToList();
                if (feeds != null)
                {
                    foreach (var f in feeds)
                    {
                        f.Read = true;
                    }
                    _EntityModel.SaveChanges();
                }
            }

            return PartialView(item);
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult RemoveNote(NoteModels model)
        {
            var item = model ?? new NoteModels();

            var finder = _EntityModel.Notes.Where(x =>
                    x.APK == item.APK).FirstOrDefault();

            if (finder != null)
            {
                _EntityModel.Notes.DeleteObject(finder);
                _EntityModel.SaveChanges();
            }

            return Json(new { result = true });
        }

        /// <summary>
        /// Save all attachments
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public ActionResult UpdateNote(NoteModels model)
        {
            var item = model ?? new NoteModels();

            var finder = _EntityModel.Notes.Where(x =>
                    x.APK == item.APK).FirstOrDefault();
            bool isSaved = false;
            var apk = item.APK;

            if (finder != null)
            {
                finder.Title = item.Title;
                finder.Description = item.Description;
                finder.LastModifyUserID = User.Identity.Name;
                finder.LastModifyDate = DateTime.Now;
                _EntityModel.SaveChanges();
                isSaved = true;
            }
            else
            {
                Note note = new Note
                {
                    APK = Guid.NewGuid(),
                    JobAPK = item.JobAPK ?? Guid.Empty,
                    Title = item.Title,
                    Description = item.Description,
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = User.Identity.Name,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = User.Identity.Name
                };

                _EntityModel.Notes.AddObject(note);
                _EntityModel.SaveChanges();
                isSaved = true;
                apk = note.APK;
            }

            if (isSaved)
            {
                var job = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.JobAPK)
                    ?? new Job();

                List<string> reader = new List<string>(){
                    job.Recipient,
                    job.Sender,
                    job.Confirmer,
                    job.Poster
                }.Distinct().ToList();

                foreach (var r in reader)
                {
                    // Không notify cho người tạo
                    if (_EmployeeID.Equals(r)) continue;

                    _EntityModel.Feeds.AddObject(new Feed{ 
                        APK = Guid.NewGuid(),
                        JobAPK = job.APK,
                        JobID = job.JobID,
                        NoteAPK = apk,
                        Read = false,
                        Reader = r
                    });
                    _EntityModel.SaveChanges();
                }
            }

            return Json(new { result = true });
        }

        [Authorize]
        public ActionResult FeedDialog()
        {
            return PartialView();
        }

        [Authorize]
        public ActionResult MakeAsReadFeed(Feed model)
        {
            Feed feed = model ?? new Feed();
            var finder = _EntityModel.Feeds.FirstOrDefault(x => x.APK == feed.APK);
            if (finder != null)
            {
                finder.Read = true;
                _EntityModel.SaveChanges();
            }
            return Json(new { result = true });
        }

        public JsonResult GridFeeds(JobModels model)
        {
            var item = model ?? new JobModels();

            var feeds = _EntityModel.Feeds.Where(x => x.Reader == _EmployeeID && !x.Read);
            List<FeedModels> feedNotes = new List<FeedModels>();
            if (feeds != null)
            {
                foreach (var f in feeds)
                {
                    feedNotes.Add(new FeedModels()
                    {
                        APK = f.APK,
                        JobAPK = f.JobAPK,
                        JobID = f.JobID,
                        NoteAPK = f.NoteAPK,
                        Title = (_EntityModel.Notes.FirstOrDefault(x => x.APK == f.NoteAPK) ?? new Note()).Title,
                        Description = (_EntityModel.Notes.FirstOrDefault(x => x.APK == f.NoteAPK) ?? new Note()).Description,
                        Status = f.Read ? 1 : 0,
                        StatusName = f.Read ? "Đã đọc" : "Chưa đọc",
                    });
                }
            }

            return Json(feedNotes, JsonRequestBehavior.AllowGet);
        }

        #endregion ---- Notes ----

        #region ---- Histories ----

        public JsonResult GridHistories(JobModels model)
        {
            JobModels item = model ?? new JobModels();
            List<History> lst = _EntityModel.Histories.Where(x => x.JobAPK == item.APK).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #endregion ---- Histories ----

        #region ---- Others ----
        
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

        #endregion ---- Others ----

    }
}
