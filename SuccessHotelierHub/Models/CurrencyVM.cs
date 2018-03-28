using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class CurrencyVM
    {
        public Guid Id { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Please enter code.")]
        public string Code { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Conversion Rate")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please enter conversion rate.")]
        public double? ConversionRate { get; set; }

        [DisplayName("Currency Symbol")]
        public string CurrencySymbol { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}