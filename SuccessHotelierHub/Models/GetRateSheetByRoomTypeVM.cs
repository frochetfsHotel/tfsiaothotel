using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class GetRateSheetByRoomTypeVM
    {
        public Guid? Id { get; set; }

        public Guid? RoomTypeId { get; set; }

        public Guid? RateTypeId { get; set; }

        public double Amount { get; set; }

        public string RateTypeCode { get; set; }

        public string Description { get; set; }
    }
}