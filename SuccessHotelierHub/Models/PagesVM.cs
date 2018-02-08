using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class PagesVM
    {
        public Guid Id { get; set; }

        [DisplayName("Page Code")]
        [Required(ErrorMessage = "Please page code.")]
        public string PageCode { get; set; }

        [DisplayName("Page Name")]
        [Required(ErrorMessage = "Please page name.")]
        public string PageName { get; set; }

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