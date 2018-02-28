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
        public static void CreateDummyReservation()
        {
            ReservationRepository reservationRepository = new ReservationRepository();
            RoomRepository roomRepository = new RoomRepository();
            PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
            ProfileRepository profileRepository = new ProfileRepository();

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


            var reservations = reservationRepository.GetUsersReservationByDate(DateTime.Now, LogInManager.LoggedInUserId);

            if (reservations == null || reservations.Count == 0)
            {
                //Delete existing reservation by user.
                reservationRepository.DeleteReservationByUserId(LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                
                ReservationController reservationController = new ReservationController();
                FrontDeskController frontDeskController = new FrontDeskController();
                CashieringController cashieringController = new CashieringController();

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
                        reservation.CurrencyId = tempReservation.CurrencyId;
                        reservation.BlockCodeId = tempReservation.BlockCodeId;
                        reservation.ETA = tempReservation.ETA;
                        reservation.ReservationTypeId = tempReservation.ReservationTypeId;
                        reservation.MarketId = tempReservation.MarketId;
                        reservation.ReservationSourceId = tempReservation.ReservationSourceId;
                        reservation.OriginId = tempReservation.OriginId;
                        reservation.PaymentMethodId = tempReservation.PaymentMethodId;
                        reservation.CreditCardNo = tempReservation.CreditCardNo;
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

                        //Create Reservation.
                        reservationController.CreateReservation(reservation);
                    }

                    var reservationList = reservationRepository.GetUsersReservationByDate(DateTime.Now, LogInManager.LoggedInUserId);

                    if (reservationList != null && reservationList.Count > 0)
                    {
                        var results = ExtensionMethod.Split(reservationList, 20); //split reservations.

                        if (results != null && results.Count >= 3)
                        {
                            //Checked In : Top 1 to 20 
                            foreach (var result in results[0])
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

                                CheckInPaymentMethodVM modelCheckInPaymentMethod = new CheckInPaymentMethodVM();
                                modelCheckInPaymentMethod.ReservationId = result.Id;
                                modelCheckInPaymentMethod.ProfileId = result.ProfileId;
                                modelCheckInPaymentMethod.NoOfRoom = result.NoOfRoom.Value;
                                modelCheckInPaymentMethod.Name = (result.LastName + " " + result.FirstName);
                                modelCheckInPaymentMethod.CheckInDate = result.ArrivalDate;
                                modelCheckInPaymentMethod.CheckInTime = result.ETA;
                                modelCheckInPaymentMethod.CheckInTimeText = string.Format("{0:hh\\:mm\\:ss}", result.ETA.Value);
                                modelCheckInPaymentMethod.PaymentMethodId = result.PaymentMethodId;
                                modelCheckInPaymentMethod.CreditCardNo = result.CreditCardNo;
                                modelCheckInPaymentMethod.CardExpiryDate = result.CardExpiryDate;                                
                                modelCheckInPaymentMethod.RoomNumbers = roomNumbers;
                                modelCheckInPaymentMethod.RoomIds = roomIds;
                                modelCheckInPaymentMethod.RoomTypeId = result.RoomTypeId;

                                frontDeskController.CheckIn(modelCheckInPaymentMethod);
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
                                modelCheckInPaymentMethod.CreditCardNo = result.CreditCardNo;
                                modelCheckInPaymentMethod.CardExpiryDate = result.CardExpiryDate;
                                modelCheckInPaymentMethod.RoomNumbers = roomNumbers;
                                modelCheckInPaymentMethod.RoomIds = roomIds;
                                modelCheckInPaymentMethod.RoomTypeId = result.RoomTypeId;

                                //Check in
                                frontDeskController.CheckIn(modelCheckInPaymentMethod);

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

                                modelCheckOutPaymentMethod.CreditCardNo = result.CreditCardNo;
                                modelCheckOutPaymentMethod.CardExpiryDate = result.CardExpiryDate;
                                modelCheckOutPaymentMethod.Reference = string.Empty;

                                //Check out
                                cashieringController.CheckOut(modelCheckOutPaymentMethod);
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
    }
}