using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class GuestRegistrationCardVM
    {
        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set;}
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }        
        public string ZipCode { get; set; }
        public string CarRegistrationNo { get; set; }


        public string RoomNumer { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public int NoOfNights { get; set; }
        public int? NoOfAdult { get; set; }
        public int? NoOfChildren { get; set; }
        public string RoomType { get; set; }
        public double? Rate { get; set; }
        public string GroupCode { get; set; }

        public string ConfirmationNo { get; set; }
        
        
    }
}