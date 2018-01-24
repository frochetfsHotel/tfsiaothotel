using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomVM
    {
        public Guid Id { get; set; }

        [DisplayName("Floor")]
        [Required(ErrorMessage = "Please select floor.")]
        public Guid FloorId { get; set; }

        [DisplayName("Room Type")]
        [Required(ErrorMessage = "Please select room type.")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room #")]
        [Required(ErrorMessage = "Please enter room no.")]
        public string RoomNo { get; set; }

        [DisplayName("Code")]
        public string Type { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public Guid? StatusId { get; set; }

        public List<Guid> RoomFeatures { get; set; }

        public bool IsOccupied { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public RoomVM()
        {
            RoomFeatures = new List<Guid>();
        }
    }

}