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
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class RTCController : Controller
    {
        #region Declaration

        private RTCRepository rtcRepository = new RTCRepository();

        #endregion

        // GET: RTC
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
        public ActionResult Create(RtcVM model)
        {
            try
            {
                string rtcId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                rtcId = rtcRepository.AddRTC(model);

                if (!string.IsNullOrWhiteSpace(rtcId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RtcId = rtcId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate to Charge details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Create");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        public ActionResult Edit(Guid id)
        {
            var rtc = rtcRepository.GetRTCById(id);

            RtcVM model = new RtcVM();

            if (rtc != null && rtc.Count > 0)
            {
                model = rtc[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RtcVM model)
        {
            try
            {
                string rtcId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                rtcId = rtcRepository.UpdateRTC(model);

                if (!string.IsNullOrWhiteSpace(rtcId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RtcId = rtcId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate to Charge details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Edit");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string rtcId = string.Empty;

                rtcId = rtcRepository.DeleteRTC(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(rtcId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RtcId = rtcId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate to Charge details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchRTCParametersVM model)
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
                var rtcList = rtcRepository.SearchRTC(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = rtcList.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = rtcList
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}