﻿using System;
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
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }        
        public int TotalCount { get; set; }
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

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

        [DisplayName("Member No.")]
        public string MemberNo { get; set; }

        public string Keyword { get; set; }
        public string Communication { get; set; }

        [DisplayName("Client ID")]
        public string ClientID { get; set; }

        [DisplayName("IATA No.")]
        public string IATANo { get; set; }

        [DisplayName("Corp No.")]
        public string CorpNo { get; set; }

        public bool IsNegRates { get; set; }
        public bool IsShowInActive { get; set; }
    }

    public class SearchAdvanceProfileResultVM
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

        [DisplayName("CRS No")]
        public string CRSNo { get; set; }

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

        [DisplayName("Member No")]
        public string MemberNo { get; set; }

        [DisplayName("Arrival From")]
        public string ArrivalFrom { get; set; }

        [DisplayName("Arrival To")]
        public string ArrivalTo { get; set; }

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
        public string PhoneNo { get; set; }
        public string ArrivalDate { get; set; }
        public int NoOfNight { get; set; }
        public string DepartureDate { get; set; }
        public int NoOfAdult { get; set; }
        public int NoOfChildren { get; set; }
        public int NoOfRoom { get; set; }
        public string RoomTypeCode { get; set; }
        public string RateTypeCode { get; set; }
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

        public List<ColumnName> columns { get; set; }
        public List<ColumnOrderInfo> order { get; set; }
    }

    public class SearchRoomTypeResultVM
    {
        public int RowNum { get; set; }
        public Guid Id { get; set; }
        public string RoomTypeCode { get; set; }
        public int NoOfRooms { get; set; }
        public string Description { get; set; }        
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int TotalCount { get; set; }
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