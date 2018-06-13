using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomPlanVM
    {
        [DisplayName("Date")]
        public DateTime? Date { get; set; }

        [DisplayName("Room Type")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Class")]
        public Guid? RoomClassId { get; set; }

        [DisplayName("Rooms")]
        public string Rooms { get; set; }

        [DisplayName("Floor")]
        public Guid? FloorId { get; set; }

        [DisplayName("Features")]
        public Guid? FeatureId { get; set; }

        [DisplayName("Smoking")]
        public string Smoking { get; set; }

        [DisplayName("Vertical Zoom")]
        public bool IsVerticalZoom { get; set; }

        [DisplayName("Horizontal Zoom")]
        public bool IsHorizontalZoom { get; set; }

        [DisplayName("Component Rooms")]
        public bool IsComponentRooms { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }
    }

    public class RoomPlanDateRangeVM
    {
        public DateTime? Date { get; set; }
        public string DateString { get; set; }
        public string DayOfTheWeek { get; set; }
    }

    public class RoomPlanRoomVM
    {
        public int RowNum { get; set; }

        public Guid Id { get; set; }

        [DisplayName("Room Type")]
        public Guid RoomTypeId { get; set; }

        public string RoomTypeCode { get; set; }

        [DisplayName("Room #")]
        public string RoomNo { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public Guid? RoomStatusId { get; set; }

        public string RoomStatus { get; set; }

        public string RoomStatusColor { get; set; }

        public bool IsOccupied { get; set; }
    }

    public class RoomPlanReservationDetailVM
    {
        public Guid ReservationId { get; set; }

        public Guid ProfileId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("No Of Night")]
        public int NoOfNight { get; set; }

        public Guid RoomId { get; set; }

        [DisplayName("Status")]
        public Guid? RoomStatusId { get; set; }

        public string RoomStatus { get; set; }

        public string RoomStatusColor { get; set; }

        [DisplayName("Check In Date")]
        public string CheckInDateString { get; set; }

        [DisplayName("Check In Date")]
        public DateTime? CheckInDate { get; set; }

        [DisplayName("Check Out Date")]
        public string CheckOutDateString { get; set; }

        [DisplayName("Check Out Date")]
        public DateTime? CheckOutDate { get; set; }

        public int NoOfAdult { get; set; }

        public int DaysInWeek { get; set; }
    }

    public class RoomPlanRoomAllocationDetailVM
    {
        public Guid ReservationId { get; set; }

        public Guid ProfileId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("No Of Night")]
        public int NoOfNight { get; set; }

        public Guid RoomId { get; set; }

        //[DisplayName("Status")]
        //public Guid? RoomStatusId { get; set; }

        //public string RoomStatus { get; set; }

        //public string RoomStatusColor { get; set; }

        [DisplayName("Check In Date")]
        public string ArrivalDateString { get; set; }

        [DisplayName("Check In Date")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Check Out Date")]
        public string DepartureDateString { get; set; }

        [DisplayName("Check Out Date")]
        public DateTime? DepartureDate { get; set; }

        public int NoOfAdult { get; set; }
    }
}