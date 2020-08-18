using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.Objects
{
    [Serializable]
    public class UserProfileSessionData
    {
        public int UserId { get; set; }
        public Int64 EmployeeID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
