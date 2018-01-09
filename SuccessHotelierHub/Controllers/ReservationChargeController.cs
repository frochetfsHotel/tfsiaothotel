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
    public class ReservationChargeController : Controller
    {
        #region Declaration

        private ReservationRepository reservationRepository = new ReservationRepository();
        private ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();

        #endregion

        // GET: ReservationCharge
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowAddReservationCharge()
        {
            return PartialView("_AddReservationCharges");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationChargeVM model)
        {
            try
            {
                string chargeId = string.Empty;

                model.CreatedBy = LogInManager.LoggedInUserId;
                chargeId = reservationChargeRepository.AddReservationCharges(model);
                
                if (!string.IsNullOrWhiteSpace(chargeId))
                {
                    model.Id = Guid.Parse(chargeId);

                    #region Update Reservation Total Balance.
                    var reservation = new ReservationVM();

                    if(model.ReservationId.HasValue)
                        reservation = reservationRepository.GetReservationById(model.ReservationId.Value).FirstOrDefault();

                    double totalBalance = 0;
                    if (reservation != null)
                    {
                        totalBalance = reservation.TotalBalance.HasValue ? reservation.TotalBalance.Value : 0;

                        totalBalance += model.Amount.HasValue ? model.Amount.Value : 0;

                        //Update Total Balance.
                        reservationRepository.UpdateReservationTotalBalance(reservation.Id, totalBalance, LogInManager.LoggedInUserId);
                    }
                    #endregion
                    

                    var transaction = new List<ReservationChargeVM>();
                    transaction.Add(model);

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ChargeId = chargeId,
                            Transaction = transaction,
                            TotalBalance = totalBalance
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Charge not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

    }
}