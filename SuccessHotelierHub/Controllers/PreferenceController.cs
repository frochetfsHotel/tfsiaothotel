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
        #region Declaration

        private PreferenceGroupRepository preferenceGroupRepository = new PreferenceGroupRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();

        #endregion

        [HotelierHubAuthorize(Roles = "ADMIN")]
        // GET: Preference
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpGet]       
        public ActionResult Create()
        {
            var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();

            PreferenceVM model = new PreferenceVM();
            model.IsActive = true;

            ViewBag.PreferenceGroupList = preferenceGroupList;

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                            PreferenceId = preferenceId
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

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Edit(Guid id)
        {
            var preference = preferenceRepository.GetPreferenceById(id);

            PreferenceVM model = new PreferenceVM();

            if (preference != null && preference.Count > 0)
            {
                model = preference[0];

                var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();

                ViewBag.PreferenceGroupList = preferenceGroupList;

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult List()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult DeleteSelected(List<Guid> ids)
        {
            try
            {
                var isDelete = false;

                if(ids != null)
                {
                    foreach (var id in ids)
                    {
                        preferenceRepository.DeletePreferences(id, LogInManager.LoggedInUserId);
                        isDelete = true;
                    }
                }

                if (isDelete)
                {
                    return Json(new
                    {
                        IsSuccess = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preferences not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                    data = preferences
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult SearchAdvancePreference(SearchAdvancePreferenceParametersVM model)
        {
            try
            {
                var preferences = preferenceRepository.SearchAdvancePreference(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = preferences
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}