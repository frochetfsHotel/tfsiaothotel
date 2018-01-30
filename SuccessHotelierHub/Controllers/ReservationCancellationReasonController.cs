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
    public class ReservationCancellationReasonController : Controller
    {
        #region Declaration

        private ReservationCancellationReasonRepository reservationCancellationReasonRepository = new ReservationCancellationReasonRepository();

        #endregion

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            ReservationCancellationReasonVM model = new ReservationCancellationReasonVM();
            model.IsActive = true;

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationCancellationReasonVM model)
        {
            try
            {
                string reasonId = string.Empty;
                
                #region Check Cancellation Code Available.

                if (this.CheckReservationCancellationCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Cancellation Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                model.CreatedBy = LogInManager.LoggedInUserId;
                reasonId = reservationCancellationReasonRepository.AddReservationCancellationReason(model);

                if (!string.IsNullOrWhiteSpace(reasonId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReasonId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Cancellation Reason not saved successfully."
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
            var reason = reservationCancellationReasonRepository.GetReservationCancellationReasonById(id);

            ReservationCancellationReasonVM model = new ReservationCancellationReasonVM();

            if (reason != null && reason.Count > 0)
            {
                model = reason[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationCancellationReasonVM model)
        {
            try
            {
                string reasonId = string.Empty;

                #region Check Cancellation Code Available.

                if (this.CheckReservationCancellationCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Cancellation Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                model.UpdatedBy = LogInManager.LoggedInUserId;
                reasonId = reservationCancellationReasonRepository.UpdateReservationCancellationReason(model);

                if (!string.IsNullOrWhiteSpace(reasonId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReasonId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Cancellation Reason not updated successfully."
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
                string reasonId = string.Empty;

                reasonId = reservationCancellationReasonRepository.DeleteReservationCancellationReason(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reasonId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReasonId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Cancellation Reason not deleted successfully."
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

                if (ids != null)
                {
                    foreach (var id in ids)
                    {
                        reservationCancellationReasonRepository.DeleteReservationCancellationReason(id, LogInManager.LoggedInUserId);
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
                        errorMessage = "Cancellation Reasons not deleted successfully."
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
        public ActionResult Search(SearchReservationCancellationReasonParametersVM model)
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
                var reasons = reservationCancellationReasonRepository.SearchReservationCancellationReason(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));
                

                int totalRecords = 0;
                var dbRecords = reasons.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reasons
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        [HttpPost]
        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT,TUTOR")]
        public ActionResult SearchAdvanceReservationCancellationReasons(SearchAdvanceReservationCancellationReasonParametersVM model)
        {
            try
            {
                var reasons = reservationCancellationReasonRepository.SearchAdvanceReservationCancellationReasons(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = reasons
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckReservationCancellationCodeAvailable(Guid? id, string code)
        {
            bool blnAvailable = true;

            var reason = reservationCancellationReasonRepository.CheckReservationCancellationCodeAvailable(id, code);

            if (reason.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}