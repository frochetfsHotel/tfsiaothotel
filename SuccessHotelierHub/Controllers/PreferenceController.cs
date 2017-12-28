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
    public class PreferenceController : Controller
    {
        private PreferenceRepository preferenceRepository = new PreferenceRepository();

        // GET: Preference
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            PreferenceVM model = new PreferenceVM();
            model.IsActive = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PreferenceVM model)
        {
            try
            {
                string preferenceId = string.Empty;
                
                model.CreatedBy = LogInManager.LoggedInUserId;
                preferenceId = preferenceRepository.AddPreferences(model);

                if (!string.IsNullOrWhiteSpace(preferenceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult Edit(Guid id)
        {
            var preference = preferenceRepository.GetPreferenceById(id);

            PreferenceVM model = new PreferenceVM();

            if (preference != null && preference.Count > 0)
            {
                model = preference[0];

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PreferenceVM model)
        {
            try
            {
                string preferenceId = string.Empty;
                
                model.UpdatedBy = LogInManager.LoggedInUserId;
                preferenceId = preferenceRepository.UpdatePreferences(model);

                if (!string.IsNullOrWhiteSpace(preferenceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference not updated successfully."
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
        public ActionResult Delete(Guid id)
        {
            try
            {
                string preferenceId = string.Empty;

                preferenceId = preferenceRepository.DeletePreferences(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(preferenceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Search(SearchPreferenceParametersVM model)
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
                var preferences = preferenceRepository.SearchPreferences(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                //var preferenceList = preferences.Take(Constants.PAGESIZE).Select(m => new PreferenceVM()
                var preferenceList = preferences.Select(m => new PreferenceVM()
                {
                    Id = m.Id,
                    Code = m.Code,
                    Description = m.Description,
                    IsActive = m.IsActive,
                    CreatedOn = m.CreatedOn
                }).ToList();

                int totalRecords = 0;
                var dbRecords = preferences.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = preferenceList
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}