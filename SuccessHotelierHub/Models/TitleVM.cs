using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class TitleVM
    {
        public Guid Id { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Please enter title.")]
        public string Title { get; set; }

        [DisplayName("Salutation")]        
        public string Salutation { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}