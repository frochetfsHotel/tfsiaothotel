using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SuccessHotelierHub.Models
{
    public class ReservationRemarkVM
    {
        public Guid Id { get; set; }

        public Guid ReservationId { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }        
        public bool IsDeleted { get; set; }
    }
}