using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;
using System.IO;

namespace SuccessHotelierHub.Controllers
{
    
    public class CashieringController : Controller
    {
        #region Declaration

        private ProfileRepository profileRepository = new ProfileRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private CheckInCheckOutRepository checkInCheckOutRepository = new CheckInCheckOutRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private ReservationLogRepository reservationLogRepository = new ReservationLogRepository();
        private AdditionalChargeRepository additionalChargeRepository = new AdditionalChargeRepository();
        private ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();
        private PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
        private TitleRepository titleRepository = new TitleRepository();
        private CityRepository cityRepository = new CityRepository();
        private StateRepository stateRepository = new StateRepository();
        private CountryRepository countryRepository = new CountryRepository();

        #endregion

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        // GET: Cashiering
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult SearchGuest()
        {
            var charges = additionalChargeRepository.GetAdditionalCharges();
            ViewBag.AdditionalChargeList = charges;


            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.SEARCH_GUESTS, "Goes to search guest page.");
            #endregion

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult SearchGuest(SearchGuestParametersVM model)
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
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.IsAdminUser = LogInManager.HasRights("ADMIN");

                var reservations = checkInCheckOutRepository.SearchGuest(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.SEARCH_GUESTS, "Searched data in search guest page.");
                #endregion

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservations
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchGuest");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult GetBillingInfo(Guid reservationId, string source = "")
        {
            try
            {
                var reservation = reservationRepository.GetReservationById(reservationId).FirstOrDefault();

                #region Room Mapping

                //Get Room Mapping
                var selectedRooms = roomRepository.GetReservationRoomMapping(reservationId, null);
                var roomIds = string.Empty;
                var roomNumbers = string.Empty;

                if (selectedRooms != null && selectedRooms.Count > 0)
                {
                    foreach (var room in selectedRooms)
                    {
                        roomIds += string.Format("{0},", room.RoomId);
                        roomNumbers += string.Format("{0}, ", room.RoomNo);
                    }

                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        //Remove Last Comma.
                        roomIds = Utility.Utility.RemoveLastCharcter(roomIds, ',');
                    }

                    if (!string.IsNullOrWhiteSpace(roomNumbers))
                    {
                        //Remove Last Comma.
                        roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                    }
                }
                #endregion

                #region Reservation Charges

                var transactions = reservationChargeRepository.GetReservationCharges(reservation.Id, null);

                #endregion

                #region Rate Type

                var rateType = new RateTypeVM();
                if (reservation.RateCodeId.HasValue)
                    rateType = rateTypeRepository.GetRateTypeById(reservation.RateCodeId.Value).FirstOrDefault();

                #endregion

                #region Room Type

                var roomType = new RoomTypeVM();
                if (reservation.RoomTypeId.HasValue)
                    roomType = roomTypeRepository.GetRoomTypeById(reservation.RoomTypeId.Value).FirstOrDefault();

                #endregion

                BillingInfoVM model = new BillingInfoVM();

                model.ReservationId = reservation.Id;
                model.ProfileId = reservation.ProfileId;
                model.Name = (reservation.LastName + " " + reservation.FirstName).Trim();

                model.Balance = reservation.GuestBalance;
                model.ArrivalDate = reservation.ArrivalDate;
                model.DepartureDate = reservation.DepartureDate;

                model.CompanyId = reservation.CompanyId;
                model.Company = string.Empty;
                model.GroupId = reservation.GroupId;
                model.Group = string.Empty;

                model.RoomIds = roomIds;
                model.RoomNumbers = roomNumbers;

                model.RateCodeId = reservation.RateCodeId;
                model.RateCode = rateType.RateTypeCode;
                model.Rate = reservation.Rate;
                model.RoomRent = reservation.TotalPrice;                

                model.RoomTypeId = reservation.RoomTypeId;
                model.RoomTypeCode = roomType.RoomTypeCode;
                model.NoOfRooms = reservation.NoOfRoom.HasValue ? reservation.NoOfRoom.Value : 1;

                model.StatusId = reservation.ReservationStatusId;

                #region Payment Method
                if (reservation.PaymentMethodId.HasValue)
                {
                    var paymentMethod = paymentMethodRepository.GetPaymentMethodById(reservation.PaymentMethodId.Value).FirstOrDefault();

                    if (paymentMethod != null)
                    {
                        model.PaymentMethodId = paymentMethod.Id;
                        model.PaymentMethodCode = paymentMethod.Code;
                        model.PaymentMethodName = paymentMethod.Name;
                    }
                }
                #endregion

                if (model.StatusId.HasValue)
                {
                    //Get Reservation Status Detail.
                    var reservationStatus = reservationRepository.GetReservationStatusById(model.StatusId.Value).FirstOrDefault();

                    if (reservationStatus != null)
                    {
                        model.Status = reservationStatus.Name;

                        if (reservation.DepartureDate.HasValue)
                        {
                            //If checkout date is current date then status must be DUE OUT.
                            //if(reservation.DepartureDate.Value.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                            if (reservation.IsCheckOut == false && reservation.DepartureDate.Value <= DateTime.Now)
                            {
                                model.Status = "DUE OUT";
                            }
                        }
                    }
                }

                if (reservation.IsCheckOut)
                {
                    model.IsCheckedOut = reservation.IsCheckOut;
                    //model.Status = "CHECKED OUT";
                }
                else
                {
                    model.IsCheckedOut = false;
                    //model.Status = "DUE OUT";
                }

                model.Transactions = transactions;


                return Json(new
                {
                    IsSuccess = true,
                    Source = source,
                    data = model
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetBillingInfo");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult ShowCheckOutPaymentMethod(Guid reservationId, string source = "")
        {
            try
            {
                var reservation = reservationRepository.GetReservationById(reservationId).FirstOrDefault();
                //var paymentMethodList = new SelectList(paymentMethodRepository.GetPaymentMethods(), "Id", "Name").ToList();
                var paymentMethodList = new SelectList(
                     paymentMethodRepository.GetPaymentMethods()
                     .Select(
                         m => new SelectListItem()
                         {
                             Value = m.Id.ToString(),
                             Text = (m.Code + " - " + m.Name)
                         }
                ), "Value", "Text").ToList();

                #region Room Mapping

                //Get Room Mapping
                var selectedRooms = roomRepository.GetReservationRoomMapping(reservationId, null);
                var roomIds = string.Empty;
                var roomNumbers = string.Empty;

                if (selectedRooms != null && selectedRooms.Count > 0)
                {
                    foreach (var room in selectedRooms)
                    {
                        roomIds += string.Format("{0},", room.RoomId);
                        roomNumbers += string.Format("{0}, ", room.RoomNo);
                    }

                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        //Remove Last Comma.
                        roomIds = Utility.Utility.RemoveLastCharcter(roomIds, ',');
                    }

                    if (!string.IsNullOrWhiteSpace(roomNumbers))
                    {
                        //Remove Last Comma.
                        roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                    }
                }

                #endregion

                CheckOutPaymentMethodVM model = new CheckOutPaymentMethodVM();

                model.ReservationId = reservation.Id;
                model.ProfileId = reservation.ProfileId;

                model.CheckOutDate = reservation.DepartureDate;

                model.NoOfRoom = reservation.NoOfRoom.HasValue ? reservation.NoOfRoom.Value : 1;
                model.Name = Convert.ToString(reservation.LastName + " " + reservation.FirstName).Trim();
                model.PaymentMethodId = reservation.PaymentMethodId;
                model.CreditCardNo = reservation.CreditCardNo;
                model.CardExpiryDate = reservation.CardExpiryDate;
                model.RoomNumbers = roomNumbers;
                model.RoomIds = roomIds;
                model.RoomTypeId = reservation.RoomTypeId;
                model.Amount = reservation.GuestBalance;

                ViewData["Source"] = source;
                ViewData["PaymentMethodList"] = paymentMethodList;

                return PartialView("_CheckOutPaymentMethod", model);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ShowCheckOutPaymentMethod");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult CheckOut(CheckOutPaymentMethodVM model)
        {
            try
            {
                CheckInCheckOutVM checkOut = new CheckInCheckOutVM();

                var checkInDetails = checkInCheckOutRepository.GetCheckInDetails(model.ReservationId, model.ProfileId.Value).FirstOrDefault();

                if (checkInDetails != null)
                {
                    string CheckOutTimeText = model.CheckOutTimeText;
                    TimeSpan checkOutTime = new TimeSpan();

                    if (!string.IsNullOrWhiteSpace(CheckOutTimeText))
                    {
                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                        string date = (todayDate + " " + CheckOutTimeText);
                        DateTime time = Convert.ToDateTime(date);
                        checkOutTime = time.TimeOfDay;
                    }

                    //Get Reservation detail.
                    var reservation = reservationRepository.GetReservationById(model.ReservationId).FirstOrDefault();

                    if (reservation != null)
                    {
                        #region Add Entry for Minus All the Expenses

                        var checkOutCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.CHECK_OUT).FirstOrDefault();

                        double totalAmount = model.Amount.HasValue ? model.Amount.Value : 0;

                        ReservationChargeVM reservationCharge = new ReservationChargeVM();
                        reservationCharge.ReservationId = reservation.Id;
                        reservationCharge.AdditionalChargeId = checkOutCharge.Id;
                        reservationCharge.AdditionalChargeSource = AdditionalChargeSource.ADDITIONAL_CHARGE;
                        reservationCharge.Code = checkOutCharge.Code;
                        reservationCharge.Description = model.PaymentMethod;
                        reservationCharge.TransactionDate = model.CheckOutDate.Value;
                        reservationCharge.Amount = -(totalAmount);
                        reservationCharge.Qty = 1;
                        reservationCharge.IsActive = true;
                        reservationCharge.CreatedBy = LogInManager.LoggedInUserId;

                        reservationChargeRepository.AddReservationCharges(reservationCharge);

                        #endregion

                        #region Update Room Occupied Flag

                        var roomIds = model.RoomIds;
                        if (!string.IsNullOrWhiteSpace(roomIds))
                        {
                            var roomIdsArr = roomIds.Split(',');

                            if (roomIdsArr != null)
                            {
                                //Remove Duplication.
                                roomIdsArr = roomIdsArr.Distinct().ToArray();

                                foreach (var item in roomIdsArr)
                                {
                                    ////Update Room Occupied Flag.
                                    //roomRepository.UpdateRoomOccupiedFlag(Guid.Parse(item.Trim()), false, LogInManager.LoggedInUserId);

                                    ////Update Room Status DIRTY to CLEAN.
                                    //roomRepository.UpdateRoomCheckOutStatus(Guid.Parse(item.Trim()), Guid.Parse(RoomStatusType.CLEAN), false, LogInManager.LoggedInUserId);

                                    #region Add Reservation Log

                                    var lstReservationLog = reservationLogRepository.GetReservationLogDetails(model.ReservationId, Guid.Parse(item.Trim()), null).FirstOrDefault();

                                    if (lstReservationLog != null)
                                    {
                                        lstReservationLog.ReservationId = model.ReservationId;
                                        lstReservationLog.ProfileId = model.ProfileId;
                                        lstReservationLog.RoomId = Guid.Parse(item.Trim());
                                        lstReservationLog.CheckInDate = reservation.ArrivalDate;
                                        lstReservationLog.CheckOutDate = model.CheckOutDate;
                                        lstReservationLog.CheckOutTime = checkOutTime;
                                        lstReservationLog.RoomStatusId = Guid.Parse(RoomStatusType.CLEAN);
                                        lstReservationLog.IsActive = true;
                                        lstReservationLog.UpdatedBy = LogInManager.LoggedInUserId;

                                        reservationLogRepository.UpdateReservationLog(lstReservationLog);
                                    }
                                    else
                                    {
                                        ReservationLogVM reservationLog = new ReservationLogVM();
                                        reservationLog.ReservationId = model.ReservationId;
                                        reservationLog.ProfileId = model.ProfileId;
                                        reservationLog.RoomId = Guid.Parse(item.Trim());
                                        reservationLog.CheckInDate = reservation.ArrivalDate;
                                        reservationLog.CheckOutDate = model.CheckOutDate;
                                        reservationLog.CheckOutTime = checkOutTime;
                                        reservationLog.RoomStatusId = Guid.Parse(RoomStatusType.CLEAN);
                                        reservationLog.IsActive = true;
                                        reservationLog.CreatedBy = LogInManager.LoggedInUserId;

                                        reservationLogRepository.AddReservationLog(reservationLog);
                                    }

                                    #endregion
                                }
                            }
                        }

                        #endregion

                        #region Update Check Out Details

                        checkOut.Id = checkInDetails.Id;
                        checkOut.ReservationId = model.ReservationId;
                        checkOut.ProfileId = model.ProfileId;
                        checkOut.CheckOutDate = model.CheckOutDate.Value;
                        checkOut.CheckOutTime = checkOutTime;
                        checkOut.CheckOutReference = model.Reference;
                        checkOut.IsActive = true;
                        checkOut.UpdatedBy = LogInManager.LoggedInUserId;

                        var checkOutId = checkInCheckOutRepository.UpdateCheckOutDetail(checkOut);

                        #endregion

                        #region Update Reservation

                        reservation.PaymentMethodId = model.PaymentMethodId;
                        reservation.CreditCardNo = model.CreditCardNo;
                        reservation.CardExpiryDate = model.CardExpiryDate;

                        //Replace Departure date with  check out date.
                        //reservation.DepartureDate = model.CheckOutDate.Value;

                        //Update Total Balance.
                        if (totalAmount > reservation.GuestBalance)
                        {   
                            reservation.GuestBalance = 0;
                        }
                        else
                        {
                            reservation.GuestBalance -= totalAmount;
                        }

                        reservation.UpdatedBy = LogInManager.LoggedInUserId;
                        reservationRepository.UpdateReservation(reservation);

                        #endregion

                        #region Update Reservation Check Out Flag

                        reservationRepository.UpdateReservationCheckOutFlag(model.ReservationId, true, LogInManager.LoggedInUserId);

                        #endregion

                        #region Update Reservation Status

                        reservationRepository.UpdateReservationStatus(model.ReservationId, Guid.Parse(ReservationStatusName.CHECKEDOUT), LogInManager.LoggedInUserId);

                        #endregion

                        #region Record Activity Log
                        RecordActivityLog.RecordActivity(Pages.CHECKOUT, string.Format("Checked out profile successfully. Name: {0} {1}, Comfirmation #: {2} ", reservation.LastName, reservation.FirstName, reservation.ConfirmationNumber));
                        #endregion

                        return Json(new
                        {
                            IsSuccess = true,
                            data = new
                            {
                                CheckOutId = checkOutId,
                                Name = model.Name
                            }
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "Check In details not exist for this guest."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Check Out details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "CheckOut");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT,TUTOR")]
        public ActionResult Preview(Guid? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }

            BillingTransactionReportVM model = new BillingTransactionReportVM();

            var reservation = reservationRepository.GetReservationById(Id.Value).FirstOrDefault();

            #region Room Mapping

            //Get Room Mapping
            var selectedRooms = roomRepository.GetReservationRoomMapping(Id, null);
            var roomIds = string.Empty;
            var roomNumbers = string.Empty;

            if (selectedRooms != null && selectedRooms.Count > 0)
            {
                foreach (var room in selectedRooms)
                {
                    roomIds += string.Format("{0},", room.RoomId);
                    roomNumbers += string.Format("{0}, ", room.RoomNo);
                }

                if (!string.IsNullOrWhiteSpace(roomNumbers))
                {
                    //Remove Last Comma.
                    roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                }
            }
            #endregion

            #region Reservation Charges

            var transactions = reservationChargeRepository.GetReservationCharges(reservation.Id, null);

            #endregion

            #region Profile

            var profile = new IndividualProfileVM();

            if (reservation.ProfileId.HasValue)
                profile = profileRepository.GetIndividualProfileById(reservation.ProfileId.Value).FirstOrDefault();

            #endregion

            #region Title 

            var title = new TitleVM();
            if (profile.TitleId.HasValue)
                title = titleRepository.GetTitlebyId(profile.TitleId.Value).FirstOrDefault();

            #endregion

            double total = 0;
            double roomRent = 0;
            double totalBalance = 0;
            double balanceDue = 0;

            model.Id = reservation.Id;
            model.ConfirmationNo = reservation.ConfirmationNumber;
            model.ProfileId = reservation.ProfileId;
            model.Title = title.Title;
            model.Name = (profile.FirstName + ' ' + profile.LastName);

            #region Fetch Address
            model.Address = !string.IsNullOrWhiteSpace(profile.Address) ? profile.Address : profile.HomeAddress;

            if (!string.IsNullOrWhiteSpace(profile.CityName))
            {
                //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + profile.CityName) : profile.CityName;
                model.City = profile.CityName;
            }

            //if (profile.StateId.HasValue)
            //{
            //    var state = stateRepository.GetStateById(profile.StateId.Value).FirstOrDefault();

            //    if (state != null)
            //    {
            //        model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + state.Name) : state.Name;
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(profile.StateName))
            {
                //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + profile.StateName) : profile.StateName;
                model.State = profile.StateName;
            }

            if (profile.CountryId.HasValue)
            {
                var country = countryRepository.GetCountryById(profile.CountryId.Value).FirstOrDefault();

                if (country != null)
                {
                    //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + country.Name) : country.Name;
                    model.Country = country.Name;
                }
            }
            #endregion
            

            model.RoomNumer = roomNumbers;
            model.FolioNumber = Convert.ToString(reservation.FolioNumber);
            model.CashierNumber = LogInManager.CashierNumber;
            model.PageNumber = "1";
            model.ArrivalDate = reservation.ArrivalDate.HasValue ? reservation.ArrivalDate.Value.ToString("dd-MMM-yyyy") : "";
            model.DepartureDate = reservation.DepartureDate.HasValue ? reservation.DepartureDate.Value.ToString("dd-MMM-yyyy") : "";
            model.GSTRegistrationNumber = "";

            model.VATTax = 9; //Default 9%.

            model.Transactions = transactions;

            //Get Amount.

            var checkoutAdditionalCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.CHECK_OUT).FirstOrDefault(); //Check out
            var roomRentAdditionalCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.ROOM_RENT).FirstOrDefault(); //Room Rent

            double checkOutPaidPayment = 0;
            foreach (var item in transactions)
            {
                int qty = 1;

                if (item.Qty.HasValue)
                    qty = item.Qty.Value;

                if (checkoutAdditionalCharge != null && checkoutAdditionalCharge.Id == item.AdditionalChargeId.Value)  //Check out
                {
                    checkOutPaidPayment = item.Amount.HasValue ? item.Amount.Value : 0;                    
                }
                else
                {
                    totalBalance += (item.Amount.HasValue ? (item.Amount.Value * qty) : 0);
                }

                //totalBalance += item.Amount.HasValue ? item.Amount.Value  : 0;

                //Room Rent
                if (roomRentAdditionalCharge != null && roomRentAdditionalCharge.Id == item.AdditionalChargeId.Value)
                    roomRent = item.Amount.HasValue ? item.Amount.Value : 0;

            }


            //Balance Due
            balanceDue = totalBalance - Math.Abs(checkOutPaidPayment);

            model.FandB = 0;
            model.Other = 0;
            model.Total = total;
            model.Room = roomRent;
            model.TotalBalance = Math.Round(totalBalance, 2);
            model.BalanceDue = Math.Round(balanceDue, 2);

            if (totalBalance > 0)
            {
                double netAmount = 0;
                netAmount = Math.Round(((totalBalance * 100) / (100 + model.VATTax)), 2);

                model.VATAmount = Math.Round((totalBalance - netAmount), 2);
                model.NetAmount = netAmount;
            }

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.CHECKOUT, "Generated folio report.");
            #endregion

            //HTML to PDF
            string html = Utility.Utility.RenderPartialViewToString((Controller)this, "Preview", model);
            byte[] pdfBytes = Utility.Utility.GetPDF(html);

            //return File(pdfBytes, "application/pdf", string.Format("bill_{0}.pdf", model.Id));
            return File(pdfBytes, "application/pdf");

            //return View(model);
        }

        #region SEND EMAIL

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT,TUTOR")]
        public ActionResult SendEmail(Guid? Id, string email)
        {
            try
            {
                string result = "Email sent successfully!";
                if (Id == null)
                {
                    result = "No GUID found";
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = result
                    });
                }


                BillingTransactionReportVM model = new BillingTransactionReportVM();

                var reservation = reservationRepository.GetReservationById(Id.Value).FirstOrDefault();

                #region Room Mapping

                //Get Room Mapping
                var selectedRooms = roomRepository.GetReservationRoomMapping(Id, null);
                var roomIds = string.Empty;
                var roomNumbers = string.Empty;

                if (selectedRooms != null && selectedRooms.Count > 0)
                {
                    foreach (var room in selectedRooms)
                    {
                        roomIds += string.Format("{0},", room.RoomId);
                        roomNumbers += string.Format("{0}, ", room.RoomNo);
                    }

                    if (!string.IsNullOrWhiteSpace(roomNumbers))
                    {
                        //Remove Last Comma.
                        roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                    }
                }
                #endregion

                #region Reservation Charges

                var transactions = reservationChargeRepository.GetReservationCharges(reservation.Id, null);

                #endregion

                #region Profile

                var profile = new IndividualProfileVM();

                if (reservation.ProfileId.HasValue)
                    profile = profileRepository.GetIndividualProfileById(reservation.ProfileId.Value).FirstOrDefault();

                #endregion

                #region Title 

                var title = new TitleVM();
                if (profile.TitleId.HasValue)
                    title = titleRepository.GetTitlebyId(profile.TitleId.Value).FirstOrDefault();

                #endregion

                double total = 0;
                double roomRent = 0;
                double totalBalance = 0;
                double balanceDue = 0;

                model.Id = reservation.Id;
                model.ConfirmationNo = reservation.ConfirmationNumber;
                model.ProfileId = reservation.ProfileId;
                model.Title = title.Title;
                model.Name = (profile.FirstName + ' ' + profile.LastName);

                #region Fetch Address
                model.Address = !string.IsNullOrWhiteSpace(profile.Address) ? profile.Address : profile.HomeAddress;

                if (!string.IsNullOrWhiteSpace(profile.CityName))
                {
                    //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + profile.CityName) : profile.CityName;
                    model.City = profile.CityName;
                }

                //if (profile.StateId.HasValue)
                //{
                //    var state = stateRepository.GetStateById(profile.StateId.Value).FirstOrDefault();

                //    if (state != null)
                //    {
                //        model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + state.Name) : state.Name;
                //    }
                //}

                if (!string.IsNullOrWhiteSpace(profile.StateName))
                {
                    //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + profile.StateName) : profile.StateName;
                    model.State = profile.StateName;
                }

                if (profile.CountryId.HasValue)
                {
                    var country = countryRepository.GetCountryById(profile.CountryId.Value).FirstOrDefault();

                    if (country != null)
                    {
                        //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (" , " + country.Name) : country.Name;
                        model.Country = country.Name;
                    }
                }
                #endregion

                model.RoomNumer = roomNumbers;
                model.FolioNumber = Convert.ToString(reservation.FolioNumber); ;
                model.CashierNumber = LogInManager.CashierNumber;
                model.PageNumber = "1";
                model.ArrivalDate = reservation.ArrivalDate.HasValue ? reservation.ArrivalDate.Value.ToString("dd-MMM-yyyy") : "";
                model.DepartureDate = reservation.DepartureDate.HasValue ? reservation.DepartureDate.Value.ToString("dd-MMM-yyyy") : "";
                model.GSTRegistrationNumber = "";

                model.VATTax = 9; //Default 9%.

                model.Transactions = transactions;

                //Get Amount.

                double checkOutPaidPayment = 0;

                var checkoutAdditionalCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.CHECK_OUT).FirstOrDefault(); //Check out
                var roomRentAdditionalCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.ROOM_RENT).FirstOrDefault(); //Room Rent

                foreach (var item in transactions)
                {
                    int qty = 1;

                    if (item.Qty.HasValue)
                        qty = item.Qty.Value;

                    if (checkoutAdditionalCharge != null && checkoutAdditionalCharge.Id == item.AdditionalChargeId.Value)  //Check out
                    {
                        checkOutPaidPayment = item.Amount.HasValue ? item.Amount.Value : 0;
                    }
                    else
                    {
                        totalBalance += (item.Amount.HasValue ? (item.Amount.Value * qty) : 0);                        
                    }

                    //totalBalance += item.Amount.HasValue ? item.Amount.Value  : 0;

                    //Room Rent
                    if (roomRentAdditionalCharge != null && roomRentAdditionalCharge.Id == item.AdditionalChargeId.Value)
                        roomRent = item.Amount.HasValue ? item.Amount.Value : 0;

                }


                //Balance Due
                balanceDue = totalBalance - Math.Abs(checkOutPaidPayment);

                model.FandB = 0;
                model.Other = 0;
                model.Total = total;
                model.Room = roomRent;
                model.TotalBalance = Math.Round(totalBalance, 2);
                model.BalanceDue = Math.Round(balanceDue, 2);

                if (totalBalance > 0)
                {
                    double netAmount = 0;
                    netAmount = Math.Round(((totalBalance * 100) / (100 + model.VATTax)), 2);

                    model.VATAmount = Math.Round((totalBalance - netAmount), 2);
                    model.NetAmount = netAmount;
                }

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.CHECKOUT, "Send folio report email.");
                #endregion


                //HTML generation
                string html = Utility.Utility.RenderPartialViewToString((Controller)this, "Preview", model);

                //HTML to PDF
                byte[] pdfBytes = Utility.Utility.GetPDF(html);

                string bodyMsg = "";
                using (var sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlTemplates/SendFolioReport.html")))
                {
                    bodyMsg = sr.ReadToEnd();
                    bodyMsg = bodyMsg.Replace("[@UserName]", model.Name);
                    bodyMsg = bodyMsg.Replace("[@ConfirmationNo]", model.ConfirmationNo);
                }

                //Send Email.
                string EmailSubject = "Folio Report for Reservation - " + model.ConfirmationNo;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    string fileName = string.Format("FolioReport-Reservation-{0}.pdf", model.ConfirmationNo);

                    bool blnMailSend = SuccessHotelierHub.Utility.Email.sendMail(email, EmailSubject, bodyMsg, fileName, true, pdfBytes);

                    if (blnMailSend)
                    {
                        return Json(new
                        {
                            IsSuccess = true,
                            SuccessMsg = result
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "Email sending fail."
                        });
                    }
                }
                else
                {
                    result = "No Email found";
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = result
                    });
                }
            }
            catch (Exception ex)
            {
                Utility.Utility.LogError(ex, "SendEmail");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = ex.Message
                });
            }
        }

        #endregion

        [HttpPost]
        public ActionResult ReverseCheckOut(Guid reservationId, string source = "")
        {
            try
            {
                var reservation = reservationRepository.GetReservationById(reservationId).FirstOrDefault();

                if (reservation != null)
                {

                    #region  Remove Reservation Charges                    

                    reservationChargeRepository.DeleteReservationChargesByReservation(reservation.Id, LogInManager.LoggedInUserId);

                    #endregion

                    #region  Remove Reservation Log (Room Occupied)

                    reservationLogRepository.DeleteReservationLogByReservation(reservation.Id, LogInManager.LoggedInUserId);

                    #endregion

                    #region  Remove Check Out Details

                    checkInCheckOutRepository.DeleteCheckInCheckOutDetailByReservation(reservation.Id, LogInManager.LoggedInUserId);

                    #endregion

                    #region Update Reservation Check In & Check Out Flag (FALSE)                    

                    reservationRepository.UpdateReservationCheckInFlag(reservation.Id, false, LogInManager.LoggedInUserId);

                    reservationRepository.UpdateReservationCheckOutFlag(reservation.Id, false, LogInManager.LoggedInUserId);

                    #endregion

                    #region Update Reservation

                    //double totalPrice = Utility.Utility.CalculateRoomRentCharges(reservation.NoOfNight, (reservation.Rate.HasValue ? reservation.Rate.Value : 0), reservation.NoOfChildren, reservation.DiscountAmount, reservation.DiscountPercentage, (reservation.DiscountPercentage.HasValue ? true : false));
                    double totalPrice = Utility.Utility.CalculateReservationTotalPrice(reservation.Id);

                    //reservation.GuestBalance = totalPrice;
                    reservation.GuestBalance = 0;
                    reservation.TotalPrice = totalPrice;

                    reservation.UpdatedBy = LogInManager.LoggedInUserId;
                    reservationRepository.UpdateReservation(reservation);

                    #endregion

                    #region Update Reservation Status (NULL)

                    reservationRepository.UpdateReservationStatus(reservation.Id, null, LogInManager.LoggedInUserId);

                    #endregion

                    #region Record Activity Log

                    RecordActivityLog.RecordActivity(Pages.CHECKIN, string.Format("Reverse Checked out profile successfully. Name: {0} {1}, Comfirmation #: {2} ", reservation.LastName, reservation.FirstName, reservation.ConfirmationNumber));

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = reservationId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reverse check in not done successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ReverseCheckIn");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}