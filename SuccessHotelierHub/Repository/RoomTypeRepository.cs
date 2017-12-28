using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

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
                    new SqlParameter { ParameterName = "@NoOfRooms", Value = roomType.NoOfRooms },
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
                    new SqlParameter { ParameterName = "@NoOfRooms", Value = roomType.NoOfRooms },
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
                    new SqlParameter { ParameterName = "@NoOfRooms", Value = model.NoOfRooms },
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

        public List<RoomTypeVM> CheckRoomTypeCodeAvaiable(Guid? roomTypeId, string roomTypeCode)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RoomTypeCode", Value = roomTypeCode }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomTypeCodeAvaiable", parameters);

            var roomType = new List<RoomTypeVM>();
            roomType = DALHelper.CreateListFromTable<RoomTypeVM>(dt);

            return roomType;
        }

        #endregion
    }
}