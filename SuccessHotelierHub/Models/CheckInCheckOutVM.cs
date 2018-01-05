using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class CheckInCheckOutVM
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid? ProfileId { get; set; }

        //Check In
        [DisplayName("Check In Date")]
        public DateTime CheckInDate { get; set; }

        [DisplayName("Check In Time")]
        public TimeSpan? CheckInTime { get; set; }

        [DisplayName("Check In Time")]
        public string CheckInTimeText { get; set; }

        //Check Out
        [DisplayName("Check Out Date")]
        public DateTime CheckOutDate { get; set; }

        [DisplayName("Check Out Time")]
        public TimeSpan? CheckOutTime { get; set; }

        [DisplayName("Check Out Time")]
        public string CheckOutTimeText { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}