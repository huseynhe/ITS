using ITS.DAL.DTO;
using ITS.DAL.Objects;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.ModelViews
{
    public class UserVM
    {
        public Search Search;
        public PagedList.IPagedList<int> Paging { get; set; }
        public IList<UserDTO> RUserList { get; set; }
        public int listCount { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa istifadəçi adını daxil edin")]
        [Display(Name = "İstifadəçi adı")]
        [StringLength(20, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa parolu daxil edin")]
        [Display(Name = "Parol")]
        [StringLength(20, MinimumLength = 1)]
        public string Password { get; set; }

        [Display(Name = "İşçi seçiniz")]
        public Int64 EmployeeID { get; set; }
        public IEnumerable<SelectListItem> EmpmloyeeList { get; set; }

        //[Required(ErrorMessage = "Zəhmət olmasa adınızı daxil edin")]
        [Display(Name = "Ad")]
        //[StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Zəhmət olmasa soyadınızı daxil edin")]
        [Display(Name = "Soyad")]
        //[StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }




        [Display(Name = "Kilid tipi")]
        public int? LockType { get; set; }
        public IEnumerable<SelectListItem> LockTypes { get; set; }

        [Display(Name = "Sonuncu giriş")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Sonuncu cəhd")]
        public DateTime? LastAccessedDate { get; set; }

        [Display(Name = "Uğursuz cəhdlərin sayı")]
        public int? LoginFailedCount { get; set; }

        [Display(Name = "IP ünvanı")]
        public string LoginIPAddress { get; set; }


    }
}