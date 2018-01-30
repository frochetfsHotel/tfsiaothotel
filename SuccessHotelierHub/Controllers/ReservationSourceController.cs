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
    public class ReservationSourceController : Controller
    {
        #region Declaration

        private ReservationSourceRepository reservationSourceRepository = new ReservationSourceRepository();

        #endregion

        // GET: ReservationSource
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
        public ActionResult Create(ReservationSourceVM model)
        {
            try
            {
                string reservationSourceId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                reservationSourceId = reservationSourceRepository.AddReservationSource(model);

                if (!string.IsNullOrWhiteSpace(reservationSourceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationSourceId = reservationSourceId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Source details not saved successfully."
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

        public ActionResult Edit(Guid id)
        {
            var reservationSource = reservationSourceRepository.GetReservationSourceById(id);

            ReservationSourceVM model = new ReservationSourceVM();

            if (reservationSource != null && reservationSource.Count > 0)
            {
                model = reservationSource[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationSourceVM model)
        {
            try
            {
                string reservationSourceId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                reservationSourceId = reservationSourceRepository.UpdateReservationSource(model);

                if (!string.IsNullOrWhiteSpace(reservationSourceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationSourceId = reservationSourceId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Source details not updated successfully."
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
        public ActionResult Delete(Guid id)
        {
            try
            {
                string reservationSourceId = string.Empty;

                reservationSourceId = reservationSourceRepository.DeleteReservationSource(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reservationSourceId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationSourceId = reservationSourceId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Source details not deleted successfully."
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
        public ActionResult Search(SearchReservationSourceParametersVM model)
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
                var reservationSources = reservationSourceRepository.SearchReservationSource(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservationSources.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservationSources
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}