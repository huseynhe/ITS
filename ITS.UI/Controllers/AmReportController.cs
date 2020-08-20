using ITS.DAL.DTO;
using ITS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{
    public class AmReportController : Controller
    {
        ReportRepository repository = new ReportRepository();

        [HttpGet]
        public JsonResult GetMachines()
        {
            long sum = 0;
            List<ReportDTO> modelList = repository.GetMachineReports();
            foreach (var item in modelList)
            {
                sum = sum + (int)item.count;
            }
            return Json(new { data = modelList, sum = sum }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult PieChart3D()
        {
            long sum = 0;
            List<ReportDTO> modelList = repository.GetMachineGroupReports();
            foreach (var item in modelList)
            {
                sum = sum + (int)item.count;
            }
            return Json(new { data = modelList, sum = sum }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult Variableheight3DPieChart()
        {
            long sum = 0;
            List<ReportDTO> modelList = repository.GetMachineReports();
            foreach (var item in modelList)
            {
                sum = sum + (int)item.count;
            }
            return Json(new { data = modelList, sum = sum }, JsonRequestBehavior.AllowGet);

        }
        
    }
}