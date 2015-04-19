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
        public int ActionType { get; set; }

        [Display(Name = "Hành động")]
        public string ActionName {
            get
            {
                string name = string.Empty;
                switch (ActionType)
                {
                    case 0:
                        name = "Tạo mới";
                        break;
                    case 1:
                        name = "Cập nhật";
                        break;
                    case 2:
                        name = "Xóa";
                        break;
                    case 9:
                        name = "Kế thừa";
                        break;
                }

                return name;
            }
        }

        [Display(Name = "Ghi chú")]
        public string Description { get; set; }

    }
}