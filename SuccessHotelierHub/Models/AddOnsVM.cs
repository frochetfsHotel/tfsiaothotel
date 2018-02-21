using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class AddOnsVM
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please enter valid price.")]
        public double? Price { get; set; }
        
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}