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
    public class EmployeeVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<PersonDTO> REmployeeList { get; set; }
        public int ID { get; set; }

        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Zəhmət olmazsa adı daxil edin")]
        public string FirstName { get; set; }

        [Display(Name = "Soyadı")]
        [Required(ErrorMessage = "Zəhmət olmazsa soyadı daxil edin")]
        public string LastName { get; set; }

        [Display(Name = "Ata adı")]
        [Required(ErrorMessage = "Zəhmət olmazsa ata adını daxil edin")]
        public string FatherName { get; set; }


        [Display(Name = "Cinsi")]
        [Required(ErrorMessage = "Zəhmət olmazsa cinsini seçin")]
        public int GenderType { get; set; }

        public string GenderTypeDesc { get; set; }
        public IEnumerable<SelectListItem> GenderTypeList { get; set; }

        [Display(Name = "Activmi")]
        public int IsActive { get; set; }

        [Display(Name = "Vəzifə adı")]
        //[Required(ErrorMessage = "Zəhmət olmazsa vəzifə seçin")]
        public int PositionID { get; set; }
        public string PositionDesc { get; set; }
        public IEnumerable<SelectListItem> PositionList { get; set; }

    }
}