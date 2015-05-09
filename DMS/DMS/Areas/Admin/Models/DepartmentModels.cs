using DMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS.Areas.Admin.Models
{
    public class DepartmentModels : BaseModels
    {

        [DisplayName("Mã phòng ban")]
        public int DepartmentIDFilter { get; set; }

        [DisplayName("Tên phòng ban")]
        public string DepartmentNameFilter { get; set; }

        [DisplayName("Người quản lý")]
        public string ManagerIDFilter { get; set; }

        public Guid? APK { get; set; }

        [DisplayName("Mã phòng ban")]
        public int DepartmentID { get; set; }

        [DisplayName("Tên phòng ban")]
        public string DepartmentName { get; set; }

        [DisplayName("Mã người quản lý")]
        public string ManagerID { get; set; }

        [DisplayName("Người quản lý")]
        public string ManagerName { get; set; }
    }
}