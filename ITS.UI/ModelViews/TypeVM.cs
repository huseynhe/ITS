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
    public class TypeVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }

        public IList<TypeDTO> RTypeDTOList { get; set; }
        public IList<tbl_Type> RTypeList { get; set; }

        [Display(Name = "İD: ")]
        public int? ID { get; set; }
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Üst ad İD: ")]
        public int? ParentID { get; set; }

        [Display(Name = "Üst ad")]
        public string ParentName { get; set; }

        [Display(Name = "Qeyd (açıqlama)")]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> ParentList { get; set; }
    }
}