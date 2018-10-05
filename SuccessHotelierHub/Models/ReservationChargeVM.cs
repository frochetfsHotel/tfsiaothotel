using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationChargeVM
    {
        public Guid Id { get; set; }

        public Guid? ReservationId { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select date.")]
        public DateTime? TransactionDate { get; set; }

        //Additional Charge        
        public Guid? AdditionalChargeId { get; set; }

        public string AdditionalChargeSource { get; set; }

        [Required(ErrorMessage = "Please search and select code.")]
        [DisplayName("Code")]
        public string Code { get; set; }

        public string Description { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Please enter amount.")]
        [DataType(DataType.Currency, ErrorMessage = "Please enter valid amount.")]
        public double? Amount { get; set; }

        public int? Qty { get; set; }
        public string Supplement { get; set; }
        public string Reference { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string CreditCardNo { get; set; }

        public string CardExpiryDate { get; set; }

        public Guid? PaymentMethodId { get; set; }

        public bool IsCreditCardPay { get; set; }

        public string CVVNo { get; set; }

        public bool IsDummyReservationPayment { get; set; }

    }
}