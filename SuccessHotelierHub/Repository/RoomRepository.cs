using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class RoomRepository
    {
        #region Room

        public List<RoomVM> GetRoomById(Guid roomId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = roomId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomById", parameters);

            var room = new List<RoomVM>();
            room  = DALHelper.CreateListFromTable<RoomVM>(dt);

            return room;
        }

        public List<RoomVM> GetRoomByRoomTypeId(Guid roomTypeId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomByRoomTypeId", parameters);

            var rooms = new List<RoomVM>();
            rooms = DALHelper.CreateListFromTable<RoomVM>(dt);

            return rooms;
        }

        public string AddRoom(RoomVM room)
        {
            string roomId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = room.RoomTypeId },                    
                    new SqlParameter { ParameterName = "@Code", Value = room.Code },
                    new SqlParameter { ParameterName = "@Name", Value = room.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = room.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = room.CreatedBy }
                };

            roomId = Convert.ToString(DALHelper.ExecuteScalar("AddRoom", parameters));

            return roomId;
        }

        public string UpdateRoom(RoomVM room)
        {
            string roomId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = room.Id },
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = room.RoomTypeId },
                    new SqlParameter { ParameterName = "@Code", Value = room.Code },
                    new SqlParameter { ParameterName = "@Name", Value = room.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = room.IsActive },                    
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = room.UpdatedBy }
                };

            roomId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoom", parameters));

            return roomId;
        }

        public string DeleteRoom(Guid id, int updatedBy)
        {
            string roomId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            roomId = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoom", parameters));

            return roomId;
        }
        
        public List<RoomVM> CheckRoomCodeAvailable(Guid roomTypeId, Guid? roomId, string code)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@Code", Value = code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomCodeAvailable", parameters);

            var room = new List<RoomVM>();
            room = DALHelper.CreateListFromTable<RoomVM>(dt);

            return room;
        }

        public List<RoomVM> CheckRoomNameAvailable(Guid roomTypeId, Guid? roomId, string name)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@Name", Value = name }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomNameAvailable", parameters);

            var room = new List<RoomVM>();
            room = DALHelper.CreateListFromTable<RoomVM>(dt);

            return room;
        }

        #endregion
    }
}