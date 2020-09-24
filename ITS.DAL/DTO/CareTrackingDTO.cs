using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class CareTrackingDTO
    {
        public int CareTrackingID { get; set; }
        public DateTime CareDate { get; set; }
        public int BusinessCenterID { get; set; }
        public string BusinessCenterName { get; set; }
        public int MachineGroupID { get; set; }
        public string MachineGroupName { get; set; }
        public int MachineID { get; set; }
        public string MachineName { get; set; }
        public string CareDescription { get; set; }
        public int CareType { get; set; }
        public string CareTypeDesc { get; set; }
        public int PlanedCareType { get; set; }
        public string PlanedCareTypeDesc { get; set; }
        public int CareTeamType { get; set; }
        public string CareTeamTypeDesc { get; set; }
        public CareTrackingDetailDTO careTrackingDetailDTO { get; set; }


    }

} 
