using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class BillingTransactionReportVM
    {
        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string RoomNumer { get; set; }
        public string FolioNumber { get; set; }
        public string CashierNumber { get; set; }
        public string PageNumber { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }

        public double Total { get; set; }
        public double Room { get; set; }
        public double FandB { get; set; }
        public double Other { get; set; }
        public double TotalBalance { get; set; }
        public double BalanceDue { get; set; }

        public double NetAmount { get; set; }
        public double VATTax { get; set; }
        public double VATTax2 { get; set; }
        public double VATAmount { get; set; }
        public double VATAmount2 { get; set; }

        public string ConfirmationNo { get; set; }

        public string GSTRegistrationNumber { get; set; }

        public string CurrencySymbol { get; set; }

        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }

        public List<ReservationChargeVM> Transactions { get; set; }


    }
}