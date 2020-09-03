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
    [LoginCheck]
    [AccessRightsCheck]
    [Description("Avadanlığ qrupunun icazələri")]
    public class MachineGroupController : Controller
    {
        // GET: MachineGroup
        [LoginCheck]
        [AccessRightsCheck]
        [Description("Avadanlığ qrupunun siyahısı")]
        public ActionResult Index(int? page, string vl, string prm = null)
        {
            MachineGroupRepository repository = new MachineGroupRepository();
            try
            {
                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                MachineGroupVM viewModel = new MachineGroupVM();
                viewModel.Search = search;

                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RMachineGroupList = repository.SW_GetMachineGroups(viewModel.Search);

                viewModel.ListCount = repository.SW_GetMachineGroupsCount(viewModel.Search);
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

        [Description("Yeni avadanlığ qrupu əlavə etmək")]
        public ActionResult Create()
        {
            MachineGroupVM viewModel = new MachineGroupVM();
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(MachineGroupVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_MachineGroup item = new tbl_MachineGroup()
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };

                    DataOperations dataOperations = new DataOperations();
                    tbl_MachineGroup dbItem = dataOperations.AddMachineGroup(item);
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
        [Description("Avadanlığ qrupu redaktə etmək")]
        public ActionResult Edit(int id)
        {
            MachineGroupVM viewModel = new MachineGroupVM();
            DataOperations dataOperations = new DataOperations();
            tbl_MachineGroup tblItem = dataOperations.GetMachineGroupById(id);
            viewModel.ID = id;
            viewModel.Name = tblItem.Name;
            viewModel.Description = tblItem.Description;
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(MachineGroupVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_MachineGroup item = new tbl_MachineGroup()
                        {
                            ID = viewModel.ID,
                            Name = viewModel.Name,
                            Description = viewModel.Description,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };


                        DataOperations dataOperations = new DataOperations();
                        tbl_MachineGroup dbItem = dataOperations.UpdateMachineGroup(item);
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
        [Description("Avadanlığı sil")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeleteMachineGroup(id, UserProfile.UserId);
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