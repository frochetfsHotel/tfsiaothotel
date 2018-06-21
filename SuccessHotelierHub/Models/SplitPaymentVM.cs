using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class SplitPaymentVM
    {
        public Guid ReservationId { get; set; }
        
        [DisplayName("Method Of Payment")]
        [Required(ErrorMessage = "Please select payment method.")]
        public Guid? PaymentMethodId { get; set; }

        public double Amount { get; set; }

        public string CreditCardNo { get; set; }

        public string CardExpiryDate { get; set; }

        public string PaymentMethod { get; set; }

        [DisplayName("Check Out Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select check out date.")]
        public DateTime? CheckOutDate { get; set; }
    }
}