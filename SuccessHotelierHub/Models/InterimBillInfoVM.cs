using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class InterimBillInfoVM
    {
        public Guid ReservationId { get; set; }
        public Guid? ProfileId { get; set; }
        public string Name { get; set; }

        [DisplayName("Balance")]
        public double? Balance { get; set; }

        [DisplayName("Arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Depart")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Check Out Time")]
        public TimeSpan? CheckOutTime { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        public string RoomIds { get; set; }

        public Guid? RoomTypeId { get; set; }

        [DisplayName("Rate")]
        public double? RoomRent { get; set; }

        [DisplayName("Amount")]
        public double Amount { get; set; }

        [DisplayName("Total Amount")]
        public double? TotalAmount { get; set; }

        [DisplayName("Total Payable Amount")]
        public double? TotalPayableAmount { get; set; }

        [DisplayName("Total Paid Amount")]
        public double? TotalPaidAmount { get; set; }

        [DisplayName("Payment Method")]
        public Guid? PaymentMethodId { get; set; }

        [DisplayName("Payment Method Code")]
        public string PaymentMethodCode { get; set; }

        [DisplayName("Method Of Payment")]
        public string PaymentMethodName { get; set; }

        public string CVVNo { get; set; }

        public string CreditCardNo { get; set; }

        public string CardExpiryDate { get; set; }

        public bool IsCheckedOut { get; set; }
    }
}