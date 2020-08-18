using ITS.DAL;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UI.Attributes;
using ITS.UI.ModelViews;
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
    public class BusinessCenterController : Controller
    {
        // GET: BusinessCenter
        [LoginCheck]
        [AccessRightsCheck]
        [Description("İş merkezlerinin siyahısı")]
        public ActionResult Index(int? page, string vl, string prm = null)
        {
            BusinessCenterRepository repository = new BusinessCenterRepository();
            try
            {
                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                BusinessCenterVM viewModel = new BusinessCenterVM();
                viewModel.Search = search;

                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RBusinessCenterList = repository.SW_GetBusinessCenters(viewModel.Search);

                viewModel.ListCount = repository.SW_GetBusinessCentersCount(viewModel.Search);
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

        [Description("Yeni iş merkezi əlavə etmək")]
        public ActionResult Create()
        {
            BusinessCenterVM viewModel = new BusinessCenterVM();
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(BusinessCenterVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_BusinessCenter item = new tbl_BusinessCenter()
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Address=viewModel.Address,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };

                    DataOperations dataOperations = new DataOperations();
                    tbl_BusinessCenter dbItem = dataOperations.AddBusinessCenter(item);
                    if (dbItem != null)
                    {
                        TempData["success"] = "Ok";
                        TempData["message"] = "Məlumatlar uğurla əlavə olundu";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["success"] = "notOk";
                        TempData["message"] = "Məlumatlar əlavə olunarkən xəta baş verdi";
                        return RedirectToAction("Index");

                    }


                }
            }
            catch (ApplicationException ex)
            {


                return View(viewModel);
            }
            throw new ApplicationException("Invalid model");

        }
        [Description("İş merkezini redaktə etmək")]
        public ActionResult Edit(int id)
        {
            BusinessCenterVM viewModel = new BusinessCenterVM();
            DataOperations dataOperations = new DataOperations();
            tbl_BusinessCenter tblItem = dataOperations.GetBusinessCenterById(id);
            viewModel.ID = id;
            viewModel.Name = tblItem.Name;
            viewModel.Description = tblItem.Description;
            viewModel.Address = tblItem.Address;
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(BusinessCenterVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_BusinessCenter item = new tbl_BusinessCenter()
                        {
                            ID = viewModel.ID,
                            Name = viewModel.Name,
                            Description = viewModel.Description,
                            Address = viewModel.Address,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };


                        DataOperations dataOperations = new DataOperations();
                        tbl_BusinessCenter dbItem = dataOperations.UpdateBusinessCenter(item);
                        if (dbItem != null)
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
                }

                throw new ApplicationException("Invalid model");
            }
            catch (ApplicationException ex)
            {

                return View(viewModel);
            }

        }
        [Description("İş merkezi sil")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeleteBusinessCenter(id, UserProfile.UserId);
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