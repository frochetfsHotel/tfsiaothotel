using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class GetPreviousReservationOrNotVM
    {
        public TimeSpan? CheckOutTime { get; set; }

        public DateTime? DepartureDate { get; set; }
    }
}