using DMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS.Models
{
    public class UserModels : BaseModels
    {
        [DisplayName("Tên người dùng")]
        public string UserNameFilter { get; set; }

        [DisplayName("Tên nhóm người dùng")]
        public string RoleNameFilter { get; set; }
        
        [DisplayName("Nhóm người dùng")]
        public string RoleID { get; set; }

        [DisplayName("Tên nhóm người dùng")]
        public string RoleName { get; set; }

        [DisplayName("Ghi chú")]
        public string Comment { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Duyệt")]
        public bool IsApproved { get; set; }

        [DisplayName("Khóa")]
        public bool IsLockedOut { get; set; }

        [DisplayName("Online")]
        public bool IsOnline { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }
        
    }
}