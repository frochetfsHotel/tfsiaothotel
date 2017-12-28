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
        [DisplayFormat(DataFormatString = "{0 : MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select arrival date.")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("No Of Night")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : MM/dd/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? DepartureDate { get; set; }

        [DisplayName("No Of Adult")]
        public int NoOfAdult { get; set; }

        [DisplayName("No Of Children")]
        public int NoOfChildren { get; set; }

        [DisplayName("No Of Room")]
        public int NoOfRoom { get; set; }
                
        public Guid? ProfileId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please search and select client name.")]
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

        [DisplayName("Member No")]
        public string MemberNo { get; set; }

        [DisplayName("Corp No")]
        public string CorpNo { get; set; }

        [DisplayName("IATA No")]
        public string IATANo { get; set; }

        [DisplayName("Source No")]
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
        public double? Amount { get; set; }

    }
}