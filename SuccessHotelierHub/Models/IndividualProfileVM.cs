using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuccessHotelierHub.Models
{
    public class IndividualProfileVM
    {
        public Guid Id { get; set; }
        public Guid ProfileTypeId { get; set; }
        
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage ="Please enter last name")]
        public string LastName { get; set; }

        public Guid? TitleId { get; set; }

        [Display(Name = "Telephone #")]
        [Required(ErrorMessage = "Please enter telephone no.")]
        public string TelephoneNo { get; set; }

        [Display(Name = "Business Telephone #")]
        public string BusinessTelephoneNo { get; set; }
        
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        public string Address { get; set; }

        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public Guid? VipId { get; set; }

        public Guid? NationalityId { get; set; }

        [Display(Name = "Car Registration #")]
        public string CarRegistrationNo { get; set; }

        [Display(Name = "Passport #")]
        public string PassportNo { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        public bool IsMailingList { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<SelectListItem> TitleList { get; set; }
        public IEnumerable<SelectListItem> VipList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }

        public string PreferenceItems { get; set; }
    }
}