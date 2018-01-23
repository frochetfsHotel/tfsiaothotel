using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ReservationSourceRepository
    {
        #region Reservation Source

        public List<ReservationSourceVM> GetReservationSources()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationSources");

            var reservationSources = new List<ReservationSourceVM>();
            reservationSources = DALHelper.CreateListFromTable<ReservationSourceVM>(ds);

            return reservationSources;
        }

        public List<ReservationSourceVM> GetReservationSourceById(Guid reservationSourceId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = reservationSourceId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationSourceById", parameters);

            var reservationSource = new List<ReservationSourceVM>();
            reservationSource = DALHelper.CreateListFromTable<ReservationSourceVM>(dt);

            return reservationSource;
        }

        public string AddReservationSource(ReservationSourceVM reservationSource)
        {
            string reservationSourceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = reservationSource.Name },
                    new SqlParameter { ParameterName = "@Description", Value = reservationSource.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationSource.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = reservationSource.CreatedBy }
                };

            reservationSourceId = Convert.ToString(DALHelper.ExecuteScalar("AddReservationSource", parameters));

            return reservationSourceId;
        }

        public string UpdateReservationSource(ReservationSourceVM reservationSource)
        {
            string reservationSourceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationSource.Id },
                    new SqlParameter { ParameterName = "@Name", Value = reservationSource.Name },
                    new SqlParameter { ParameterName = "@Description", Value = reservationSource.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationSource.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservationSource.UpdatedBy }
                };

            reservationSourceId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationSource", parameters));

            return reservationSourceId;
        }

        public string DeleteReservationSource(Guid id, int updatedBy)
        {
            string reservationSourceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationSourceId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationSource", parameters));

            return reservationSourceId;
        }

        public List<SearchReservationSourceResultVM> SearchReservationSource(SearchReservationSourceParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchReservationSource", parameters);

            var reservationSources = new List<SearchReservationSourceResultVM>();
            reservationSources = DALHelper.CreateListFromTable<SearchReservationSourceResultVM>(dt);

            return reservationSources;
        }

        #endregion
    }
}