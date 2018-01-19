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
    public class PreferenceGroupController : Controller
    {

        #region Declaration

        private PreferenceGroupRepository preferenceGroupRepository = new PreferenceGroupRepository();

        #endregion

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PreferenceGroupVM model)
        {
            try
            {
                string preferenceGroupId = string.Empty;

                model.CreatedBy = LogInManager.LoggedInUserId;
                preferenceGroupId = preferenceGroupRepository.AddPreferenceGroup(model);

                if (!string.IsNullOrWhiteSpace(preferenceGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceGroupId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference Group not saved successfully."
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
            var preferenceGroup = preferenceGroupRepository.GetPreferenceGroupById(id);

            PreferenceGroupVM model = new PreferenceGroupVM();

            if (preferenceGroup != null && preferenceGroup.Count > 0)
            {
                model = preferenceGroup[0];
                

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PreferenceGroupVM model)
        {
            try
            {
                string preferenceGroupId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;
                preferenceGroupId = preferenceGroupRepository.UpdatePreferenceGroup(model);

                if (!string.IsNullOrWhiteSpace(preferenceGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceGroupId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference Group not updated successfully."
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
                string preferenceGroupId = string.Empty;

                preferenceGroupId = preferenceGroupRepository.DeletePreferenceGroup(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(preferenceGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            PreferenceGroupId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Preference Group not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Search(SearchPreferenceGroupParametersVM model)
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
                var preferenceGroups = preferenceGroupRepository.SearchPreferenceGroup(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = preferenceGroups.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = preferenceGroups
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

    }
}