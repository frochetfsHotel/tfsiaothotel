using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class BillingInfoVM
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

        public Guid? CompanyId { get; set; }
        public string Company { get; set; }

        public Guid? GroupId { get; set; }
        public string Group { get; set; }

        public string RoomIds { get; set; }
        public string RoomNumbers { get; set; }

        public Guid? RateCodeId { get; set; }

        [DisplayName("Rate Code")]
        public string RateCode { get; set; }

        [DisplayName("Rate")]
        public double? Rate { get; set; }

        [DisplayName("Rate")]
        public double? RoomRent { get; set; }

        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeCode { get; set; }

        [DisplayName("Prs")]
        public int NoOfRooms { get; set; }

        public Guid? StatusId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public bool IsCheckedOut { get; set; }

        public List<ReservationChargeVM> Transactions { get; set; }

    }
}
