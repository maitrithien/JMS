using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class HistoryModels : BaseModels
    {
        public Guid? APK { get; set; }

        [Display(Name = "Hành động")]
        [DisplayName("Hành động")]
        public byte ActionType { get; set; }

        [Display(Name = "Hành động")]
        [DisplayName("Hành động")]
        public string ActionName {
            get
            {
                string name = string.Empty;
                switch (ActionType)
                {
                    case 0:
                        name = "Thay đổi trạng thái";
                        break;
                    case 1:
                        name = "Thay đổi nội dung";
                        break;
                    case 2:
                        name = "Thay đổi người thực hiện";
                        break;
                    default:
                        name = "Thay đổi khác";
                        break;
                }

                return name;
            }
        }

        [Display(Name = "Nội dung trước")]
        [DisplayName("Nội dung trước")]
        public string OldData { get; set; }

        [Display(Name = "Nội dung thay đổi")]
        [DisplayName("Nội dung thay đổi")]
        public string NewData { get; set; }
    }
}