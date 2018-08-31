using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
namespace SuccessHotelierHub.Repository
{
    public class CountryRepository
    {
        #region Country

        public List<CountryVM> GetCountries()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCountries");

            var countries = new List<CountryVM>();
            countries = DALHelper.CreateListFromTable<CountryVM>(dt);

            return countries;
        }
        
        public List<CountryVM> GetCountryById(int countryId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = countryId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCountryById", parameters);

            var country = new List<CountryVM>();
            country = DALHelper.CreateListFromTable<CountryVM>(dt);

            return country;
        }

        public string AddCountry(CountryVM country)
        {
            string countryId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = country.Code },
                    new SqlParameter { ParameterName = "@Name", Value = country.Name },
                    new SqlParameter { ParameterName = "@SortOrder", Value = country.SortOrder },
                    new SqlParameter { ParameterName = "@IsActive", Value = country.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = country.CreatedBy }
                };

            countryId = Convert.ToString(DALHelper.ExecuteScalar("AddCountry", parameters));

            return countryId;
        }

        public string UpdateCountry(CountryVM country)
        {
            string countryId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = country.Id },
                    new SqlParameter { ParameterName = "@Code", Value = country.Code },
                    new SqlParameter { ParameterName = "@Name", Value = country.Name },
                    new SqlParameter { ParameterName = "@SortOrder", Value = country.SortOrder },
                    new SqlParameter { ParameterName = "@IsActive", Value = country.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = country.UpdatedBy }
                };

            countryId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCountry", parameters));

            return countryId;
        }

        public string DeleteCountry(int id, int updatedBy)
        {
            string countryId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            countryId = Convert.ToString(DALHelper.ExecuteScalar("DeleteCountry", parameters));
            
            return countryId;
        }

        public List<SearchCountryResultVM> SearchCountry(SearchCountryParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@SortOrder", Value = model.SortOrder },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCountry", parameters);

            var countries = new List<SearchCountryResultVM>();
            countries = DALHelper.CreateListFromTable<SearchCountryResultVM>(dt);

            return countries;
        }

        public List<CountryVM> CheckCountryCodeAvailable(int? id, string code)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Code", Value = code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckCountryCodeAvailable", parameters);

            var country = new List<CountryVM>();
            country = DALHelper.CreateListFromTable<CountryVM>(dt);

            return country;
        }

        #endregion
    }
}