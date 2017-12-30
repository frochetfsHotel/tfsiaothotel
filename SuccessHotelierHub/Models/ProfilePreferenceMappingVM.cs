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
        public string PreferenceCode { get; set; }
        public string PreferenceDescription { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}