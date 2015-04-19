using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    [Flags]
    public enum FormAction
    {
        AddNew = 1,
        Edit = 2,
        View = 3,
        LimitEdit = 4,
        Inherit = 5
    }

    public class BaseModels
    {
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Người tạo")]
        public string CreatedUserID { get; set; }

        [Display(Name = "Người tạo")]
        public string CreatedUserName { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime LastModifyDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string LastModifyUserID { get; set; }

        [Display(Name = "Người cập nhật")]
        public string LastModifyUserName { get; set; }

        public FormAction Action { get; set; }
    }
}