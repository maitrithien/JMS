using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DMS.Models;

namespace DMS
{
    public static class SystemEnvironments
    {
        public static HttpContext HttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        internal static HttpSessionState SessionState
        {
            get
            {
                if (HttpContext.Current == null) return null;
                var session = HttpContext.Current.Session;
                return session;
            }
        }

        public static string KEY_USERNAME = "UserName";
        public static string UserName
        {
            set { SystemEnvironments.SessionState[KEY_USERNAME] = value; }
            get { return SystemEnvironments.GetSession<string>(KEY_USERNAME); }
        }

        public static string KEY_EMPLOYEENAME = "EmployeeName";
        public static string EmployeeName
        {
            set { SystemEnvironments.SessionState[KEY_EMPLOYEENAME] = value; }
            get { return SystemEnvironments.GetSession<string>(KEY_EMPLOYEENAME); }
        }

        public static string KEY_DEPARTMENTID = "DepartmentID";
        public static string DepartmentID
        {
            set { SystemEnvironments.SessionState[KEY_DEPARTMENTID] = value; }
            get { return SystemEnvironments.GetSession<string>(KEY_DEPARTMENTID); }
        }

        public static string KEY_MANAGERID = "ManagerID";
        public static string ManagerID
        {
            set { SystemEnvironments.SessionState[KEY_MANAGERID] = value; }
            get { return SystemEnvironments.GetSession<string>(KEY_MANAGERID); }
        }

        public static string KEY_GROUPID = "GroupID";
        public static string GroupID
        {
            set { SystemEnvironments.SessionState[KEY_GROUPID] = value; }
            get { return SystemEnvironments.GetSession<string>(KEY_GROUPID); }
        }

        public static string DisplayUserName {
            get
            {
                return string.IsNullOrEmpty(SystemEnvironments.EmployeeName)
                    ? SystemEnvironments.UserName
                    : SystemEnvironments.EmployeeName;
            } 
        }

        public static void SetEmployeeInfo(string username)
        {
            // set current user
            SystemEnvironments.UserName = username;

            using (JMSEntities _EntityModel = new JMSEntities())
            {
                var employee = _EntityModel.Employees.FirstOrDefault(
                        x => username.Equals(x.UserName)) ?? new Employee();

                SystemEnvironments.EmployeeName = employee.FullName;
                SystemEnvironments.DepartmentID = employee.DepartmentID;
                SystemEnvironments.GroupID = employee.GroupID;

                SystemEnvironments.ManagerID = (_EntityModel.Departments
                    .FirstOrDefault(x => x.DepartmentID == SystemEnvironments.DepartmentID)
                    ?? new Department()).ManagerID;
            }
        }

        public static T GetSession<T>(string key)
        {
            if (SessionState == null)
                return default(T);

            var value = SessionState[key];
            T result = default(T);
            if (value != null && value is T) result = (T)value;
            return result;
        }

    }
}