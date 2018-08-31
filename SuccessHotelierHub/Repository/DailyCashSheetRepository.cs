using SuccessHotelierHub.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Repository
{
    public class DailyCashSheetRepository
    {
        public List<SearchDaliyCashSheetReportResultVM> SearchDailyCashSeetReport(SearchDailyCashSheetParameterVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.UserId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetDailyCashSheetList", parameters);

            var dailycashsheet = new List<SearchDaliyCashSheetReportResultVM>();
            dailycashsheet = DALHelper.CreateListFromTable<SearchDaliyCashSheetReportResultVM>(dt);

            return dailycashsheet;
        }

        public List<DaliyCashSheetReportVM> GetDailyCashSheet(int CreatedBy,string CreatedOn)
        {
            SqlParameter[] parameters =
                {                   
                    new SqlParameter { ParameterName = "@CreatedBy", Value = CreatedBy },
                    new SqlParameter { ParameterName = "@CreatedOn", Value = !string.IsNullOrWhiteSpace(CreatedOn) ? CreatedOn : (object)DBNull.Value }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetDailyCashSheet", parameters);

            var dailycashsheet = new List<DaliyCashSheetReportVM>();
            dailycashsheet = DALHelper.CreateListFromTable<DaliyCashSheetReportVM>(dt);

            return dailycashsheet;
        }

    }
}