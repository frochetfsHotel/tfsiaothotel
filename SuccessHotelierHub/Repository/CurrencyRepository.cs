using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub.Repository
{
    public class CurrencyRepository
    {
        #region Currency

        public List<CurrencyVM> GetCurrencyInfo()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCurrencyInfo");

            var currencyList = new List<CurrencyVM>();
            currencyList = DALHelper.CreateListFromTable<CurrencyVM>(dt);

            return currencyList;
        }

        public List<CurrencyVM> GetCurrencyInfoById(Guid currencyId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Id", Value = currencyId }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCurrencyInfoById", parameters);

            var currencyList = new List<CurrencyVM>();
            currencyList = DALHelper.CreateListFromTable<CurrencyVM>(dt);

            return currencyList;
        }

        public List<CurrencyVM> GetCurrencyInfoByCode(string code)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Code", Value = code }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCurrencyInfoByCode", parameters);

            var currencyList = new List<CurrencyVM>();
            currencyList = DALHelper.CreateListFromTable<CurrencyVM>(dt);

            return currencyList;
        }

        #endregion
    }
}