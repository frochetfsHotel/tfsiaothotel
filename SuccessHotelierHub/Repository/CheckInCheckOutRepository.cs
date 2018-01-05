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
                    new SqlParameter { ParameterName = "@ArrivalFrom", Value = model.ArrivalFrom },
                    new SqlParameter { ParameterName = "@ArrivalTo", Value = model.ArrivalTo },
                    new SqlParameter { ParameterName = "@ConfirmationNo", Value = model.ConfirmationNo },
                    new SqlParameter { ParameterName = "@PostalCode", Value = model.PostalCode },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
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

        #endregion
    }
}