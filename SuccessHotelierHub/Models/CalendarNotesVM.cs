using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SuccessHotelierHub.Models
{
    public class CalendarNotesVM
    {
        public Guid? Id { get; set; }
                
        public DateTime Date { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}