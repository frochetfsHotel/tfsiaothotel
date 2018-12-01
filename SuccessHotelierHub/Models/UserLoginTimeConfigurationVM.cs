using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SuccessHotelierHub.Models
{
    public class UserLoginTimeConfigurationVM
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }

        [DisplayName("Week Day")]
        public int WeekDay { get; set; }

        [DisplayName("Is Allow Login")]
        public bool IsAllowLogin { get; set; }

        [DisplayName("Login Start Time")]
        [Required(ErrorMessage = "Please select login start time.")]
        public TimeSpan? LoginStartTime { get; set; }

        [DisplayName("Login Start Time (24 hours format)")]
        [Required(ErrorMessage = "Please select login start time.")]
        public string LoginStartTimeText { get; set; }

        [DisplayName("Login End Time")]
        [Required(ErrorMessage = "Please select login end time.")]
        public TimeSpan? LoginEndTime { get; set; }

        [DisplayName("Login End Time (24 hours format)")]
        [Required(ErrorMessage = "Please select login end time.")]
        public string LoginEndTimeText { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }        
        public bool IsDeleted { get; set; }
    }
}