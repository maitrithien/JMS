﻿using DMS.Models;
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
        private string _DepartmentID
        {
            get
            {
                return (_EntityModel.Employees.FirstOrDefault(
                    x => User.Identity.Name.Equals(x.UserName)) ?? new Employee()).DepartmentID;
            }
        }
        private string _GroupID
        {
            get
            {
                return (_EntityModel.Employees.FirstOrDefault(
                    x => User.Identity.Name.Equals(x.UserName)) ?? new Employee()).GroupID;
            }
        }

        public JsonResult EmployeeID()
        {
            List<Employee> model = _EntityModel.Employees.ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentEmployeeID()
        {
            List<Employee> model = _EntityModel.Employees.Where(x => x.DepartmentID == SystemEnvironments.DepartmentID && x.EmployeeID != SystemEnvironments.EmployeeID).ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecipientID(string DepartmentID)
        {
            List<Employee> model = new List<Employee>();

            if (DepartmentID != _DepartmentID && _GroupID == "0")
            {
                model = _EntityModel.Employees.Where(x => x.DepartmentID == DepartmentID && x.GroupID != "0").ToList();
            }
            else
            {
                model = _EntityModel.Employees.Where(x => x.DepartmentID == DepartmentID).ToList();
            }

            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConfirmerID()
        {
            List<Employee> model = _EntityModel.Employees.Where(x => x.GroupID != "0").ToList();
            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManagerID(string DepartmentID)
        {
            var departmentInfo = _EntityModel.Departments.FirstOrDefault(x => x.DepartmentID == DepartmentID) ?? new Department();
            
            // Chỉ load người trực tiếp hoặc đang được ủy nhiệm
            string currentPerson = string.IsNullOrEmpty(departmentInfo.AssignedPerson) ? departmentInfo.ManagerID : departmentInfo.AssignedPerson;

            List<Employee> model = _EntityModel.Employees.Where(x => x.EmployeeID == currentPerson).ToList();

            model.Insert(0, new Employee() { EmployeeID = string.Empty, FullName = "", UserName = string.Empty });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.STATUS_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.STATUS_CODE, CodeName = ""});

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusConfirmID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.STATUS_CONFIRM_CODE).ToList();
            //model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.STATUS_CODE, CodeName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComplexID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.COMPLEX_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.COMPLEX_CODE, CodeName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PriorityID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.PRIORITY_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.PRIORITY_CODE, CodeName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RateID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.RATE_CODE).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.RATE_CODE, CodeName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CompletedID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == JobModels.STATUS_COMPLETED).ToList();
            model.Insert(0, new Code() { CodeID = string.Empty, CodeGroupID = JobModels.STATUS_COMPLETED, CodeName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentID()
        {
            //var employee = _EntityModel.Employees.FirstOrDefault(x
            //    => x.UserName == User.Identity.Name && x.GroupID == "0");

            //List<Department> model = new List<Department>();
            //if (employee != null)
            //{
            //    model = _EntityModel.Departments
            //        .Where(x => x.DepartmentID == employee.DepartmentID).ToList();
            //}
            //else
            //{
            //    model = _EntityModel.Departments.ToList();
            //}

            List<Department> model = _EntityModel.Departments.ToList();
            model.Insert(0, new Department() { DepartmentID = string.Empty, DepartmentName = "" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
