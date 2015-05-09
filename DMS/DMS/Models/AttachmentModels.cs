using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS.Models
{
    public class AttachmentModels : BaseModels
    {
        public Guid? APK { get; set; }

        public Guid? JobAPK { get; set; }

        [Display(Name = "Tên tập tin")]
        [DisplayName("Tên tập tin")]
        public string AttachmentFileName { get; set; }

        [Display(Name = "Kích thước")]
        [DisplayName("Kích thước")]
        public string AttachmentFileSize { get; set; }

        [Display(Name = "Loại tập tin")]
        [DisplayName("Loại tập tin")]
        public string AttachmentFileType { get; set; }

        [Display(Name = "Mô tả")]
        [DisplayName("Mô tả")]
        public string AttachmentComment { get; set; }

        [Display(Name = "Người đăng")]
        [DisplayName("Người đăng")]
        public string AttachmentOwner { get; set; }

        [Display(Name = "Phần mở rộng")]
        [DisplayName("Phần mở rộng")]
        public string AttachmentFileExtension { get; set; }

    }
}