using ITS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class AccessRightDTO
    {

        public tbl_AccessRight AccessRightObj { get; set; }
        public tbl_User UserObj { get; set; }
        public string ContollerDescription { get; set; }
        public string ActionDescription { get; set; }

        public AccessRightDTO()
        {
            this.AccessRightObj = new tbl_AccessRight();
            this.UserObj = new tbl_User();
        }
    }
}
