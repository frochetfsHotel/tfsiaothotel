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
                    new SqlParameter { ParameterName = "@FloorId", Value = room.FloorId },
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
                    new SqlParameter { ParameterName = "@FloorId", Value = room.FloorId },
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
        
        public List<RoomVM> CheckRoomNoAvailable(Guid? roomId, string roomNo)
        {
            SqlParameter[] parameters =
               {
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
                    new SqlParameter { ParameterName = "@Type", Value = model.Type },
                    new SqlParameter { ParameterName = "@RoomFeatures", Value = model.RoomFeaturesIds },
                    new SqlParameter { ParameterName = "@IsClean", Value = model.IsClean },
                    new SqlParameter { ParameterName = "@IsDirty", Value = model.IsDirty },
                    new SqlParameter { ParameterName = "@IsInspected", Value = model.IsInspected }

                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceRoom", parameters);

            var results = new List<SearchAdvanceRoomResultVM>();
            results = DALHelper.CreateListFromTable<SearchAdvanceRoomResultVM>(dt);

            return results;
        }

        public List<RoomPlanRoomVM> GetRoomDetailsForRoomPlan(Guid? roomTypeId, string roomNo, string startDate, string endDate)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = roomTypeId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = roomNo },
                    new SqlParameter { ParameterName = "@StartDate", Value = startDate },
                    new SqlParameter { ParameterName = "@EndDate", Value = endDate }

                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomDetailsForRoomPlan", parameters);

            var rooms = new List<RoomPlanRoomVM>();
            rooms = DALHelper.CreateListFromTable<RoomPlanRoomVM>(dt);

            return rooms;
        }


        public List<SearchRoomResultVM> SearchRoom(SearchRoomParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomTypeId", Value = model.RoomTypeId },
                    new SqlParameter { ParameterName = "@FloorId", Value = model.FloorId },
                    new SqlParameter { ParameterName = "@RoomNo", Value = model.RoomNo },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRoom", parameters);

            var rooms = new List<SearchRoomResultVM>();
            rooms = DALHelper.CreateListFromTable<SearchRoomResultVM>(dt);

            return rooms;
        }

        public string UpdateRoomOccupiedFlag(Guid roomId, bool isOccupied, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@IsOccupied", Value = isOccupied },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomOccupiedFlag", parameters));

            return id;
        }

        public string UpdateRoomCheckInStatus(Guid roomId, Guid statusId, bool isOccupied, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@StatusId", Value = statusId },
                    new SqlParameter { ParameterName = "@IsOccupied", Value = isOccupied },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomCheckInStatus", parameters));

            return id;
        }

        public string UpdateRoomCheckOutStatus(Guid roomId, Guid statusId, bool isOccupied, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = roomId },
                    new SqlParameter { ParameterName = "@StatusId", Value = statusId },
                    new SqlParameter { ParameterName = "@IsOccupied", Value = isOccupied },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomCheckOutStatus", parameters));

            return id;
        }

        #endregion

        #region Reservation Room Mapping 

        public string AddUpdateReservationRoomMapping(ReservationRoomMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = model.ReservationId },
                    new SqlParameter { ParameterName = "@RoomId", Value = model.RoomId } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateReservationRoomMapping", parameters));

            return id;
        }

        public string DeleteReservationRoomMappingByReservation(Guid reservationId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationRoomMappingByReservation", parameters));

            return id;
        }

        public List<ReservationRoomMappingVM> GetReservationRoomMapping(Guid? reservationId, Guid? roomId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = (object) reservationId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@RoomId", Value = (object) roomId ?? DBNull.Value }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationRoomMapping", parameters);

            var reservationRoomMapping = new List<ReservationRoomMappingVM>();
            reservationRoomMapping = DALHelper.CreateListFromTable<ReservationRoomMappingVM>(dt);

            return reservationRoomMapping;
        }

        #endregion

        #region Room Features Mapping

        public string AddUpdateRoomFeaturesMapping(RoomFeaturesMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomId", Value = model.RoomId },
                    new SqlParameter { ParameterName = "@RoomFeatureId", Value = model.RoomFeatureId } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateRoomFeaturesMapping", parameters));

            return id;
        }

        public string DeleteRoomFeaturesMappingByRoom(Guid roomId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomId", Value = roomId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoomFeaturesMappingByRoom", parameters));

            return id;
        }

        public string DeleteRoomFeaturesMapping(Guid mappingId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = mappingId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoomFeaturesMapping", parameters));

            return id;
        }

        public List<RoomFeaturesMappingVM> GetRoomFeaturesMapping(Guid? roomId, Guid? roomFeatureId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@RoomId", Value = (object) roomId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@RoomFeatureId", Value = (object) roomFeatureId ?? DBNull.Value }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomFeaturesMapping", parameters);

            var roomFeaturesMapping = new List<RoomFeaturesMappingVM>();
            roomFeaturesMapping = DALHelper.CreateListFromTable<RoomFeaturesMappingVM>(dt);

            return roomFeaturesMapping;
        }


        #endregion
    }
}