﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class PreferenceGroupVM
    {
        public Guid Id { get; set; }
        

        [DisplayName("Preference Group Name")]
        [Required(ErrorMessage = "Please enter preference group name.")]
        public string Name { get; set; }

        [DisplayName("Description")]        
        public string Description { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}