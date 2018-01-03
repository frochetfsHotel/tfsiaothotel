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
                    new SqlParameter { ParameterName = "@RoomNo", Value = room.RoomNo },
                    new SqlParameter { ParameterName = "@Type", Value = room.Type },
                    new SqlParameter { ParameterName = "@Description", Value = room.Description },
                    new SqlParameter { ParameterName = "@StatusId", Value = room.StatusId },
                    new SqlParameter { ParameterName = "@IsOccupied", Value = room.IsOccupied },
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
                    new SqlParameter { ParameterName = "@RoomNo", Value = room.RoomNo },
                    new SqlParameter { ParameterName = "@Type", Value = room.Type },
                    new SqlParameter { ParameterName = "@Description", Value = room.Description },
                    new SqlParameter { ParameterName = "@StatusId", Value = room.StatusId },
                    new SqlParameter { ParameterName = "@IsOccupied", Value = room.IsOccupied },
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
        
        public List<RoomVM> CheckRoomNoAvailable(Guid roomTypeId, Guid? roomId, string roomNo)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = roomNo }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckRoomNoAvailable", parameters);

            var room = new List<RoomVM>();
            room = DALHelper.CreateListFromTable<RoomVM>(dt);

            return room;
        }

        public List<SearchAdvanceRoomResultVM> SearchAdvanceRoom(SearchAdvanceRoomParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@ArrivalDate", Value = model.ArrivalDate },
                    new SqlParameter { ParameterName = "@NoOfNight", Value = model.NoOfNight },
                    new SqlParameter { ParameterName = "@DepartureDate", Value = model.DepartureDate },
                    new SqlParameter { ParameterName = "@RoomNo", Value = model.RoomNo },
                    new SqlParameter { ParameterName = "@Type", Value = model.Type }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceRoom", parameters);

            var results = new List<SearchAdvanceRoomResultVM>();
            results = DALHelper.CreateListFromTable<SearchAdvanceRoomResultVM>(dt);

            return results;
        }

        #endregion
    }
}