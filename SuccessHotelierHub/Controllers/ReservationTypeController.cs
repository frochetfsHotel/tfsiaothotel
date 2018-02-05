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
    public class ReservationTypeController : Controller
    {
        #region Declaration

        private ReservationTypeRepository reservationTypeRepository = new ReservationTypeRepository();

        #endregion

        // GET: ReservationType
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
        public ActionResult Create(ReservationTypeVM model)
        {
            try
            {
                string reservationTypeId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                reservationTypeId = reservationTypeRepository.AddReservationType(model);

                if (!string.IsNullOrWhiteSpace(reservationTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationTypeId = reservationTypeId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Type details not saved successfully."
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
            var reservationType = reservationTypeRepository.GetReservationTypeById(id);

            ReservationTypeVM model = new ReservationTypeVM();

            if (reservationType != null && reservationType.Count > 0)
            {
                model = reservationType[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationTypeVM model)
        {
            try
            {
                string reservationTypeId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                reservationTypeId = reservationTypeRepository.UpdateReservationType(model);

                if (!string.IsNullOrWhiteSpace(reservationTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationTypeId = reservationTypeId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Type details not updated successfully."
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
                string reservationTypeId = string.Empty;

                reservationTypeId = reservationTypeRepository.DeleteReservationType(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reservationTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationTypeId = reservationTypeId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation Type details not deleted successfully."
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
        public ActionResult Search(SearchReservationTypeParametersVM model)
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
                var reservationTypes = reservationTypeRepository.SearchReservationType(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservationTypes.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservationTypes
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