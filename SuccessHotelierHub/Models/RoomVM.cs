﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RoomVM
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsOccupied { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

}