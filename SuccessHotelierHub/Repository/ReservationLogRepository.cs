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

        public List<ReservationLogVM> GetReservationLogById(Guid reservationLogId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationLogId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationLogById", parameters);

            var reservationLog = new List<ReservationLogVM>();
            reservationLog = DALHelper.CreateListFromTable<ReservationLogVM>(dt);

            return reservationLog;
        }

        public List<ReservationLogVM> GetReservationLogDetails(Guid? reservationId, Guid? roomId, Guid? reservationLogId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = reservationLogId },
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }

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

        public string DeleteReservationLog(Guid id, int updatedBy, int userId)
        {
            string reservationLogId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            reservationLogId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationLog", parameters));

            return reservationLogId;
        }

        public string DeleteReservationLogByReservation(Guid reservationId, int updatedBy, int userId)
        {
            string reservationLogId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            reservationLogId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationLogByReservation", parameters));

            return reservationLogId;
        }

        public List<ReservationLogVM> GetReservationLogByRoom(Guid? roomId, Guid? reservationId, Guid? roomStatusId, DateTime? arrivaldate, DateTime? departureDate, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@RoomStatusId", Value = roomStatusId },
                    new SqlParameter { ParameterName = "@Arrivaldate", Value = arrivaldate },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = departureDate },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }

                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationLogByRoom", parameters);

            var reservationLog = new List<ReservationLogVM>();
            reservationLog = DALHelper.CreateListFromTable<ReservationLogVM>(dt);

            return reservationLog;
        }


        #endregion
    }
}