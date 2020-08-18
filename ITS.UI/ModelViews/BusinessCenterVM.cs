using ITS.DAL.Model;
using ITS.DAL.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITS.UI.ModelViews
{
    public class BusinessCenterVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<tbl_BusinessCenter> RBusinessCenterList { get; set; }
        public int ID { get; set; }


        [Display(Name = "İş merkezinin adı")]
        [Required(ErrorMessage = "Zəhmət olmazsa digər avadanlığın adı daxil edin")]
        public string Name { get; set; }

        [Display(Name = "Açıqlama")]
        public string Description { get; set; }

        [Display(Name = "Ünvan")]
        public string Address { get; set; }
    }
}