using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DMS.Models
{
    public class DepartmentModels : BaseModels
    {
        [Display(Name = "Mã phòng ban")]
        public string DepartmentID { get; set; }

        [Display(Name = "Tên phòng ban")]
        public string DepartmentName { get; set; }

        [Display(Name = "Người ủy nhiệm")]
        public string AssignedPerson { get; set; }
    }
}