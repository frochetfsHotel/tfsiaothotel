using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

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

            var rateSheetResultList = new List<RateSheetResultVM> ();
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
                    new SqlParameter { ParameterName = "@PackageId", Value = reservation.PackageId },
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
                    new SqlParameter { ParameterName = "@DiscountReasonId", Value = reservation.DiscountReasonId },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@ConfirmationNumber", Value = reservation.ConfirmationNumber },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@TotalBalance", Value = reservation.TotalBalance },
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
                    new SqlParameter { ParameterName = "@PackageId", Value = reservation.PackageId },
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
                    new SqlParameter { ParameterName = "@DiscountReasonId", Value = reservation.DiscountReasonId },
                    new SqlParameter { ParameterName = "@TARecordLocator", Value = reservation.TARecordLocator },
                    new SqlParameter { ParameterName = "@SpecialsId", Value = reservation.SpecialsId },
                    new SqlParameter { ParameterName = "@ReservationComments", Value = reservation.ReservationComments },
                    new SqlParameter { ParameterName = "@InHouseComments", Value = reservation.InHouseComments },
                    new SqlParameter { ParameterName = "@CashieringComments", Value = reservation.CashieringComments },
                    new SqlParameter { ParameterName = "@HouseKeepingComments", Value = reservation.HouseKeepingComments },
                    new SqlParameter { ParameterName = "@ItemInventoryId", Value = reservation.ItemInventoryId },
                    new SqlParameter { ParameterName = "@Remarks", Value = reservation.Remarks },
                    new SqlParameter { ParameterName = "@TotalPrice", Value = reservation.TotalPrice },
                    new SqlParameter { ParameterName = "@TotalBalance", Value = reservation.TotalBalance },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservation.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservation.UpdatedBy }
                };

            reservationId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservation", parameters));

            return reservationId;
        }

        public string DeleteReservation(Guid reservationId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
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

        public List<ReservationVM> GetReservationById(Guid reservationId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationId }
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
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
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

        #endregion



    }
}
