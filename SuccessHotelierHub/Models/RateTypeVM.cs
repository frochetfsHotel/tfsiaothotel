using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RateTypeVM
    {
        public Guid Id { get; set; }

        [DisplayName("Rate Type Code")]
        [Required(ErrorMessage = "Please enter rate type code.")]
        public string RateTypeCode { get; set; }

        public string Description { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Please enter amount.")]
        [DataType(DataType.Currency, ErrorMessage = "Please enter valid amount.")]
        public double Amount { get; set; }

        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}