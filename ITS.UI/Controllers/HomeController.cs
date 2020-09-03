using ITS.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{
    public class HomeController : Controller
    {
        [LoginCheck]
        [AccessRightsCheck]
        [Description("Əsas səifə")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AccessRightsError(string CName, string AName)
        {
            TempData["success"] = "notOk";
            TempData["message"] = "Bu əməliyyat üçün icazəniz yoxdur";

            return RedirectToAction(AName, CName);
        }
    }
}