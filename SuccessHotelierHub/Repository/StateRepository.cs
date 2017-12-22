using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class StateRepository
    {
        public List<StateVM> GetStates(int? countryId)
        {
            SqlParameter[] parameters = 
                {
                    new SqlParameter { ParameterName = "@CountryId", Value = countryId}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetStates", parameters);

            var states = new List<StateVM>();
            states = DALHelper.CreateListFromTable<StateVM>(dt);

            return states;
        }
    }
}