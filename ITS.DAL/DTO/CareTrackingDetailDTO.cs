using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class CareTrackingDetailDTO
    {
        public int CareTrackingDetailID { get; set; }
        public int CareTrackingID { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
        public int MechanicID { get; set; }
        public string MechanicSAA { get; set; }
        public int ReceivingPersonID { get; set; }
        public string ReceivingPersonSAA { get; set; }
    }
}
