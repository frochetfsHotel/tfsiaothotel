using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ReservationLogRepository
    {
        #region ReservationLog

        public List<ReservationLogVM> GetReservationLogById(Guid reservationLogId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationLogId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationLogById", parameters);

            var reservationLog = new List<ReservationLogVM>();
            reservationLog = DALHelper.CreateListFromTable<ReservationLogVM>(dt);

            return reservationLog;
        }

        public List<ReservationLogVM> GetReservationLogDetails(Guid? reservationId, Guid? roomId, Guid? reservationLogId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationLogId },
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId }

                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationLogDetails", parameters);

            var reservationLog = new List<ReservationLogVM>();
            reservationLog = DALHelper.CreateListFromTable<ReservationLogVM>(dt);

            return reservationLog;
        }

        public string AddReservationLog(ReservationLogVM reservationLog)
        {
            string reservationLogId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationLog.ReservationId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservationLog.ProfileId },
                    new SqlParameter { ParameterName = "@RoomId", Value = reservationLog.RoomId },
                    new SqlParameter { ParameterName = "@RoomStatusId", Value = reservationLog.RoomStatusId },
                    new SqlParameter { ParameterName = "@CheckInDate", Value = reservationLog.CheckInDate },
                    new SqlParameter { ParameterName = "@CheckInTime", Value = reservationLog.CheckInTime },
                    new SqlParameter { ParameterName = "@CheckOutDate", Value = reservationLog.CheckOutDate },
                    new SqlParameter { ParameterName = "@CheckOutTime", Value = reservationLog.CheckOutTime },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationLog.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = reservationLog.CreatedBy }
                };

            reservationLogId = Convert.ToString(DALHelper.ExecuteScalar("AddReservationLog", parameters));

            return reservationLogId;
        }

        public string UpdateReservationLog(ReservationLogVM reservationLog)
        {
            string reservationLogId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationLog.Id },
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationLog.ReservationId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = reservationLog.ProfileId },
                    new SqlParameter { ParameterName = "@RoomId", Value = reservationLog.RoomId },
                    new SqlParameter { ParameterName = "@RoomStatusId", Value = reservationLog.RoomStatusId },
                    new SqlParameter { ParameterName = "@CheckInDate", Value = reservationLog.CheckInDate },
                    new SqlParameter { ParameterName = "@CheckInTime", Value = reservationLog.CheckInTime },
                    new SqlParameter { ParameterName = "@CheckOutDate", Value = reservationLog.CheckOutDate },
                    new SqlParameter { ParameterName = "@CheckOutTime", Value = reservationLog.CheckOutTime },
                    new SqlParameter { ParameterName = "@IsActive", Value = reservationLog.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = reservationLog.UpdatedBy }
                };

            reservationLogId = Convert.ToString(DALHelper.ExecuteScalar("UpdateReservationLog", parameters));

            return reservationLogId;
        }

        public string DeleteReservationLog(int id, int updatedBy)
        {
            string reservationLogId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            reservationLogId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationLog", parameters));

            return reservationLogId;
        }

        #endregion
    }
}