using DMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS.Areas.Admin.Models
{
    public class EmployeeModels : BaseModels
    {
        [DisplayName("Mã nhân viên")]
        public string EmployeeIDFilter { get; set; }

        [DisplayName("Tên nhân viên")]
        public string FullNameFilter { get; set; }

        [DisplayName("Chức vụ")]
        public string GroupIDFilter { get; set; }

        [DisplayName("Phòng ban")]
        public string DepartmentIDFilter { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserNameFilter { get; set; }

        [DisplayName("Tên nhóm người dùng")]
        public string RoleNameFilter { get; set; }

        public Guid? APK { get; set; }

        [DisplayName("Mã nhân viên")]
        public string EmployeeID { get; set; }

        [DisplayName("Tên nhân viên")]
        public string FullName { get; set; }

        [DisplayName("Chức vụ")]
        public string GroupID { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [DisplayName("Nhóm người dùng")]
        public string RoleID { get; set; }

        [DisplayName("Nhóm người dùng")]
        public string RoleName { get; set; }

        [DisplayName("Mã phòng ban")]
        public string DepartmentID { get; set; }

        [DisplayName("Tên phòng ban")]
        public string DepartmentName { get; set; }

        [DisplayName("Người quản lý")]
        public string ManagerID { get; set; }

        [DisplayName("Người quản lý")]
        public string ManagerName { get; set; }
    }
}