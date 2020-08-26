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
        public string BusinessCenterDesc { get; set; }
        public IEnumerable<SelectListItem> BusinessCenterList { get; set; }

        [Display(Name = "Makina grubu adı")]
        public int MachineGroupID { get; set; }
        public string MachineGroupDesc { get; set; }
        public IEnumerable<SelectListItem> MachineGroupList { get; set; }

        [Display(Name = "Makina adı")]
        public int MachineID { get; set; }
        public string MachineDesc { get; set; }
        public IEnumerable<SelectListItem> MachineList { get; set; }

        [Display(Name = "Təmir və ya baxım işinin təsviri")]
        public string CareDescription { get; set; }

        [Display(Name = "Baxım növü")]
        public int CareType { get; set; }
        public string CareTypeDesc { get; set; }
        public IEnumerable<SelectListItem> CareTypeList { get; set; }

        [Display(Name = "Planlı baxım növü")]
        public int PlanedCareType { get; set; }
        public string PlanedCareTypeDesc { get; set; }
        public IEnumerable<SelectListItem> PlanedCareTypeList { get; set; }

        [Display(Name = "Baxım komandası")]
        public int CareTeamType { get; set; }
        public string CareTeamTypeDesc { get; set; }
        public IEnumerable<SelectListItem> CareTeamTypeList { get; set; }

        [Display(Name = "Nəticə")]
        public int ResultType { get; set; }
        public string ResultTypeDesc { get; set; }
        public IEnumerable<SelectListItem> ResultTypeList { get; set; }

        [Display(Name = "Nəticə təsviri")]
        public string ResultDescription { get; set; }

        #region CareTrackingDetail
        public IList<CareTrackingDetailDTO> RCareTrackingDetailList { get; set; }
        public int CareTrackingDetailID { get; set; }
        [Display(Name = "Başlama tarixi")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Başlama zamanı")]
        public TimeSpan? StartTime { get; set; }
        [Display(Name = "Bitiş tarixi")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Bitiş zamanı")]
        public TimeSpan? EndTime { get; set; }
        [Display(Name = "Aciqlama")]
        public string Description { get; set; }

        [Display(Name = "Mexanik(Soyad Ad Ata adı)")]
        public int MechanicID { get; set; }
        public string MechanicSAA { get; set; }
        public IEnumerable<SelectListItem> MechanicList { get; set; }
        [Display(Name = "Təhvil alan şexs (Soyad Ad Ata adı)")]
        public int ReceivingPersonID { get; set; }
        public string ReceivingPersonSAA { get; set; }
        public IEnumerable<SelectListItem> ReceivingPersonList { get; set; }
      
        #endregion
    }
}