using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class ReservationStatusVM
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter status name.")]
        public string Name { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Please status code.")]
        public string Code { get; set; }
        
        [DisplayName("Description")]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}