using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class CityVM
    {
        public int Id { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select country.")]
        public int? CountryId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select state.")]
        public int? StateId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}