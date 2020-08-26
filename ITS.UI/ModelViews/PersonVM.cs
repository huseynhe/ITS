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
    public class PersonVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<PersonDTO> RPersonList { get; set; }

        [Display(Name = "Şəxs")]
        public int ID { get; set; }
        [Display(Name = "FIN")]
        public string PIN { get; set; }

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

        [Display(Name = "Ünvan")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Açıqlama")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Şəxs tipi")]
        [Required(ErrorMessage = "Zəhmət olmazsa şəxs tipini seçin")]
        public int PersonType { get; set; }
        public string PersonTypeDesc { get; set; }
        public IEnumerable<SelectListItem> PersonTypeList { get; set; }

        [Display(Name = "Prfoil şəkili")]
        public string Photo { get; set; }


    }
}