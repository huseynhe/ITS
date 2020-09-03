using ITS.DAL;
using ITS.DAL.DTO;
using ITS.DAL.Model;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UI.Attributes;
using ITS.UI.Enums;
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
    [Description("Baxım izləmə icazələri")]
    public class CareTrackingController : Controller
    {
        // GET: CareTracking
        [LoginCheck]
        [AccessRightsCheck]
        [Description("Baxım izləmə siyahısı")]
        public ActionResult Index(int? page, string vl, string prm = null)
        {
            CareTrackingRepository repository = new CareTrackingRepository();
            try
            {
                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                CareTrackingVM viewModel = new CareTrackingVM();
                viewModel.Search = search;

                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RCareTrackingList = repository.SW_GetCareTrackings(viewModel.Search);

                viewModel.ListCount = repository.SW_GetCareTrackingsCount(viewModel.Search);
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
        [Description("Yeni baxım izləmə əlavə etmək")]
        public ActionResult Create()
        {
            CareTrackingVM viewModel = new CareTrackingVM();
            viewModel = populateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(CareTrackingVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_CareTracking item = new tbl_CareTracking()
                    {
                        CareDate = viewModel.CareDate,
                        BusinessCenterID = viewModel.BusinessCenterID,
                        MachineGroupID = viewModel.MachineGroupID,
                        MachineID = viewModel.MachineID,
                        CareDescription = viewModel.CareDescription,
                        CareType = viewModel.CareType,
                        PlanedCareType = viewModel.PlanedCareType,
                        CareTeamType = viewModel.CareTeamType,
                        ResultType = viewModel.ResultType,
                        ResultDescription=viewModel.ResultDescription,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };

                    DataOperations dataOperations = new DataOperations();
                    tbl_CareTracking dbItem = dataOperations.AddCareTracking(item);
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
        [Description("Baxım izləmə redaktə etmək")]
        public ActionResult Edit(int id)
        {
            CareTrackingVM viewModel = new CareTrackingVM();
            DataOperations dataOperations = new DataOperations();

            tbl_CareTracking tblItem = dataOperations.GetCareTrackingById(id);

            viewModel.ID = tblItem.ID;
            viewModel.CareDate = tblItem.CareDate;
            viewModel.BusinessCenterID = tblItem.BusinessCenterID==null?0:(int)tblItem.BusinessCenterID;
            viewModel.MachineGroupID = tblItem.MachineGroupID == null ? 0 : (int)tblItem.MachineGroupID;
            viewModel.MachineID = tblItem.MachineID == null ? 0 : (int)tblItem.MachineID;
            viewModel.CareDescription = tblItem.CareDescription;
            viewModel.CareType = tblItem.CareType == null ? 0 : (int)tblItem.CareType;
            viewModel.PlanedCareType = tblItem.PlanedCareType == null ? 0 : (int)tblItem.PlanedCareType;
            viewModel.CareTeamType = tblItem.CareTeamType == null ? 0 : (int)tblItem.CareTeamType;
            viewModel.ResultType = tblItem.ResultType == null ? 0 : (int)tblItem.ResultType;
            viewModel.ResultDescription = tblItem.ResultDescription;
   
            viewModel = populateDropDownList(viewModel);
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(CareTrackingVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_CareTracking item = new tbl_CareTracking()
                        {
                            ID = viewModel.ID,
                            CareDate = viewModel.CareDate,
                            BusinessCenterID = viewModel.BusinessCenterID,
                            MachineGroupID = viewModel.MachineGroupID,
                            MachineID = viewModel.MachineID,
                            CareDescription = viewModel.CareDescription,
                            CareType = viewModel.CareType,
                            PlanedCareType = viewModel.PlanedCareType,
                            CareTeamType = viewModel.CareTeamType,
                            ResultType = viewModel.ResultType,
                            ResultDescription = viewModel.ResultDescription,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };


                        DataOperations dataOperations = new DataOperations();
                        tbl_CareTracking dbItem = dataOperations.UpdateCareTracking(item);
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
        [Description("Baxım izləmə sil")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeleteCareTracking(id, UserProfile.UserId);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }
        public ActionResult Detail(int careTrackingID)
        {
            CareTrackingVM viewModel = new CareTrackingVM();
            CareTrackingRepository repository = new CareTrackingRepository();

            CareTrackingDTO careTrackingDTO = repository.GetCareTrackingByID(careTrackingID);



            viewModel.ID = careTrackingDTO.CareTrackingID;
            viewModel.CareDate = careTrackingDTO.CareDate;
            viewModel.BusinessCenterID = careTrackingDTO.BusinessCenterID;
            viewModel.BusinessCenterDesc = careTrackingDTO.BusinessCenterName;
            viewModel.MachineGroupID = careTrackingDTO.MachineGroupID;
            viewModel.MachineGroupDesc = careTrackingDTO.MachineGroupName;
            viewModel.MachineID = careTrackingDTO.MachineID;
            viewModel.MachineDesc = careTrackingDTO.MachineName;
            viewModel.CareDescription = careTrackingDTO.CareDescription;
            viewModel.CareType = careTrackingDTO.CareType;
            viewModel.CareTypeDesc = careTrackingDTO.CareTypeDesc;
            viewModel.PlanedCareType = careTrackingDTO.PlanedCareType;
            viewModel.PlanedCareTypeDesc = careTrackingDTO.PlanedCareTypeDesc;
            viewModel.CareTeamType = careTrackingDTO.CareTeamType;
            viewModel.CareTeamTypeDesc = careTrackingDTO.CareTeamTypeDesc;
            viewModel.ResultTypeDesc = careTrackingDTO.ResultTypeDesc;
            viewModel.ResultDescription = careTrackingDTO.ResultDescription;

            viewModel.RCareTrackingDetailList = repository.GetCareTrackingDetailsByCTID(careTrackingDTO.CareTrackingID);

            
            return View(viewModel);
        }
        private CareTrackingVM populateDropDownList(CareTrackingVM viewModel)
        {
            viewModel.BusinessCenterList = EnumService.GetBusinessCenterList();
            viewModel.MachineGroupList = EnumService.GetMachineGroupList();
            viewModel.MachineList = EnumService.GetMachineList();
            viewModel.CareTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.CareType);
            viewModel.PlanedCareTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.PlanedCareType);
            viewModel.CareTeamTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.CareTeamType);
            viewModel.ResultTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.ResultType);
            return viewModel;
        }

        #region CareTrackingDetail  
        public ActionResult CareTrackingDetailIndex(int careTrackingID)
        {
            CareTrackingRepository repository = new CareTrackingRepository();
            List<CareTrackingDetailDTO> careTrackingDetailDTOs = repository.GetCareTrackingDetailsByCTID(careTrackingID);
            CareTrackingVM viewModel = new CareTrackingVM();
            viewModel.ID = careTrackingID;
            viewModel.RCareTrackingDetailList = careTrackingDetailDTOs;
            return View(viewModel);
        }
        public ActionResult CreateCareTrackingDetail(int careTrackingID)
        {
            CareTrackingVM viewModel = new CareTrackingVM();
            viewModel.ID = careTrackingID;
            viewModel = populateCTDDropDownList(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CreateCareTrackingDetail(CareTrackingVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_CareTrackingDetail item = new tbl_CareTrackingDetail()
                    {
                        CareTrackingID = viewModel.ID,
                        StartDate = viewModel.StartDate,
                        StartTime = viewModel.StartTime,
                        EndDate = viewModel.EndDate,
                        EndTime = viewModel.EndTime,
                        Description = viewModel.Description,
                        MechanicID = viewModel.MechanicID,
                        ReceivingPersonID = viewModel.ReceivingPersonID,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };

                    DataOperations operations = new DataOperations();
                    tbl_CareTrackingDetail itemDB = operations.AddCareTrackingDetail(item);
                    if (itemDB != null)
                    {
                        TempData["success"] = "Ok";
                        TempData["message"] = "Məlumatlar uğurla əlavə olundu";
                        return RedirectToAction("CareTrackingDetailIndex", new { careTrackingID = itemDB.CareTrackingID });

                    }
                    else
                    {
                        TempData["success"] = "notOk";
                        TempData["message"] = "Məlumatlar əlavə olunarkən xəta baş verdi";
                        return RedirectToAction("CareTrackingDetailIndex", new { careTrackingID = itemDB.CareTrackingID });

                    }


                }
                throw new ApplicationException("Invalid model");
            }
            catch (ApplicationException ex)
            {

                return View(viewModel);
            }
        }

        public ActionResult EditCareTrackingDetail(Int64 ctDetailId)
        {
            CareTrackingVM viewModel = new CareTrackingVM();
            DataOperations operations = new DataOperations();
            tbl_CareTrackingDetail careTrackingDetail = operations.GetCareTrackingDetailById(ctDetailId);
            viewModel.ID = careTrackingDetail.CareTrackingID;
            viewModel.CareTrackingDetailID = careTrackingDetail.ID;
            viewModel.StartDate = careTrackingDetail.StartDate;
            viewModel.StartTime = careTrackingDetail.StartTime;
            viewModel.EndDate = careTrackingDetail.EndDate;
            viewModel.EndTime = careTrackingDetail.EndTime;
            viewModel.Description = careTrackingDetail.Description;
            viewModel.MechanicID = careTrackingDetail.MechanicID==null?0:(int)careTrackingDetail.MechanicID;
            viewModel.ReceivingPersonID = careTrackingDetail.ReceivingPersonID==null?0 :(int) careTrackingDetail.ReceivingPersonID;
            viewModel = populateCTDDropDownList(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult EditCareTrackingDetail(CareTrackingVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    tbl_CareTrackingDetail item = new tbl_CareTrackingDetail()
                    {
                        ID=viewModel.CareTrackingDetailID,
                        CareTrackingID = viewModel.ID,
                        StartDate = viewModel.StartDate,
                        StartTime = viewModel.StartTime,
                        EndDate = viewModel.EndDate,
                        EndTime = viewModel.EndTime,
                        Description = viewModel.Description,
                        MechanicID = viewModel.MechanicID,
                        ReceivingPersonID = viewModel.ReceivingPersonID,
                        UpdateDate = DateTime.Now,
                        UpdateUser = UserProfile.UserId

                    };

                    DataOperations operations = new DataOperations();
                    tbl_CareTrackingDetail itemDB = operations.UpdateCareTrackingDetail(item);
                    if (itemDB != null)
                    {
                        TempData["success"] = "Ok";
                        TempData["message"] = "Məlumatlar uğurla redakte olundu";
                        return RedirectToAction("CareTrackingDetailIndex", new { careTrackingID = itemDB.CareTrackingID });

                    }
                    else
                    {
                        TempData["success"] = "notOk";
                        TempData["message"] = "Məlumatlar redakte olunarkən xəta baş verdi";
                        return RedirectToAction("CareTrackingDetailIndex", new { careTrackingID = itemDB.CareTrackingID });
                    }


                }
                throw new ApplicationException("Invalid model");
            }
            catch (ApplicationException ex)
            {
                viewModel = populateCTDDropDownList(viewModel);
                return View(viewModel);
            }

        }
        public ActionResult DeleteCareTrackingDetail(int ctDetailId, int ctId)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeleteCareTrackingDetail(ctDetailId, UserProfile.UserId);
                }
                return RedirectToAction("CareTrackingDetailIndex", new { careTrackingID = ctId });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }
        private CareTrackingVM populateCTDDropDownList(CareTrackingVM viewModel)
        {
            viewModel.MechanicList = EnumService.GetPersonTypeList((int)PersonType.Mecanic);
            viewModel.ReceivingPersonList = EnumService.GetPersonTypeList((int)PersonType.ReceivingPerson);
            return viewModel;
        }
        #endregion
        #region Print
        public ActionResult Print(int id)
        {

            var report = new Rotativa.ActionAsPdf("PrintCareTracking", new { careTrackingID = id });
            return report;
        }
        public ActionResult PrintCareTracking(int careTrackingID) {

            CareTrackingVM viewModel = new CareTrackingVM();
            CareTrackingRepository repository = new CareTrackingRepository();

            CareTrackingDTO careTrackingDTO = repository.GetCareTrackingByID(careTrackingID);



            viewModel.ID = careTrackingDTO.CareTrackingID;
            viewModel.CareDate = careTrackingDTO.CareDate;
            viewModel.BusinessCenterID = careTrackingDTO.BusinessCenterID;
            viewModel.BusinessCenterDesc = careTrackingDTO.BusinessCenterName;
            viewModel.MachineGroupID = careTrackingDTO.MachineGroupID;
            viewModel.MachineGroupDesc = careTrackingDTO.MachineGroupName;
            viewModel.MachineID = careTrackingDTO.MachineID;
            viewModel.MachineDesc = careTrackingDTO.MachineName;
            viewModel.CareDescription = careTrackingDTO.CareDescription;
            viewModel.CareType = careTrackingDTO.CareType;
            viewModel.CareTypeDesc = careTrackingDTO.CareTypeDesc;
            viewModel.PlanedCareType = careTrackingDTO.PlanedCareType;
            viewModel.PlanedCareTypeDesc = careTrackingDTO.PlanedCareTypeDesc;
            viewModel.CareTeamType = careTrackingDTO.CareTeamType;
            viewModel.CareTeamTypeDesc = careTrackingDTO.CareTeamTypeDesc;
            viewModel.ResultTypeDesc = careTrackingDTO.ResultTypeDesc;
            viewModel.ResultDescription = careTrackingDTO.ResultDescription;

            viewModel.RCareTrackingDetailList = repository.GetCareTrackingDetailsByCTID(careTrackingDTO.CareTrackingID);


            return View(viewModel);
        }
        #endregion
    }
}