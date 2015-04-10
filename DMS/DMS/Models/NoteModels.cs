using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class NoteModels
    {
        public Guid? APK { get; set; }

        [Display(Name = "Ghi chú")]
        public string Description { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Người tạo")]
        public string CreatedUserID { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime LastModifyDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string LastModifyUserID { get; set; }

    }
}