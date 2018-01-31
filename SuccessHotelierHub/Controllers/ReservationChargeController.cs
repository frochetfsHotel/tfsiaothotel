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
    [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
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

                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.BILLING_TRANSACTION, "Added new biling transaction.");
                    #endregion

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

        [HttpPost]
        public ActionResult AddCharges(List<ReservationChargeVM> models)
        {
            try
            {
                bool blnIsChargesInserted = false;
                double totalBalance = 0;

                var transactions = new List<ReservationChargeVM>();

                if (models != null)
                {
                    Guid? reservationId = models[0].ReservationId;
                    double totalAmount = 0;

                    foreach (var model in models)
                    {
                        string chargeId = string.Empty;

                        model.CreatedBy = LogInManager.LoggedInUserId;
                        chargeId = reservationChargeRepository.AddReservationCharges(model);

                        if (!string.IsNullOrWhiteSpace(chargeId))
                        {
                            //Calculate total Amount.
                            totalAmount = totalAmount + (model.Amount.HasValue ? model.Amount.Value : 0);

                            model.Id = Guid.Parse(chargeId);

                            transactions.Add(model);

                            blnIsChargesInserted = true;
                        }
                    }

                    #region Update Reservation Total Balance.
                    var reservation = new ReservationVM();

                    if (reservationId.HasValue)
                        reservation = reservationRepository.GetReservationById(reservationId.Value).FirstOrDefault();

                    if (reservation != null)
                    {
                        totalBalance = reservation.TotalBalance.HasValue ? reservation.TotalBalance.Value : 0;

                        totalBalance += totalAmount;

                        //Update Total Balance.
                        reservationRepository.UpdateReservationTotalBalance(reservation.Id, totalBalance, LogInManager.LoggedInUserId);
                    }
                    #endregion
                }

                if (blnIsChargesInserted)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            Transactions = transactions,
                            TotalBalance = totalBalance
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Charges not saved successfully."
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