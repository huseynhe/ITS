using ITS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class UserDTO
    {
        public Int64 UserID { get; set; }
        public Int64 EmployeeID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AccountLocked { get; set; }

    }
}
