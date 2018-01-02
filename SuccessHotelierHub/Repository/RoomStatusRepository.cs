using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class RoomStatusRepository
    {
        #region Room Status

        public List<RoomStatusVM> GetRoomStatus()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomStatus");

            var roomStatus = new List<RoomStatusVM>();
            roomStatus = DALHelper.CreateListFromTable<RoomStatusVM>(dt);

            return roomStatus;
        }

        #endregion
    }
}