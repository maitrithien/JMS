using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class FeedModels : BaseModels
    {
        public Guid? APK { get; set; }

        public Guid? JobAPK { get; set; }

        public Guid? NoteAPK { get; set; }
        
        public string JobID { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        public string Description { get; set; }

        [Display(Name = "Tình trạng")]
        public int Status { get; set; }

        [Display(Name = "Tình trạng")]
        public string StatusName { get; set; }

    }
}