using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class PreferenceVM
    {
        public Guid Id { get; set; }

        [DisplayName("Preference Group")]
        [Required(ErrorMessage = "Please select preference group.")]
        public Guid PreferenceGroupId { get; set; }

        [DisplayName("Preference Code")]
        [Required(ErrorMessage = "Please enter code.")]
        public string Code { get; set; }

        [DisplayName("Preference")]
        [Required(ErrorMessage = "Please enter preference.")]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}