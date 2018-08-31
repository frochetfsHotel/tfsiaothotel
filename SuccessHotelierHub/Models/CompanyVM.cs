using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class CompanyVM
    {
        public Guid Id { get; set; }

        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Please enter company name.")]
        public string CompanyName { get; set; }

        [DisplayName("Company Address")]
        [Required(ErrorMessage = "Please enter company address.")]
        public string CompanyAddress { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}