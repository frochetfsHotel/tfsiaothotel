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
    [HotelierHubAuthorize]
    public class CountryController : Controller
    {
        #region Declaration

        private CountryRepository countryRepository = new CountryRepository();        

        #endregion

        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryVM model)
        {
            try
            {
                string countryId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Country Code Available.

                if (this.CheckCountryCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Country Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                countryId = countryRepository.AddCountry(model);

                if (!string.IsNullOrWhiteSpace(countryId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CountryId = countryId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Country details not saved successfully."
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
            var country = countryRepository.GetCountryById(id);

            CountryVM model = new CountryVM();

            if (country != null && country.Count > 0)
            {
                model = country[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountryVM model)
        {
            try
            {
                string countryId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Country Code Available.

                if (this.CheckCountryCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Country Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                countryId = countryRepository.UpdateCountry(model);

                if (!string.IsNullOrWhiteSpace(countryId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CountryId = countryId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Country details not updated successfully."
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
                string countryId = string.Empty;

                countryId = countryRepository.DeleteCountry(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(countryId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CountryId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Country details not deleted successfully."
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
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCountryParametersVM model)
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
                var countries = countryRepository.SearchCountry(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = countries.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = countries
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckCountryCodeAvailable(int? id, string code)
        {
            bool blnAvailable = true;

            var country = countryRepository.CheckCountryCodeAvailable(id, code);

            if (country.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}