using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class JobModels
    {
        #region ---- Filter ----

        /// <summary>
        /// Mã hồ sơ
        /// </summary>
        [Display(Name="Mã hồ sơ")]
        public string JobIDFilter { get; set; }

        /// <summary>
        /// Tên hồ sơ
        /// </summary>
        [Display(Name = "Tên hồ sơ")]
        public string JobNameFilter { get; set; }

        /// <summary>
        /// Tình trạng hồ sơ
        /// </summary>
        [Display(Name = "Tình trạng hồ sơ")]
        public int StatusFilter { get; set; }

        /// <summary>
        /// Người thực hiện
        /// </summary>
        [Display(Name = "Người thực hiện")]
        public string RecipientFilter { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        [Display(Name = "Người duyệt")]
        public string ConfirmerFilter { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [Display(Name = "Ngày hết hạn")]
        public DateTime? DeadlineFilter { get; set; }

        /// <summary>
        /// Người lập
        /// </summary>
        [Display(Name = "Người lập")]
        public string PosterFilter { get; set; }

        /// <summary>
        /// Độ ưu tiên
        /// </summary>
        [Display(Name = "Độ ưu tiên")]
        public string PriorityFilter { get; set; }

        /// <summary>
        /// Độ phức tạp
        /// </summary>
        [Display(Name = "Độ phức tạp")]
        public string ComplexFilter { get; set; }

        /// <summary>
        /// Độ phức tạp
        /// </summary>
        [Display(Name = "Đánh giá")]
        public string RateFilter { get; set; }

        /// <summary>
        /// Hồ sơ quá hạn
        /// </summary>
        [Display(Name="Hồ sơ quá hạn")]
        public bool IsOverDateFiler { get; set; }

        [Display(Name = "Phòng ban")]
        public string DepartmentIDFilter { get; set; }

        #endregion ---- Filter ----

        public Guid? APK { get; set; }

        [Display(Name = "Mã hồ sơ")]
        public string JobID { get; set; }

        [Display(Name = "Tên hồ sơ")]
        public string JobName { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        [Display(Name = "Tình trạng")]
        public int Status { get; set; }

        [Display(Name = "Tình trạng")]
        public string StatusName
        {
            get
            {
                string name = string.Empty;
                switch (this.Status)
                {
                    case 1:
                        name = "Hồ sơ mới";
                        break;
                    case 2:
                        name = "Đang xử lý";
                        break;
                    case 3:
                        name = "Đã hoàn tất";
                        break;
                    default:
                        name = "Đã hủy";
                        break;
                }

                return name;
            }
        }

        [Display(Name = "Người lập")]
        public string Poster { get; set; }

        [Display(Name = "Phòng ban")]
        public string DepartmentID { get; set; }

        [Display(Name = "Độ ưu tiên")]
        public int Priority { get; set; }
        
        [Display(Name = "Độ phức tạp")]
        public int Complex { get; set; }

        [Display(Name = "Đánh giá")]
        public int Rate { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Người tạo")]
        public string CreatedUserID { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime LastModifyDate { get; set; }

        [Display(Name = "Người cập nhật")]
        public string LastModifyUserID { get; set; }

        [Display(Name = "Người thực hiện")]
        public string Recipient { get; set; }

        [Display(Name = "Người duyệt")]
        public string Confirmer { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
    }
}