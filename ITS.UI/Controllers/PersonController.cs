using ITS.DAL;
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
    [Description("Şəxslərin icazələri")]
    public class PersonController : Controller
    {
        PersonVM vModel = new PersonVM();
        [LoginCheck]
        [AccessRightsCheck]
        [Description("Şəxslərin siyahısı")]
        public ActionResult Index(int? page, string vl, string prm = null)
        {
            PersonRepository repository = new PersonRepository();
            try
            {
                Search search = new Search();

                search = SetValue(page, vl, prm);

                int pageSize = 15;
                int pageNumber = (page ?? 1);

                PersonVM viewModel = new PersonVM();
                viewModel.Search = search;

                viewModel.Search.pageSize = pageSize;
                viewModel.Search.pageNumber = pageNumber;

                viewModel.RPersonList = repository.SW_GetPersons(viewModel.Search);

                viewModel.ListCount = repository.SW_GetPersonsCount(viewModel.Search);
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
        [Description("Yeni şəxs əlavə etmək")]
        public ActionResult Create()
        {
            vModel = populateDropDownList(vModel);
            return View(vModel);
        }
        [HttpPost]
        public ActionResult Create(PersonVM viewModel)
        {

            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {

                    tbl_Person person = new tbl_Person()
                    {
                        PIN = viewModel.PIN,
                        Name = viewModel.FirstName,
                        Surname = viewModel.LastName,
                        Fathername = viewModel.FatherName,
                        Gender = viewModel.GenderType,
                        Description = viewModel.Description,
                        Address = viewModel.Address,
                        PersonType = viewModel.PersonType,
                        InsertDate = DateTime.Now,
                        InsertUser = UserProfile.UserId

                    };
                    DataOperations operations = new DataOperations();
                    tbl_Person personDB = operations.AddPerson(person);
                    if (personDB != null)
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
                throw new ApplicationException("Invalid model");
            }
            catch (ApplicationException ex)
            {

                viewModel = populateDropDownList(viewModel);
                return View(viewModel);
            }


        }
        [Description("Şəxs redaktə etmək")]
        public ActionResult Edit(int id)
        {
            PersonVM viewModel = new PersonVM();
            DataOperations dataOperations = new DataOperations();
            RegionRepository regionRepository = new RegionRepository();
            tbl_Person tblItem = dataOperations.GetPersonById(id);


            viewModel = populateDropDownList(viewModel);

            viewModel.ID = id;
            viewModel.PIN = tblItem.PIN;
            viewModel.FirstName = tblItem.Name;
            viewModel.LastName = tblItem.Surname;
            viewModel.FatherName = tblItem.Fathername;
            viewModel.GenderType = (int)tblItem.Gender;
            viewModel.PersonType = (int)tblItem.PersonType;
            viewModel.Address = tblItem.Address;
            viewModel.Description = tblItem.Description;

            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(PersonVM viewModel)
        {
            try
            {
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    if (ModelState.IsValid)
                    {

                        tbl_Person person = new tbl_Person()
                        {
                            ID = viewModel.ID,
                            PIN = viewModel.PIN,
                            Name = viewModel.FirstName,
                            Surname = viewModel.LastName,
                            Fathername = viewModel.FatherName,
                            Gender = viewModel.GenderType,
                            PersonType=viewModel.PersonType,
                            Description = viewModel.Description,
                            Address = viewModel.Address,
                            UpdateDate = DateTime.Now,
                            UpdateUser = UserProfile.UserId

                        };

                        DataOperations operations = new DataOperations();
                        tbl_Person personDB = operations.UpdatePerson(person);
                        if (personDB != null)
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
        [Description("Şəxs sil")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataOperations dataOperations = new DataOperations();
                var UserProfile = (UserProfileSessionData)this.Session["UserProfile"];
                if (UserProfile != null)
                {
                    dataOperations.DeletePerson(id, UserProfile.UserId);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Error", "Error"));
            }
        }
        private PersonVM populateDropDownList(PersonVM viewModel)
        {
            viewModel.GenderTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.GenderType);
            viewModel.PersonTypeList = EnumService.GetEnumTypesByParent((int)TypeEnum.PersonType);

            return viewModel;
        }
    }
}