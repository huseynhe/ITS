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

        public Int32 RegionID { get; set; }
        public string RegionFullName { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }

        public Int32 CountryID { get; set; }
        public string CountryName { get; set; }
        public string WorkAddress { get; set; }
        public string WorkFullAddress { get; set; }

        public int ContactID { get; set; }
        public string SpecialCommunication { get; set; }
        public string CityPhone { get; set; }
        public string InternalPhone { get; set; }
        public string HomePhone { get; set; }
        public string RoomNumber { get; set; }
        public string Email { get; set; }
         

        public Int32 OrganisationID { get; set; }
        public string OrganisationName { get; set; }
        public Int32 PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public int Sort { get; set; }


    }
}
