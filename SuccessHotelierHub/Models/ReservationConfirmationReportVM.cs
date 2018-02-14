using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class ReservationConfirmationReportVM
    {
        public Guid Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Last Name")]
        public string GuestName { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNumber { get; set; }

        public Guid? ProfileId { get; set; }

        [DisplayName("Arrival Date")]
        public string ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]     
        public string DepartureDate { get; set; }

        [DisplayName("Adults")]
        public int? NoOfAdult { get; set; }

        [DisplayName("Children")]
        public int? NoOfChildren { get; set; }

        [DisplayName("Infant")]
        public int? NoOfInfant { get; set; }

        [DisplayName("No Of Rooms.")]
        public int? NoOfRoom { get; set; }

        [DisplayName("Room Type")]        
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]        
        public string RoomTypeCode { get; set; }
        
        public string Accommodation { get; set; }

        [DisplayName("Room")]
        public Guid? RoomId { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        [DisplayName("Deposit Paid")]
        public string DepositPaid { get; set; }        

        [DisplayName("ETA")]
        public TimeSpan? ETA { get; set; }

        [DisplayName("ETA")]
        public string ETAText { get; set; }
        
        [DisplayName("Payment")]        
        public Guid? PaymentMethodId { get; set; }

        [DisplayName("Method Of Payment")]        
        public string MethodOfPayment { get; set; }    
        

        public double? Rate { get; set; }

        public string RatePerNight { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        
    }
}