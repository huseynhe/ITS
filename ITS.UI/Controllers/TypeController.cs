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
    [Description("Mövzullar")]
    public class TypeController : Controller
    {
        // GET: Type
        public ActionResult Index(int? page, int? vehicleId, string vl, string prm = null)
        {

            try
            {

                TypeRepository repository = new TypeRepository();
                TypeVM viewModel = new TypeVM();

                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);


                viewModel.Search = search;
                viewModel.Search = search;
                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RTypeDTOList = repository.SW_GetTypes(viewModel.Search);

                viewModel.ListCount = repository.SW_GetTypesCount(viewModel.Search);
                int[] pc = new int[viewModel.ListCount];

                viewModel.Paging = pc.ToPagedList(pageNumber, pageSize);



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

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(TypeVM typeViewModel)
        {
            var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
            if (UserProfile != null)
            {
                tbl_Type typeItem = new tbl_Type()
                {
                    Name = typeViewModel.Name,
                    Description = typeViewModel.Description,
                    InsertUser = UserProfile.UserId
                };

                DataOperations dateOperation = new DataOperations();
                tbl_Type typeDB = dateOperation.AddType(typeItem);
                if (typeDB != null)
                {
                    TempData["success"] = "Ok";
                    TempData["message"] = "Məlumatlar uğurla əlavə olundu";


                }
                else
                {
                    TempData["success"] = "notOk";
                    TempData["message"] = "Məlumatlar əlavə olunarkən xəta baş verdi";


                }
            }
            return RedirectToAction("Index");

        }

        public ActionResult CreateSubMenu(int id)
        {

            TypeVM typeViewModel = new TypeVM();
            typeViewModel.ParentID = id;
            typeViewModel = poulateParentList(typeViewModel);
            return View(typeViewModel);
        }
        [HttpPost]
        public ActionResult CreateSubMenu(TypeVM typeViewModel)
        {
            var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
            if (UserProfile != null)
            {
                tbl_Type typeItem = new tbl_Type()
                {
                    Name = typeViewModel.Name,
                    ParentID = (int)typeViewModel.ParentID,
                    Description = typeViewModel.Description,
                    InsertUser = UserProfile.UserId
                };

                DataOperations dateOperation = new DataOperations();
                tbl_Type typeDB = dateOperation.AddType(typeItem);
                if (typeDB != null)
                {
                    TempData["success"] = "Ok";
                    TempData["message"] = "Məlumatlar uğurla əlavə olundu";


                }
                else
                {
                    TempData["success"] = "notOk";
                    TempData["message"] = "Məlumatlar əlavə olunarkən xəta baş verdi";


                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    DataOperations dataOperations = new DataOperations();
                    tbl_Type typleDB = dataOperations.GetTypeById(id);
                    TypeVM viewModel = new TypeVM()
                    {
                        ID = typleDB.ID,
                        Name = typleDB.Name,
                        ParentID = typleDB.ParentID,
                        Description = typleDB.Description

                    };
                    viewModel = poulateParentList(viewModel);

                    return View(viewModel);

                }
                return null;
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }

        }

        [HttpPost]
        public ActionResult Edit(TypeVM viewModel)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (!ModelState.IsValid)
                    {

                        return View(viewModel);
                    }
                    int parentID = viewModel.ParentID == null ? 0 : (int)viewModel.ParentID;

                    tbl_Type type = new tbl_Type()
                    {
                        ID = (int)viewModel.ID,
                        ParentID = parentID,
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        UpdateUser = UserProfile.UserId

                    };


                    tbl_Type updatedItemDB = dataOperations.UpdateType(type);
                    if (updatedItemDB != null)
                    {
                        TempData["success"] = "Ok";
                        TempData["message"] = "Məlumatlar uğurla dəyişdirildi";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["success"] = "notOk";
                        TempData["message"] = "Məlumatlar dəyişdirilərkən xəta baş verdi";
                        return RedirectToAction("Index");

                    }
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }

        [Description("Sorağça sil")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeleteType(id, UserProfile.UserId);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }

        public ActionResult Detail(int id)
        {
            TypeVM typeViewModel = new TypeVM();
            TypeRepository repository = new TypeRepository();
            DataOperations dataOperations = new DataOperations();

            TypeDTO typeDTO = repository.GetTypeDTOByID(id);
            typeViewModel.ParentID = typeDTO.ParentID;
            typeViewModel.ParentName = typeDTO.ParentTypeName;
            typeViewModel.ID = typeDTO.ID;
            typeViewModel.Name = typeDTO.Name;
            typeViewModel.Description = typeDTO.Description;
            if (typeDTO != null)
            {
                typeViewModel.RTypeList = dataOperations.GetTypeByParentId(typeDTO.ID);
            }

            return PartialView(typeViewModel);
        }

        public TypeVM poulateParentList(TypeVM typeViewModel)
        {
            typeViewModel.ParentList = EnumService.GetTypeEnumParentList();
            return typeViewModel;
        }
    }
}