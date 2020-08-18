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
    public class MachineVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<MachineDTO> RMachineList { get; set; }
        public int ID { get; set; }

        [Display(Name = "Avadanlığın kodu")]
        [Required(ErrorMessage = "Zəhmət olmazsa avadanlığın kodu daxil edin")]
        public string Code { get; set; }

        [Display(Name = "Avadanlığın  adı")]
        //[Required(ErrorMessage = "Zəhmət olmazsa digər ad daxil edin")]
        public string Name { get; set; }

        [Display(Name = "Açıqlama")]
        //[Required(ErrorMessage = "Zəhmət olmazsa digər ad daxil edin")]
        public string Description { get; set; }

        [Display(Name = "Miqdarı (ədəd)")]
        public Nullable<int> Quantity { get; set; }

        [Display(Name = "Makina Grubu")]
        public int MachineGroupID { get; set; }
        public string MachineGroupDesc { get; set; }
        public IEnumerable<SelectListItem> MachineGroupList { get; set; }

    }
}