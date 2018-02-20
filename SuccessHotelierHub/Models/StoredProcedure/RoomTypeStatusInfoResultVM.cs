using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class RoomTypeStatusInfoResultVM
    {
        public Guid RoomTypeId { get; set; }

        public string RoomTypeCode { get; set; }

        public string RoomTypeDescription { get; set; }

        public int TotalRoom { get; set; }
        public int AvailableRoom { get; set; }
    }
}