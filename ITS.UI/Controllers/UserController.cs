using ITS.DAL;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UI.Attributes;
using ITS.UI.ModelViews;
using ITS.UI.Services;
using ITS.UTILITY;
using ITS.UTILITY.Custom;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{

    [LoginCheck]
    [AccessRightsCheck]
    [Description("İstifadəçi icazələri")]
    public class UserController : Controller
    {
        // GET: Users

        public ActionResult Index(int? page, int? userId, string vl, string prm = null)
        {
            UserVM userViewModel = null;
            try
            {
                UsersRepository repository = new UsersRepository();
                DataOperations dataOperations = new DataOperations();
                tbl_User userObj = new tbl_User();

                Search search = new Search();

                if (userId != null)
                {
                    Search ss = new Search();
                    //  ss.VehicleId = (int)vehicleId;
                    Session["SearchInfo"] = ss;
                    search = ss;
                }
                else
                {
                    search = SetValue(page, vl, prm);
                }


                int pageSize = 15;
                int pageNumber = (page ?? 1);

                userViewModel = new UserVM();
                userViewModel.Search = search;
                userViewModel.Search.pageSize = pageSize;
                userViewModel.Search.pageNumber = pageNumber;

                userViewModel.RUserList = repository.SW_GetUserDetails(userViewModel.Search);
                userViewModel.listCount = repository.SW_GetUserDetailsCount(userViewModel.Search);

                int[] pc = new int[userViewModel.listCount];

                userViewModel.Paging = pc.ToPagedList(pageNumber, pageSize);

                return Request.IsAjaxRequest()
         ? (ActionResult)PartialView("PartialIndex", userViewModel)
         : View(userViewModel);

            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }
        public Search SetValue(int? page, string vl, string prm = null)
        {
            if (prm == null && page == null)
            {
                Search ss = new Search();
                Session["SearchInfo"] = ss;
            }

            if (!string.IsNullOrEmpty(vl))
            {
                vl = StripTag.strSqlBlocker(vl);
            }

            Search search = new Search();

            search = (Search)Session["SearchInfo"];

            if (prm != null)
            {
                PropertyInfo propertyInfos = search.GetType().GetProperty(prm);
                propertyInfos.SetValue(search, Convert.ChangeType(vl, propertyInfos.PropertyType), null);
            }

            Session["SearchInfo"] = search;

            return search;

        }

        [Description("Yeni istifadəçi əlavə etmək")]
        public ActionResult Create()
        {
            UserVM viewModel = new UserVM();
            viewModel = poulateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(UserVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_User userItem = new tbl_User()
                        {
                            UserName = viewModel.UserName,
                            //FirstName = viewModel.FirstName,
                            //LastName = viewModel.LastName,

                            EmployeeID = viewModel.EmployeeID,

                            AccountLocked = viewModel.LockType,
                            Password = EncodeAndDecode.Base64Encode(viewModel.Password),
                            InsertDate = DateTime.Now,
                            InsertUser = UserProfile.UserId

                        };

                        DataOperations dataOperations = new DataOperations();
                        string responseMsj = string.Empty;
                        bool saved = dataOperations.AddUser(userItem, out responseMsj);
                        if (saved)
                        {
                            TempData["success"] = "Ok";
                            TempData["title"] = "Uğurlu";
                            TempData["message"] = responseMsj;
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            TempData["title"] = "Uğursuz cəhd!";
                            TempData["success"] = "notOk";
                            TempData["message"] = responseMsj;

                        }
                    }

                }

            }
            catch (ApplicationException ex)
            {

                viewModel = poulateDropDownList(viewModel);

                return View(viewModel);
            }
            viewModel = poulateDropDownList(viewModel);

            return View(viewModel);


        }

        [Description("İstifadəçini redaktə etmək")]
        public ActionResult Edit(int id)
        {

            DataOperations dataOperations = new DataOperations();
            tbl_User userobj = dataOperations.GetUser(id);
            UserVM viewModel = new UserVM()
            {
                UserName = userobj.UserName,
                EmployeeID = userobj.EmployeeID == null ? 0 : (Int64)userobj.EmployeeID,

                LockType = userobj.AccountLocked,
                Password = userobj.Password,
                Id = userobj.ID,

            };
            viewModel = poulateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(UserVM viewModel)
        {

            var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
            if (UserProfile != null)
            {
                tbl_User userItem = new tbl_User()
                {
                    ID = viewModel.Id,
                    UserName = viewModel.UserName,
                    EmployeeID = viewModel.EmployeeID,
                    AccountLocked = viewModel.LockType,
                    Password = EncodeAndDecode.Base64Encode(viewModel.Password),
                    UpdateUser = UserProfile.UserId,

                };

                DataOperations dataOperations = new DataOperations();
                string responseMsj = string.Empty;
                bool updated = dataOperations.UpdateUser(userItem, out responseMsj);
                if (updated)
                {
                    TempData["success"] = "Ok";
                    TempData["title"] = "Uğurlu";
                    TempData["message"] = responseMsj;
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["title"] = "Uğursuz cəhd!";
                    TempData["success"] = "notOk";
                    TempData["message"] = responseMsj;

                }

            }
            return RedirectToAction("Index");
        }
        private UserVM poulateDropDownList(UserVM viewModel)
        {
            viewModel.LockTypes = EnumService.GetLockEnumTypes();
            viewModel.EmpmloyeeList = EnumService.GetEmployeeList();

            return viewModel;
        }

        public ActionResult Delete(int id)
        {


            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    DataOperations dataOperations = new DataOperations();

                    string responseMsj = string.Empty;
                    bool updated = dataOperations.DeleteUser(id, UserProfile.UserId, out responseMsj);
                    if (updated)
                    {
                        TempData["success"] = "Ok";
                        TempData["message"] = responseMsj;


                    }
                    else
                    {
                        TempData["success"] = "notOk";
                        TempData["message"] = responseMsj;


                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }



        }

    }
}
