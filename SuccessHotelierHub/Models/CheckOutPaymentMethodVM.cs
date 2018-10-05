using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace SuccessHotelierHub.Models
{
    public class CheckOutPaymentMethodVM
    {
        public Guid ReservationId { get; set; }
        public Guid? ProfileId { get; set; }

        public int NoOfRoom { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Check Out Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select check out date.")]
        public DateTime? CheckOutDate { get; set; }

        [DisplayName("Check Out Time")]
        public TimeSpan? CheckOutTime { get; set; }

        [DisplayName("Check Out Time")]
        [Required(ErrorMessage = "Please select check out time.")]
        public string CheckOutTimeText { get; set; }

        [DisplayName("Method Of Payment")]
        [Required(ErrorMessage = "Please select payment method.")]
        public Guid? PaymentMethodId { get; set; }

        public string PaymentMethod { get; set; }

        [DisplayName("CVV #")]
        public string CVVNo { get; set; }

        [DisplayName("Credit Card #")]
        public string CreditCardNo { get; set; }

        [DisplayName("Expiration Date")]
        public string CardExpiryDate { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        public string RoomIds { get; set; }

        public Guid? RoomTypeId { get; set; }

        public double? Amount { get; set; }

        public string Reference { get; set; }

        public Boolean SplitPayment { get; set; }

        //Company Details.
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string AccountNumber { get; set; }
        public string ContactPerson { get; set; }
        public string TelephoneNo { get; set; }
        public string Email { get; set; }
    }
}