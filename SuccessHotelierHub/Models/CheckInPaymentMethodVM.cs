using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class CheckInPaymentMethodVM
    {
        public Guid ReservationId { get; set; }
        public Guid? ProfileId { get; set; }

        public int NoOfRoom { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Check In Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select check in date.")]
        public DateTime? CheckInDate { get; set; }

        [DisplayName("Check In Time")]
        public TimeSpan? CheckInTime { get; set; }

        [DisplayName("Check In Time")]
        public string CheckInTimeText { get; set; }

        [DisplayName("Method Of Payment")]
        public Guid? PaymentMethodId { get; set; }

        [DisplayName("Credit Card #")]
        public string CreditCardNo { get; set; }

        [DisplayName("Expiration Date")]
        public string CardExpiryDate { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        public string RoomIds { get; set; }

        public Guid? RoomTypeId { get; set; }
    }
}