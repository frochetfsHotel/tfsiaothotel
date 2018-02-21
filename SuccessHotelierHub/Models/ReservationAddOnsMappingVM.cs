using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;   

namespace SuccessHotelierHub.Models
{
    public class ReservationAddOnsMappingVM
    {
        public Guid Id { get; set; }
        public Guid? ReservationId { get; set; }

        public Guid? AddOnsId { get; set; }
        public string AddOnsName { get; set; }
        public double? AddOnsPrice { get; set; }
        public string AddOnsDescription { get; set; }        

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}