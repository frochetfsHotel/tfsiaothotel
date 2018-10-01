using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using SuccessHotelierHub.Controllers;

namespace SuccessHotelierHub.Utility
{
    public class TempReservation
    {
        #region Declaration

        public static ReservationRepository reservationRepository = new ReservationRepository();
        public static RoomRepository roomRepository = new RoomRepository();
        public static PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
        public static ProfileRepository profileRepository = new ProfileRepository();
        public static ReservationLogRepository reservationLogRepository = new ReservationLogRepository();
        public static PackageRepository packageRepository = new PackageRepository();
        public static PreferenceRepository preferenceRepository = new PreferenceRepository();
        public static ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();
        public static RateRepository rateRepository = new RateRepository();
        public static AdditionalChargeRepository additionalChargeRepository = new AdditionalChargeRepository();
        public static CheckInCheckOutRepository checkInCheckOutRepository = new CheckInCheckOutRepository();

        #endregion

        public static void CreateDummyReservation()
        {
            try
            {
                //Get Profiles by user id.
                var profiles = profileRepository.GetIndividualProfileByUserId(LogInManager.LoggedInUserId);

                if (profiles == null || profiles.Count == 0)
                {
                    //Load Default Profile by user id.
                    profileRepository.LoadDefaultIndividualProfile(LogInManager.LoggedInUserId);
                }

                //Get temp bulk reservation by user id.
                var tempReservations = reservationRepository.GetTempBulkReservationByDate(DateTime.Now, LogInManager.LoggedInUserId);

                if (tempReservations == null || tempReservations.Count == 0)
                {
                    //Load Default temp bulk reservation by user id.
                    reservationRepository.LoadTempBulkReservation(LogInManager.LoggedInUserId);
                }

                var reservations = reservationRepository.GetUsersReservationByDate(DateTime.Now, LogInManager.LoggedInUserId, true);

                if (reservations == null || reservations.Count == 0)
                {
                    //Delete existing reservation by user.
                    reservationRepository.DeleteReservationByUserId(LogInManager.LoggedInUserId, LogInManager.LoggedInUserId, true);
                    
                    tempReservations = reservationRepository.GetTempBulkReservationByDate(DateTime.Now, LogInManager.LoggedInUserId);

                    if (tempReservations != null && tempReservations.Count > 0)
                    {
                        foreach (var tempReservation in tempReservations)
                        {
                            ReservationVM reservation = new ReservationVM();

                            reservation.TitleId = tempReservation.TitleId;
                            reservation.LastName = tempReservation.LastName;
                            reservation.FirstName = tempReservation.FirstName;
                            reservation.ProfileId = tempReservation.ProfileId;
                            reservation.MemberTypeId = tempReservation.MemberTypeId;
                            reservation.CountryId = tempReservation.CountryId;
                            reservation.LanguageId = tempReservation.LanguageId;
                            reservation.VipId = tempReservation.VipId;
                            reservation.PhoneNo = tempReservation.PhoneNo;
                            reservation.MemberNo = tempReservation.MemberNo;
                            reservation.MemberLvt = tempReservation.MemberLvt;
                            reservation.AgentId = tempReservation.AgentId;
                            reservation.CompanyId = tempReservation.CompanyId;
                            reservation.GroupId = tempReservation.GroupId;
                            reservation.SourceId = tempReservation.SourceId;
                            reservation.ContactId = tempReservation.ContactId;
                            reservation.ArrivalDate = DateTime.Now;
                            reservation.NoOfNight = tempReservation.NoOfNight;
                            reservation.DepartureDate = DateTime.Now.AddDays(tempReservation.NoOfNight);

                            reservation.NoOfAdult = tempReservation.NoOfAdult;
                            reservation.NoOfChildren = tempReservation.NoOfChildren;
                            reservation.NoOfInfant = tempReservation.NoOfInfant;
                            reservation.NoOfRoom = tempReservation.NoOfRoom;
                            reservation.RoomTypeId = tempReservation.RoomTypeId;
                            reservation.RtcId = tempReservation.RtcId;
                            reservation.ExtnId = tempReservation.ExtnId;
                            reservation.RateCodeId = tempReservation.RateCodeId;
                            reservation.IsFixedRate = tempReservation.IsFixedRate;
                            reservation.Rate = tempReservation.Rate;
                            
                            reservation.CurrencyId = LogInManager.CurrencyId;
                            reservation.BlockCodeId = tempReservation.BlockCodeId;
                            reservation.ETA = tempReservation.ETA;
                            reservation.ReservationTypeId = tempReservation.ReservationTypeId;
                            reservation.MarketId = tempReservation.MarketId;
                            reservation.ReservationSourceId = tempReservation.ReservationSourceId;
                            reservation.OriginId = tempReservation.OriginId;
                            reservation.PaymentMethodId = tempReservation.PaymentMethodId;

                            //Decrypt Credit Card #
                            reservation.CreditCardNo = Utility.Decrypt(tempReservation.CreditCardNo, Utility.EncryptionKey);                            
                            
                            reservation.CardExpiryDate = tempReservation.CardExpiryDate;
                            reservation.CVVNo = tempReservation.CVVNo;
                            reservation.ApprovalCode = tempReservation.ApprovalCode;
                            reservation.ApprovalAmount = tempReservation.ApprovalAmount;
                            reservation.SuitWith = tempReservation.SuitWith;
                            reservation.IsEmailConfirmation = tempReservation.IsEmailConfirmation;
                            reservation.GuestBalance = tempReservation.GuestBalance;
                            reservation.DiscountAmount = tempReservation.DiscountAmount;
                            reservation.DiscountPercentage = tempReservation.DiscountPercentage;
                            reservation.DiscountApprovedBy = tempReservation.DiscountApprovedBy;
                            reservation.DiscountReason = tempReservation.DiscountReason;
                            reservation.TARecordLocator = tempReservation.TARecordLocator;
                            reservation.ItemInventoryId = tempReservation.ItemInventoryId;
                            reservation.TotalPrice = tempReservation.TotalPrice;
                            reservation.RoomNumbers = tempReservation.RoomNo;
                            reservation.RoomIds = Convert.ToString(tempReservation.RoomId);
                            reservation.CreatedBy = LogInManager.LoggedInUserId;
                            reservation.IsActive = true;
                            reservation.IsDummyReservation = true;

                            if (tempReservation.RoomTypeId.HasValue && tempReservation.RateCodeId.HasValue)
                            {
                                //Get Week Day & Week End Price
                                var weekDayPrice = rateRepository.GetWeekDayPrice(tempReservation.RoomTypeId.Value, tempReservation.RateCodeId.Value).FirstOrDefault();

                                var weekEndPrice = rateRepository.GetWeekEndPrice(tempReservation.RoomTypeId.Value, tempReservation.RateCodeId.Value).FirstOrDefault();

                                if (weekDayPrice != null)
                                {
                                    reservation.Rate = weekDayPrice.Amount;
                                }

                                if (weekEndPrice != null)
                                {
                                    //Check Week End 
                                    if (reservation.ArrivalDate.Value.DayOfWeek == DayOfWeek.Friday || reservation.ArrivalDate.Value.DayOfWeek == DayOfWeek.Saturday)
                                    {
                                        reservation.Rate = weekEndPrice.Amount;
                                    }
                                }
                            }

                            //Create Reservation.
                            CreateReservation(reservation);
                        }

                        var reservationList = reservationRepository.GetUsersReservationByDate(DateTime.Now, LogInManager.LoggedInUserId, true);

                        if (reservationList != null && reservationList.Count > 0)
                        {
                            var results = ExtensionMethod.Split(reservationList, 20); //split reservations.

                            if (results != null && results.Count >= 3)
                            {
                                //Checked In : Top 1 to 20 
                                int count = 0;
                                foreach (var result in results[0])
                                {
                                    if (count < 10)
                                    {
                                        //Manage Due-Out reservation. Update arrival and departure date.
                                        result.DepartureDate = result.ArrivalDate;
                                        result.ArrivalDate = result.ArrivalDate.Value.AddDays(-(result.NoOfNight));
                                        result.IsActive = true;
                                        result.IsDummyReservation = true;
                                        result.UpdatedBy = LogInManager.LoggedInUserId;

                                        reservationRepository.UpdateReservation(result);
                                    }

                                    #region Room Mapping
                                    var selectedRooms = roomRepository.GetReservationRoomMapping(result.Id, null, LogInManager.LoggedInUserId);
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
                                            roomIds = Utility.RemoveLastCharcter(roomIds, ',');
                                        }

                                        if (!string.IsNullOrWhiteSpace(roomNumbers))
                                        {
                                            //Remove Last Comma.
                                            roomNumbers = Utility.RemoveLastCharcter(roomNumbers, ',');
                                        }
                                    }
                                    #endregion

                                    CheckInPaymentMethodVM modelCheckInPaymentMethod = new CheckInPaymentMethodVM();
                                    modelCheckInPaymentMethod.ReservationId = result.Id;
                                    modelCheckInPaymentMethod.ProfileId = result.ProfileId;
                                    modelCheckInPaymentMethod.NoOfRoom = result.NoOfRoom.Value;
                                    modelCheckInPaymentMethod.Name = (result.LastName + " " + result.FirstName);
                                    modelCheckInPaymentMethod.CheckInDate = result.ArrivalDate;
                                    modelCheckInPaymentMethod.CheckInTime = result.ETA;
                                    modelCheckInPaymentMethod.CheckInTimeText = string.Format("{0:hh\\:mm\\:ss}", result.ETA.Value);
                                    modelCheckInPaymentMethod.PaymentMethodId = result.PaymentMethodId;

                                    //Decrypt Credit Card #
                                    modelCheckInPaymentMethod.CreditCardNo = Utility.Decrypt(result.CreditCardNo, Utility.EncryptionKey);

                                    modelCheckInPaymentMethod.CardExpiryDate = result.CardExpiryDate;
                                    modelCheckInPaymentMethod.RoomNumbers = roomNumbers;
                                    modelCheckInPaymentMethod.RoomIds = roomIds;
                                    modelCheckInPaymentMethod.RoomTypeId = result.RoomTypeId;

                                    CheckIn(modelCheckInPaymentMethod);

                                    count++;
                                }

                                //Checked Out : 21 to 40
                                foreach (var result in results[1])
                                {
                                    #region Room Mapping
                                    var selectedRooms = roomRepository.GetReservationRoomMapping(result.Id, null, LogInManager.LoggedInUserId);
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
                                            roomIds = Utility.RemoveLastCharcter(roomIds, ',');
                                        }

                                        if (!string.IsNullOrWhiteSpace(roomNumbers))
                                        {
                                            //Remove Last Comma.
                                            roomNumbers = Utility.RemoveLastCharcter(roomNumbers, ',');
                                        }
                                    }
                                    #endregion

                                    #region Payment Method

                                    var paymentMethod = new PaymentMethodVM();
                                    if (result.PaymentMethodId.HasValue)
                                    {
                                        paymentMethod = paymentMethodRepository.GetPaymentMethodById(result.PaymentMethodId.Value).FirstOrDefault();
                                    }

                                    #endregion

                                    CheckInPaymentMethodVM modelCheckInPaymentMethod = new CheckInPaymentMethodVM();
                                    modelCheckInPaymentMethod.ReservationId = result.Id;
                                    modelCheckInPaymentMethod.ProfileId = result.ProfileId;
                                    modelCheckInPaymentMethod.NoOfRoom = result.NoOfRoom.Value;
                                    modelCheckInPaymentMethod.Name = (result.LastName + " " + result.FirstName);
                                    modelCheckInPaymentMethod.CheckInDate = result.ArrivalDate;
                                    modelCheckInPaymentMethod.CheckInTime = result.ETA;
                                    modelCheckInPaymentMethod.CheckInTimeText = string.Format("{0:hh\\:mm\\:ss}", result.ETA.Value);
                                    modelCheckInPaymentMethod.PaymentMethodId = result.PaymentMethodId;

                                    //Decrypt Credit Card #
                                    modelCheckInPaymentMethod.CreditCardNo = Utility.Decrypt(result.CreditCardNo, Utility.EncryptionKey);
                                    
                                    modelCheckInPaymentMethod.CardExpiryDate = result.CardExpiryDate;
                                    modelCheckInPaymentMethod.RoomNumbers = roomNumbers;
                                    modelCheckInPaymentMethod.RoomIds = roomIds;
                                    modelCheckInPaymentMethod.RoomTypeId = result.RoomTypeId;

                                    //Check in
                                    CheckIn(modelCheckInPaymentMethod);

                                    CheckOutPaymentMethodVM modelCheckOutPaymentMethod = new CheckOutPaymentMethodVM();
                                    modelCheckOutPaymentMethod.ReservationId = result.Id;
                                    modelCheckOutPaymentMethod.ProfileId = result.ProfileId;
                                    modelCheckOutPaymentMethod.NoOfRoom = result.NoOfRoom.Value;
                                    modelCheckOutPaymentMethod.CheckOutTimeText = string.Format("{0:hh\\:mm\\:ss}", result.ETA.Value);
                                    modelCheckOutPaymentMethod.CheckOutDate = result.DepartureDate;
                                    modelCheckOutPaymentMethod.Amount = result.GuestBalance;

                                    modelCheckOutPaymentMethod.RoomIds = roomIds;
                                    modelCheckOutPaymentMethod.RoomNumbers = roomNumbers;
                                    modelCheckOutPaymentMethod.RoomTypeId = result.RoomTypeId;

                                    modelCheckOutPaymentMethod.PaymentMethodId = result.PaymentMethodId.Value;
                                    if (paymentMethod != null && !string.IsNullOrWhiteSpace(paymentMethod.Name))
                                    {
                                        modelCheckOutPaymentMethod.PaymentMethod = (paymentMethod.Code + " - " + paymentMethod.Name);
                                    }

                                    //Decrypt Credit Card #
                                    modelCheckOutPaymentMethod.CreditCardNo = Utility.Decrypt(result.CreditCardNo, Utility.EncryptionKey);
                                    
                                    modelCheckOutPaymentMethod.CardExpiryDate = result.CardExpiryDate;
                                    modelCheckOutPaymentMethod.Reference = string.Empty;

                                    //Check out
                                    CheckOut(modelCheckOutPaymentMethod);
                                }

                                //New Reservation : 41 to 50.
                                foreach (var result in results[2])
                                {
                                    if (result != null)
                                    {
                                        //Delete Reservation Room Mapping.
                                        roomRepository.DeleteReservationRoomMappingByReservation(result.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CreateReservation(ReservationVM model)
        {
            string reservationId = string.Empty;
            string confirmationNo = string.Empty;
            long folioNo = 0;

            model.CreatedBy = LogInManager.LoggedInUserId;
            model.IsActive = true;

            string ETAText = model.ETAText;
            if (!string.IsNullOrWhiteSpace(ETAText))
            {
                string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                string date = (todayDate + " " + ETAText);
                DateTime time = Convert.ToDateTime(date);

                model.ETA = time.TimeOfDay;
            }


            var lastReservation = reservationRepository.GetLastReservationByDate(null);

            #region Generate Confirmation No            
            Int64 confirmationSuffix = 1;

            if (lastReservation != null)
            {
                string lastConfirmationNo = lastReservation.ConfirmationNumber;
                if (!string.IsNullOrWhiteSpace(lastConfirmationNo))
                {
                    confirmationSuffix = !string.IsNullOrWhiteSpace(lastConfirmationNo) ? (Convert.ToInt64(lastConfirmationNo) + 1) : Constants.DEFAULT_CONFIRMATION_NO;

                    confirmationNo = confirmationSuffix.ToString();
                }
                else
                {
                    confirmationNo = Constants.DEFAULT_CONFIRMATION_NO.ToString();
                }
            }
            else
            {
                //Default confirmation no.
                confirmationNo = Constants.DEFAULT_CONFIRMATION_NO.ToString();
            }

            model.ConfirmationNumber = confirmationNo;

            #endregion

            #region Generate Folio Number
            Int64 folioSuffix = 1;

            if (lastReservation != null)
            {
                long lastFolioNumber = lastReservation.FolioNumber;
                if (lastFolioNumber > 0)
                {
                    folioSuffix = Convert.ToInt64(lastFolioNumber) + 1;

                    folioNo = folioSuffix;
                }
                else
                {
                    folioNo = Constants.DEFAULT_FOLIO_NUMBER;
                }
            }
            else
            {
                //Default confirmation no.
                folioNo = Constants.DEFAULT_FOLIO_NUMBER;
            }

            model.FolioNumber = folioNo;

            #endregion

            double totalBalance = 0, totalPrice = 0;

            totalPrice = CalculateRoomRentCharges(model.NoOfNight, (model.Rate.HasValue ? model.Rate.Value : 0), model.NoOfChildren, model.DiscountAmount, model.DiscountPercentage, (model.DiscountPercentage.HasValue ? true : false));


            totalBalance = totalPrice;

            model.GuestBalance = totalBalance;
            model.TotalPrice = totalPrice;

            //Encrypt Credit Card#
            model.CreditCardNo = Utility.Encrypt(model.CreditCardNo, Utility.EncryptionKey);

            reservationId = reservationRepository.AddReservation(model);

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                model.Id = Guid.Parse(reservationId);

                #region Save Reservation Room Mapping
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
                            //Save Reservation Room Mapping.
                            ReservationRoomMappingVM reservationRoomMapping = new ReservationRoomMappingVM();
                            reservationRoomMapping.RoomId = Guid.Parse(item.Trim());
                            reservationRoomMapping.ReservationId = model.Id;
                            reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                            reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);


                            #region Remove Existing reservation if room status are dirty.

                            var reservationLog = reservationLogRepository.GetReservationLogByRoom(Guid.Parse(item.Trim()), model.Id, Guid.Parse(RoomStatusType.DIRTY), model.ArrivalDate, model.DepartureDate, LogInManager.LoggedInUserId).FirstOrDefault();

                            if (reservationLog != null)
                            {
                                //Delete Reservation.
                                reservationRepository.DeleteReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Room Mapping.
                                roomRepository.DeleteReservationRoomMappingByReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Log.
                                reservationLogRepository.DeleteReservationLog(reservationLog.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }

                            #endregion Remove Existing reservation if room status are dirty.
                        }
                    }
                }
                #endregion

                #region Save Reservation Preference Mapping
                var preferenceItems = model.PreferenceItems;

                if (!string.IsNullOrWhiteSpace(preferenceItems))
                {
                    var preferenceItemsArr = preferenceItems.Split(',');

                    if (preferenceItemsArr != null)
                    {
                        //Remove Duplication.
                        preferenceItemsArr = preferenceItemsArr.Distinct().ToArray();

                        foreach (var item in preferenceItemsArr)
                        {
                            //Save Reservation Preference Mapping.
                            ReservationPreferenceMappingVM reservationPreferenceMapping = new ReservationPreferenceMappingVM();
                            reservationPreferenceMapping.PreferenceId = Guid.Parse(item);
                            reservationPreferenceMapping.ReservationId = model.Id;
                            reservationPreferenceMapping.CreatedBy = LogInManager.LoggedInUserId;

                            preferenceRepository.AddReservationPreferenceMapping(reservationPreferenceMapping);
                        }
                    }
                }
                #endregion

                #region Save Reservation Add Ons Mapping               

                if (model.AddOnsList != null && model.AddOnsList.Count > 0)
                {
                    foreach (var addOns in model.AddOnsList)
                    {
                        //Save Reservation AddOns Mapping.
                        ReservationAddOnsMappingVM reservationAddOnsMapping = new ReservationAddOnsMappingVM();
                        reservationAddOnsMapping.AddOnsId = addOns.Id;
                        reservationAddOnsMapping.ReservationId = model.Id;
                        reservationAddOnsMapping.CreatedBy = LogInManager.LoggedInUserId;
                        reservationAddOnsMapping.UpdatedBy = LogInManager.LoggedInUserId;

                        reservationRepository.AddUpdateReservationAddOnsMapping(reservationAddOnsMapping);
                    }
                }
                #endregion

                #region Save Reservation Package Mapping               

                if (model.PackageMappingList != null && model.PackageMappingList.Count > 0)
                {
                    var packageMapping = model.PackageMappingList[0];

                    if (packageMapping != null)
                    {
                        var packageDetail = packageRepository.GetPackageById(packageMapping.PackageId.Value).FirstOrDefault();
                        if (packageDetail != null)
                        {
                            packageMapping.PackagePrice = packageDetail.Price;
                        }

                        packageMapping.ReservationId = model.Id;
                        packageMapping.CreatedBy = LogInManager.LoggedInUserId;
                        packageMapping.UpdatedBy = LogInManager.LoggedInUserId;

                        reservationRepository.AddUpdateReservationPackageMapping(packageMapping);
                    }
                }
                #endregion

                #region Reservation Remarks 

                if (model.RemarksList != null && model.RemarksList.Count > 0)
                {
                    foreach (var remark in model.RemarksList)
                    {
                        remark.ReservationId = model.Id;
                        remark.CreatedBy = LogInManager.LoggedInUserId;
                        if (!remark.CreatedOn.HasValue)
                        {
                            remark.CreatedOn = DateTime.Now;
                        }

                        reservationRepository.AddReservationRemark(remark);
                    }
                }

                #endregion

                #region Update Reservation Total Price

                totalPrice = CalculateReservationTotalPrice(model.Id);

                //Update Total Price.
                reservationRepository.UpdateReservationTotalPrice(model.Id, totalPrice, LogInManager.LoggedInUserId);

                #endregion

                #region Update Reservation Total Balance.

                double totalGuestBalance = CalculateTotalBalance(model.Id);

                //Update Total Balance.
                reservationRepository.UpdateReservationTotalBalance(model.Id, totalGuestBalance, LogInManager.LoggedInUserId);

                #endregion

                //#region Record Activity Log
                //RecordActivityLog.RecordActivity(Pages.RESERVATION, string.Format("Created new reservation, Confirmation# : {0}", model.ConfirmationNumber));
                //#endregion

            }

            return confirmationNo;
        }

        public static double CalculateRoomRentCharges(int noOfNights, double rate, int? noOfChildren, double? discountAmount, double? discountPercentage, bool blnIsDisocuntInPercentage = false)
        {
            double dblTotalBalance = 0;
            double totalDiscount = 0;
            double childrenCharges = 0;

            if (blnIsDisocuntInPercentage)
            {
                if (discountPercentage.HasValue && discountPercentage.Value > 0)
                    totalDiscount = ((rate * discountPercentage.Value) / 100) * noOfNights;
            }
            else
            {
                if (discountAmount.HasValue)
                    totalDiscount = discountAmount.Value * noOfNights;
            }

            rate = (rate - totalDiscount);

            //Default 5 Euro for the one children.
            if (noOfChildren.HasValue)
            {
                childrenCharges = (noOfChildren.Value * (double)Constants.CHILDREN_CHARGE);
            }

            dblTotalBalance = (noOfNights * rate) + childrenCharges;

            return Math.Round(dblTotalBalance, 2);
        }

        public static double CalculateReservationTotalPrice(Guid reservationId)
        {
            var reservation = reservationRepository.GetReservationById(reservationId, LogInManager.LoggedInUserId).FirstOrDefault();

            double dblTotalPrice = 0;

            if (reservation != null)
            {
                int noOfNights;
                double rate;
                int? noOfChildren;
                double? discountAmount;
                double? discountPercentage;
                bool blnIsDisocuntInPercentage = false;

                double totalDiscount = 0;
                double childrenCharges = 0;

                noOfNights = reservation.NoOfNight;
                rate = reservation.Rate.HasValue ? reservation.Rate.Value : 0;
                noOfChildren = reservation.NoOfChildren;
                discountAmount = reservation.DiscountAmount;
                discountPercentage = reservation.DiscountPercentage;

                if (discountPercentage.HasValue) { blnIsDisocuntInPercentage = true; }

                double? dblWeekEndPrice = rate;

                if (reservation.RoomTypeId.HasValue && reservation.RateCodeId.HasValue)
                {
                    var weekEndPrice = rateRepository.GetWeekEndPrice(reservation.RoomTypeId.Value, reservation.RateCodeId.Value).FirstOrDefault();

                    if (weekEndPrice != null)
                    {
                        dblWeekEndPrice = weekEndPrice.Amount;
                    }
                }

                int totalNoOfDays = noOfNights;

                DateTime dtStartDate = reservation.ArrivalDate.Value;

                for (int i = 1; i <= totalNoOfDays; i++)
                {
                    double discount = 0;
                    rate = reservation.Rate.HasValue ? reservation.Rate.Value : 0;

                    if (dtStartDate.DayOfWeek == DayOfWeek.Friday || dtStartDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        rate = dblWeekEndPrice.HasValue ? dblWeekEndPrice.Value : rate;
                    }

                    if (blnIsDisocuntInPercentage)
                    {
                        if (discountPercentage.HasValue && discountPercentage.Value > 0)
                            discount = (rate * discountPercentage.Value) / 100;
                    }
                    else
                    {
                        if (discountAmount.HasValue)
                            discount = discountAmount.Value;
                    }

                    totalDiscount = totalDiscount + discount;

                    dblTotalPrice += (1 * rate); // Each night (1, 2, 3 etc...). Here 1 = current night

                    dtStartDate = dtStartDate.AddDays(1);
                }

                //Deduct Discount from total price.
                if (totalDiscount > 0)
                {
                    dblTotalPrice = (dblTotalPrice - totalDiscount);
                }


                //Default 5 Euro for the one children.
                if (noOfChildren.HasValue)
                    childrenCharges = (noOfChildren.Value * (double)Constants.CHILDREN_CHARGE);

                //dblTotalPrice = (noOfNights * rate) + childrenCharges;
                dblTotalPrice += childrenCharges;

                //Append Add-Ons Price.
                var addOnsDetails = reservationRepository.GetReservationAddOnsMapping(reservation.Id, null, LogInManager.LoggedInUserId);
                if (addOnsDetails != null && addOnsDetails.Count > 0)
                {
                    var totalAddOnsPrice = addOnsDetails.Where(m => m.AddOnsPrice.HasValue).Sum(m => m.AddOnsPrice);

                    dblTotalPrice += (totalAddOnsPrice.HasValue) ? totalAddOnsPrice.Value : 0;
                }

                //Append Package Price.
                var packageDetails = reservationRepository.GetReservationPackageMapping(reservation.Id, null, LogInManager.LoggedInUserId);
                if (packageDetails != null && packageDetails.Count > 0)
                {
                    var totalPackagePrice = packageDetails.Where(m => m.TotalAmount.HasValue).Sum(m => m.TotalAmount);

                    dblTotalPrice += (totalPackagePrice.HasValue) ? totalPackagePrice.Value : 0;
                }

            }

            return Math.Round(dblTotalPrice, 2);
        }

        public static double CalculateTotalBalance(Guid reservationId)
        {
            double dblTotalBalance = 0;

            var reservation = reservationRepository.GetReservationById(reservationId, LogInManager.LoggedInUserId).FirstOrDefault();

            if (reservation != null)
            {
                if (reservation.IsCheckIn == true)
                {
                    //Reservation Charges
                    var reservationCharges = reservationChargeRepository.GetReservationCharges(reservationId, null, LogInManager.LoggedInUserId);
                    if (reservationCharges != null && reservationCharges.Count > 0)
                    {
                        var totalReservationCharges = reservationCharges.Where(m => m.Amount.HasValue)
                                    .Sum(m => (m.Amount.Value * (m.Qty.HasValue ? m.Qty.Value : 1)));

                        dblTotalBalance += totalReservationCharges;

                        dblTotalBalance = CurrencyManager.ConvertAmountToBaseCurrency(dblTotalBalance, LogInManager.CurrencyCode);
                    }
                }
                else
                {
                    double totalRoomRentCharge = 0;

                    totalRoomRentCharge = CalculateReservationTotalPrice(reservation.Id);

                    dblTotalBalance = totalRoomRentCharge;
                }

            }

            return Math.Round(dblTotalBalance, 2);
        }

        public static void CheckIn(CheckInPaymentMethodVM model)
        {
            try
            {
                CheckInCheckOutVM checkIn = new CheckInCheckOutVM();
                ReservationChargeVM reservationCharge = new ReservationChargeVM();

                //Get Reservation detail.
                var reservation = reservationRepository.GetReservationById(model.ReservationId, LogInManager.LoggedInUserId).FirstOrDefault();

                if (reservation != null)
                {
                    reservation.PaymentMethodId = model.PaymentMethodId;

                    //Encrypt Credit Card #
                    reservation.CreditCardNo = Utility.Encrypt(model.CreditCardNo, Utility.EncryptionKey);
                    
                    reservation.CardExpiryDate = model.CardExpiryDate;
                    reservation.ArrivalDate = model.CheckInDate;
                    if (model.RoomTypeId.HasValue)
                    {
                        reservation.RoomTypeId = model.RoomTypeId.Value;
                    }

                    DateTime dtDepartureDate;
                    dtDepartureDate = model.CheckInDate.Value.AddDays(reservation.NoOfNight);
                    reservation.DepartureDate = dtDepartureDate;

                    string CheckInTimeText = model.CheckInTimeText;
                    TimeSpan checkInTime = new TimeSpan();
                    if (!string.IsNullOrWhiteSpace(CheckInTimeText))
                    {
                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                        string date = (todayDate + " " + CheckInTimeText);
                        DateTime time = Convert.ToDateTime(date);
                        checkInTime = time.TimeOfDay;

                        reservation.ETA = checkInTime;
                    }

                    if (reservation.ETA.HasValue)
                    {
                        checkInTime = reservation.ETA.Value;
                    }

                    reservation.UpdatedBy = LogInManager.LoggedInUserId;
                    reservationRepository.UpdateReservation(reservation);

                    #region Save Room Rent Charges 

                    double totalPrice = CalculateReservationTotalPrice(reservation.Id);

                    var roomRentCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.ROOM_RENT).FirstOrDefault();

                    reservationCharge = new ReservationChargeVM();
                    reservationCharge.ReservationId = reservation.Id;
                    reservationCharge.AdditionalChargeId = roomRentCharge.Id;
                    reservationCharge.AdditionalChargeSource = AdditionalChargeSource.ADDITIONAL_CHARGE;
                    reservationCharge.Code = roomRentCharge.Code;
                    reservationCharge.Description = roomRentCharge.Description;
                    reservationCharge.TransactionDate = reservation.ArrivalDate;
                    reservationCharge.Amount = totalPrice;
                    reservationCharge.Qty = 1;
                    reservationCharge.IsActive = true;
                    reservationCharge.CreatedBy = LogInManager.LoggedInUserId;

                    reservationChargeRepository.AddReservationCharges(reservationCharge);

                    #endregion

                    #region Save Reservation Room Mapping & Update Room Occupied Flag

                    var roomIds = model.RoomIds;

                    string[] roomIdsArr = null;
                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        roomIdsArr = roomIds.Split(',');
                        if (roomIdsArr != null)
                        {
                            //Remove Duplication.
                            roomIdsArr = roomIdsArr.Distinct().ToArray();
                        }
                    }

                    #region Delete Room Mapping

                    var roomMappings = roomRepository.GetReservationRoomMapping(reservation.Id, null, LogInManager.LoggedInUserId);

                    if (roomMappings != null && roomMappings.Count > 0)
                    {
                        List<Guid> roomMappingIds = new List<Guid>();

                        foreach (var roomMapping in roomMappings)
                        {
                            if (roomMapping.RoomId.HasValue)
                            {
                                if (roomIdsArr == null || !roomIdsArr.Contains(roomMapping.RoomId.Value.ToString()))
                                {
                                    roomMappingIds.Add(roomMapping.Id);
                                }
                            }
                        }

                        //Delete Room Mapping
                        if (roomMappingIds != null && roomMappingIds.Count > 0)
                        {
                            foreach (var mappingId in roomMappingIds)
                            {
                                roomRepository.DeleteReservationRoomMapping(mappingId, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }
                        }
                    }

                    #endregion


                    if (roomIdsArr != null)
                    {
                        foreach (var item in roomIdsArr)
                        {
                            //Save Reservation Room Mapping.
                            ReservationRoomMappingVM reservationRoomMapping = new ReservationRoomMappingVM();
                            reservationRoomMapping.RoomId = Guid.Parse(item.Trim());
                            reservationRoomMapping.ReservationId = reservation.Id;
                            reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                            reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);



                            //#region Remove Existing Reservation & Room Mapping (Who selected this Room# but not checked in yet.)

                            //reservationRepository.DeleteReservationAndRoomMappingByRoom(Guid.Parse(item.Trim()), reservation.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                            //#endregion

                            #region Remove Existing Reservation who did just booking but not checked-in yet.

                            var reservationRoomMappingDetails = roomRepository.GetReservationRoomMappingByRoom(Guid.Parse(item.Trim()), reservation.Id, reservation.ArrivalDate, reservation.DepartureDate, LogInManager.LoggedInUserId);

                            if (reservationRoomMappingDetails != null && reservationRoomMappingDetails.Count > 0)
                            {
                                foreach (var reservationRoomMappingDetail in reservationRoomMappingDetails)
                                {
                                    //Delete Reservation.
                                    reservationRepository.DeleteReservation(reservationRoomMappingDetail.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                    //Delete Reservation Room Mapping.
                                    roomRepository.DeleteReservationRoomMappingByReservation(reservationRoomMappingDetail.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                    //Delete Reservation Log.
                                    reservationLogRepository.DeleteReservationLog(reservationRoomMappingDetail.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                                }
                            }

                            #endregion

                            #region Add Reservation Log

                            var lstReservationLog = reservationLogRepository.GetReservationLogDetails(model.ReservationId, Guid.Parse(item.Trim()), null, LogInManager.LoggedInUserId).FirstOrDefault();

                            if (lstReservationLog != null)
                            {
                                lstReservationLog.ReservationId = model.ReservationId;
                                lstReservationLog.ProfileId = model.ProfileId;
                                lstReservationLog.RoomId = Guid.Parse(item.Trim());
                                lstReservationLog.CheckInDate = model.CheckInDate.Value;
                                lstReservationLog.CheckInTime = checkInTime;
                                lstReservationLog.CheckOutTime = checkInTime;
                                lstReservationLog.CheckOutDate = reservation.DepartureDate;
                                lstReservationLog.RoomStatusId = Guid.Parse(RoomStatusType.DIRTY);
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
                                reservationLog.CheckInDate = model.CheckInDate.Value;
                                reservationLog.CheckInTime = checkInTime;
                                reservationLog.CheckOutTime = checkInTime;
                                reservationLog.CheckOutDate = reservation.DepartureDate;
                                reservationLog.RoomStatusId = Guid.Parse(RoomStatusType.DIRTY);
                                reservationLog.IsActive = true;
                                reservationLog.CreatedBy = LogInManager.LoggedInUserId;

                                reservationLogRepository.AddReservationLog(reservationLog);
                            }

                            #endregion

                        }
                    }

                    #endregion

                    #region Save CheckIn Details

                    checkIn.ReservationId = model.ReservationId;
                    checkIn.ProfileId = model.ProfileId;
                    checkIn.CheckInDate = model.CheckInDate.Value;
                    checkIn.CheckInTime = checkInTime;
                    checkIn.IsActive = true;
                    checkIn.CreatedBy = LogInManager.LoggedInUserId;

                    var checkInId = checkInCheckOutRepository.AddCheckInDetail(checkIn);

                    #endregion

                    #region Update Reservation Check In Flag

                    reservationRepository.UpdateReservationCheckInFlag(model.ReservationId, true, LogInManager.LoggedInUserId);

                    #endregion

                    #region Update Reservation Status

                    reservationRepository.UpdateReservationStatus(model.ReservationId, Guid.Parse(ReservationStatusName.CHECKEDIN), LogInManager.LoggedInUserId);

                    #endregion

                    #region Update Reservation Total Balance.

                    double totalGuestBalance = CalculateTotalBalance(reservation.Id);

                    //Update Total Balance.
                    reservationRepository.UpdateReservationTotalBalance(reservation.Id, totalGuestBalance, LogInManager.LoggedInUserId);

                    #endregion

                    //#region Record Activity Log
                    //RecordActivityLog.RecordActivity(Pages.CHECKIN, string.Format("Checked in profile successfully. Name: {0} {1}, Comfirmation #: {2} ", reservation.LastName, reservation.FirstName, reservation.ConfirmationNumber));
                    //#endregion

                }

            }
            catch (Exception e)
            {
                Utility.LogError(e, "CheckIn");
            }
        }

        public static void CheckOut(CheckOutPaymentMethodVM model)
        {
            try
            {
                CheckInCheckOutVM checkOut = new CheckInCheckOutVM();

                var checkInDetails = checkInCheckOutRepository.GetCheckInDetails(model.ReservationId, model.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

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
                    var reservation = reservationRepository.GetReservationById(model.ReservationId, LogInManager.LoggedInUserId).FirstOrDefault();

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
                        reservationCharge.PaymentMethodId = model.PaymentMethodId;
                        reservationCharge.Description = model.PaymentMethod;
                        reservationCharge.CVVNo = model.CVVNo;
                        reservationCharge.TransactionDate = model.CheckOutDate.Value;
                        reservationCharge.Amount = -(totalAmount);
                        reservationCharge.Qty = 1;
                        
                        //Encrypt Credit Card #
                        reservationCharge.CreditCardNo = Utility.Encrypt(model.CreditCardNo, Utility.EncryptionKey);

                        reservationCharge.CardExpiryDate = reservation.CardExpiryDate;
                        reservationCharge.IsActive = true;
                        reservationCharge.IsDummyReservationPayment = true;
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
                                    #region Add Reservation Log

                                    var lstReservationLog = reservationLogRepository.GetReservationLogDetails(model.ReservationId, Guid.Parse(item.Trim()), null, LogInManager.LoggedInUserId).FirstOrDefault();

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

                        //Encrypt Credit Card #
                        reservation.CreditCardNo = Utility.Encrypt(model.CreditCardNo, Utility.EncryptionKey);

                        reservation.CardExpiryDate = model.CardExpiryDate;

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

                        //#region Record Activity Log
                        //RecordActivityLog.RecordActivity(Pages.CHECKOUT, string.Format("Checked out profile successfully. Name: {0} {1}, Comfirmation #: {2} ", reservation.LastName, reservation.FirstName, reservation.ConfirmationNumber));
                        //#endregion

                    }

                }

            }
            catch (Exception e)
            {
                Utility.LogError(e, "CheckOut");
            }
        }
    }

}