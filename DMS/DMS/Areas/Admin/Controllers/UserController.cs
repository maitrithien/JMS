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

        [Authorize(Roles = "Administrator")]
        public JsonResult Grid()
        {
            List<UserModels> models = new List<UserModels>();
            var users = Membership.GetAllUsers();

            foreach (MembershipUser item in users)
            {
                models.Add(new UserModels {
                    UserName = item.UserName,
                    RoleName = string.Join(", ", Roles.GetRolesForUser(item.UserName)),
                    CreationDate = item.CreationDate,
                    IsApproved = item.IsApproved,
                    IsLockedOut = item.IsLockedOut,
                    IsOnline = item.IsOnline
                });
            }

            return Json(users, JsonRequestBehavior.AllowGet);
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
