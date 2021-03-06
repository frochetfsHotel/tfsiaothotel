﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class FloorVM
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter floor name.")]
        public string Name { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Please enter code.")]
        public string Code { get; set; }

        [DisplayName("Floor #")]
        public int? FloorNo { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter no of room.")]
        [DisplayName("No Of Room")]
        public int? NoOfRoom { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}