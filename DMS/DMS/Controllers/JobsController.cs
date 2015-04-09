using DMS.Bussiness;
using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    public class JobsController : Controller
    {
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
        public ActionResult JobView(string id)
        {
            JobModels model = new JobModels();

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
