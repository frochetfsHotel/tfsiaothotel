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
    public class RoomTypeRepository
    {

        #region Room Type

        public List<RoomTypeVM> GetRoomType(string roomTypeCode)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomTypeCode}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomType", parameters);

            var roomTypes = new List<RoomTypeVM>();
            roomTypes = DALHelper.CreateListFromTable<RoomTypeVM>(dt);

            return roomTypes;
        }
        

        public List<RoomTypeVM> GetRoomTypeById(Guid roomTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = roomTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomTypeById", parameters);

            var roomType = new List<RoomTypeVM>();
            roomType = DALHelper.CreateListFromTable<RoomTypeVM>(dt);

            return roomType;
        }

        public string AddRoomType(RoomTypeVM roomType)
        {
            string roomTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomType.RoomTypeCode },
                    new SqlParameter { ParameterName = "@Description", Value = roomType.Description },                    
                    new SqlParameter { ParameterName = "@RoomCapacity", Value = roomType.RoomCapacity },
                    new SqlParameter { ParameterName = "@IsActive", Value = roomType.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = roomType.CreatedBy }
                };

            roomTypeId = Convert.ToString(DALHelper.ExecuteScalar("AddRoomType", parameters));

            return roomTypeId;
        }

        public string UpdateRoomType(RoomTypeVM roomType)
        {
            string roomTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = roomType.Id },
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomType.RoomTypeCode },
                    new SqlParameter { ParameterName = "@Description", Value = roomType.Description },                    
                    new SqlParameter { ParameterName = "@RoomCapacity", Value = roomType.RoomCapacity },
                    new SqlParameter { ParameterName = "@IsActive", Value = roomType.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = roomType.UpdatedBy }
                };

            roomTypeId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomType", parameters));

            return roomTypeId;
        }

        public string DeleteRoomType(Guid id, int updatedBy)
        {
            string roomTypeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            roomTypeId = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoomType", parameters));

            return roomTypeId;
        }

        public List<SearchRoomTypeResultVM> SearchRoomType(SearchRoomTypeParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = model.RoomTypeCode },                    
                    new SqlParameter { ParameterName = "@RoomCapacity", Value = model.RoomCapacity },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRoomType", parameters);

            var roomTypes = new List<SearchRoomTypeResultVM>();
            roomTypes = DALHelper.CreateListFromTable<SearchRoomTypeResultVM>(dt);

            return roomTypes;
        }


        public List<SearchAdvanceRoomTypeResultVM> SearchAdvanceRoomType(SearchAdvanceRoomTypeParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RateTypeId", Value = model.RateTypeId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = model.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = model.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = model.DepartureDate },
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = model.RoomTypeCode },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@AvailableRooms", Value = model.AvailableRooms },
                    new SqlParameter { ParameterName = "@RoomCapacity", Value = model.RoomCapacity },
                    new SqlParameter { ParameterName = "@IsWeekEndPrice", Value = model.IsWeekEndPrice },
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceRoomType", parameters);

            var results = new List<SearchAdvanceRoomTypeResultVM>();
            results = DALHelper.CreateListFromTable<SearchAdvanceRoomTypeResultVM>(dt);

            return results;
        }

        public List<RoomTypeVM> CheckRoomTypeCodeAvailable(Guid? roomTypeId, string roomTypeCode)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomTypeCode }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomTypeCodeAvailable", parameters);

            var roomType = new List<RoomTypeVM>();
            roomType = DALHelper.CreateListFromTable<RoomTypeVM>(dt);

            return roomType;
        }

        public List<RateSheetRoomTypeVM> GetRoomTypeDetailsForRateSheet(string roomTypeCode, string arrivalDate, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomTypeCode},
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = arrivalDate},
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomTypeDetailsForRateSheet", parameters);

            var roomTypes = new List<RateSheetRoomTypeVM>();
            roomTypes = DALHelper.CreateListFromTable<RateSheetRoomTypeVM>(dt);

            return roomTypes;
        }


        public List<RoomTypeStatusInfoResultVM> GetRoomTypeStatusInfo(string date, Guid roomTypeId, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Date", Value = date },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomTypeStatusInfo", parameters);

            var results = new List<RoomTypeStatusInfoResultVM>();
            results = DALHelper.CreateListFromTable<RoomTypeStatusInfoResultVM>(dt);

            return results;
        }

        #endregion
    }
}