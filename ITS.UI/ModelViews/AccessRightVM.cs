using ITS.DAL.DTO;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.ModelViews
{
    public class AccessRightVM
    {

        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public IList<tbl_AccessRight> RAccessRightList { get; set; }



        public List<AccessRightDTO> RAccessRightsDTOList { get; set; }
        public int ListCount { get; set; }
        public int PageSize { get; set; }


        [Required(ErrorMessage = "Zəhmət olmasa icazəni daxil edin")]
        [Display(Name = "İcazə")]
        public int AccessType { get; set; }
        public IEnumerable<SelectListItem> AccessTypes { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa istifadəçini daxil edin")]
        [Display(Name = "İstifadəçi")]
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa modulu daxil edin")]
        [Display(Name = "Modul")]
        public string Controller { get; set; }
        public IEnumerable<SelectListItem> Controllers { get; set; }
        public string ControllerDesciption { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa əməliyyatı daxil edin")]
        [Display(Name = "Əməliyyat")]
        public string Action { get; set; }
        public IEnumerable<SelectListItem> Actions { get; set; }
        public string ActionDescription { get; set; }

    }
}