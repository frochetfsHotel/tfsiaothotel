using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomFeaturesMappingVM
    {
        public Guid Id { get; set; }     
           
        //Room
        public Guid? RoomId { get; set; }
        public string RoomNo { get; set; }
        public string RoomDescription { get; set; }

        //Room Features
        public Guid? RoomFeatureId { get; set; }
        public string RoomFeature { get; set; }
        public string RoomFeatureDescription { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}