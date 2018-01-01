using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Fist Name")]
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

        [DisplayName("Fist Name")]
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
        public int NoOfRooms { get; set; }
        public int RoomCapacity { get; set; }

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomTypeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string RoomTypeCode { get; set; }
        public int NoOfRooms { get; set; }
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