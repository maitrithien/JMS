using DMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS.Areas.Admin.Models
{
    public class RoleModels : BaseModels
    {
        [DisplayName("Tên nhóm người dùng")]
        public string RoleNameFilter { get; set; }

        [DisplayName("Mã nhóm người dùng")]
        public int RoleID { get; set; }

        [DisplayName("Tên nhóm người dùng")]
        public string RoleName { get; set; }
    }
}