using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class RateQueryVM
    {
        [DisplayName("Arrival Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select arrival date.")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int? NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Adults")]
        public int? NoOfAdult { get; set; }

        [DisplayName("Children")]
        public int? NoOfChildren { get; set; }

        [DisplayName("Rooms")]
        public int? NoOfRoom { get; set; }
                
        public Guid? ProfileId { get; set; }

        [DisplayName("Name")]
        //[Required(ErrorMessage = "Please search and select client name.")]
        public string Name { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        [DisplayName("Member Type")]
        public Guid? MemberTypeId { get; set; }

        [DisplayName("Company")]
        public Guid? CompanyId { get; set; }

        [DisplayName("Agent")]
        public Guid? AgentId { get; set; }

        [DisplayName("Source")]
        public Guid? SourceId { get; set; }

        [DisplayName("Group Code")]
        public Guid? BlockCodeId { get; set; }

        [DisplayName("Member #")]
        public string MemberNo { get; set; }

        [DisplayName("Corp #")]
        public string CorpNo { get; set; }

        [DisplayName("IATA #")]
        public string IATANo { get; set; }

        [DisplayName("Source #")]
        public string SourceNo { get; set; }

        public bool IsClosed { get; set; }
        public bool IsDayUse { get; set; }
        public bool IsPseudo{ get; set; }

        [DisplayName("Rate Class")]
        public Guid? RateClassId { get; set; }

        [DisplayName("Rate Category")]
        public Guid? RateCategoryId { get; set; }

        [DisplayName("Rate Code")]
        public Guid? RateCodeId { get; set; }

        [DisplayName("Room Class")]
        public Guid? RoomClassId { get; set; }

        [DisplayName("Features")]
        public Guid? FeatureId { get; set; }

        [DisplayName("Packages")]
        public Guid? PackageId { get; set; }

        public bool IsInclNonDeduct { get; set; }

        //RateSheet
        public Guid? RateTypeId { get; set; }
        public string RateTypeCode { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public float? Amount { get; set; }

    }
}