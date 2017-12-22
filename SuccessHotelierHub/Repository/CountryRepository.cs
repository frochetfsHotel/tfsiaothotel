using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class CountryRepository
    {
        public List<CountryVM> GetCountries()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCountries");

            var countries = new List<CountryVM>();
            countries = DALHelper.CreateListFromTable<CountryVM>(dt);

            return countries;
        }
    }
}