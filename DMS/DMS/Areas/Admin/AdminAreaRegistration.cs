using System.Web.Mvc;

namespace DMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", area = "Admin", id = UrlParameter.Optional },
                new[] { "DMS.Areas.Admin.Controllers" }
            );
        }
    }
}
