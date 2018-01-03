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
        //[RegularExpression("(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Please enter valid arrival date.")]
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
        public int NoOfAdult { get; set; }

        [DisplayName("Childrens")]
        public int NoOfChildren { get; set; }

        [DisplayName("No Of Rooms.")]
        public int NoOfRoom { get; set; }

        [DisplayName("Room Type")]
        [Required(ErrorMessage = "Please select room type.")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]
        [Required(ErrorMessage = "Please search and select room type.")]
        public string RoomTypeCode { get; set; }

        [DisplayName("RTC")]
        public Guid? RtcId { get; set; }

        [DisplayName("Room")]
        public Guid? RoomId { get; set; }

        public string RoomIds { get; set; }

        [DisplayName("Room #")]
        [Required(ErrorMessage = "Please search and select room.")]
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
        public float? Rate { get; set; }

        [DisplayName("Currency")]
        public Guid? CurrencyId { get; set; }

        [DisplayName("Packages")]
        public Guid? PackageId { get; set; }

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
        public string CreditCardNo { get; set; }

        [DisplayName("Exp. Date (MM/YY)")]
        public string CardExpiryDate { get; set; }

        [DisplayName("CVV #")]
        public string CVVNo { get; set; }

        [DisplayName("Approval Code ")]
        public string ApprovalCode { get; set; }

        [DisplayName("Approval Amt.")]
        public double? ApprovalAmount { get; set; }

        [DisplayName("Suit With")]
        public string SuitWith { get; set; }
        
        public bool IsEmailConfirmation { get; set; }

        [DisplayName("Guest Balance")]
        public double? GuestBalance { get; set; }

        [DisplayName("Dis. Amt.")]
        public double? DiscountAmount { get; set; }

        [DisplayName("Dis. Per.")]
        public double? DiscountPercentage { get; set; }

        [DisplayName("Reason")]
        public Guid? DiscountReasonId { get; set; }

        [DisplayName("TA Rec Loc")]
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

        [DisplayName("Item Inv.")]
        public Guid? ItemInventoryId { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNumber { get; set; }

        public Guid? CancellationReasonId { get; set; }
        public string CancellationReasonComment { get; set; }
        public bool IsReservationCancel { get; set; }

        public string Remarks { get; set; }
        public string PreferenceItems { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}