using ITS.DAL;
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
   
    public class MachineController : Controller
    {
        // GET: Machine
        [LoginCheck]
        [AccessRightsCheck]
        [Description("Avadanlığların siyahısı")]
        public ActionResult Index(int? page, string vl, string prm = null)
        {
            MachineRepository repository = new MachineRepository();
            try
            {
                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                MachineVM viewModel = new MachineVM();
                viewModel.Search = search;

                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RMachineList = repository.SW_GetMachines(viewModel.Search);

                viewModel.ListCount = repository.SW_GetMachinesCount(viewModel.Search);
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

        [Description("Yeni avadanlığ əlavə etmək")]
        public ActionResult Create()
        {
            MachineVM viewModel = new MachineVM();
            viewModel = populateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(MachineVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_Machine item = new tbl_Machine()
                    {
                        Code = viewModel.Code,
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Quantity = viewModel.Quantity,
                        MachineGroupID = viewModel.MachineGroupID,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };

                    DataOperations dataOperations = new DataOperations();
                    tbl_Machine dbItem = dataOperations.AddMachine(item);
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

                viewModel = populateDropDownList(viewModel);
                return View(viewModel);
            }
            throw new ApplicationException("Invalid model");

        }
        [Description("Avadanlığ redaktə etmək")]
        public ActionResult Edit(int id)
        {
            MachineVM viewModel = new MachineVM();
            DataOperations dataOperations = new DataOperations();

            tbl_Machine tblItem = dataOperations.GetMachineById(id);

            viewModel.ID = id;
            viewModel.Code = tblItem.Code;
            viewModel.Name = tblItem.Name;
            viewModel.Description = tblItem.Description;
            viewModel.Quantity = tblItem.Quantity;
            viewModel.MachineGroupID = tblItem.MachineGroupID==null?0:(int)tblItem.MachineGroupID;
            viewModel = populateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(MachineVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_Machine item = new tbl_Machine()
                        {
                            ID = viewModel.ID,
                            Code = viewModel.Code,
                            Name = viewModel.Name,
                            Description = viewModel.Description,
                            Quantity = viewModel.Quantity,
                            MachineGroupID=viewModel.MachineGroupID,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };


                        DataOperations dataOperations = new DataOperations();
                        tbl_Machine dbItem = dataOperations.UpdateMachine(item);
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
                viewModel = populateDropDownList(viewModel);
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
                    dataOperations.DeleteMachine(id, UserProfile.UserId);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }

        private MachineVM populateDropDownList(MachineVM viewModel)
        {
            //viewModel.GenderTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.GenderType);
            viewModel.MachineGroupList = EnumService.GetMachineGroupList();

            return viewModel;
        }

    }

}