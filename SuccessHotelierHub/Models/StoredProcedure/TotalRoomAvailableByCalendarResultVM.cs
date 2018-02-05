using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class TotalRoomAvailableByCalendarResultVM
    {
        public int day { get; set; }
        public DateTime date { get; set; }
        public int AvailableRooms { get; set; }
    }
}