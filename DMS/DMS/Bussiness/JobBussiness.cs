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
    }
}