using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class RateRepository
    {
        #region Room Type Rate Type Mapping

        public List<RoomTypeRateTypeMappingVM> GetRoomTypeRateTypeMappingId(Guid id)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomTypeRateTypeMappingId", parameters);

            var roomTypeRateTypeMapping = new List<RoomTypeRateTypeMappingVM>();
            roomTypeRateTypeMapping = DALHelper.CreateListFromTable<RoomTypeRateTypeMappingVM>(dt);

            return roomTypeRateTypeMapping;
        }

        public string AddRoomTypeRateTypeMapping(RoomTypeRateTypeMappingVM model)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = model.RateTypeId },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Amount", Value = model.Amount },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = model.IsWeekEndPrice },
                    new SqlParameter { ParameterName = "@IsActive", Value = model.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("AddRoomTypeRateTypeMapping", parameters));

            return mappingId;
        }

        public string UpdateRoomTypeRateTypeMapping(RoomTypeRateTypeMappingVM model)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = model.Id },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = model.RateTypeId },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Amount", Value = model.Amount },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = model.IsWeekEndPrice },
                    new SqlParameter { ParameterName = "@IsActive", Value = model.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomTypeRateTypeMapping", parameters));

            return mappingId;
        }

        public string DeleteRoomTypeRateTypeMapping(Guid id, int updatedBy)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoomTypeRateTypeMapping", parameters));

            return mappingId;
        }

        public List<SearchRoomTypeRateTypeMappingResultVM> SearchRoomTypeRateTypeMapping(SearchRoomTypeRateTypeMappingParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = model.RateTypeId },
                    new SqlParameter { ParameterName = "@Amount", Value = model.Amount },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = model.IsWeekEndPrice },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRoomTypeRateTypeMapping", parameters);

            var results = new List<SearchRoomTypeRateTypeMappingResultVM>();
            results = DALHelper.CreateListFromTable<SearchRoomTypeRateTypeMappingResultVM>(dt);

            return results;
        }

        public List<RoomTypeRateTypeMappingVM> CheckRoomTypeRateTypeMappingAvailable(Guid? mappingId, Guid roomTypeId, Guid ratetypeId, bool isWeekEndPrice)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = mappingId },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = ratetypeId },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = isWeekEndPrice }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomTypeRateTypeMappingAvailable", parameters);

            var mapping = new List<RoomTypeRateTypeMappingVM>();
            mapping = DALHelper.CreateListFromTable<RoomTypeRateTypeMappingVM>(dt);

            return mapping;
        }

        public List<RoomTypeRateTypeMappingVM> GetWeekDayPrice(Guid roomTypeId, Guid rateTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = rateTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetWeekDayPrice", parameters);

            var roomTypeRateTypeMapping = new List<RoomTypeRateTypeMappingVM>();
            roomTypeRateTypeMapping = DALHelper.CreateListFromTable<RoomTypeRateTypeMappingVM>(dt);

            return roomTypeRateTypeMapping;
        }

        public List<RoomTypeRateTypeMappingVM> GetWeekEndPrice(Guid roomTypeId, Guid rateTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RateTypeId", Value = rateTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetWeekEndPrice", parameters);

            var roomTypeRateTypeMapping = new List<RoomTypeRateTypeMappingVM>();
            roomTypeRateTypeMapping = DALHelper.CreateListFromTable<RoomTypeRateTypeMappingVM>(dt);

            return roomTypeRateTypeMapping;
        }

        #endregion
    }
}