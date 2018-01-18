using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ReservationTypeRepository
    {
        #region Reservation Type

        public List<ReservationTypeVM> GetReservationTypes()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationTypes");

            var reservationTypes = new List<ReservationTypeVM>();
            reservationTypes = DALHelper.CreateListFromTable<ReservationTypeVM>(ds);

            return reservationTypes;
        }

        public List<ReservationTypeVM> GetReservationTypeById(Guid reservationTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationTypeById", parameters);

            var reservationType = new List<ReservationTypeVM>();
            reservationType = DALHelper.CreateListFromTable<ReservationTypeVM>(dt);

            return reservationType;
        }

        public string AddReservationType(ReservationTypeVM reservationType)
        {
            string reservationTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = reservationType.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationType.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = reservationType.CreatedBy }
                };

            reservationTypeId = Convert.ToString(DALHelper.ExecuteScalar("AddReservationType", parameters));

            return reservationTypeId;
        }

        public string UpdateReservationType(ReservationTypeVM reservationType)
        {
            string reservationTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationType.Id },
                    new SqlParameter { ParameterName = "@Name", Value = reservationType.Name },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationType.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservationType.UpdatedBy }
                };

            reservationTypeId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationType", parameters));

            return reservationTypeId;
        }

        public string DeleteReservationType(Guid id, int updatedBy)
        {
            string reservationTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationTypeId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationType", parameters));

            return reservationTypeId;
        }

        public List<SearchReservationTypeResultVM> SearchReservationType(SearchReservationTypeParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchReservationType", parameters);

            var reservationTypes = new List<SearchReservationTypeResultVM>();
            reservationTypes = DALHelper.CreateListFromTable<SearchReservationTypeResultVM>(dt);

            return reservationTypes;
        }

        #endregion
    }
}