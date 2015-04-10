using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class AttachmentModels
    {
        public Guid? APK { get; set; }

        [Display(Name = "Tên tập tin")]
        public string AttachmentFileName { get; set; }

        [Display(Name = "Kích thước")]
        public string AttachmentFileSize { get; set; }

        [Display(Name = "Loại tập tin")]
        public string AttachmentFileType { get; set; }

        [Display(Name = "Mô tả")]
        public string AttachmentComment { get; set; }

        [Display(Name = "Người đăng")]
        public string AttachmentOwner { get; set; }

        [Display(Name = "Đường dẫn")]
        public string AttachmentFilePath { get; set; }

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