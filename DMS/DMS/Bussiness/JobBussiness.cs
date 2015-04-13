using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS.Bussiness
{
    public class JobBussiness
    {
        public List<JobModels> GetAll()
        {
            List<JobModels> _lst = new List<JobModels>();
            var rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                _lst.Add(new JobModels
                {
                    APK = Guid.NewGuid(),
                    JobID = string.Format("HS/{0:00}/{1:0000}/{2:000}", DateTime.Now.Month, DateTime.Now.Year, i),
                    JobName = string.Format("Hồ sơ {0:000}", i),
                    Deadline = DateTime.Now,
                    Note = string.Format("Ghi chú cho hồ sơ {0:000}", i),
                    Status = rand.Next(4),
                    CreatedDate = DateTime.Now,
                    CreatedUserID = "NHANVIEN",
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = "NHANVIEN",
                    Recipient = "TRUONGPHONG",
                    Confirmer = "GIAMDOC"
                });
            }

            return _lst;
        }

        public List<NoteModels> GetNotes(Guid apk)
        {
            List<NoteModels> _lst = new List<NoteModels>();
            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                _lst.Add(new NoteModels
                {
                    APK = Guid.NewGuid(),
                    Description = string.Format("Ghi chú {0:000}", i),
                    CreatedDate = DateTime.Now,
                    CreatedUserID = "NHANVIEN",
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = "NHANVIEN",
                });
            }

            return _lst;
        }

        public List<AttachmentModels> GetAttachments(Guid apk)
        {
            List<AttachmentModels> _lst = new List<AttachmentModels>();
            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                _lst.Add(new AttachmentModels
                {
                    APK = Guid.NewGuid(),
                    AttachmentFileName = string.Format("File{0:000}.txt", i),
                    AttachmentFileSize = string.Format("{0}kb", i*100),
                    AttachmentFileType = "Text",
                    AttachmentFileExtension = ".txt",
                    AttachmentOwner = "NHANVIEN",
                    AttachmentComment = string.Format("Ghi chú cho đính kèm {0:000}", i),
                    CreatedDate = DateTime.Now,
                    CreatedUserID = "NHANVIEN",
                    LastModifyDate = DateTime.Now,
                    LastModifyUserID = "NHANVIEN",
                });
            }

            return _lst;
        }

        public List<HistoryModels> GetHistories(Guid apk)
        {
            List<HistoryModels> _lst = new List<HistoryModels>();
            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                _lst.Add(new HistoryModels
                {
                    APK = Guid.NewGuid(),
                    ActionType = rand.Next(2),
                    Description = string.Format("Ghi chú cho đính kèm {0:000}", i),
                    CreatedDate = DateTime.Now,
                    CreatedUserID = "NHANVIEN"
                });
            }

            return _lst;
        }
    }
}