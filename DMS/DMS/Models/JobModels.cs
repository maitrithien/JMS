using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class JobModels : BaseModels
    {
        #region ---- Constants & Member variables ----

        public const string STATUS_CODE = "STATUS";
        public const string STATUS_CONFIRM_CODE = "CONFIRM_STATUS";
        public const string COMPLEX_CODE = "COMPLEX";
        public const string PRIORITY_CODE = "PRIORITY";
        public const string RATE_CODE = "RATE";
        public const string EMPLOYEE_LEVEL_CODE = "EMPLOYEE_LEVEL";

        public const int JOBS_P_TYPE = 0;
        public const int JOBS_R_TYPE = 1;
        public const int JOBS_S_TYPE = 2;
        public const int JOBS_O_TYPE = 3;
        public const int JOBS_E_TYPE = 4;

        #endregion ---- Constants & Member variables ----

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
        public string StatusFilter { get; set; }

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
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString="dd/MM/yyyy")]
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
        /// Đánh giá
        /// </summary>
        [Display(Name = "Đánh giá")]
        public string RateFilter { get; set; }

        /// <summary>
        /// Hồ sơ quá hạn
        /// </summary>
        [Display(Name="Hồ sơ quá hạn")]
        public bool IsOverDateFiler { get; set; }

        [Display(Name = "Phòng nhận hồ sơ")]
        public string DepartmentIDFilter { get; set; }

        #endregion ---- Filter ----

        /// <summary>
        /// Có sử dụng filter hay không
        /// </summary>
        public byte IsFilter { get; set; }

        public Guid? APK { get; set; }

        public Guid? ReAPK { get; set; }

        public string ReJobID { get; set; }

        [Display(Name = "Mã hồ sơ")]
        public string JobID { get; set; }

        [Display(Name = "Tên hồ sơ")]
        public string JobName { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { 
            get { return _deadline; }
            set { _deadline = value; }
        }

        private DateTime? _deadline = DateTime.Now;

        /// <summary>
        /// Số ngày quá hạn
        /// </summary>
        [Display(Name = "Số ngày quá hạn")]
        public double OverDeadlineNumber { get; set; }

        [Display(Name = "Tình trạng")]
        public string Status { get; set; }

        [Display(Name = "Tình trạng")]
        public string StatusName { get; set; }

        [Display(Name = "Duyệt hồ sơ")]
        public string StatusConfirm { get; set; }

        [Display(Name = "Duyệt hồ sơ")]
        public string StatusConfirmName { get; set; }

        [Display(Name = "Người lập")]
        public string Poster { get; set; }

        [Display(Name = "Người lập")]
        public string PosterName { get; set; }

        [Display(Name = "Phòng nhận hồ sơ")]
        public string DepartmentID { get; set; }

        [Display(Name = "Phòng nhận hồ sơ")]
        public string DepartmentName { get; set; }

        [Display(Name = "Độ ưu tiên")]
        public string Priority { get; set; }

        [Display(Name = "Độ ưu tiên")]
        public string PriorityName { get; set; }

        [Display(Name = "Độ phức tạp")]
        public string Complex { get; set; }

        [Display(Name = "Độ phức tạp")]
        public string ComplexName { get; set; }

        [Display(Name = "Đánh giá loại")]
        public string Rate { get; set; }

        [Display(Name = "Đánh giá loại")]
        public string RateName { get; set; }

        [Display(Name = "Người thực hiện")]
        public string Recipient { get; set; }

        [Display(Name = "Người thực hiện")]
        public string RecipientName { get; set; }

        [Display(Name = "Người duyệt")]
        public string Confirmer { get; set; }

        [Display(Name = "Người duyệt")]
        public string ConfirmerName { get; set; }

        [Display(Name = "Nhận xét")]
        public string RateComment { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Nội dung")]
        public string SentMessage { get; set; }

        [Display(Name = "Người gửi")]
        public string Sender { get; set; }

        [Display(Name = "Người gửi")]
        public string SenderName { get; set; }

        public int ReadStatus { get; set; }

    }
}