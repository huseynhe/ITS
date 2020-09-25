using ITS.DAL.DTO;
using ITS.DAL.Objects;
using ITS.DAL.Repositories;
using ITS.UTILITY.Custom;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ITS.UI.Controllers
{
    public class ExcellOperationController : Controller
    {
        // GET: ExcellOperation
        public ActionResult Index()
        {
            return View();
        }

        public void DownloadExcel()
        {
            Search search = new Search();

            //search = SetValue(page, vl, prm);

            //int pageSize = 15;
            //int pageNumber = (page ?? 1);


            CareTrackingRepository careTrackingRepository = new CareTrackingRepository();
            IList<CareTrackingDTO> careTrackingDTOs = careTrackingRepository.SW_GetCareTrackingsForExcel(search);

            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Baxim Izləmə");

            #region Əsas başlıq
            Sheet.Cells[1, 1, 1, 10].Merge = true;
            Sheet.Cells[1, 1].Value = "BAXIM İZLƏMƏ HESABATI";
            Sheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
            Sheet.Cells[1, 1].Style.Font.Bold = true;
            Sheet.Cells[1, 1].Style.Font.Size = 14;
            Sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
            #endregion

            #region Cədvəl başlığı
            Sheet.Cells[2, 1].Value = "Baxım vaxtı";
            Sheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 1].Style.Font.Bold = true;
            Sheet.Cells[2, 1].Style.Font.Size = 12;
            Sheet.Cells[2, 1].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 2].Value = "İş merkezi adı";
            Sheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 2].Style.Font.Bold = true;
            Sheet.Cells[2, 2].Style.Font.Size = 12;
            Sheet.Cells[2, 2].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 3].Value = "Makina grubu adı";
            Sheet.Cells[2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 3].Style.Font.Bold = true;
            Sheet.Cells[2, 3].Style.Font.Size = 12;
            Sheet.Cells[2, 3].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 4].Value = "Makina adı";
            Sheet.Cells[2, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 4].Style.Font.Bold = true;
            Sheet.Cells[2, 4].Style.Font.Size = 12;
            Sheet.Cells[2, 4].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 5].Value = "Təmir və ya baxım işinin təsviri";
            Sheet.Cells[2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 5].Style.Font.Bold = true;
            Sheet.Cells[2, 5].Style.Font.Size = 12;
            Sheet.Cells[2, 5].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 6].Value = "Baxım növü";
            Sheet.Cells[2, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 6].Style.Font.Bold = true;
            Sheet.Cells[2, 6].Style.Font.Size = 12;
            Sheet.Cells[2, 6].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 7].Value = "Planlı baxım növü";
            Sheet.Cells[2, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 7].Style.Font.Bold = true;
            Sheet.Cells[2, 7].Style.Font.Size = 12;
            Sheet.Cells[2, 7].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 8].Value = "Baxım komandası";
            Sheet.Cells[2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 8].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 8].Style.Font.Bold = true;
            Sheet.Cells[2, 8].Style.Font.Size = 12;
            Sheet.Cells[2, 8].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 9].Value = "Başlama tarixi";
            Sheet.Cells[2, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 9].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 9].Style.Font.Bold = true;
            Sheet.Cells[2, 9].Style.Font.Size = 12;
            Sheet.Cells[2, 9].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 10].Value = "Başlama zamanı";
            Sheet.Cells[2, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 10].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 10].Style.Font.Bold = true;
            Sheet.Cells[2, 10].Style.Font.Size = 12;
            Sheet.Cells[2, 10].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 11].Value = "Bitiş tarixi";
            Sheet.Cells[2, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 11].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 11].Style.Font.Bold = true;
            Sheet.Cells[2, 11].Style.Font.Size = 12;
            Sheet.Cells[2, 11].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 12].Value = "Bitiş zamanı";
            Sheet.Cells[2, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 12].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 12].Style.Font.Bold = true;
            Sheet.Cells[2, 12].Style.Font.Size = 12;
            Sheet.Cells[2, 12].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 13].Value = "Keçən zaman";
            Sheet.Cells[2, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 13].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 13].Style.Font.Bold = true;
            Sheet.Cells[2, 13].Style.Font.Size = 12;
            Sheet.Cells[2, 13].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 14].Value = "Aciqlama";
            Sheet.Cells[2, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 14].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 14].Style.Font.Bold = true;
            Sheet.Cells[2, 14].Style.Font.Size = 12;
            Sheet.Cells[2, 14].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 15].Value = "Mexanik(Soyad Ad Ata adı)";
            Sheet.Cells[2, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 15].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 15].Style.Font.Bold = true;
            Sheet.Cells[2, 15].Style.Font.Size = 12;
            Sheet.Cells[2, 15].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 16].Value = "Təhvil alan şexs (Soyad Ad Ata adı)";
            Sheet.Cells[2, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 16].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 16].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 16].Style.Font.Bold = true;
            Sheet.Cells[2, 16].Style.Font.Size = 12;
            Sheet.Cells[2, 16].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 17].Value = "Nəticə";
            Sheet.Cells[2, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 17].Style.Font.Bold = true;
            Sheet.Cells[2, 17].Style.Font.Size = 12;
            Sheet.Cells[2, 17].Style.Font.Color.SetColor(Color.Black);

            Sheet.Cells[2, 18].Value = "Nəticə təsviri";
            Sheet.Cells[2, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            Sheet.Cells[2, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            Sheet.Cells[2, 18].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            Sheet.Cells[2, 18].Style.Font.Bold = true;
            Sheet.Cells[2, 18].Style.Font.Size = 12;
            Sheet.Cells[2, 18].Style.Font.Color.SetColor(Color.Black);
            #endregion

            int rowcounter = 2;
            foreach (CareTrackingDTO careTracking in careTrackingDTOs)
            {
                rowcounter++;

                bool flagResultType = false;
                if (careTracking.careTrackingDetailDTO!=null && careTracking.careTrackingDetailDTO.ResultType==5)
                {
                    flagResultType = true;
                }

                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 1].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }            
                Sheet.Cells[rowcounter, 1].Value = careTracking.CareDate.ToShortDateString();
                Sheet.Cells[rowcounter, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                Sheet.Cells[rowcounter, 1].Style.Font.Size = 12;


                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 2].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 2].Value = careTracking.BusinessCenterName;
                Sheet.Cells[rowcounter, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                Sheet.Cells[rowcounter, 2].Style.Font.Size = 12;


                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 3].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 3].Value = careTracking.MachineGroupName;
                Sheet.Cells[rowcounter, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                Sheet.Cells[rowcounter, 3].Style.Font.Size = 12;

                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 4].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 4].Value = careTracking.MachineName;
                Sheet.Cells[rowcounter, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                Sheet.Cells[rowcounter, 4].Style.Font.Size = 12;


                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 5].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 5].Value = careTracking.CareDescription;
                Sheet.Cells[rowcounter, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet.Cells[rowcounter, 5].Style.Font.Size = 12;


                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 6].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 6].Value = careTracking.CareTypeDesc;
                Sheet.Cells[rowcounter, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                Sheet.Cells[rowcounter, 6].Style.Font.Size = 12;

                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 7].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 7].Value = careTracking.PlanedCareTypeDesc;
                Sheet.Cells[rowcounter, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet.Cells[rowcounter, 7].Style.Font.Size = 12;

                if (flagResultType)
                {
                    Sheet.Cells[rowcounter, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Sheet.Cells[rowcounter, 8].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                }
                Sheet.Cells[rowcounter, 8].Value = careTracking.CareTeamTypeDesc;
                Sheet.Cells[rowcounter, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                Sheet.Cells[rowcounter, 8].Style.Font.Size = 12;

                if (careTracking.careTrackingDetailDTO!=null)
                {
                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 9].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 9].Value = careTracking.careTrackingDetailDTO.StartDate==null?null:((DateTime)careTracking.careTrackingDetailDTO.StartDate).ToShortDateString();
                    Sheet.Cells[rowcounter, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 9].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 10].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 10].Value = careTracking.careTrackingDetailDTO.StartTimeDesc;
                    Sheet.Cells[rowcounter, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet.Cells[rowcounter, 10].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 11].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 11].Value = careTracking.careTrackingDetailDTO.EndDate == null ? null : ((DateTime)careTracking.careTrackingDetailDTO.EndDate).ToShortDateString();
                    Sheet.Cells[rowcounter, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 11].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 12].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 12].Value = careTracking.careTrackingDetailDTO.EndTimeDesc;
                    Sheet.Cells[rowcounter, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet.Cells[rowcounter, 12].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 13].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 13].Value = careTracking.careTrackingDetailDTO.DurationTime;
                    Sheet.Cells[rowcounter, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 13].Style.Font.Size = 12;
                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 14].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 14].Value = careTracking.careTrackingDetailDTO.Description;
                    Sheet.Cells[rowcounter, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 14].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 15].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 15].Value = careTracking.careTrackingDetailDTO.MechanicSAA;
                    Sheet.Cells[rowcounter, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 15].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 16].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 16].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 16].Value = careTracking.careTrackingDetailDTO.ReceivingPersonSAA;
                    Sheet.Cells[rowcounter, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 16].Style.Font.Size = 12;

                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 17].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 17].Value = careTracking.careTrackingDetailDTO.ResultTypeDesc;
                    Sheet.Cells[rowcounter, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    Sheet.Cells[rowcounter, 17].Style.Font.Size = 12;
                   
                    if (flagResultType)
                    {
                        Sheet.Cells[rowcounter, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        Sheet.Cells[rowcounter, 18].Style.Fill.BackgroundColor.SetColor(Color.Coral);
                    }
                    Sheet.Cells[rowcounter, 18].Value = careTracking.careTrackingDetailDTO.ResultDescription;
                    Sheet.Cells[rowcounter, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Sheet.Cells[rowcounter, 18].Style.Font.Size = 12;
                }
       

            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            for (int k = 1; k <= rowcounter; k++)
            {
                Sheet.Row(k).Height = 23;
                Sheet.Row(k).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
            using (ExcelRange range = Sheet.Cells[1, 1, rowcounter, 18])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(Color.Gray);
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(Color.Gray);
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(Color.Gray);
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.Gray);
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Baxim_Izleme_Hesabati_" + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".xlsx;");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Ep.Dispose();
            Response.End();

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
    }
}