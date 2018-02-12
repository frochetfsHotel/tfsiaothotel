using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub.Repository
{
    public class RateTypeRepository
    {
        #region Rate Type

        public List<RateTypeVM> GetRateType(string rateTypeCode)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeCode", Value = rateTypeCode}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateType", parameters);

            var rateTypes = new List<RateTypeVM>();
            rateTypes = DALHelper.CreateListFromTable<RateTypeVM>(dt);

            return rateTypes;
        }
        

        public List<RateTypeVM> GetRateTypeById(Guid rateTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = rateTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeById", parameters);
            
            var rateType = new List<RateTypeVM>();
            rateType = DALHelper.CreateListFromTable<RateTypeVM>(dt);

            return rateType;
        }

        public string AddRateType(RateTypeVM rateType)
        {
            string rateTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeCode", Value = rateType.RateTypeCode },
                    new SqlParameter { ParameterName = "@Description", Value = rateType.Description },
                    new SqlParameter { ParameterName = "@Amount", Value = rateType.Amount },
                    new SqlParameter { ParameterName = "@IsLeisRateType", Value = rateType.IsLeisRateType },
                    new SqlParameter { ParameterName = "@IsActive", Value = rateType.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = rateType.CreatedBy }
                };

            rateTypeId = Convert.ToString(DALHelper.ExecuteScalar("AddRateType", parameters));

            return rateTypeId;
        }

        public string UpdateRateType(RateTypeVM rateType)
        {
            string rateTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = rateType.Id },
                    new SqlParameter { ParameterName = "@RateTypeCode", Value = rateType.RateTypeCode },
                    new SqlParameter { ParameterName = "@Description", Value = rateType.Description },
                    new SqlParameter { ParameterName = "@Amount", Value = rateType.Amount },
                    new SqlParameter { ParameterName = "@IsLeisRateType", Value = rateType.IsLeisRateType },
                    new SqlParameter { ParameterName = "@IsActive", Value = rateType.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = rateType.UpdatedBy }
                };

            rateTypeId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRateType", parameters));

            return rateTypeId;
        }

        public string DeleteRateType(Guid id, int updatedBy)
        {
            string rateTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            rateTypeId = Convert.ToString(DALHelper.ExecuteScalar("DeleteRateType", parameters));

            return rateTypeId;
        }

        public List<SearchRateTypeResultVM> SearchRateType(SearchRateTypeParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeCode", Value = model.RateTypeCode },
                    new SqlParameter { ParameterName = "@Amount", Value = model.Amount },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRateType", parameters);

            var rateTypes = new List<SearchRateTypeResultVM>();
            rateTypes = DALHelper.CreateListFromTable<SearchRateTypeResultVM>(dt);

            return rateTypes;
        }

        public List<RateTypeVM> CheckRateTypeCodeAvailable(Guid? rateTypeId, string rateTypeCode)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = rateTypeId },
                    new SqlParameter { ParameterName = "@RateTypeCode", Value = rateTypeCode }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRateTypeCodeAvailable", parameters);

            var rateType = new List<RateTypeVM>();
            rateType = DALHelper.CreateListFromTable<RateTypeVM>(dt);

            return rateType;
        }

        public List<RateTypeDetailsByRoomTypeResultVM> GetRateTypeDetailsByRoomType(Guid roomTypeId, bool isWeekEndPrice)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = isWeekEndPrice }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeDetailsByRoomType", parameters);

            var rateTypes = new List<RateTypeDetailsByRoomTypeResultVM>();
            rateTypes = DALHelper.CreateListFromTable<RateTypeDetailsByRoomTypeResultVM>(dt);

            return rateTypes;
        }

        #endregion
    }
}