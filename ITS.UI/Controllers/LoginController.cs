using ITS.DAL;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UI.Attributes;
using ITS.UI.ModelViews;
using ITS.UTILITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{
    public class LoginController : Controller
    {
        //UserViewModel usersViewModel = new UserViewModel();
        UserProfileSessionData UserProfile;

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserVM uvm)
        {
            if (ModelState.IsValid)
            {
                string IPAddress = GetIPAddress();
                LoginRepository repository = new LoginRepository();
                DataOperations dataOperation = new DataOperations();
                string result = repository.DoLogin(uvm.UserName, EncodeAndDecode.Base64Encode(uvm.Password), IPAddress);

                if (result == "Uğurlu")
                {
                    tbl_User userObj = dataOperation.GetUserByUserName(uvm.UserName);
                    tbl_Employee employeeObj = dataOperation.GetEmployeeById(userObj.EmployeeID == null ? 0 : (Int64)userObj.EmployeeID);
                    UserProfile = new UserProfileSessionData()
                    {
                        UserId = userObj.ID,
                        EmployeeID = employeeObj.ID,
                        UserName = userObj.UserName,
                        FirstName = employeeObj.FirstName,
                        LastName = employeeObj.LastName,

                    };

                    this.Session["UserProfile"] = UserProfile;
                    UrlSessionData CurrentUrl = new UrlSessionData
                    {
                        Controller = "Home",
                        Action = "Index"
                    };
                    this.Session["CurrentUrl"] = CurrentUrl;
                    return RedirectToAction("Index", "Home");
                }
                else if (result == "İstifadəçi adı tapılmadı")

                {
                    ViewBag.NotValidUser = result;

                }
                else
                {
                    ViewBag.Failedcount = result;
                }
                return View("Index");
            }
            else
            {
                return View("Index", uvm);
            }
        }

        public ActionResult LoginOut()
        {
            this.Session["UserProfile"] = null;
            return RedirectToAction("Login");
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}