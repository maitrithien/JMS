using DMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS.Areas.Admin.Models
{
    public class UserModels : BaseModels
    {
        [DisplayName("Tên người dùng")]
        public string UserNameFilter { get; set; }

        [DisplayName("Tên nhóm người dùng")]
        public string RoleNameFilter { get; set; }

        [DisplayName("Mã người dùng")]
        public int UserID { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }
    }
}