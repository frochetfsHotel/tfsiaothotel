using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class AdditionalChargeVM
    {
        public Guid Id { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Please enter code.")]
        public string Code { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Please enter description.")]
        public string Description { get; set; }

        [DisplayName("Price")]
        [DataType(DataType.Currency, ErrorMessage = "Please enter valid price.")]
        public double? Price { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select category.")]
        public Guid? CategoryId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}