using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class AvailableRoomInfoByDateResultVM
    {
        public Guid RoomTypeId { get; set; }

        public string RoomTypeCode { get; set; }

        public int TotalRoom { get; set; }
        public int AvailableRoom { get; set; }
    }
}