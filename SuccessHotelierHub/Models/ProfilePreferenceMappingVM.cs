using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ProfilePreferenceMappingVM
    {
        public Guid Id { get; set; }
        public Guid? ProfileTypeId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? PreferenceId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}