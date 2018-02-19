using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models.StoredProcedure
{
    public class ReservationRemarksResultVM
    {
        public Guid Id { get; set; }

        public Guid ReservationId { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public string UserName { get; set; }

        public string TrackLog { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}