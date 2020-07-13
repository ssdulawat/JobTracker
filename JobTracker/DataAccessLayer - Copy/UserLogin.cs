using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;

namespace DataAccessLayer
{
    public class UserLogin
    {

        public List<LoginAuthentication> GetUsers(string UserName, string Password)
        {
            TestVariousInfoEntities db = new TestVariousInfoEntities();
            //List<LoginAuthentication> userLogins = new List<LoginAuthentication>();

            string sql = "SELECT e.UserType,e.UserName,e.Id FROM EmployeeDetails e LEFT JOIN MasterItem m ON e.id=m.EmpId Where e.UserName = '"+ UserName + "' AND e.Password = '"+ Password + "' AND (e.IsDelete IS NULL OR e.IsDelete = 0) AND(m.IsDisable = 0 OR m.IsDisable IS NULL)" ;

            var result = db.Database.SqlQuery<LoginAuthentication>(sql).ToList();

            //var UserDetails = (from userlist in db.EmployeeDetails
            //                           where userlist.UserName == UserName && userlist.Password == Password
            //                           select new
            //                           {
            //                               userlist.UserName,
            //                               userlist.Password,
            //                               userlist.UserType
            //                           }).ToList();

            return result;
        }
    }

}

