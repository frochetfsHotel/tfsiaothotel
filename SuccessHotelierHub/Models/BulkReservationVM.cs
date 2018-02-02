using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class BulkReservationVM
    {
        public Guid ProfileId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public Guid? TitleId { get; set; }
        public int? CountryId { get; set; }

        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }

        public int NoOfNight { get; set; }

        public string RoomNumber { get; set; }

        public Guid? RoomId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public Guid? RateTypeId { get; set; }

        public bool IsWeekEndPrice { get; set; }
        public float? Rate { get; set; }

        public bool IsActive { get; set; }

        public string PreferenceItems { get; set; }
    }
}