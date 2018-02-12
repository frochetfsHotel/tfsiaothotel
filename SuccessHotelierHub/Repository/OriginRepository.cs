using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class OriginRepository
    {
        #region Origin

        public List<OriginVM> GetOrigins()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetOrigins");

            var origins = new List<OriginVM>();
            origins = DALHelper.CreateListFromTable<OriginVM>(ds);

            return origins;
        }

        public List<OriginVM> GetOriginById(Guid originId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = originId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetOriginById", parameters);

            var origin = new List<OriginVM>();
            origin = DALHelper.CreateListFromTable<OriginVM>(dt);

            return origin;
        }

        public string AddOrigin(OriginVM origin)
        {
            string originId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = origin.Code },
                    new SqlParameter { ParameterName = "@Description", Value = origin.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = origin.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = origin.CreatedBy }
                };

            originId = Convert.ToString(DALHelper.ExecuteScalar("AddOrigin", parameters));

            return originId;
        }

        public string UpdateOrigin(OriginVM origin)
        {
            string originId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = origin.Id },
                    new SqlParameter { ParameterName = "@Code", Value = origin.Code },
                    new SqlParameter { ParameterName = "@Description", Value = origin.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = origin.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = origin.UpdatedBy }
                };

            originId = Convert.ToString(DALHelper.ExecuteScalar("UpdateOrigin", parameters));

            return originId;
        }

        public string DeleteOrigin(Guid id, int updatedBy)
        {
            string originId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            originId = Convert.ToString(DALHelper.ExecuteScalar("DeleteOrigin", parameters));

            return originId;
        }

        public List<SearchOriginResultVM> SearchOrigin(SearchOriginParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchOrigin", parameters);

            var origins = new List<SearchOriginResultVM>();
            origins = DALHelper.CreateListFromTable<SearchOriginResultVM>(dt);

            return origins;
        }

        #endregion
    }
}