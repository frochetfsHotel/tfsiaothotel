using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class TitleRepository
    {
        #region Title

        public List<TitleVM>  GetTitle()
        {   
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetTitle");

            var titles = new List<TitleVM>();
            titles = DALHelper.CreateListFromTable<TitleVM>(ds);

            return titles;
        }

        public List<TitleVM> GetTitlebyId(Guid titleId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = titleId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTitlebyId", parameters);


            var title = new List<TitleVM>();
            title = DALHelper.CreateListFromTable<TitleVM>(dt);

            return title;
        }

        public string AddTitle(TitleVM title)
        {
            string titleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Title", Value = title.Title },
                    new SqlParameter { ParameterName = "@Salutation", Value = title.Salutation },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = title.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = title.CreatedBy }
                };

            titleId = Convert.ToString(DALHelper.ExecuteScalar("AddTitle", parameters));

            return titleId;
        }

        public string UpdateTitle(TitleVM title)
        {
            string titleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = title.Id },
                    new SqlParameter { ParameterName = "@Title", Value = title.Title },
                    new SqlParameter { ParameterName = "@Salutation", Value = title.Salutation },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = title.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = title.UpdatedBy }
                };

            titleId = Convert.ToString(DALHelper.ExecuteScalar("UpdateTitle", parameters));

            return titleId;
        }

        public string DeleteTitle(Guid id, int updatedBy)
        {
            string titleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            titleId = Convert.ToString(DALHelper.ExecuteScalar("DeleteTitle", parameters));

            return titleId;
        }

        public List<SearchTitleResultVM> SearchTitle(SearchTitleParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Title", Value = model.Title },
                    new SqlParameter { ParameterName = "@Salutation", Value = model.Salutation },                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchTitle", parameters);

            var titles = new List<SearchTitleResultVM>();
            titles = DALHelper.CreateListFromTable<SearchTitleResultVM>(dt);

            return titles;
        }

        #endregion
    }
}