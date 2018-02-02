using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class RateTypeDetailsByRoomTypeResultVM
    {
        public Guid RateTypeId { get; set; }

        public string RateTypeCode { get; set; }

        public string RateTypeDescription { get; set; }

        public double Amount { get; set; }
    }
}