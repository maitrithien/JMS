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
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == "STATUS").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusConfirmID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == "CONFIRM_STATUS").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComplexID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == "COMPLEX").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PriorityID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == "PRIORITY").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RateID()
        {
            List<Code> model = _EntityModel.Codes.Where(x => x.CodeGroupID == "RATE").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentID()
        {
            List<Department> model = _EntityModel.Departments.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
