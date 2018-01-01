using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomTypeVM
    {
        public Guid Id { get; set; }

        [DisplayName("Room Type Code")]
        [Required(ErrorMessage = "Please enter room type code.")]
        public string RoomTypeCode { get; set; }

        public string Description { get; set; }

        [DisplayName("No Of Rooms")]
        [Required(ErrorMessage = "Please enter no of rooms.")]        
        public int NoOfRooms { get; set; }

        [DisplayName("Room Capacity")]
        public int RoomCapacity { get; set; }

        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}