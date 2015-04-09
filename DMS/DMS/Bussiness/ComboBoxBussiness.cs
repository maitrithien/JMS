using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS.Bussiness
{
    public class ComboBoxBussiness
    {
        public List<UserProfile> GetAll()
        {
            List<UserProfile> _lst = new List<UserProfile>();
            UsersContext dbContext = new UsersContext();
            _lst = dbContext.UserProfiles.Select(x => x).ToList();

            return _lst;
        }
    }
}