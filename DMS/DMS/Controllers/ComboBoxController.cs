using DMS.Bussiness;
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
        //
        // GET: /ComboBox/

        public JsonResult UserID()
        {
            ComboBoxBussiness _comboBuzz = new ComboBoxBussiness();
            List<UserProfile> model = _comboBuzz.GetAll();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
