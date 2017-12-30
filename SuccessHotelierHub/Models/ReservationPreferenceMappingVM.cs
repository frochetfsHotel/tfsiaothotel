using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationPreferenceMappingVM
    {
        public Guid Id { get; set; }        
        public Guid? ReservationId { get; set; }
        public Guid? PreferenceId { get; set; }
        public string PreferenceCode { get; set; }
        public string PreferenceDescription { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}