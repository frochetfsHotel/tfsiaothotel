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
    public class StateController : Controller
    {
        #region Declaration 

        private CountryRepository countryRepository = new CountryRepository();
        private StateRepository stateRepository = new StateRepository();

        #endregion

        // GET: State
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();

            ViewBag.CountryList = countryList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StateVM model)
        {
            try
            {
                string stateId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                stateId = stateRepository.AddState(model);

                if (!string.IsNullOrWhiteSpace(stateId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            StateId = stateId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "State details not saved successfully."
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
            var state = stateRepository.GetStateById(id);

            if (state != null && state.Count > 0)
            {
                StateVM model = new StateVM();

                model = state[0];

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                ViewBag.CountryList = countryList;

                return View(model);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StateVM model)
        {
            try
            {
                string stateId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                stateId = stateRepository.UpdateState(model);

                if (!string.IsNullOrWhiteSpace(stateId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            StateId = stateId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "State details not updated successfully."
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
                string stateId = string.Empty;

                stateId = stateRepository.DeleteState(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(stateId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            StateId = stateId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "State details not deleted successfully."
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

            ViewBag.CountryList = countryList;            

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchStateParametersVM model)
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
                var states = stateRepository.SearchState(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = states.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = states
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}