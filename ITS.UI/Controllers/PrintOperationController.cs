using ITS.DAL.DTO;
using ITS.DAL.Repositories;
using ITS.UI.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{
    public class PrintOperationController : Controller
    {
        #region Print
        public ActionResult Print(int id)
        {

            var report = new Rotativa.ActionAsPdf("PrintCareTracking", new { careTrackingID = id });
            return report;
        }
        public ActionResult PrintCareTracking(int careTrackingID)
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
        #endregion
    }
}