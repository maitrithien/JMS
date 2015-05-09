using DMS.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DMS.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(UserProfile user)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                // TODO: Add delete logic here
                if (Roles.GetRolesForUser(user.UserName).Count() > 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }

                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.UserName); // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(user.UserName, true); // deletes record from UserProfile table

                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { result = result, message = message });
        }
    }
}
