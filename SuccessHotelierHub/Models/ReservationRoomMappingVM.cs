using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ReservationRoomMappingVM
    {
        public Guid Id { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomId { get; set; }
        public string RoomNo { get; set; }
        public string RoomDescription { get; set; }

        //Room Status
        public Guid? StatusId { get; set; }        
        public string RoomStatusName { get; set; }
        public string RoomStatusCode { get; set; }
        public string RoomStatusColorCode { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}