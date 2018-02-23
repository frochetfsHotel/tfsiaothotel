using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
namespace SuccessHotelierHub.Repository
{
    public class CheckInCheckOutRepository
    {
        #region Check In

        public List<SearchArrivalsResultVM> SearchArrivals(SearchArrivalsParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchArrivals", parameters);

            var reservations = new List<SearchArrivalsResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchArrivalsResultVM>(dt);

            return reservations;
        }

        public string AddCheckInDetail(CheckInCheckOutVM checkInCheckOut)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = checkInCheckOut.ReservationId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = checkInCheckOut.ProfileId },
                    new SqlParameter { ParameterName = "@CheckInDate", Value = checkInCheckOut.CheckInDate },
                    new SqlParameter { ParameterName = "@CheckInTime", Value = checkInCheckOut.CheckInTime },
                    new SqlParameter { ParameterName = "@IsActive", Value = checkInCheckOut.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = checkInCheckOut.CreatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddCheckInDetail", parameters));

            return id;
        }

        public List<CheckInCheckOutVM> GetCheckInDetails(Guid reservationId, Guid profileId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = profileId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCheckInDetails", parameters);

            var checkInDetails = new List<CheckInCheckOutVM>();
            checkInDetails = DALHelper.CreateListFromTable<CheckInCheckOutVM>(dt);

            return checkInDetails;
        }


        #endregion

        #region Check Out

        public List<SearchGuestResultVM> SearchGuest(SearchGuestParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@RoomNo", Value = model.RoomNo },
                    new SqlParameter { ParameterName = "@CompanyId", Value = model.CompanyId },                    
                    new SqlParameter { ParameterName = "@GroupId", Value = model.GroupId },
                    new SqlParameter { ParameterName = "@BlockCodeId", Value = model.BlockCodeId },                    
                    new SqlParameter { ParameterName = "@ConfirmationNo", Value = model.ConfirmationNo },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = model.DepartureDate },
                    new SqlParameter { ParameterName = "@IsShowCheckedOut", Value = model.IsShowCheckedOut },
                    new SqlParameter { ParameterName = "@IsShowCheckedIn", Value = model.IsShowCheckedIn },
                    new SqlParameter { ParameterName = "@IsShowDueOut", Value = model.IsShowDueOut },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@IsAdminUser", Value = model.IsAdminUser }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchGuest", parameters);

            var reservations = new List<SearchGuestResultVM>();
            reservations = DALHelper.CreateListFromTable<SearchGuestResultVM>(dt);

            return reservations;
        }

        public string UpdateCheckOutDetail(CheckInCheckOutVM checkInCheckOut)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = checkInCheckOut.Id },
                    new SqlParameter { ParameterName = "@ReservationId", Value = checkInCheckOut.ReservationId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = checkInCheckOut.ProfileId },
                    new SqlParameter { ParameterName = "@CheckOutDate", Value = checkInCheckOut.CheckOutDate },
                    new SqlParameter { ParameterName = "@CheckOutTime", Value = checkInCheckOut.CheckOutTime },
                    new SqlParameter { ParameterName = "@CheckOutReference", Value = checkInCheckOut.CheckOutReference },
                    new SqlParameter { ParameterName = "@IsActive", Value = checkInCheckOut.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = checkInCheckOut.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateCheckOutDetail", parameters));

            return id;
        }

        #endregion Check Out

        public string DeleteCheckInCheckOutDetailByReservation(Guid reservationId, int updatedBy, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteCheckInCheckOutDetailByReservation", parameters));

            return id;
        }
    }
}