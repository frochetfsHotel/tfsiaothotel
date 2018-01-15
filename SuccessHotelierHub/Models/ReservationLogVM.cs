using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationLogVM
    {
        public int Id { get; set; }

        [DisplayName("Reservation")]        
        public Guid? ReservationId { get; set; }

        [DisplayName("Profile")]
        public Guid? ProfileId { get; set; }

        [DisplayName("Room")]
        public Guid? RoomId { get; set; }

        [DisplayName("Room Status")]
        public Guid? RoomStatusId { get; set; }

        //Check In
        [DisplayName("Check In Date")]
        public DateTime? CheckInDate { get; set; }

        [DisplayName("Check In Time")]
        public TimeSpan? CheckInTime { get; set; }

        [DisplayName("Check In Time")]
        public string CheckInTimeText { get; set; }

        //Check Out
        [DisplayName("Check Out Date")]
        public DateTime? CheckOutDate { get; set; }

        [DisplayName("Check Out Time")]
        public TimeSpan? CheckOutTime { get; set; }

        [DisplayName("Check Out Time")]
        public string CheckOutTimeText { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}