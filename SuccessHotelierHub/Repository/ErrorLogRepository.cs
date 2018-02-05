using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ErrorLogRepository
    {
        #region Error Log

        public string AddErrorLog(ErrorLogVM erroLog)
        {
            string errorId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@PageUrl", Value = erroLog.PageUrl },
                    new SqlParameter { ParameterName = "@MethodName", Value = erroLog.MethodName },
                    new SqlParameter { ParameterName = "@ErrorMessage", Value = erroLog.ErrorMessage },
                    new SqlParameter { ParameterName = "@TargetSite", Value = erroLog.TargetSite },
                    new SqlParameter { ParameterName = "@StackTrace", Value = erroLog.StackTrace },
                    new SqlParameter { ParameterName = "@InnerException", Value = erroLog.InnerException },
                    new SqlParameter { ParameterName = "@CreatedDateTime", Value = erroLog.CreatedDateTime }
                };

            errorId = Convert.ToString(DALHelper.ExecuteScalar("AddErrorLog", parameters));

            return errorId;
        }

        public List<SearchErrorLogResultVM> SearchErroLog(SearchErrorLogParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Error", Value = model.Error },
                    new SqlParameter { ParameterName = "@Date", Value = model.Date },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchErroLog", parameters);

            var errorLogs = new List<SearchErrorLogResultVM>();
            errorLogs = DALHelper.CreateListFromTable<SearchErrorLogResultVM>(dt);

            return errorLogs;
        }

        #endregion
    }
}