using ITS.DAL;
using ITS.DAL.DTO;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UI.Attributes;
using ITS.UI.ModelViews;
using ITS.UI.Services;
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
    public class AccessRightController : Controller
    {
        // GET: AccessRights

        public ActionResult Index(int? page, int? userId, string vl, string prm = null)
        {
            try
            {
                AccessRightVM viewModel = new AccessRightVM();

                AccessRightsRepository accessrepository = new AccessRightsRepository();
                DataOperations dataOperations = new DataOperations();
                UsersRepository usersRepository = new UsersRepository();

                //Edit sehifesi ucundur
                Session["UserId"] = userId;

                Search search = new Search();
                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);



                viewModel.Search = search;
                viewModel.UserId = (int)userId;
                viewModel.Search.UserId = (int)userId;
                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;
                List<AccessRightDTO> list = accessrepository.SW_GetAccessRightsDTO(viewModel.Search);
                viewModel.RAccessRightsDTOList = FillDescriptions(list);

                viewModel.ListCount = accessrepository.SW_GetAccessRightsDTOCount(viewModel.Search);
                int[] pc = new int[viewModel.ListCount];

                viewModel.Paging = pc.ToPagedList(pageNumber, pageSize);


                //ViewBag.Message = UC.User.FirstName + " " + UC.User.LastName;
                //ViewBag.Data = UserId;
                //return View(accessrightsviewmodel);

                return Request.IsAjaxRequest()
              ? (ActionResult)PartialView("PartialIndex", viewModel)
              : View(viewModel);
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

        [Description("Yeni icazə əlavə etmək")]
        public ActionResult Create()
        {
            AccessRightVM viewModel = new AccessRightVM();
            viewModel.UserId = (int)Session["UserId"];
            viewModel = poulateDropDownList(viewModel);
            viewModel = FillControllers(viewModel);
            viewModel = FillActions(viewModel, "UsersController");
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(AccessRightVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_AccessRight accessItem = new tbl_AccessRight()
                        {
                            Controller = viewModel.Controller,
                            ControllerDesc = viewModel.ControllerDesciption,
                            Action = viewModel.Action,
                            ActionDesc = viewModel.ActionDescription,
                            HasAccess = viewModel.AccessType,
                            UserId = viewModel.UserId,
                            InsertDate = DateTime.Now,
                            InsertUser = UserProfile.UserId

                        };

                        DataOperations dataOperations = new DataOperations();
                        string responseMsj = string.Empty;
                        bool saved = dataOperations.AddAccessRights(accessItem, out responseMsj);
                        if (saved)
                        {
                            TempData["success"] = "Ok";
                            TempData["title"] = "Uğurlu";
                            TempData["message"] = responseMsj;
                            return RedirectToAction("Index", new { userId = viewModel.UserId });
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

        public ActionResult Edit(int id)
        {

            AccessRightVM viewModel = new AccessRightVM();
            DataOperations dataOperations = new DataOperations();
            tbl_AccessRight _AccessRight = dataOperations.GetAccessRight(id);
            viewModel.Id = id;
            viewModel.UserId = _AccessRight.UserId;
            viewModel.Controller = _AccessRight.Controller;
            viewModel.Action = _AccessRight.Action;
            viewModel.AccessType = _AccessRight.HasAccess;

            viewModel = poulateDropDownList(viewModel);
            viewModel = FillControllers(viewModel);
            viewModel = FillActions(viewModel, viewModel.Controller);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(AccessRightVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_AccessRight accessItem = new tbl_AccessRight()
                        {
                            ID = viewModel.Id,
                            Controller = viewModel.Controller,
                            ControllerDesc = viewModel.ControllerDesciption,
                            Action = viewModel.Action,
                            ActionDesc = viewModel.ActionDescription,
                            HasAccess = viewModel.AccessType,
                            UserId = viewModel.UserId,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };

                        DataOperations dataOperations = new DataOperations();
                        string responseMsj = string.Empty;
                        bool saved = dataOperations.UpdateAccessRights(accessItem, out responseMsj);
                        if (saved)
                        {
                            TempData["success"] = "Ok";
                            TempData["title"] = "Uğurlu";
                            TempData["message"] = responseMsj;
                            return RedirectToAction("Index", new { userId = viewModel.UserId });
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

        public ActionResult Delete(int id, int userId)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    DataOperations dataOperations = new DataOperations();

                    string responseMsj = string.Empty;
                    bool updated = dataOperations.DeleteAccessRights(id, UserProfile.UserId, out responseMsj);
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
                return RedirectToAction("Index", new { userId = userId });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }


        }
        private List<AccessRightDTO> FillDescriptions(List<AccessRightDTO> ARCS)
        {
            IEnumerable<SelectListItem> ControllerList = EnumService.GetControllers();
            IEnumerable<SelectListItem> ActionList;

            foreach (AccessRightDTO ARC in ARCS)
            {
                ARC.ContollerDescription = ControllerList.Where(c => c.Value == ARC.AccessRightObj.Controller).Select(c => c.Text).FirstOrDefault();
                ActionList = EnumService.GetActions(ARC.AccessRightObj.Controller);
                ARC.ActionDescription = ActionList.Where(a => a.Value == ARC.AccessRightObj.Action).Select(a => a.Text).FirstOrDefault();
            }
            ARCS = ARCS.OrderBy(u => u.AccessRightObj.Controller).ToList();
            return ARCS;
        }
        public AccessRightVM FillControllers(AccessRightVM viewModel)
        {
            viewModel.Controllers = EnumService.GetControllers();
            return viewModel;
        }
        public AccessRightVM FillActions(AccessRightVM viewModel, string ControllerName)
        {
            viewModel.Actions = EnumService.GetActions(ControllerName);
            return viewModel;
        }

        private AccessRightVM poulateDropDownList(AccessRightVM viewModel)
        {
            viewModel.AccessTypes = EnumService.GetAccessEnumTypes();

            return viewModel;
        }

        [HttpPost]
        public JsonResult GetActionList(string controlerName)
        {

            IEnumerable<SelectListItem> selectListItems = EnumService.GetActions(controlerName);

            return Json(selectListItems, JsonRequestBehavior.AllowGet);


        }

    }
}