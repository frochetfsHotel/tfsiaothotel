using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    #region Search Preferences

    public class SearchPreferenceParametersVM
    {
        public SearchPreferenceParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchPreferenceResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string PreferenceGroupName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }        
        public int TotalCount { get; set; }
    }
    #endregion

    #region Advance Preference Search

    public class SearchAdvancePreferenceParametersVM
    {
        [DisplayName("Preference Group")]
        public Guid? PreferenceGroupId { get; set; }

        [DisplayName("Preference")]
        public string Preference { get; set; }
    }

    public class SearchAdvancePreferenceResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public string PreferenceGroupName { get; set; }
        public string PreferenceGroupDescription { get; set; }
        public string Preference { get; set; }
        
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region Search Individual Profile

    public class SearchIndividualProfileParametersVM
    {
        public SearchIndividualProfileParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Telephone #")]
        public string TelephoneNo { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public int? CreatedBy { get; set; }

        public bool IsAdminUser { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchIndividualProfileResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelephoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Advance Profile Search

    public class SearchAdvanceProfileParametersVM
    {
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("View By")]
        public Guid? ViewBy { get; set; }

        public string City { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Member Type")]
        public Guid? MemberTypeId { get; set; }

        [DisplayName("Member #")]
        public string MemberNo { get; set; }

        public string Keyword { get; set; }
        public string Communication { get; set; }

        [DisplayName("Client ID")]
        public string ClientID { get; set; }

        [DisplayName("IATA #.")]
        public string IATANo { get; set; }

        [DisplayName("Corp #.")]
        public string CorpNo { get; set; }

        public int? CreatedBy { get; set; }

        public bool IsAdminUser { get; set; }

        public bool IsNegRates { get; set; }
        public bool IsShowInActive { get; set; }
    }

    public class SearchAdvanceProfileResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public Guid? TitleId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayName("Telephone #")]
        public string TelephoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Remarks { get; set; }

        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion
    
    #region Search Reservation

    public class SearchReservationParametersVM
    {
        public SearchReservationParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("CVV #")]
        public string CVVNo { get; set; }

        [DisplayName("TA Record Locator")]
        public string TARecordLocator { get; set; }

        [DisplayName("Company")]
        public Guid? CompanyId { get; set; }

        [DisplayName("Group")]
        public Guid? GroupId { get; set; }

        [DisplayName("Group Code")]
        public Guid? BlockCodeId { get; set; }

        [DisplayName("Source")]
        public Guid? SourceId { get; set; }

        [DisplayName("Agent")]
        public Guid? AgentId { get; set; }

        [DisplayName("Contact")]
        public Guid? ContactId { get; set; }

        [DisplayName("Member Type")]
        public Guid? MemberTypeId { get; set; }

        [DisplayName("Member #")]
        public string MemberNo { get; set; }

        [DisplayName("Arrival From")]
        public string ArrivalFrom { get; set; }

        [DisplayName("Arrival To")]
        public string ArrivalTo { get; set; }

        [DisplayName("Confirmation#")]
        public string ConfirmationNo { get; set; }

        public bool IsShowCancelledReservation { get; set; }

        [DisplayName("Show Checked In")]
        public bool IsShowCheckedInReservation { get; set; }

        [DisplayName("Show Checked Out")]
        public bool IsShowCheckedOutReservation { get; set; }

        public int? CreatedBy { get; set; }
        public bool IsAdminUser { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchReservationResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [DisplayName("Phone #")]
        public string PhoneNo { get; set; }
        public string ArrivalDate { get; set; }
        public int NoOfNight { get; set; }
        public string DepartureDate { get; set; }
        public int NoOfAdult { get; set; }
        public int NoOfChildren { get; set; }
        public int NoOfRoom { get; set; }
        public string RoomTypeCode { get; set; }
        public string RateTypeCode { get; set; }
        public bool IsReservationCancel { get; set; }
        public bool IsCheckIn { get; set; }
        public bool IsCheckOut { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Rate Type

    public class SearchRateTypeParametersVM
    {
        public SearchRateTypeParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string RateTypeCode { get; set; }
        public double Amount { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRateTypeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string RateTypeCode { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }        
        public DateTime? CreatedOn { get; set; }
        public bool IsLeisRateType { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Room Type

    public class SearchRoomTypeParametersVM
    {
        public SearchRoomTypeParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string RoomTypeCode { get; set; }                
        public int RoomCapacity { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomTypeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string RoomTypeCode { get; set; }        
        public int RoomCapacity { get; set; }
        public string Description { get; set; }        
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Advance Room Type

    public class SearchAdvanceRoomTypeParametersVM
    {
        public SearchAdvanceRoomTypeParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Rate Type")]
        public Guid? RateTypeId { get; set; }

        [DisplayName("Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeCode { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Available Rooms")]
        public int AvailableRooms { get; set; }

        [DisplayName("Room Capacity")]
        public int RoomCapacity { get; set; }

        public bool IsWeekEndPrice { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchAdvanceRoomTypeResultVM
    {
        public int RowNum { get; set; }

        //Room Type
        public Guid RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public int AvailableRooms { get; set; }    
        public int RoomCapacity { get; set; }
        public string RoomTypeDescription { get; set; }

        //Rate Type
        public Guid RateTypeId { get; set; }
        public string RateTypeCode { get; set; }        
        public string RateTypeDescription { get; set; }

        //Room Type Rate Type Mapping
        public Guid RoomTypeRateTypeMappingId { get; set; }
        public double Amount { get; set; }        
        public string RoomTypeRateTypeMappingDescription { get; set; }        
    }
    #endregion

    #region Search Advance Room

    public class SearchAdvanceRoomParametersVM
    {
        public SearchAdvanceRoomParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Room Type")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Room#")]
        public string RoomNo { get; set; }

        [DisplayName("Code")]
        public string Type { get; set; }

        [DisplayName("Room Features")]
        public List<Guid> RoomFeatures { get; set; }

        public string RoomFeaturesIds { get; set; }

        [DisplayName("All")]
        public bool IsAll { get; set; }

        [DisplayName("Clean")]
        public bool IsClean { get; set; }

        [DisplayName("Dirty")]
        public bool IsDirty { get; set; }

        [DisplayName("Inspected")]
        public bool IsInspected { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchAdvanceRoomResultVM
    {
        public int RowNum { get; set; }

        //Room
        public Guid RoomId { get; set; }
        public string RoomNo { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        //Room Status
        public Guid? RoomStatusId { get; set; }
        public string RoomStatusName { get; set; }
        public string RoomStatusColor { get; set; }

        //Room Type
        public Guid RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public string RoomTypeDescription { get; set; }

        public string RoomFeatures { get; set; }

        public Guid? ReservationId { get; set; }


    }
    #endregion

    #region Search Room Type Rate Type Mapping 

    public class SearchRoomTypeRateTypeMappingParametersVM
    {
        public SearchRoomTypeRateTypeMappingParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public Guid? RoomTypeId { get; set; }
        public Guid? RateTypeId { get; set; }
        public double Amount { get; set; }
        public bool IsWeekEndPrice { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomTypeRateTypeMappingResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public Guid? RateTypeId { get; set; }
        public string RateTypeCode { get; set; }        
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Reservation Cancellation Reasons 

    public class SearchReservationCancellationReasonParametersVM
    {
        public SearchReservationCancellationReasonParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }        

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchReservationCancellationReasonResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Advance Search Reservation Cancellation Reasons 

    public class SearchAdvanceReservationCancellationReasonParametersVM
    {
        [DisplayName("Find")]
        public string Reason { get; set; }
    }

    public class SearchAdvanceReservationCancellationReasonResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }        

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region Search Preference Group

    public class SearchPreferenceGroupParametersVM
    {
        public SearchPreferenceGroupParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchPreferenceGroupResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Arrivals

    public class SearchArrivalsParametersVM
    {
        public SearchArrivalsParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }        

        [DisplayName("TA Record Locator")]
        public string TARecordLocator { get; set; }

        [DisplayName("Company")]
        public Guid? CompanyId { get; set; }

        [DisplayName("Contact")]
        public Guid? ContactId { get; set; }

        [DisplayName("Group")]
        public Guid? GroupId { get; set; }

        [DisplayName("Group Code")]
        public Guid? BlockCodeId { get; set; }

        [DisplayName("Agent")]
        public Guid? AgentId { get; set; }

        [DisplayName("Member Type")]
        public Guid? MemberTypeId { get; set; }

        [DisplayName("Member #")]
        public string MemberNo { get; set; }

        [DisplayName("Arrival Date")]
        public string ArrivalDate { get; set; }

        //[DisplayName("Arrival To")]
        //public string ArrivalTo { get; set; }

        [DisplayName("Confirmation#")]
        public string ConfirmationNo { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Room #")]
        public string RoomNumber { get; set; }

        [DisplayName("Checked In")]
        public bool IsShowCheckedIn { get; set; }

        public int? CreatedBy { get; set; }

        public bool IsAdminUser { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchArrivalsResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayName("Phone #")]
        public string PhoneNo { get; set; }
        public string ArrivalDate { get; set; }
        public int NoOfNight { get; set; }
        public string DepartureDate { get; set; }
        public int NoOfAdult { get; set; }
        public int NoOfChildren { get; set; }
        public int NoOfRoom { get; set; }
        public string RoomNumbers { get; set; }
        public string RoomTypeCode { get; set; }
        public string RateTypeCode { get; set; }
        public bool IsReservationCancel { get; set; }
        public bool IsCheckIn { get; set; }
        public bool IsCheckOut { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Guest

    public class SearchGuestParametersVM
    {
        public SearchGuestParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Room #")]
        public string RoomNo { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNo { get; set; }

        [DisplayName("Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Company")]
        public Guid? CompanyId { get; set; }

        [DisplayName("Group")]
        public Guid? GroupId { get; set; }

        [DisplayName("Group Code")]
        public Guid? BlockCodeId { get; set; }

        [DisplayName("Checked Out")]
        public  bool IsShowCheckedOut { get; set; }
        
        [DisplayName("Checked In")]
        public bool IsShowCheckedIn { get; set; }

        [DisplayName("Due Out")]
        public bool IsShowDueOut { get; set; }

        public int? CreatedBy { get; set; }
        public bool IsAdminUser { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchGuestResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string ArrivalDate { get; set; }        
        public string DepartureDate { get; set; }
                
        public string RoomNumbers { get; set; }
        public double? Balance { get; set; }
        public Guid? ReservationStatusId { get; set; }
        public string ReservationStatusName { get; set; }

        public bool IsReservationCancel { get; set; }
        public bool IsCheckIn { get; set; }
        public bool IsCheckOut { get; set; }

        public Guid? CompanyId { get; set; }
        public string Company { get; set; }

        public Guid? GroupId { get; set; }
        public string Group { get; set; }
                
        public DateTime? CreatedOn { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Additional Charge

    public class SearchAdditionalChargeParametersVM
    {
        public SearchAdditionalChargeParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Please enter valid price.")]
        public double Price { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchAdditionalChargeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }        
        public string Code { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Advance Search Additional Charge 

    public class SearchAdvanceAdditionalChargeParametersVM
    {
        [DisplayName("Find")]
        public string Code { get; set; }
    }

    public class SearchAdvanceAdditionalChargeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region Search Country

    public class SearchCountryParametersVM
    {
        public SearchCountryParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchCountryResultVM
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search State

    public class SearchStateParametersVM
    {
        public SearchStateParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Country")]        
        public int? CountryId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchStateResultVM
    {
        public int RowNum { get; set; }
        public int Id { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search City

    public class SearchCityParametersVM
    {
        public SearchCityParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [DisplayName("State")]
        public int? StateId { get; set; }
        
        public string Name { get; set; }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchCityResultVM
    {
        public int RowNum { get; set; }
        public int Id { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public int? StateId { get; set; }
        public string StateName { get; set; }
        
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Nationality

    public class SearchNationalityParametersVM
    {
        public SearchNationalityParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchNationalityResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion
    
    #region Search Floor

    public class SearchFloorParametersVM
    {
        public SearchFloorParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchFloorResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        [DisplayName("Floor #")]
        public int? FloorNo { get; set; }

        [DisplayName("No Of Room")]
        public int NoOfRoom { get; set; }

        [DisplayName("Room Types Info")]
        public string RoomTypesInfo { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Reservation Type

    public class SearchReservationTypeParametersVM
    {
        public SearchReservationTypeParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }   

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchReservationTypeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Package

    public class SearchPackageParametersVM
    {
        public SearchPackageParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? Price { get; set; }

        [DisplayName("Calculation Ratio")]
        public Guid? CalculationRatioId { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchPackageResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double Price { get; set; }

        [DisplayName("Calculation Ratio")]
        public Guid? CalculationRatioId { get; set; }

        public string CalculationRatio { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Market

    public class SearchMarketParametersVM
    {
        public SearchMarketParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchMarketResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Reservation Source

    public class SearchReservationSourceParametersVM
    {
        public SearchReservationSourceParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchReservationSourceResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Payment Method

    public class SearchPaymentMethodParametersVM
    {
        public SearchPaymentMethodParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchPaymentMethodResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search User Role

    public class SearchUserRoleParametersVM
    {
        public SearchUserRoleParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchUserRoleResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search User Detail

    public class SearchUserDetailParametersVM
    {
        public SearchUserDetailParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("User Role")]
        public Guid? UserRoleId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchUserDetailResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("User Role")]
        public Guid? UserRoleId { get; set; }

        public string UserRoleName { get; set; }
        public string UserRoleCode { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsRecordActivity { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Tutor Detail

    public class SearchTutorDetailParametersVM
    {
        public SearchTutorDetailParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchTutorDetailResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Student Detail

    public class SearchStudentDetailParametersVM
    {
        public SearchStudentDetailParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }        

        public Guid TutorId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchStudentDetailResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsRecordActivity { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Room Features

    public class SearchRoomFeatureParametersVM
    {
        public SearchRoomFeatureParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomFeatureResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Room

    public class SearchRoomParametersVM
    {
        public SearchRoomParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Room Type")]
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Floor")]
        public Guid? FloorId { get; set; }

        [DisplayName("Room #")]
        public string RoomNo { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomResultVM
    {
        public int RowNum { get; set; }

        public Guid Id { get; set; }
        public string Type { get; set; }
        public string RoomNo { get; set; }
        public string Description { get; set; }

        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type Code")]
        public string RoomTypeCode { get; set; }

        public Guid? FloorId { get; set; }

        [DisplayName("Floor Name")]
        public string FloorName { get; set; }

        [DisplayName("Floor #")]
        public int? FloorNo { get; set; }

        [DisplayName("Room Features")]
        public string RoomFeatures { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Users Activity Log

    public class SearchUsersActivityLogParametersVM
    {
        public SearchUsersActivityLogParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("User")]
        public Guid? UserId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchUsersActivityLogResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public string PageName { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string RecordedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Error Log

    public class SearchErrorLogParametersVM
    {
        public SearchErrorLogParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string Error { get; set; }
        public DateTime? Date { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchErrorLogResultVM
    {
        public int RowNum { get; set; }

        public int? Id { get; set; }

        [DisplayName("Page Url")]
        public string PageUrl { get; set; }

        [DisplayName("Method Name")]
        public string MethodName { get; set; }

        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }

        [DisplayName("Target Site")]
        public string TargetSite { get; set; }

        [DisplayName("Stack Trace")]
        public string StackTrace { get; set; }

        [DisplayName("Inner Exception")]
        public string InnerException { get; set; }

        [DisplayName("Created DateTime")]
        public string CreatedDateTime { get; set; }

        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Title

    public class SearchTitleParametersVM
    {
        public SearchTitleParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Title")]        
        public string Title { get; set; }

        [DisplayName("Salutation")]
        public string Salutation { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchTitleResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Salutation")]
        public string Salutation { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Holiday

    public class SearchHolidayParametersVM
    {
        public SearchHolidayParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }
        
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? Date { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchHolidayResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Reservation Log

    public class SearchUsersReservationLogParametersVM
    {
        public SearchUsersReservationLogParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("User")]
        public Guid? UserGUID { get; set; }

        public int UserId { get; set; }
        
        [DisplayName("Start Date")]        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchUsersReservationLogResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public Guid? ProfileId { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNumber { get; set; }

        [DisplayName("Last Name")]        
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Arrival Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Adults")]
        public int NoOfAdult { get; set; }

        [DisplayName("Children")]
        public int NoOfChildren { get; set; }

        [DisplayName("No Of Rooms.")]
        public int NoOfRoom { get; set; }
                        
        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]        
        public string RoomTypeCode { get; set; }       

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }
        
        public Guid? RateTypeId { get; set; }

        public string RateTypeCode { get; set; }

        [DisplayName("Rate")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? Rate { get; set; }

        [DisplayName("Guest Balance")]
        public double? GuestBalance { get; set; }

        public DateTime? CreatedOn { get; set; }        
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Reservation Folio Log

    public class SearchUsersReservationFolioLogParametersVM
    {
        public SearchUsersReservationFolioLogParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("User")]
        public Guid? UserGUID { get; set; }

        public int UserId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchUsersReservationFolioLogResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }

        public Guid? ProfileId { get; set; }

        [DisplayName("Confirmation #")]
        public string ConfirmationNumber { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Arrival Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Nights")]
        public int NoOfNight { get; set; }

        [DisplayName("Departure Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0 : dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }

        [DisplayName("Adults")]
        public int NoOfAdult { get; set; }

        [DisplayName("Children")]
        public int NoOfChildren { get; set; }

        [DisplayName("No Of Rooms.")]
        public int NoOfRoom { get; set; }

        public Guid? RoomTypeId { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeCode { get; set; }

        [DisplayName("Room #")]
        public string RoomNumbers { get; set; }

        public Guid? RateTypeId { get; set; }

        public string RateTypeCode { get; set; }

        [DisplayName("Rate")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? Rate { get; set; }

        [DisplayName("Guest Balance")]
        public double? GuestBalance { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int TotalCount { get; set; }
    }

    #endregion

    #region Search Origin

    public class SearchOriginParametersVM
    {
        public SearchOriginParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchOriginResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search Discount Reason

    public class SearchDiscountReasonParametersVM
    {
        public SearchDiscountReasonParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchDiscountReasonResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Search RTC

    public class SearchRTCParametersVM
    {
        public SearchRTCParametersVM()
        {
            columns = new List<ColumnName>();
            order = new List<ColumnOrderInfo>();
        }

        public int PageNum { get; set; }
        public int PageSize { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRTCResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Advance Package Search

    public class SearchAdvancePackageParametersVM
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? Price { get; set; }

        [DisplayName("Calculation Ratio")]
        public Guid? CalculationRatioId { get; set; }
    }

    public class SearchAdvancePackageResultVM
    {
        public int RowNum { get; set; }        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double Price { get; set; }

        [DisplayName("Calculation Ratio")]
        public Guid? CalculationRatioId { get; set; }

        public string CalculationRatio { get; set; }

        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }        
    }

    #endregion

    #region DataTable Column Info
    public class ColumnName
    {
        public string data { get; set; }
    }

    public class ColumnOrderInfo
    {
        public string column { get; set; }

        public string dir { get; set; }
    }
    #endregion

}
