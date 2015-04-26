using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    public class ComboBoxController : Controller
    {
        /// <summary>
        /// JMS entities
        /// </summary>
        JMSEntities _EntityModel = new JMSEntities();

        public JsonResult EmployeeID()
        {
            List<Employee> model = _EntityModel.Employees.ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "< Không >", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecipientID(string DepartmentID)
        {
            List<Employee> model = _EntityModel.Employees.Where(x => x.DepartmentID == DepartmentID).ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "< Không >", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConfirmerID()
        {
            List<Employee> model = _EntityModel.Employees.Where(x => x.GroupID != "0").ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "< Không >", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManagerID(string DepartmentID)
        {
            List<Employee> model = _EntityModel.Employees.Where(x => x.GroupID != "0" && x.DepartmentID == DepartmentID).ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "< Không >", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.STATUS_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.STATUS_CODE, CodeName = "< Không >"});

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusConfirmID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.STATUS_CONFIRM_CODE).ToList();
            //model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.STATUS_CODE, CodeName = "< Không >" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComplexID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.COMPLEX_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.COMPLEX_CODE, CodeName = "< Không >" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PriorityID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.PRIORITY_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.PRIORITY_CODE, CodeName = "< Không >" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RateID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.RATE_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.RATE_CODE, CodeName = "< Không >" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentID()
        {
            var employee = _EntityModel.Employees.FirstOrDefault(x
                => x.UserName == User.Identity.Name && x.GroupID == "0");

            List<Department> model = new List<Department>();
            if (employee != null)
            {
                model = _EntityModel.Departments
                    .Where(x => x.DepartmentID == employee.DepartmentID).ToList();
            }
            else
            {
                model = _EntityModel.Departments.ToList();
            }

            model.Insert(0, new Department() { DepartmentID = string.Empty, DepartmentName = "< Không >" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
