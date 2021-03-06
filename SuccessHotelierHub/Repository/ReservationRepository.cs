﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub.Repository
{
    public class ReservationRepository
    {
        #region Rate Sheet

        public List<RateSheetResultVM> GetRateSheetDetail(Guid? rateTypeId, Guid? roomTypeId, bool isShowWeekEndPrice = false)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeId", Value = rateTypeId},
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId},
                    new SqlParameter { ParameterName = "@IsShowWeekEndPrice", Value = isShowWeekEndPrice}
                };



            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateSheetDetail", parameters);

            var rateSheetResultList = new List<RateSheetResultVM>();
            rateSheetResultList = DALHelper.CreateListFromTable<RateSheetResultVM>(dt);

            return rateSheetResultList;
        }

        public double GetRateSheetAmount(Guid? rateTypeId, Guid? roomTypeId)
        {
            double amount = 0;
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeId", Value = rateTypeId},
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId}
                };



            var objAmount = DALHelper.ExecuteScalar("GetRateSheetAmount", parameters);

            double.TryParse(Convert.ToString(objAmount), out amount);

            return amount;
        }

        #endregion

        #region Reservation 

        public string AddReservation(ReservationVM reservation)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TitleId", Value = reservation.TitleId },
                    new SqlParameter { ParameterName = "@LastName", Value = reservation.LastName } ,
                    new SqlParameter { ParameterName = "@FirstName", Value = reservation.FirstName },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservation.ProfileId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = reservation.MemberTypeId },
                    new SqlParameter { ParameterName = "@CountryId", Value = reservation.CountryId },
                    new SqlParameter { ParameterName = "@LanguageId", Value = reservation.LanguageId },
                    new SqlParameter { ParameterName = "@VipId", Value = reservation.VipId },
                    new SqlParameter { ParameterName = "@PhoneNo", Value = reservation.PhoneNo },
                    new SqlParameter { ParameterName = "@MemberNo", Value = reservation.MemberNo },
                    new SqlParameter { ParameterName = "@MemberLvt", Value = reservation.MemberLvt },
                    new SqlParameter { ParameterName = "@AgentId", Value = reservation.AgentId },
                    new SqlParameter { ParameterName = "@CompanyId", Value = reservation.CompanyId },
                    new SqlParameter { ParameterName = "@GroupId", Value = reservation.GroupId },
                    new SqlParameter { ParameterName = "@SourceId", Value = reservation.SourceId },
                    new SqlParameter { ParameterName = "@ContactId", Value = reservation.ContactId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = reservation.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = reservation.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = reservation.DepartureDate },
                    new SqlParameter { ParameterName = "@NoOfAdult", Value = reservation.NoOfAdult },
                    new SqlParameter { ParameterName = "@NoOfChildren", Value = reservation.NoOfChildren },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = reservation.NoOfRoom },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = reservation.RoomTypeId },
                    new SqlParameter { ParameterName = "@RtcId", Value = reservation.RtcId },
                    new SqlParameter { ParameterName = "@ExtnId", Value = reservation.ExtnId },
                    new SqlParameter { ParameterName = "@RateCodeId", Value = reservation.RateCodeId },
                    new SqlParameter { ParameterName = "@IsFixedRate", Value = reservation.IsFixedRate },
                    new SqlParameter { ParameterName = "@Rate", Value = reservation.Rate },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = reservation.CurrencyId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = reservation.BlockCodeId },
                    new SqlParameter { ParameterName = "@ETA", Value = reservation.ETA },
                    new SqlParameter { ParameterName = "@ReservationTypeId", Value = reservation.ReservationTypeId },
                    new SqlParameter { ParameterName = "@MarketId", Value = reservation.MarketId },
                    new SqlParameter { ParameterName = "@ReservationSourceId", Value = reservation.ReservationSourceId },
                    new SqlParameter { ParameterName = "@OriginId", Value = reservation.OriginId },
                    new SqlParameter { ParameterName = "@PaymentMethodId", Value = reservation.PaymentMethodId },
                    new SqlParameter { ParameterName = "@CreditCardNo", Value = reservation.CreditCardNo },
                    new SqlParameter { ParameterName = "@CardExpiryDate", Value = reservation.CardExpiryDate },
                    new SqlParameter { ParameterName = "@CVVNo", Value = reservation.CVVNo },
                    new SqlParameter { ParameterName = "@ApprovalCode", Value = reservation.ApprovalCode },
                    new SqlParameter { ParameterName = "@ApprovalAmount", Value = reservation.ApprovalAmount },
                    new SqlParameter { ParameterName = "@SuitWith", Value = reservation.SuitWith },
                    new SqlParameter { ParameterName = "@IsEmailConfirmation", Value = reservation.IsEmailConfirmation },
                    new SqlParameter { ParameterName = "@GuestBalance", Value = reservation.GuestBalance },
                    new SqlParameter { ParameterName = "@DiscountAmount", Value = reservation.DiscountAmount },
                    new SqlParameter { ParameterName = "@DiscountPercentage", Value = reservation.DiscountPercentage },
                    new SqlParameter { ParameterName = "@DiscountApprovedBy", Value = reservation.DiscountApprovedBy },
                    new SqlParameter { ParameterName = "@DiscountReason", Value = reservation.DiscountReason },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    //new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@Remarks", Value = string.Empty },
                    new SqlParameter { ParameterName = "@ConfirmationNumber", Value = reservation.ConfirmationNumber },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@FolioNumber", Value = reservation.FolioNumber },
                    new SqlParameter { ParameterName = "@IsDummyReservation", Value = reservation.IsDummyReservation },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservation.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = reservation.CreatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("AddReservation", parameters));

            return reservationId;
        }

        public string UpdateReservation(ReservationVM reservation)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservation.Id },
                    new SqlParameter { ParameterName = "@TitleId", Value = reservation.TitleId },
                    new SqlParameter { ParameterName = "@LastName", Value = reservation.LastName } ,
                    new SqlParameter { ParameterName = "@FirstName", Value = reservation.FirstName },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservation.ProfileId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = reservation.MemberTypeId },
                    new SqlParameter { ParameterName = "@CountryId", Value = reservation.CountryId },
                    new SqlParameter { ParameterName = "@LanguageId", Value = reservation.LanguageId },
                    new SqlParameter { ParameterName = "@VipId", Value = reservation.VipId },
                    new SqlParameter { ParameterName = "@PhoneNo", Value = reservation.PhoneNo },
                    new SqlParameter { ParameterName = "@MemberNo", Value = reservation.MemberNo },
                    new SqlParameter { ParameterName = "@MemberLvt", Value = reservation.MemberLvt },
                    new SqlParameter { ParameterName = "@AgentId", Value = reservation.AgentId },
                    new SqlParameter { ParameterName = "@CompanyId", Value = reservation.CompanyId },
                    new SqlParameter { ParameterName = "@GroupId", Value = reservation.GroupId },
                    new SqlParameter { ParameterName = "@SourceId", Value = reservation.SourceId },
                    new SqlParameter { ParameterName = "@ContactId", Value = reservation.ContactId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = reservation.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = reservation.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = reservation.DepartureDate },
                    new SqlParameter { ParameterName = "@NoOfAdult", Value = reservation.NoOfAdult },
                    new SqlParameter { ParameterName = "@NoOfChildren", Value = reservation.NoOfChildren },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = reservation.NoOfRoom },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = reservation.RoomTypeId },
                    new SqlParameter { ParameterName = "@RtcId", Value = reservation.RtcId },
                    new SqlParameter { ParameterName = "@ExtnId", Value = reservation.ExtnId },
                    new SqlParameter { ParameterName = "@RateCodeId", Value = reservation.RateCodeId },
                    new SqlParameter { ParameterName = "@IsFixedRate", Value = reservation.IsFixedRate },
                    new SqlParameter { ParameterName = "@Rate", Value = reservation.Rate },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = reservation.CurrencyId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = reservation.BlockCodeId },
                    new SqlParameter { ParameterName = "@ETA", Value = reservation.ETA },
                    new SqlParameter { ParameterName = "@ReservationTypeId", Value = reservation.ReservationTypeId },
                    new SqlParameter { ParameterName = "@MarketId", Value = reservation.MarketId },
                    new SqlParameter { ParameterName = "@ReservationSourceId", Value = reservation.ReservationSourceId },
                    new SqlParameter { ParameterName = "@OriginId", Value = reservation.OriginId },
                    new SqlParameter { ParameterName = "@PaymentMethodId", Value = reservation.PaymentMethodId },
                    new SqlParameter { ParameterName = "@CreditCardNo", Value = reservation.CreditCardNo },
                    new SqlParameter { ParameterName = "@CardExpiryDate", Value = reservation.CardExpiryDate },
                    new SqlParameter { ParameterName = "@CVVNo", Value = reservation.CVVNo },
                    new SqlParameter { ParameterName = "@ApprovalCode", Value = reservation.ApprovalCode },
                    new SqlParameter { ParameterName = "@ApprovalAmount", Value = reservation.ApprovalAmount },
                    new SqlParameter { ParameterName = "@SuitWith", Value = reservation.SuitWith },
                    new SqlParameter { ParameterName = "@IsEmailConfirmation", Value = reservation.IsEmailConfirmation },
                    new SqlParameter { ParameterName = "@GuestBalance", Value = reservation.GuestBalance },
                    new SqlParameter { ParameterName = "@DiscountAmount", Value = reservation.DiscountAmount },
                    new SqlParameter { ParameterName = "@DiscountPercentage", Value = reservation.DiscountPercentage },
                    new SqlParameter { ParameterName = "@DiscountApprovedBy", Value = reservation.DiscountApprovedBy },
                    new SqlParameter { ParameterName = "@DiscountReason", Value = reservation.DiscountReason },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    //new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@Remarks", Value = string.Empty },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservation.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservation.UpdatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservation", parameters));

            return reservationId;
        }

        public string DeleteReservation(Guid reservationId, int updatedBy, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservation", parameters));

            return id;
        }

        public string CancelReservation(Guid reservationId, Guid cancellationReasonId, string comment, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@CancellationReasonId", Value = cancellationReasonId },
                    new SqlParameter { ParameterName = "@Comment", Value = comment },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("CancelReservation", parameters));

            return id;
        }

        public List<ReservationVM> GetReservationById(Guid reservationId, int? userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationById", parameters);


            var reservation = new List<ReservationVM>();
            reservation = DALHelper.CreateListFromTable<ReservationVM>(dt);

            return reservation;
        }

        public List<SearchReservationResultVM> SearchReservation(SearchReservationParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@CVVNo", Value = model.CVVNo },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = model.TARecordLocator },
                    new SqlParameter { ParameterName = "@CompanyId", Value = model.CompanyId },
                    new SqlParameter { ParameterName = "@GroupId", Value = model.GroupId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = model.BlockCodeId },
                    new SqlParameter { ParameterName = "@SourceId", Value = model.SourceId },
                    new SqlParameter { ParameterName = "@AgentId", Value = model.AgentId },
                    new SqlParameter { ParameterName = "@ContactId", Value = model.ContactId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = model.MemberTypeId },
                    new SqlParameter { ParameterName = "@MemberNo", Value = model.MemberNo },
                    new SqlParameter { ParameterName = "@ArrivalFrom", Value = model.ArrivalFrom },
                    new SqlParameter { ParameterName = "@ArrivalTo", Value = model.ArrivalTo },
                    new SqlParameter { ParameterName = "@ConfirmationNo", Value = model.ConfirmationNo },
                    new SqlParameter { ParameterName = "@IsShowCancelledReservation", Value = model.IsShowCancelledReservation },
                    new SqlParameter { ParameterName = "@IsShowCheckedInReservation", Value = model.IsShowCheckedInReservation },
                    new SqlParameter { ParameterName = "@IsShowCheckedOutReservation", Value = model.IsShowCheckedOutReservation },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@IsAdminUser", Value = model.IsAdminUser }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchReservation", parameters);

            var reservations = new List<SearchReservationResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchReservationResultVM>(dt);

            return reservations;
        }

        public ReservationVM GetLastReservationByDate(DateTime? date)
        {
            SqlParameter[] parameters =
            {
                    new SqlParameter { ParameterName = "@Date", Value = date }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetLastReservationByDate", parameters);

            var reservation = new ReservationVM();
            reservation = DALHelper.CreateListFromTable<ReservationVM>(dt).FirstOrDefault();

            return reservation;
        }

        public string UpdateReservationCheckInFlag(Guid reservationId, bool isCheckIn, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@IsCheckIn", Value = isCheckIn },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationCheckInFlag", parameters));

            return id;
        }

        public string UpdateReservationCheckOutFlag(Guid reservationId, bool isCheckOut, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@IsCheckOut", Value = isCheckOut },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationCheckOutFlag", parameters));

            return id;
        }

        public string UpdateReservationTotalBalance(Guid id, double totalBalance, int updatedBy)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@TotalBalance", Value = totalBalance },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationTotalBalance", parameters));

            return reservationId;
        }

        public string UpdateReservationTotalPrice(Guid id, double totalPrice, int updatedBy)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = totalPrice },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationTotalPrice", parameters));

            return reservationId;
        }

        public string UpdateReservationStatus(Guid id, Guid? reservationStatusId, int updatedBy)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@ReservationStatusId", Value = reservationStatusId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationStatus", parameters));

            return reservationId;
        }

        public void DeleteAllReservation(int updatedBy, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            DALHelper.ExecuteScalar("DeleteAllReservation", parameters);
        }

        public void DeleteReservationAndRoomMappingByRoom(Guid roomId, Guid reservationId, int updatedBy, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            DALHelper.ExecuteScalar("DeleteReservationAndRoomMappingByRoom", parameters);
        }

        public List<SearchUsersReservationLogResultVM> SearchReservationByUserId(SearchUsersReservationLogParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId },
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@StartDate", Value = model.StartDate },
                    new SqlParameter { ParameterName = "@EndDate", Value = model.EndDate },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchReservationByUserId", parameters);

            var reservations = new List<SearchUsersReservationLogResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchUsersReservationLogResultVM>(dt);

            return reservations;
        }

        public List<SearchUsersReservationFolioLogResultVM> SearchCheckedOutReservationByUserId(SearchUsersReservationFolioLogParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId",   Value = model.UserId },
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@StartDate", Value = model.StartDate },
                    new SqlParameter { ParameterName = "@EndDate", Value = model.EndDate },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCheckedOutReservationByUserId", parameters);

            var reservations = new List<SearchUsersReservationFolioLogResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchUsersReservationFolioLogResultVM>(dt);

            return reservations;
        }

        public void DeleteReservationByUserId(int userId, int updatedBy, bool isDummyReservation = false)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@IsDummyReservation", Value = isDummyReservation }
                };

            DALHelper.ExecuteScalar("DeleteReservationByUserId", parameters);
        }

        public List<ReservationVM> GetUsersReservationByDate(DateTime date, int userId, bool isDummyReservation = false)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Date", Value = date },
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@IsDummyReservation", Value = isDummyReservation }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUsersReservationByDate", parameters);


            var reservations = new List<ReservationVM>();
            reservations = DALHelper.CreateListFromTable<ReservationVM>(dt);

            return reservations;
        }

        public string UpdateTrackFolioLogStatusByReservationId(Guid id, bool isTrackFolioLog)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = id },
                    new SqlParameter { ParameterName = "@IsTrackFolioLog", Value = isTrackFolioLog }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateTrackFolioLogStatusByReservationId", parameters));

            return reservationId;
        }

        public string UpdateTrackReservationLogStatusByReservationId(Guid id, bool isTrackReservationLog)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = id },
                    new SqlParameter { ParameterName = "@IsTrackReservationLog", Value = isTrackReservationLog }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateTrackReservationLogStatusByReservationId", parameters));

            return reservationId;
        }

        #endregion

        #region Room Plan

        public List<RoomPlanReservationDetailVM> GetReservationDetailsForRoomPlan(Guid? roomId, string startDate, string endDate, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@StartDate", Value = startDate },
                    new SqlParameter { ParameterName = "@EndDate", Value = endDate },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationDetailsForRoomPlan", parameters);

            var reservationDetails = new List<RoomPlanReservationDetailVM>();
            reservationDetails = DALHelper.CreateListFromTable<RoomPlanReservationDetailVM>(dt);

            return reservationDetails;
        }

        public string ChangeRoomAllocation(Guid reservationId, Guid roomId, DateTime? arrivalDate, DateTime? departureDate, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = arrivalDate },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = departureDate },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("ChangeRoomAllocation", parameters));

            return id;
        }

        #endregion

        #region Reservation Status
        public List<ReservationStatusVM> GetReservationStatusById(Guid reservationId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationStatusById", parameters);


            var reservationStatus = new List<ReservationStatusVM>();
            reservationStatus = DALHelper.CreateListFromTable<ReservationStatusVM>(dt);

            return reservationStatus;
        }
        #endregion

        #region Reservation Remarks

        public List<ReservationRemarksResultVM> GetReservationRemarks(Guid reservationId, Guid? id, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationRemarks", parameters);

            var remarks = new List<ReservationRemarksResultVM>();
            remarks = DALHelper.CreateListFromTable<ReservationRemarksResultVM>(dt);

            return remarks;
        }

        public List<ReservationRemarkVM> GetReservationRemarkById(Guid id, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationRemarkById", parameters);

            var remark = new List<ReservationRemarkVM>();
            remark = DALHelper.CreateListFromTable<ReservationRemarkVM>(dt);

            return remark;
        }

        public string AddReservationRemark(ReservationRemarkVM remark)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = remark.ReservationId },
                    new SqlParameter { ParameterName = "@Remarks", Value = remark.Remarks },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = remark.CreatedBy },
                    new SqlParameter { ParameterName = "@CreatedOn", Value = remark.CreatedOn },
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("AddReservationRemark", parameters));

            return remarkId;
        }

        public string UpdateReservationRemark(ReservationRemarkVM remark)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = remark.Id },
                    new SqlParameter { ParameterName = "@ReservationId", Value = remark.ReservationId },
                    new SqlParameter { ParameterName = "@Remarks", Value = remark.Remarks },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = remark.UpdatedBy },
                    new SqlParameter { ParameterName = "@UpdatedOn", Value = remark.UpdatedOn },
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationRemark", parameters));

            return remarkId;
        }

        public string DeleteReservationRemark(Guid id, int updatedBy, int userId)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationRemark", parameters));

            return remarkId;
        }

        #endregion

        #region Reservation Package Mapping 

        public string AddUpdateReservationPackageMapping(ReservationPackageMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = model.ReservationId },
                    new SqlParameter { ParameterName = "@PackageId", Value = model.PackageId } ,
                    new SqlParameter { ParameterName = "@Qty", Value = model.Qty } ,
                    new SqlParameter { ParameterName = "@PostingRhythm", Value = model.PostingRhythm } ,
                    new SqlParameter { ParameterName = "@BeginDate", Value = model.BeginDate } ,
                    new SqlParameter { ParameterName = "@EndDate", Value = model.EndDate } ,
                    new SqlParameter { ParameterName = "@TotalAmount", Value = model.TotalAmount } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateReservationPackageMapping", parameters));

            return id;
        }

        public string DeleteReservationPackageMappingByReservation(Guid reservationId, int updatedBy, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationPackageMappingByReservation", parameters));

            return id;
        }

        public string DeleteReservationPackageMapping(Guid id, int updatedBy, int userId)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationPackageMapping", parameters));

            return mappingId;
        }

        public List<ReservationPackageMappingVM> GetReservationPackageMapping(Guid? reservationId, Guid? packageId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = (object) reservationId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@PackageId", Value = (object) packageId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationPackageMapping", parameters);

            var reservationPackageMapping = new List<ReservationPackageMappingVM>();
            reservationPackageMapping = DALHelper.CreateListFromTable<ReservationPackageMappingVM>(dt);

            return reservationPackageMapping;
        }

        #endregion

        #region Reservation Add Ons Mapping 

        public string AddUpdateReservationAddOnsMapping(ReservationAddOnsMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = model.ReservationId },
                    new SqlParameter { ParameterName = "@AddOnsId", Value = model.AddOnsId } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateReservationAddOnsMapping", parameters));

            return id;
        }

        public string DeleteReservationAddOnsMappingByReservation(Guid reservationId, int updatedBy, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationAddOnsMappingByReservation", parameters));

            return id;
        }

        public string DeleteReservationAddOnsMapping(Guid id, int updatedBy, int userId)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationAddOnsMapping", parameters));

            return mappingId;
        }

        public List<ReservationAddOnsMappingVM> GetReservationAddOnsMapping(Guid? reservationId, Guid? addOnsId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = (object) reservationId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@AddOnsId", Value = (object) addOnsId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationAddOnsMapping", parameters);

            var reservationAddOnsMapping = new List<ReservationAddOnsMappingVM>();
            reservationAddOnsMapping = DALHelper.CreateListFromTable<ReservationAddOnsMappingVM>(dt);

            return reservationAddOnsMapping;
        }

        #endregion

        #region Temp Bulk Reservation

        public List<TempBulkReservationVM> GetTempBulkReservation(int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTempBulkReservation", parameters);

            var reservations = new List<TempBulkReservationVM>();
            reservations = DALHelper.CreateListFromTable<TempBulkReservationVM>(dt);

            return reservations;
        }

        public List<TempBulkReservationVM> GetTempBulkReservationByDate(DateTime date, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = date },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTempBulkReservationByDate", parameters);

            var reservations = new List<TempBulkReservationVM>();
            reservations = DALHelper.CreateListFromTable<TempBulkReservationVM>(dt);

            return reservations;
        }

        public TempBulkReservationVM GetTempBulkReservationById(Guid reservationId, int userId)
        {
            SqlParameter[] parameters =
              {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
             };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTempBulkReservationById", parameters);


            var reservation = new TempBulkReservationVM();
            reservation = DALHelper.CreateListFromTable<TempBulkReservationVM>(dt).FirstOrDefault();

            return reservation;
        }

        public string UpdateTempBulkReservation(TempBulkReservationVM reservation)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservation.Id },
                    new SqlParameter { ParameterName = "@TitleId", Value = reservation.TitleId },
                    new SqlParameter { ParameterName = "@LastName", Value = reservation.LastName } ,
                    new SqlParameter { ParameterName = "@FirstName", Value = reservation.FirstName },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservation.ProfileId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = reservation.MemberTypeId },
                    new SqlParameter { ParameterName = "@CountryId", Value = reservation.CountryId },
                    new SqlParameter { ParameterName = "@LanguageId", Value = reservation.LanguageId },
                    new SqlParameter { ParameterName = "@VipId", Value = reservation.VipId },
                    new SqlParameter { ParameterName = "@PhoneNo", Value = reservation.PhoneNo },
                    new SqlParameter { ParameterName = "@MemberNo", Value = reservation.MemberNo },
                    new SqlParameter { ParameterName = "@MemberLvt", Value = reservation.MemberLvt },
                    new SqlParameter { ParameterName = "@AgentId", Value = reservation.AgentId },
                    new SqlParameter { ParameterName = "@CompanyId", Value = reservation.CompanyId },
                    new SqlParameter { ParameterName = "@GroupId", Value = reservation.GroupId },
                    new SqlParameter { ParameterName = "@SourceId", Value = reservation.SourceId },
                    new SqlParameter { ParameterName = "@ContactId", Value = reservation.ContactId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = reservation.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = reservation.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = reservation.DepartureDate },
                    new SqlParameter { ParameterName = "@NoOfAdult", Value = reservation.NoOfAdult },
                    new SqlParameter { ParameterName = "@NoOfChildren", Value = reservation.NoOfChildren },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = reservation.NoOfRoom },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = reservation.RoomTypeId },
                    new SqlParameter { ParameterName = "@RtcId", Value = reservation.RtcId },
                    new SqlParameter { ParameterName = "@ExtnId", Value = reservation.ExtnId },
                    new SqlParameter { ParameterName = "@RateCodeId", Value = reservation.RateCodeId },
                    new SqlParameter { ParameterName = "@IsFixedRate", Value = reservation.IsFixedRate },
                    new SqlParameter { ParameterName = "@Rate", Value = reservation.Rate },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = reservation.CurrencyId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = reservation.BlockCodeId },
                    new SqlParameter { ParameterName = "@ETA", Value = reservation.ETA },
                    new SqlParameter { ParameterName = "@ReservationTypeId", Value = reservation.ReservationTypeId },
                    new SqlParameter { ParameterName = "@MarketId", Value = reservation.MarketId },
                    new SqlParameter { ParameterName = "@ReservationSourceId", Value = reservation.ReservationSourceId },
                    new SqlParameter { ParameterName = "@OriginId", Value = reservation.OriginId },
                    new SqlParameter { ParameterName = "@PaymentMethodId", Value = reservation.PaymentMethodId },
                    new SqlParameter { ParameterName = "@CreditCardNo", Value = reservation.CreditCardNo },
                    new SqlParameter { ParameterName = "@CardExpiryDate", Value = reservation.CardExpiryDate },
                    new SqlParameter { ParameterName = "@CVVNo", Value = reservation.CVVNo },
                    new SqlParameter { ParameterName = "@ApprovalCode", Value = reservation.ApprovalCode },
                    new SqlParameter { ParameterName = "@ApprovalAmount", Value = reservation.ApprovalAmount },
                    new SqlParameter { ParameterName = "@SuitWith", Value = reservation.SuitWith },
                    new SqlParameter { ParameterName = "@IsEmailConfirmation", Value = reservation.IsEmailConfirmation },
                    new SqlParameter { ParameterName = "@GuestBalance", Value = reservation.GuestBalance },
                    new SqlParameter { ParameterName = "@DiscountAmount", Value = reservation.DiscountAmount },
                    new SqlParameter { ParameterName = "@DiscountPercentage", Value = reservation.DiscountPercentage },
                    new SqlParameter { ParameterName = "@DiscountApprovedBy", Value = reservation.DiscountApprovedBy },
                    new SqlParameter { ParameterName = "@DiscountReason", Value = reservation.DiscountReason },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@RoomId", Value = reservation.RoomId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = reservation.RoomNo },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservation.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservation.UpdatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateTempBulkReservation", parameters));

            return reservationId;
        }

        public void LoadTempBulkReservation(int userId, DateTime? currentDate)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@CurrentDate", Value = currentDate }
                };

            DALHelper.Execute("LoadTempBulkReservation", parameters);

        }

        public List<SearchBulkReservationResultVM> SearchBulkReservation(SearchBulkReservationParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@ArrivalFrom", Value = model.ArrivalFrom },
                    new SqlParameter { ParameterName = "@ArrivalTo", Value = model.ArrivalTo },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = model.RateTypeId },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = model.RoomNo },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchBulkReservation", parameters);

            var reservations = new List<SearchBulkReservationResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchBulkReservationResultVM>(dt);

            return reservations;
        }


        public TempBulkReservationMasterVM GetTempBulkReservationMasterById(Guid reservationId, int userId)
        {
            SqlParameter[] parameters =
              {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
             };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTempBulkReservationMasterById", parameters);


            var reservation = new TempBulkReservationMasterVM();
            reservation = DALHelper.CreateListFromTable<TempBulkReservationMasterVM>(dt).FirstOrDefault();

            return reservation;
        }

        public string UpdateTempBulkReservationMaster(TempBulkReservationMasterVM reservation)
        {
            string reservationId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservation.Id },
                    new SqlParameter { ParameterName = "@TitleId", Value = reservation.TitleId },
                    new SqlParameter { ParameterName = "@LastName", Value = reservation.LastName } ,
                    new SqlParameter { ParameterName = "@FirstName", Value = reservation.FirstName },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservation.ProfileId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = reservation.MemberTypeId },
                    new SqlParameter { ParameterName = "@CountryId", Value = reservation.CountryId },
                    new SqlParameter { ParameterName = "@LanguageId", Value = reservation.LanguageId },
                    new SqlParameter { ParameterName = "@VipId", Value = reservation.VipId },
                    new SqlParameter { ParameterName = "@PhoneNo", Value = reservation.PhoneNo },
                    new SqlParameter { ParameterName = "@MemberNo", Value = reservation.MemberNo },
                    new SqlParameter { ParameterName = "@MemberLvt", Value = reservation.MemberLvt },
                    new SqlParameter { ParameterName = "@AgentId", Value = reservation.AgentId },
                    new SqlParameter { ParameterName = "@CompanyId", Value = reservation.CompanyId },
                    new SqlParameter { ParameterName = "@GroupId", Value = reservation.GroupId },
                    new SqlParameter { ParameterName = "@SourceId", Value = reservation.SourceId },
                    new SqlParameter { ParameterName = "@ContactId", Value = reservation.ContactId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = reservation.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = reservation.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = reservation.DepartureDate },
                    new SqlParameter { ParameterName = "@NoOfAdult", Value = reservation.NoOfAdult },
                    new SqlParameter { ParameterName = "@NoOfChildren", Value = reservation.NoOfChildren },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = reservation.NoOfRoom },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = reservation.RoomTypeId },
                    new SqlParameter { ParameterName = "@RtcId", Value = reservation.RtcId },
                    new SqlParameter { ParameterName = "@ExtnId", Value = reservation.ExtnId },
                    new SqlParameter { ParameterName = "@RateCodeId", Value = reservation.RateCodeId },
                    new SqlParameter { ParameterName = "@IsFixedRate", Value = reservation.IsFixedRate },
                    new SqlParameter { ParameterName = "@Rate", Value = reservation.Rate },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = reservation.CurrencyId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = reservation.BlockCodeId },
                    new SqlParameter { ParameterName = "@ETA", Value = reservation.ETA },
                    new SqlParameter { ParameterName = "@ReservationTypeId", Value = reservation.ReservationTypeId },
                    new SqlParameter { ParameterName = "@MarketId", Value = reservation.MarketId },
                    new SqlParameter { ParameterName = "@ReservationSourceId", Value = reservation.ReservationSourceId },
                    new SqlParameter { ParameterName = "@OriginId", Value = reservation.OriginId },
                    new SqlParameter { ParameterName = "@PaymentMethodId", Value = reservation.PaymentMethodId },
                    new SqlParameter { ParameterName = "@CreditCardNo", Value = reservation.CreditCardNo },
                    new SqlParameter { ParameterName = "@CardExpiryDate", Value = reservation.CardExpiryDate },
                    new SqlParameter { ParameterName = "@CVVNo", Value = reservation.CVVNo },
                    new SqlParameter { ParameterName = "@ApprovalCode", Value = reservation.ApprovalCode },
                    new SqlParameter { ParameterName = "@ApprovalAmount", Value = reservation.ApprovalAmount },
                    new SqlParameter { ParameterName = "@SuitWith", Value = reservation.SuitWith },
                    new SqlParameter { ParameterName = "@IsEmailConfirmation", Value = reservation.IsEmailConfirmation },
                    new SqlParameter { ParameterName = "@GuestBalance", Value = reservation.GuestBalance },
                    new SqlParameter { ParameterName = "@DiscountAmount", Value = reservation.DiscountAmount },
                    new SqlParameter { ParameterName = "@DiscountPercentage", Value = reservation.DiscountPercentage },
                    new SqlParameter { ParameterName = "@DiscountApprovedBy", Value = reservation.DiscountApprovedBy },
                    new SqlParameter { ParameterName = "@DiscountReason", Value = reservation.DiscountReason },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@RoomId", Value = reservation.RoomId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = reservation.RoomNo },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservation.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservation.UpdatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateTempBulkReservationMaster", parameters));

            return reservationId;
        }

        public List<TempBulkReservationMasterVM> CheckRoomNoExistInTempBulkReservationMaster(Guid? reservationId, string roomNo)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = roomNo }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomNoExistInTempBulkReservationMaster", parameters);

            var reservation = new List<TempBulkReservationMasterVM>();
            reservation = DALHelper.CreateListFromTable<TempBulkReservationMasterVM>(dt);

            return reservation;
        }

        #endregion

        #region Registration  Card

        public List<SearchPendingCheckInReservationResultVM> SearchPendingCheckInReservation(SearchPendingCheckInReservationParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = model.TARecordLocator },
                    new SqlParameter { ParameterName = "@CompanyId", Value = model.CompanyId },
                    new SqlParameter { ParameterName = "@ContactId", Value = model.ContactId },
                    new SqlParameter { ParameterName = "@GroupId", Value = model.GroupId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = model.BlockCodeId },
                    new SqlParameter { ParameterName = "@AgentId", Value = model.AgentId },
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = model.MemberTypeId },
                    new SqlParameter { ParameterName = "@MemberNo", Value = model.MemberNo },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = model.ArrivalDate },
                    new SqlParameter { ParameterName = "@ConfirmationNo", Value = model.ConfirmationNo },
                    new SqlParameter { ParameterName = "@PostalCode", Value = model.PostalCode },
                    new SqlParameter { ParameterName = "@RoomNumber", Value = model.RoomNumber },
                    new SqlParameter { ParameterName = "@IsShowCheckedIn", Value = model.IsShowCheckedIn },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@IsAdminUser", Value = model.IsAdminUser }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchPendingCheckInReservation", parameters);

            var reservations = new List<SearchPendingCheckInReservationResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchPendingCheckInReservationResultVM>(dt);

            return reservations;
        }

        #endregion

        #region Breakfast Report

        public List<SearchBreakFastReportResultVM> SearchBreakfastReport(SearchBreakFastReportParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = model.Date },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchBreakfastReport", parameters);

            var reservations = new List<SearchBreakFastReportResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchBreakFastReportResultVM>(dt);

            return reservations;
        }

        public List<SearchBreakFastReportResultVM> GetBreakfastReport(string date, int? userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = !string.IsNullOrWhiteSpace(date) ? date : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetBreakfastReport", parameters);

            var reservations = new List<SearchBreakFastReportResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchBreakFastReportResultVM>(dt);

            return reservations;
        }

        #endregion

        #region "CHECK PREVIOUS RESERVATION OR NOT"
        public List<GetPreviousReservationOrNotVM> GetPreviousReservationOrNot(Guid RoomId, DateTime? CheckInDate, TimeSpan? CheckInTime, Guid? ReservationId, int? userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomId", Value = RoomId },
                    new SqlParameter { ParameterName = "@CheckInDate", Value = CheckInDate },
                    new SqlParameter { ParameterName = "@CheckInTime", Value = CheckInTime },
                    new SqlParameter { ParameterName = "@ReservationId", Value = ReservationId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreviousReservationOrNot", parameters);

            var results = new List<GetPreviousReservationOrNotVM>();
            results = DALHelper.CreateListFromTable<GetPreviousReservationOrNotVM>(dt);

            return results;
        }
        #endregion

        #region "GET ROOM ALLOCATION DETAIL FOR ROOM PLAN"
        public List<RoomPlanRoomAllocationDetailVM> GetRoomAllocationDetailsForRoomPlan(Guid? roomId, string startDate, string endDate, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = startDate },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = endDate },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomAllocationDetailsForRoomPlan", parameters);

            var AllocationDetails = new List<RoomPlanRoomAllocationDetailVM>();
            AllocationDetails = DALHelper.CreateListFromTable<RoomPlanRoomAllocationDetailVM>(dt);

            return AllocationDetails;
        }
        #endregion


        #region
        public List<GetRateSheetByRoomTypeVM> GetRateTypeOrderBy(Guid RoomTypeId, bool IsShowWeekEndPrice, bool IsShowCorporateRate)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = RoomTypeId },                   
                    new SqlParameter { ParameterName = "@IsShowWeekEndPrice", Value = IsShowWeekEndPrice},
                     new SqlParameter { ParameterName = "@IsShowCorporateRate", Value = IsShowCorporateRate}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeOrderBy", parameters);

            var results = new List<GetRateSheetByRoomTypeVM>();
            results = DALHelper.CreateListFromTable<GetRateSheetByRoomTypeVM>(dt);

            return results;
        }
        #endregion

        #region
        public List<GetRateSheetByRoomTypeVM> GetRateSheetByRoomType(bool IsShowWeekEndPrice, bool IsShowCorporateRate)
        {
            SqlParameter[] parameters =
               {                   
                    new SqlParameter { ParameterName = "@IsShowWeekEndPrice", Value = IsShowWeekEndPrice},
                    new SqlParameter { ParameterName = "@IsShowCorporateRate", Value = IsShowCorporateRate},
                }; 

             var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateSheetByRoomType", parameters);

            var results = new List<GetRateSheetByRoomTypeVM>();
            results = DALHelper.CreateListFromTable<GetRateSheetByRoomTypeVM>(dt);

            return results;
        }
        #endregion                
    }
}

