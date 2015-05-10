using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class CompletedFeedModels : BaseModels
    {
        public Guid? APK { get; set; }

        public Guid? JobAPK { get; set; }

        [Display(Name = "Trạng thái")]
        public string Completed { get; set; }

        [Display(Name = "Trạng thái")]
        public string CompletedName { get; set; }

        [Display(Name = "Mã JOBS")]
        public string JobID { get; set; }

        [Display(Name = "Tình trạng JOBS")]
        public string JobStatusName { get; set; }

        [Display(Name = "Tình trạng")]
        public int Status { get; set; }

        [Display(Name = "Tình trạng")]
        public string StatusName { get; set; }


    }
}