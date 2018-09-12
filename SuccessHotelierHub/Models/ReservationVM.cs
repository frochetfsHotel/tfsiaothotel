using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuccessHotelierHub.Models
{
    public class ReservationVM
    {
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public Guid? TitleId { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please search and select profile.")]
        public string LastName { get; set; }

        [DisplayName("First Name")]        
        public string FirstName { get; set; }

        public Guid? ProfileId { get; set; }

        [DisplayName("Member Type")]
        public Guid? MemberTypeId { get; set; }
        
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [DisplayName("Language")]
        public Guid? LanguageId { get; set; }

        [DisplayName("VIP")]
        public Guid? VipId { get; set; }

        [DisplayName("Phone #")]
        public string PhoneNo { get; set; }

        [DisplayName("Member #")]
        public string MemberNo { get; set; }

        [DisplayName("Member LVT.")]
        public string MemberLvt { get; set; }

        [DisplayName("Agent")]
        public Guid? AgentId { get; set; }

        [DisplayName("Company")]
        public Guid? CompanyId { get; set; }

        [DisplayName("Group")]
        public Guid? GroupId { get; set; }

        [DisplayName("Source")]
        public Guid? SourceId { get; set; }

        [DisplayName("Contact")]
        public Guid? ContactId { get; set; }

        [DisplayName("Arrival Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        [Required(ErrorMessage = "Please select arrival date.")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please select departure date.")]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Adults")]
        public int? NoOfAdult { get; set; }

        [DisplayName("Children")]
        public int? NoOfChildren { get; set; }

        [DisplayName("Infant")]
        public int? NoOfInfant { get; set; }

        [DisplayName("No Of Rooms.")]
        public int? NoOfRoom { get; set; }

        [DisplayName("Room Type")]
        [Required(ErrorMessage = "Please select room type.")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]
        [Required(ErrorMessage = "Please search and select room type.")]
        public string RoomTypeCode { get; set; }

        [DisplayName("Rate to Charge")]
        public Guid? RtcId { get; set; }

        [DisplayName("Room")]
        public Guid? RoomId { get; set; }

        public string RoomIds { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        [DisplayName("Extn")]
        public Guid? ExtnId { get; set; }

        [DisplayName("Rate Code")]
        [Required(ErrorMessage = "Please select rate code.")]
        public Guid? RateCodeId { get; set; }
        
        public string RateTypeCode { get; set; }

        [DisplayName("Fixed Rate")]
        public bool IsFixedRate { get; set; }

        [DisplayName("Rate")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? Rate { get; set; }

        [DisplayName("Currency")]
        public Guid? CurrencyId { get; set; }

        [DisplayName("Package")]
        public Guid? PackageId { get; set; }

        [DisplayName("Package Name")]
        public string PackageName { get; set; }

        [DisplayName("Group Code")]
        public Guid? BlockCodeId { get; set; }

        [DisplayName("ETA")]
        public TimeSpan? ETA { get; set; }

        [DisplayName("ETA")]
        public string ETAText { get; set; }

        [DisplayName("Res. Type")]
        [Required(ErrorMessage = "Please select reservation type.")]
        public Guid? ReservationTypeId { get; set; }

        [DisplayName("Market")]
        [Required(ErrorMessage = "Please select market.")]
        public Guid? MarketId { get; set; }

        [DisplayName("Source")]
        [Required(ErrorMessage = "Please select reservation source.")]
        public Guid? ReservationSourceId { get; set; }

        [DisplayName("Origin")]
        public Guid? OriginId { get; set; }

        [DisplayName("Payment")]
        [Required(ErrorMessage ="Please select payment method.")]
        public Guid? PaymentMethodId { get; set; }

        [DisplayName("Credit Card #")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Please enter 16 digits of your credit card.")]
        public string CreditCardNo { get; set; }

        [DisplayName("Exp. Date (MM/YY)")]
        public string CardExpiryDate { get; set; }

        [DisplayName("CVV #")]
        //[StringLength(4, MinimumLength = 3, ErrorMessage = "CVV # must be only 3 or 4 digits.")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Please enter only 3 or 4 digits.")]
        public string CVVNo { get; set; }

        [DisplayName("Pre-Authorised Approval Code")]
        public string ApprovalCode { get; set; }

        [DisplayName("Pre-Authorised Approval Amount")]
        public double? ApprovalAmount { get; set; }

        [DisplayName("Suit With")]
        public string SuitWith { get; set; }
        
        public bool IsEmailConfirmation { get; set; }

        [DisplayName("Guest Balance")]
        public double? GuestBalance { get; set; }

        [DisplayName("Discount Amount")]
        public double? DiscountAmount { get; set; }

        [DisplayName("Discount (%)")]
        public double? DiscountPercentage { get; set; }

        [DisplayName("Discount Approved By")]
        public Guid? DiscountApprovedBy { get; set; }

        [DisplayName("Discount Reason")]
        public string DiscountReason { get; set; }

        [DisplayName("Agent Booking Reference")]
        public string TARecordLocator { get; set; }

        [DisplayName("Specials")]
        public Guid? SpecialsId { get; set; }

        [DisplayName("Reservation Comments")]
        public string ReservationComments { get; set; }

        [DisplayName("In-House Comments")]
        public string InHouseComments { get; set; }

        [DisplayName("Cashiering Comments")]
        public string CashieringComments { get; set; }

        [DisplayName("House Keeping Comments")]
        public string HouseKeepingComments { get; set; }

        [DisplayName("Room Inventory")]
        public Guid? ItemInventoryId { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNumber { get; set; }

        [DisplayName("Folio #")]
        public long FolioNumber { get; set; }

        public Guid? CancellationReasonId { get; set; }
        public string CancellationReasonComment { get; set; }
        public bool IsReservationCancel { get; set; }

        //CheckIn-CheckOut
        public bool IsCheckIn { get; set; }
        public bool IsCheckOut { get; set; }

        public string Remarks { get; set; }
        public string PreferenceItems { get; set; }

        public double? TotalPrice { get; set; }        


        public Guid? ReservationStatusId { get; set; }

        public bool IsWeekEndPrice { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDummyReservation { get; set; }

        public List<ReservationRemarkVM> RemarksList { get; set; }

        public List<PackageVM> PackageList { get; set; }

        public List<ReservationPackageMappingVM> PackageMappingList { get; set; }

        public List<AddOnsVM> AddOnsList { get; set; }
    }
}