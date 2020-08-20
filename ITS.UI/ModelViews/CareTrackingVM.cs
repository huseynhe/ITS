using ITS.DAL.DTO;
using ITS.DAL.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.ModelViews
{
    public class CareTrackingVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<CareTrackingDTO> RCareTrackingList { get; set; }
        public int ID { get; set; }

        [Display(Name = "Baxım vaxtı")]
        [Required(ErrorMessage = "Zəhmət olmazsa baxım vaxtını daxil edin")]
        public DateTime CareDate { get; set; }

        [Display(Name = "İş merkezi adı")]
        public int BusinessCenterID { get; set; }
        public IEnumerable<SelectListItem> BusinessCenterList { get; set; }

        [Display(Name = "Makina grubu adı")]
        public int MachineGroupID { get; set; }
        public IEnumerable<SelectListItem> MachineGroupList { get; set; }

        [Display(Name = "Makina adı")]
        public int MachineID { get; set; }
        public IEnumerable<SelectListItem> MachineList { get; set; }

        [Display(Name = "Təmir və ya baxım işinin təsviri")]
        public string CareDescription { get; set; }

        [Display(Name = "Baxım növü")]
        public int CareType { get; set; }
        public IEnumerable<SelectListItem> CareTypeList { get; set; }

        [Display(Name = "Planlı baxım növü")]
        public int PlanedCareType { get; set; }
        public IEnumerable<SelectListItem> PlanedCareTypeList { get; set; }

        [Display(Name = "Baxım komandası")]
        public int CareTeamType { get; set; }
        public IEnumerable<SelectListItem> CareTeamTypeList { get; set; }

        [Display(Name = "Nəticə")]
        public int ResultType { get; set; }
        public IEnumerable<SelectListItem> ResultTypeList { get; set; }

        [Display(Name = "Nəticə təsviri")]
        public string ResultDescription { get; set; }
    }
}