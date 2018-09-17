using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class UserVM
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Please select user role.")]
        public Guid UserRoleId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter email.")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [DisplayName("Password")]
        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Please enter confirm password.")]
        //[Compare("Password", ErrorMessage = "Confirm password and password must be same. ")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Record User's Activity")]
        public bool IsRecordActivity { get; set; }

        [DisplayName("Cashier #")]
        [Required(ErrorMessage = "Please enter cashier number.")]
        public string CashierNumber { get; set; }

        [DisplayName("Can Delete Profile")]
        public bool IsAllowToDeleteProfile { get; set; }

        [DisplayName("User Group")]
        [Required(ErrorMessage = "Please select user group.")]
        public Guid? UserGroupId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? LastLoggedOn { get; set; }
        public bool? IsLoggedIn { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Telephone Number")]        
        public string TelephoneNo { get; set; }

        public bool? IsFromRegistrationPage { get; set; }

        [DisplayName("College Group")]
        [Required(ErrorMessage = "Please select college group.")]
        public Guid? CollegeGroupId { get; set; }

        [DisplayName("College Group Name")]
        public string CollegeGroupName { get; set; }

        [DisplayName("Tutor")]
        [Required(ErrorMessage = "Please select tutor.")]
        public Guid? TutorId { get; set; }

        public bool? IsLoginCredentialSent { get; set; }

    }
}