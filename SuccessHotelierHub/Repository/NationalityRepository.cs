using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;


namespace SuccessHotelierHub.Repository
{
    public class NationalityRepository
    {
        public List<NationalityVM> GetNationality()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetNationality");

            var nationalities = new List<NationalityVM>();
            nationalities = DALHelper.CreateListFromTable<NationalityVM>(ds);

            return nationalities;
        }
    }
}