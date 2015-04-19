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

        public JsonResult JobsGridReceived(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            if (model.IsFilter == 1)
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.JobID.Contains(string.IsNullOrEmpty(item.JobIDFilter) ? x.JobID : item.JobIDFilter)
                       && x.JobName.Contains(string.IsNullOrEmpty(item.JobNameFilter) ? x.JobName : item.JobNameFilter)
                       && x.Status == (string.IsNullOrEmpty(item.StatusFilter) ? x.Status : item.StatusFilter)
                       && x.Poster.Contains(string.IsNullOrEmpty(item.PosterFilter) ? x.Poster : item.PosterFilter)
                       && x.Recipient.Contains(string.IsNullOrEmpty(item.RecipientFilter) ? x.Recipient : item.RecipientFilter)
                       && x.Confirmer.Contains(string.IsNullOrEmpty(item.ConfirmerFilter) ? x.Confirmer : item.ConfirmerFilter)
                       && x.Deadline == (item.DeadlineFilter == null ? x.Deadline : item.DeadlineFilter)
                       && x.Priority == (string.IsNullOrEmpty(item.PriorityFilter) ? x.Priority : item.PriorityFilter)
                       && (x.Rate == (string.IsNullOrEmpty(item.RateFilter) ? x.Rate : item.RateFilter) || x.Rate == null)
                       && x.Complex == (string.IsNullOrEmpty(item.ComplexFilter) ? x.Complex : item.ComplexFilter)
                       && x.DepartmentID.Contains(string.IsNullOrEmpty(item.DepartmentIDFilter) ? x.DepartmentID : item.DepartmentIDFilter)
                   ) && (
                       x.Confirmer == User.Identity.Name
                       || (
                            x.Recipient == User.Identity.Name
                            && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                            && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                          )
                   )).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }
            else
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.Confirmer == User.Identity.Name
                       || (
                                x.Recipient == User.Identity.Name
                                && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                                && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                          )
                   )).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }

            if (lst.Count > 0)
            {
                foreach (JobModels job in lst)
                {
                    job.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                                s.CodeID == job.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                    job.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                    job.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                    job.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                    job.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                    job.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Poster) ?? new Employee()).FullName;
                    job.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Recipient) ?? new Employee()).FullName;
                    job.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Confirmer) ?? new Employee()).FullName;
                    job.CreatedUserName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.CreatedUserID) ?? new Employee()).FullName;
                    job.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == job.DepartmentID) ?? new Department()).DepartmentName;
                }
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JobsGridSent(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            if (model.IsFilter == 1)
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.JobID.Contains(string.IsNullOrEmpty(item.JobIDFilter) ? x.JobID : item.JobIDFilter)
                       && x.JobName.Contains(string.IsNullOrEmpty(item.JobNameFilter) ? x.JobName : item.JobNameFilter)
                       && x.Status == (string.IsNullOrEmpty(item.StatusFilter) ? x.Status : item.StatusFilter)
                       && x.Poster.Contains(string.IsNullOrEmpty(item.PosterFilter) ? x.Poster : item.PosterFilter)
                       && x.Recipient.Contains(string.IsNullOrEmpty(item.RecipientFilter) ? x.Recipient : item.RecipientFilter)
                       && x.Confirmer.Contains(string.IsNullOrEmpty(item.ConfirmerFilter) ? x.Confirmer : item.ConfirmerFilter)
                       && x.Deadline == (item.DeadlineFilter == null ? x.Deadline : item.DeadlineFilter)
                       && x.Priority == (string.IsNullOrEmpty(item.PriorityFilter) ? x.Priority : item.PriorityFilter)
                       && (x.Rate == (string.IsNullOrEmpty(item.RateFilter) ? x.Rate : item.RateFilter) || x.Rate == null)
                       && x.Complex == (string.IsNullOrEmpty(item.ComplexFilter) ? x.Complex : item.ComplexFilter)
                       && x.DepartmentID.Contains(string.IsNullOrEmpty(item.DepartmentIDFilter) ? x.DepartmentID : item.DepartmentIDFilter)
                   ) && (
                       x.CreatedUserID == User.Identity.Name
                       || x.Poster == User.Identity.Name
                   )).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }
            else
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.CreatedUserID == User.Identity.Name
                       || x.Poster == User.Identity.Name
                   )).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }

            if (lst.Count > 0)
            {
                foreach (JobModels job in lst)
                {
                    job.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                                s.CodeID == job.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                    job.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                    job.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                    job.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                    job.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                    job.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Poster) ?? new Employee()).FullName;
                    job.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Recipient) ?? new Employee()).FullName;
                    job.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Confirmer) ?? new Employee()).FullName;
                    job.CreatedUserName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.CreatedUserID) ?? new Employee()).FullName;
                    job.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == job.DepartmentID) ?? new Department()).DepartmentName;
                }
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JobsGridOverdue(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            if (model.IsFilter == 1)
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.JobID.Contains(string.IsNullOrEmpty(item.JobIDFilter) ? x.JobID : item.JobIDFilter)
                       && x.JobName.Contains(string.IsNullOrEmpty(item.JobNameFilter) ? x.JobName : item.JobNameFilter)
                       && x.Status == (string.IsNullOrEmpty(item.StatusFilter) ? x.Status : item.StatusFilter)
                       && x.Poster.Contains(string.IsNullOrEmpty(item.PosterFilter) ? x.Poster : item.PosterFilter)
                       && x.Recipient.Contains(string.IsNullOrEmpty(item.RecipientFilter) ? x.Recipient : item.RecipientFilter)
                       && x.Confirmer.Contains(string.IsNullOrEmpty(item.ConfirmerFilter) ? x.Confirmer : item.ConfirmerFilter)
                       && x.Deadline == (item.DeadlineFilter == null ? x.Deadline : item.DeadlineFilter)
                       && x.Priority == (string.IsNullOrEmpty(item.PriorityFilter) ? x.Priority : item.PriorityFilter)
                       && (x.Rate == (string.IsNullOrEmpty(item.RateFilter) ? x.Rate : item.RateFilter) || x.Rate == null)
                       && x.Complex == (string.IsNullOrEmpty(item.ComplexFilter) ? x.Complex : item.ComplexFilter)
                       && x.DepartmentID.Contains(string.IsNullOrEmpty(item.DepartmentIDFilter) ? x.DepartmentID : item.DepartmentIDFilter)
                       //&& x.Status != "2" && x.Status != "9"
                   ) && (
                       x.CreatedUserID == User.Identity.Name
                       || x.Poster == User.Identity.Name
                       || (x.Confirmer == User.Identity.Name && x.StatusConfirm != "1")
                       || (
                            x.Recipient == User.Identity.Name
                            && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                            && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                          )
                        )
                   ).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }
            else
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       //x.Status != "2" && x.Status != "9" &&
                       (x.CreatedUserID == User.Identity.Name
                        || x.Poster == User.Identity.Name
                        || (x.Confirmer == User.Identity.Name && x.StatusConfirm != "1")
                        || (
                                x.Recipient == User.Identity.Name
                                && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                                && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                            )
                        )
                   )
                   ).Select(x => new JobModels()
                   {
                       APK = x.APK,
                       JobID = x.JobID,
                       JobName = x.JobName,
                       Poster = x.Poster,
                       Recipient = x.Recipient,
                       Confirmer = x.Confirmer,
                       Deadline = x.Deadline,
                       Status = x.Status,
                       Rate = x.Rate,
                       Complex = x.Complex,
                       Priority = x.Priority,
                       DepartmentID = x.DepartmentID,
                       CreatedUserID = x.CreatedUserID,
                       LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }

            List<JobModels> overList = new List<JobModels>();

            if (lst.Count > 0)
            {
                foreach (JobModels job in lst)
                {
                    if (Math.Round((DateTime.Now.Date - (job.Deadline ?? DateTime.Now).Date).TotalDays, 0) > 0)
                    {
                        job.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                                    s.CodeID == job.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                        job.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                            s.CodeID == job.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                        job.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                            s.CodeID == job.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                        job.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                            s.CodeID == job.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                        job.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                            s.CodeID == job.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                        job.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Poster) ?? new Employee()).FullName;
                        job.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Recipient) ?? new Employee()).FullName;
                        job.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Confirmer) ?? new Employee()).FullName;
                        job.CreatedUserName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.CreatedUserID) ?? new Employee()).FullName;
                        job.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == job.DepartmentID) ?? new Department()).DepartmentName;
                        job.OverDeadlineNumber = Math.Round((DateTime.Now.Date - (job.Deadline ?? DateTime.Now).Date).TotalDays, 0);

                        overList.Add(job);
                    }
                }
            }

            return Json(overList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JobsGrid(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            if (model.IsFilter == 1)
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.JobID.Contains(string.IsNullOrEmpty(item.JobIDFilter) ? x.JobID : item.JobIDFilter)
                       && x.JobName.Contains(string.IsNullOrEmpty(item.JobNameFilter) ? x.JobName : item.JobNameFilter)
                       && x.Status == (string.IsNullOrEmpty(item.StatusFilter) ? x.Status : item.StatusFilter)
                       && x.Poster.Contains(string.IsNullOrEmpty(item.PosterFilter) ? x.Poster : item.PosterFilter)
                       && x.Recipient.Contains(string.IsNullOrEmpty(item.RecipientFilter) ? x.Recipient : item.RecipientFilter)
                       && x.Confirmer.Contains(string.IsNullOrEmpty(item.ConfirmerFilter) ? x.Confirmer : item.ConfirmerFilter)
                       && x.Deadline == (item.DeadlineFilter == null ? x.Deadline : item.DeadlineFilter)
                       && x.Priority == (string.IsNullOrEmpty(item.PriorityFilter) ? x.Priority : item.PriorityFilter)
                       && (x.Rate == (string.IsNullOrEmpty(item.RateFilter) ? x.Rate : item.RateFilter) || x.Rate == null)
                       && x.Complex == (string.IsNullOrEmpty(item.ComplexFilter) ? x.Complex : item.ComplexFilter)
                       && x.DepartmentID.Contains(string.IsNullOrEmpty(item.DepartmentIDFilter) ? x.DepartmentID : item.DepartmentIDFilter)
                   ) && (
                       x.CreatedUserID == User.Identity.Name
                       || x.Poster == User.Identity.Name
                       || x.Confirmer == User.Identity.Name
                       || (
                            x.Recipient == User.Identity.Name 
                            && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                            && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                          )
                   )).Select(x => new JobModels() { 
                        APK = x.APK,
                        JobID = x.JobID,
                        JobName = x.JobName,
                        Poster = x.Poster,
                        Recipient = x.Recipient,
                        Confirmer = x.Confirmer,
                        Deadline = x.Deadline,
                        Status = x.Status,
                        Rate = x.Rate,
                        Complex = x.Complex,
                        Priority = x.Priority,
                        DepartmentID = x.DepartmentID,
                        CreatedUserID = x.CreatedUserID,
                        LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }
            else
            {
                lst = _EntityModel.Jobs.Where(x =>
                   (
                       x.CreatedUserID == User.Identity.Name
                       || x.Poster == User.Identity.Name
                       || x.Confirmer == User.Identity.Name
                       || (
                                x.Recipient == User.Identity.Name 
                                && x.StatusConfirm.Equals((string.IsNullOrEmpty(x.StatusConfirm) ? x.StatusConfirm : "1"))
                                && !x.Status.Equals((string.IsNullOrEmpty(x.Status) ? x.Status : "0"))
                          )
                   )).Select(x => new JobModels() { 
                        APK = x.APK,
                        JobID = x.JobID,
                        JobName = x.JobName,
                        Poster = x.Poster,
                        Recipient = x.Recipient,
                        Confirmer = x.Confirmer,
                        Deadline = x.Deadline,
                        Status = x.Status,
                        Rate = x.Rate,
                        Complex = x.Complex,
                        Priority = x.Priority,
                        DepartmentID = x.DepartmentID,
                        CreatedUserID = x.CreatedUserID,
                        LastModifyUserID = x.LastModifyUserID
                   }).ToList() ?? new List<JobModels>();
            }

            if (lst.Count > 0)
            {
                foreach (JobModels job in lst)
                {
                    job.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                                s.CodeID == job.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                    job.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                    job.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                    job.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                    job.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                    job.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Poster) ?? new Employee()).FullName;
                    job.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Recipient) ?? new Employee()).FullName;
                    job.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Confirmer) ?? new Employee()).FullName;
                    job.CreatedUserName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.CreatedUserID) ?? new Employee()).FullName;
                    job.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == job.DepartmentID) ?? new Department()).DepartmentName;
                }
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        private List<Employee> GetEmployeeByManager()
        {
            List<Employee> result = new List<Employee>();
            var emps = _EntityModel.GetEmployeeByManager(User.Identity.Name);
            if (emps != null)
            {
                foreach (var item in emps)
                {
                    result.Add(new Employee() { EmployeeID = item.EmployeeID, ManagerID = item.ManagerID });
                }
            }

            return result;
        }

        public JsonResult JobsGridEmployee(JobModels model)
        {
            var item = model ?? new JobModels();

            List<JobModels> lst = new List<JobModels>();

            List<Employee> emps = GetEmployeeByManager();
            
            if (model.IsFilter == 1)
            {
                foreach (Employee em in emps)
                {
                    #region ---- Lấy dữ liệu của từng nhân viên theo người quản lý trực tiếp ----
                    List<JobModels> emList = _EntityModel.Jobs.Where(x =>
                                                   (
                                                       x.JobID.Contains(string.IsNullOrEmpty(item.JobIDFilter) ? x.JobID : item.JobIDFilter)
                                                       && x.JobName.Contains(string.IsNullOrEmpty(item.JobNameFilter) ? x.JobName : item.JobNameFilter)
                                                       && x.Status == (string.IsNullOrEmpty(item.StatusFilter) ? x.Status : item.StatusFilter)
                                                       && x.Poster.Contains(string.IsNullOrEmpty(item.PosterFilter) ? x.Poster : item.PosterFilter)
                                                       && x.Recipient.Contains(string.IsNullOrEmpty(item.RecipientFilter) ? x.Recipient : item.RecipientFilter)
                                                       && x.Confirmer.Contains(string.IsNullOrEmpty(item.ConfirmerFilter) ? x.Confirmer : item.ConfirmerFilter)
                                                       && x.Deadline == (item.DeadlineFilter == null ? x.Deadline : item.DeadlineFilter)
                                                       && x.Priority == (string.IsNullOrEmpty(item.PriorityFilter) ? x.Priority : item.PriorityFilter)
                                                       && (x.Rate == (string.IsNullOrEmpty(item.RateFilter) ? x.Rate : item.RateFilter) || x.Rate == null)
                                                       && x.Complex == (string.IsNullOrEmpty(item.ComplexFilter) ? x.Complex : item.ComplexFilter)
                                                       && x.DepartmentID.Contains(string.IsNullOrEmpty(item.DepartmentIDFilter) ? x.DepartmentID : item.DepartmentIDFilter)
                                                   ) && ( x.Poster == em.EmployeeID || x.Confirmer == em.EmployeeID || x.Recipient == em.EmployeeID)
                                                   ).Select(x => new JobModels()
                                                                       {
                                                                           APK = x.APK,
                                                                           JobID = x.JobID,
                                                                           JobName = x.JobName,
                                                                           Poster = x.Poster,
                                                                           Recipient = x.Recipient,
                                                                           Confirmer = x.Confirmer,
                                                                           Deadline = x.Deadline,
                                                                           Status = x.Status,
                                                                           Rate = x.Rate,
                                                                           Complex = x.Complex,
                                                                           Priority = x.Priority,
                                                                           DepartmentID = x.DepartmentID,
                                                                           CreatedUserID = x.CreatedUserID,
                                                                           LastModifyUserID = x.LastModifyUserID
                                                                       }).ToList() ?? new List<JobModels>();

                    lst.AddRange(emList);

                    #endregion ---- Lấy dữ liệu của từng nhân viên theo người quản lý trực tiếp ----
                }

            }
            else
            {
                foreach (Employee em in emps)
                {
                    #region ---- Lấy dữ liệu của từng nhân viên theo người quản lý trực tiếp ----

                    List<JobModels> emList = _EntityModel.Jobs.Where(x => (x.Poster == em.EmployeeID || x.Confirmer == em.EmployeeID || x.Recipient == em.EmployeeID))
                                                                .Select(x => new JobModels()
                                                                        {
                                                                            APK = x.APK,
                                                                            JobID = x.JobID,
                                                                            JobName = x.JobName,
                                                                            Poster = x.Poster,
                                                                            Recipient = x.Recipient,
                                                                            Confirmer = x.Confirmer,
                                                                            Deadline = x.Deadline,
                                                                            Status = x.Status,
                                                                            Rate = x.Rate,
                                                                            Complex = x.Complex,
                                                                            Priority = x.Priority,
                                                                            DepartmentID = x.DepartmentID,
                                                                            CreatedUserID = x.CreatedUserID,
                                                                            LastModifyUserID = x.LastModifyUserID
                                                                        }).ToList() ?? new List<JobModels>();

                    lst.AddRange(emList);

                    #endregion ---- Lấy dữ liệu của từng nhân viên theo người quản lý trực tiếp ----
                }
                
            }

            if (lst.Count > 0)
            {
                foreach (JobModels job in lst)
                {
                    job.StatusName = (_EntityModel.Codes.FirstOrDefault(s =>
                                s.CodeID == job.Status && s.CodeGroupID == JobModels.STATUS_CODE) ?? new Code()).CodeName;
                    job.RateName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Rate && s.CodeGroupID == JobModels.RATE_CODE) ?? new Code()).CodeName;
                    job.PriorityName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Priority && s.CodeGroupID == JobModels.PRIORITY_CODE) ?? new Code()).CodeName;
                    job.StatusConfirmName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.StatusConfirm && s.CodeGroupID == JobModels.STATUS_CONFIRM_CODE) ?? new Code()).CodeName;
                    job.ComplexName = (_EntityModel.Codes.FirstOrDefault(s =>
                        s.CodeID == job.Complex && s.CodeGroupID == JobModels.COMPLEX_CODE) ?? new Code()).CodeName;
                    job.PosterName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Poster) ?? new Employee()).FullName;
                    job.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Recipient) ?? new Employee()).FullName;
                    job.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.Confirmer) ?? new Employee()).FullName;
                    job.CreatedUserName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == job.CreatedUserID) ?? new Employee()).FullName;
                    job.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == job.DepartmentID) ?? new Department()).DepartmentName;
                }
            }

            List<JobModels> distinct = lst.GroupBy(x => x.APK).Select(x => x.First()).ToList();

            return Json(distinct, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NotesGrid(string apk)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            List<Note> lst = _EntityModel.Notes.Where(x => x.JobAPK == id).ToList();
                
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AttachmentsGrid(string apk)
        {
            Guid id = Guid.Empty;
            if(!string.IsNullOrEmpty(apk))
            {
                Guid.TryParse(apk, out id);
            }

            List<Attachment> model = _EntityModel.Attachments
                                                .Where(x => x.JobAPK == id).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HistoriesGrid(JobModels model)
        {
            JobModels item = model ?? new JobModels();
            List<History> lst = _EntityModel.Histories.Where(x => x.JobAPK == item.APK).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
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
                    && (User.Identity.Name.Equals(finder.Poster) || User.Identity.Name.Equals(finder.CreatedUserID)))
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
                    if (!User.Identity.Name.Equals(finder.Recipient))
                    {
                        allowEdit = false;
                        message = string.Format("Hồ sơ {0} đã hoàn tất. Bạn không có quyền sửa hồ sơ này.", finder.JobID);
                    }
                }
            }

            return Json(new { result = allowEdit, message = message });
        }

        public ActionResult CheckEditNote(NoteModels model)
        {
            var item = model ?? new NoteModels();

            bool allowEdit = false;
            string message = string.Empty;

            var finder = _EntityModel.Notes.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
                if (User.Identity.Name.Equals(finder.CreatedUserID))
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
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/" + User.Identity.Name), att.AttachmentFileName);

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

            item.Poster = User.Identity.Name;
            item.Status = "0";
            item.Complex = "1";
            item.Priority = "1";
            item.Deadline = DateTime.Now.Date;

            var finder = _EntityModel.Jobs.FirstOrDefault(x => x.APK == item.APK);
            if (finder != null)
            {
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

            return PartialView(item);
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

            if (finder != null)
            {
                finder.Title = item.Title;
                finder.Description = item.Description;
                finder.LastModifyUserID = User.Identity.Name;
                finder.LastModifyDate = DateTime.Now;
                _EntityModel.SaveChanges();
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
            }

            return Json(new { result = true });
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
                model.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Recipient) ?? new Employee()).FullName;
                model.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Confirmer) ?? new Employee()).FullName;
                model.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == finder.DepartmentID) ?? new Department()).DepartmentName;
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
                model.RecipientName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Recipient) ?? new Employee()).FullName;
                model.ConfirmerName = (_EntityModel.Employees.FirstOrDefault(s => s.EmployeeID == finder.Confirmer) ?? new Employee()).FullName;
                model.DepartmentName = (_EntityModel.Departments.FirstOrDefault(s => s.DepartmentID == finder.DepartmentID) ?? new Department()).DepartmentName;
            }

            return PartialView(model);
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
