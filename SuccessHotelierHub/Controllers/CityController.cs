using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;

namespace SuccessHotelierHub.Controllers
{
    public class CityController : Controller
    {
        #region Declaration 

        private CountryRepository countryRepository = new CountryRepository();
        private StateRepository stateRepository = new StateRepository();
        private CityRepository cityRepository = new CityRepository();

        #endregion

        // GET: City
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            //Get Country
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();

            //Get State
            var stateList = new SelectList(new List<StateVM>(), "Id", "Name").ToList();

            ViewBag.CountryList = countryList;
            ViewBag.StateList = stateList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CityVM model)
        {
            try
            {
                string cityId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                cityId = cityRepository.AddCity(model);

                if (!string.IsNullOrWhiteSpace(cityId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CityId = cityId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "City details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        public ActionResult Edit(int id)
        {
            var city = cityRepository.GetCityById(id);
            
            if (city != null && city.Count > 0)
            {
                CityVM model = new CityVM();
                model = city[0];

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                ViewBag.CountryList = countryList;

                //Get State
                var stateList = new SelectList(new List<StateVM>(), "Id", "Name").ToList();

                if (model.CountryId.HasValue)
                    stateList = new SelectList(stateRepository.GetStates(model.CountryId), "Id", "Name").ToList();

                ViewBag.StateList = stateList;

                return View(model);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CityVM model)
        {
            try
            {
                string cityId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                cityId = cityRepository.UpdateCity(model);

                if (!string.IsNullOrWhiteSpace(cityId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CityId = cityId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "City details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string cityId = string.Empty;

                cityId = cityRepository.DeleteCity(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(cityId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CityId = cityId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "City details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            //Get Country
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();

            //Get State
            var stateList = new SelectList(new List<StateVM>(), "Id", "Name").ToList();

            ViewBag.CountryList = countryList;
            ViewBag.StateList = stateList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCityParametersVM model)
        {
            try
            {
                object sortColumn = "";
                object sortDirection = "";

                if (model.order.Count == 0)
                {
                    sortColumn = "CreatedOn";
                    sortDirection = "desc";
                }
                else
                {
                    sortColumn = model.columns[Convert.ToInt32(model.order[0].column)].data ?? (object)DBNull.Value;
                    sortDirection = model.order[0].dir ?? (object)DBNull.Value;
                }

                model.PageSize = Constants.PAGESIZE;

                var cities = cityRepository.SearchCity(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = cities.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = cities
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}