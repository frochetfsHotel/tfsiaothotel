using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class CityRepository
    {
        #region City

        public List<CityVM> GetCities(int? stateId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@StateId", Value = stateId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCities", parameters);

            var cities = new List<CityVM>();
            cities = DALHelper.CreateListFromTable<CityVM>(dt);

            return cities;
        }

        public List<CityVM> GetCityById(int cityId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = cityId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCityById", parameters);

            var city = new List<CityVM>();
            city = DALHelper.CreateListFromTable<CityVM>(dt);

            return city;
        }

        public string AddCity(CityVM city)
        {
            string cityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@StateId", Value = city.StateId },                    
                    new SqlParameter { ParameterName = "@Name", Value = city.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = city.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = city.CreatedBy }
                };

            cityId = Convert.ToString(DALHelper.ExecuteScalar("AddCity", parameters));

            return cityId;
        }

        public string UpdateCity(CityVM city)
        {
            string cityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = city.Id },
                    new SqlParameter { ParameterName = "@StateId", Value = city.StateId },
                    new SqlParameter { ParameterName = "@Name", Value = city.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = city.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = city.UpdatedBy }
                };

            cityId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCity", parameters));

            return cityId;
        }

        public string DeleteCity(int id, int updatedBy)
        {
            string cityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            cityId = Convert.ToString(DALHelper.ExecuteScalar("DeleteCity", parameters));

            return cityId;
        }

        public List<SearchCityResultVM> SearchCity(SearchCityParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CountryId", Value = model.CountryId },
                    new SqlParameter { ParameterName = "@StateId", Value = model.StateId },                    
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCity", parameters);

            var cities = new List<SearchCityResultVM>();
            cities = DALHelper.CreateListFromTable<SearchCityResultVM>(dt);

            return cities;
        }

        #endregion
    }
}