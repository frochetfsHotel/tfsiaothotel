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
                model.AdditionalChargeSource = AdditionalChargeSource.ADDITIONAL_CHARGE;
                chargeId = reservationChargeRepository.AddReservationCharges(model);

                if (!string.IsNullOrWhiteSpace(chargeId))
                {
                    model.Id = Guid.Parse(chargeId);

                    #region Update Reservation Total Balance.
                    var reservation = new ReservationVM();

                    if (model.ReservationId.HasValue)
                        reservation = reservationRepository.GetReservationById(model.ReservationId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                    double totalBalance = 0;
                    if (reservation != null)
                    {
                        totalBalance = reservation.GuestBalance.HasValue ? reservation.GuestBalance.Value : 0;

                        //Calculate total Amount.
                        int qty = 1;

                        if (model.Qty.HasValue)
                            qty = model.Qty.Value;

                        totalBalance = totalBalance + (model.Amount.HasValue ? (model.Amount.Value * qty) : 0);

                        //totalBalance += model.Amount.HasValue ? model.Amount.Value : 0;

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
                Utility.Utility.LogError(e, "Create");
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
                Guid? reservationId = null;
                double totalAmount = 0;

                if (models != null)
                {
                    reservationId = models[0].ReservationId;


                    foreach (var model in models)
                    {
                        string chargeId = string.Empty;

                        model.CreatedBy = LogInManager.LoggedInUserId;
                        model.AdditionalChargeSource = AdditionalChargeSource.ADDITIONAL_CHARGE;

                        double? transactionAmount = 0;
                        double? oldAmount = 0;
                        if (model.Amount.HasValue)
                        {
                            oldAmount = model.Amount;
                            transactionAmount = CurrencyManager.ConvertAmountToBaseCurrency(model.Amount, LogInManager.CurrencyCode);
                            model.Amount = transactionAmount;
                        }

                        chargeId = reservationChargeRepository.AddReservationCharges(model);

                        if (!string.IsNullOrWhiteSpace(chargeId))
                        {
                            //Calculate total Amount.
                            int qty = 1;

                            if (model.Qty.HasValue)
                                qty = model.Qty.Value;

                            totalAmount = totalAmount + (transactionAmount.HasValue ? (transactionAmount.Value * qty) : 0);

                            model.Id = Guid.Parse(chargeId);

                            transactions.Add(model);

                            blnIsChargesInserted = true;

                            model.Amount = oldAmount;
                        }
                    }

                }

                if (blnIsChargesInserted)
                {
                    #region Update Reservation Total Balance.
                    var reservation = new ReservationVM();

                    if (reservationId.HasValue)
                        reservation = reservationRepository.GetReservationById(reservationId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                    if (reservation != null)
                    {

                        if (reservation.GuestBalance.HasValue)
                        {
                            totalBalance = reservation.GuestBalance.Value;
                        }


                        totalBalance += totalAmount;

                        //Update Total Balance.
                        reservationRepository.UpdateReservationTotalBalance(reservation.Id, totalBalance, LogInManager.LoggedInUserId);
                    }
                    #endregion

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
                Utility.Utility.LogError(e, "AddCharges");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteCharge(Guid id)
        {
            try
            {
                double totalBalance = 0;

                var chargeDetail = reservationChargeRepository.GetReservationChargesById(id, LogInManager.LoggedInUserId).FirstOrDefault();

                if (chargeDetail != null)
                {
                    Guid? reservationId = chargeDetail.ReservationId;

                    double chargeAmount = 0;

                    //Calculate total Amount.
                    int qty = 1;

                    if (chargeDetail.Qty.HasValue)
                        qty = chargeDetail.Qty.Value;

                    chargeAmount = (chargeDetail.Amount.HasValue ? (chargeDetail.Amount.Value * qty) : 0);

                    //Delete Reservation Charge
                    reservationChargeRepository.DeleteReservationCharges(chargeDetail.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                    #region Update Reservation Total Balance.
                    var reservation = new ReservationVM();

                    if (reservationId.HasValue)
                        reservation = reservationRepository.GetReservationById(reservationId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                    if (reservation != null)
                    {
                        totalBalance = reservation.GuestBalance.HasValue ? reservation.GuestBalance.Value : 0;

                        if (totalBalance > chargeAmount)
                        {
                            totalBalance -= chargeAmount;
                        }
                        else
                        {
                            totalBalance = 0;
                        }

                        //Update Total Balance.
                        reservationRepository.UpdateReservationTotalBalance(reservation.Id, totalBalance, LogInManager.LoggedInUserId);
                    }
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        errorMessage = "Charge details deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Charge details not found."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteCharge");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}