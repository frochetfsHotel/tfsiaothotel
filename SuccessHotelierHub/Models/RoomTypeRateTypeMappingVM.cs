using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomTypeRateTypeMappingVM
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid RateTypeId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}