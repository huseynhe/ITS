using ITS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class PersonDTO
    {
        public Int64 ID { get; set; }
        public string PIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public int GenderType { get; set; }
        public string GenderTypeDesc { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public int PersonType { get; set; }
        public string PersonTypeDesc { get; set; }

        public Int32 RegionID { get; set; }
        public string RegionFullName { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }

   


    }
}
