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

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Contact Person")]
        public string ContactPerson { get; set; }

        [DisplayName("Telephone No")]
        public string TelephoneNo { get; set; }

        [DisplayName("Email")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}