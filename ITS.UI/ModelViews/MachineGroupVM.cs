using ITS.DAL.Model;
using ITS.DAL.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITS.UI.ModelViews
{
    public class MachineGroupVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public int ListCount { get; set; }
        public IList<tbl_MachineGroup> RMachineGroupList { get; set; }
        public int ID { get; set; }


        [Display(Name = "Avadanlığın adı")]
        [Required(ErrorMessage = "Zəhmət olmazsa digər avadanlığın adı daxil edin")]
        public string Name { get; set; }

        [Display(Name = "Açıqlama")]
        //[Required(ErrorMessage = "Zəhmət olmazsa digər ad daxil edin")]
        public string Description { get; set; }
    }
}