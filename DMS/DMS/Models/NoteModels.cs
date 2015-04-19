using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class NoteModels : BaseModels
    {
        public Guid? APK { get; set; }

        public Guid? JobAPK { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        public string Description { get; set; }

    }
}