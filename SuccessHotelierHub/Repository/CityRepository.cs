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
        public List<CityVM> GetCities(int? stateId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@StateId", Value = stateId}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCities", parameters);

            var cities = new List<CityVM>();
            cities = DALHelper.CreateListFromTable<CityVM>(dt);

            return cities;
        }
    }
}