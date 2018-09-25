using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;


namespace SuccessHotelierHub.Repository
{
    public class UsersActivityLogRepository
    {
        #region User Activity Log

        public List<UsersActivityLogVM> GetUsersActivityLog()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetUsersActivityLog");

            var activities = new List<UsersActivityLogVM>();
            activities = DALHelper.CreateListFromTable<UsersActivityLogVM>(ds);

            return activities;
        }

        public string AddUsersActivityLog(UsersActivityLogVM usersActivityLog)
        {
            string usersActivityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = usersActivityLog.UserId},
                    new SqlParameter { ParameterName = "@Url", Value = usersActivityLog.Url },
                    new SqlParameter { ParameterName = "@PageName", Value = usersActivityLog.PageName },
                    new SqlParameter { ParameterName = "@Description", Value = usersActivityLog.Description },
                    new SqlParameter { ParameterName = "@IpAddress", Value = usersActivityLog.IpAddress },
                    new SqlParameter { ParameterName = "@RecordedOn", Value = usersActivityLog.RecordedOn },
                    new SqlParameter { ParameterName = "@SessionId", Value = usersActivityLog.SessionId },
                    new SqlParameter { ParameterName = "@IsActive", Value = usersActivityLog.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = usersActivityLog.CreatedBy }
                };

            usersActivityId = Convert.ToString(DALHelper.ExecuteScalar("AddUsersActivityLog", parameters));

            return usersActivityId;
        }

        public string DeleteUsersActivityLog(Guid id, int updatedBy)
        {
            string usersActivityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            usersActivityId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUsersActivityLog", parameters));

            return usersActivityId;
        }

        public List<SearchUsersActivityLogResultVM> SearchUsersActivityLog(SearchUsersActivityLogParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId },
                    new SqlParameter { ParameterName = "@StartDate", Value = model.StartDate },
                    new SqlParameter { ParameterName = "@EndDate", Value = model.EndDate },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchUsersActivityLog", parameters);

            var activities = new List<SearchUsersActivityLogResultVM>();
            activities = DALHelper.CreateListFromTable<SearchUsersActivityLogResultVM>(dt);

            return activities;
        }

        public string UpdateLoggedInDurationInUsersActivity(Guid id, double? loggedInDurationInSeconds, int updatedBy)
        {
            string usersActivityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@LoggedInDurationInSeconds", Value = loggedInDurationInSeconds },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            usersActivityId = Convert.ToString(DALHelper.ExecuteScalar("UpdateLoggedInDurationInUsersActivity", parameters));

            return usersActivityId;
        }

        public List<UsersActivityLogVM> GetUsersActivityLogBySessionId(string sessionId)
        {

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@SessionId", Value = sessionId }
                };

            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetUsersActivityLogBySessionId", parameters);

            var activities = new List<UsersActivityLogVM>();
            activities = DALHelper.CreateListFromTable<UsersActivityLogVM>(ds);

            return activities;
        }

        public List<UsersActivityLogVM> GetUsersActivityLogByUserId(Guid userId)
        {

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetUsersActivityLogByUserId", parameters);

            var activities = new List<UsersActivityLogVM>();
            activities = DALHelper.CreateListFromTable<UsersActivityLogVM>(ds);

            return activities;
        }

        public string UpdateSessionIdInUsersActivity(Guid id, string sessionId, int updatedBy)
        {
            string usersActivityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@SessionId", Value = sessionId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            usersActivityId = Convert.ToString(DALHelper.ExecuteScalar("UpdateSessionIdInUsersActivity", parameters));

            return usersActivityId;
        }
        #endregion
    }
}

