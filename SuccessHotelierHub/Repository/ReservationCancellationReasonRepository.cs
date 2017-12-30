using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
namespace SuccessHotelierHub.Repository
{
    public class ReservationCancellationReasonRepository
    {
        public List<ReservationCancellationReasonVM> GetReservationCancellationReasons()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationCancellationReasons");

            var reasons = new List<ReservationCancellationReasonVM>();
            reasons = DALHelper.CreateListFromTable<ReservationCancellationReasonVM>(dt);

            return reasons;
        }

        public List<ReservationCancellationReasonVM> GetReservationCancellationReasonById(Guid id)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationCancellationReasonById", parameters);


            var reason = new List<ReservationCancellationReasonVM>();
            reason = DALHelper.CreateListFromTable<ReservationCancellationReasonVM>(dt);

            return reason;
        }

        public string AddReservationCancellationReason(ReservationCancellationReasonVM reason)
        {
            string reasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = reason.Code },
                    new SqlParameter { ParameterName = "@Description", Value = reason.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = reason.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = reason.CreatedBy }
                };

            reasonId = Convert.ToString(DALHelper.ExecuteScalar("AddReservationCancellationReason", parameters));

            return reasonId;
        }

        public string UpdateReservationCancellationReason(ReservationCancellationReasonVM reason)
        {
            string reasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reason.Id },                    
                    new SqlParameter { ParameterName = "@Code", Value = reason.Code },
                    new SqlParameter { ParameterName = "@Description", Value = reason.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = reason.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reason.CreatedBy }
                };

            reasonId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationCancellationReason", parameters));

            return reasonId;
        }

        public string DeleteReservationCancellationReason(Guid id, int updatedBy)
        {
            string reasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reasonId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationCancellationReason", parameters));

            return reasonId;
        }

        public List<SearchReservationCancellationReasonResultVM> SearchReservationCancellationReason(SearchReservationCancellationReasonParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchReservationCancellationReason", parameters);

            var reasons = new List<SearchReservationCancellationReasonResultVM>();
            reasons = DALHelper.CreateListFromTable<SearchReservationCancellationReasonResultVM>(dt);

            return reasons;
        }


        public List<SearchAdvanceReservationCancellationReasonResultVM> SearchAdvanceReservationCancellationReasons(SearchAdvanceReservationCancellationReasonParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Reason", Value = model.Reason }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceReservationCancellationReasons", parameters);

            var reasons = new List<SearchAdvanceReservationCancellationReasonResultVM>();
            reasons = DALHelper.CreateListFromTable<SearchAdvanceReservationCancellationReasonResultVM>(dt);

            return reasons;
        }

        public List<ReservationCancellationReasonVM> CheckReservationCancellationCodeAvailable(Guid? id, string code)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Code", Value = code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckReservationCancellationCodeAvailable", parameters);

            var reason = new List<ReservationCancellationReasonVM>();
            reason = DALHelper.CreateListFromTable<ReservationCancellationReasonVM>(dt);

            return reason;
        }

    }
}