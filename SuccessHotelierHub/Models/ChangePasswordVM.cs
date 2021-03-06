﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class ChangePasswordVM
    {
        public Guid UserId { get; set; }

        [DisplayName("Old Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter old password.")]
        public string OldPassword { get; set; }

        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter new password.")]
        public string Password { get; set; }

        [DisplayName("Confirm New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm new password.")]
        [Compare("Password", ErrorMessage = "Confirm password and new password must be same. ")]
        public string ConfirmPassword { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}