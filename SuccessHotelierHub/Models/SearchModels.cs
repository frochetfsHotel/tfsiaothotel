using System;
using System.Collections.Generic;
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