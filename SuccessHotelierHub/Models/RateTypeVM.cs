using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RateTypeVM
    {
        public Guid Id { get; set; }
        public string RateTypeCode { get; set; }
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