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

        public string AddCurrency(CurrencyVM currency)
        {
            string currencyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = currency.Code },
                    new SqlParameter { ParameterName = "@Name", Value = currency.Name },
                    new SqlParameter { ParameterName = "@Description", Value = currency.Description },
                    new SqlParameter { ParameterName = "@ConversionRate", Value = currency.ConversionRate },
                    new SqlParameter { ParameterName = "@CurrencySymbol", Value = currency.CurrencySymbol },
                    new SqlParameter { ParameterName = "@IsActive", Value = currency.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = currency.CreatedBy }
                };

            currencyId = Convert.ToString(DALHelper.ExecuteScalar("AddCurrency", parameters));

            return currencyId;
        }

        public string UpdateCurrency(CurrencyVM currency)
        {
            string currencyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = currency.Id },
                    new SqlParameter { ParameterName = "@Code", Value = currency.Code },
                    new SqlParameter { ParameterName = "@Name", Value = currency.Name },
                    new SqlParameter { ParameterName = "@Description", Value = currency.Description },
                    new SqlParameter { ParameterName = "@ConversionRate", Value = currency.ConversionRate },
                    new SqlParameter { ParameterName = "@CurrencySymbol", Value = currency.CurrencySymbol },
                    new SqlParameter { ParameterName = "@IsActive", Value = currency.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = currency.UpdatedBy }
                };

            currencyId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCurrency", parameters));

            return currencyId;
        }

        public string DeleteCurrency(Guid id, int updatedBy)
        {
            string currencyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            currencyId = Convert.ToString(DALHelper.ExecuteScalar("DeleteCurrency", parameters));

            return currencyId;
        }

        public List<SearchCurrencyResultVM> SearchCurrency(SearchCurrencyParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCurrency", parameters);

            var currencyInfo = new List<SearchCurrencyResultVM>();
            currencyInfo = DALHelper.CreateListFromTable<SearchCurrencyResultVM>(dt);

            return currencyInfo;
        }

        public List<CurrencyVM> CheckCurrencyCodeExist(Guid? id, string code)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Code", Value = code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckCurrencyCodeExist", parameters);

            var currency = new List<CurrencyVM>();
            currency = DALHelper.CreateListFromTable<CurrencyVM>(dt);

            return currency;
        }

        public List<CurrencyVM> CheckCurrencyNameExist(Guid? id, string name)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Name", Value = name }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckCurrencyNameExist", parameters);

            var currency = new List<CurrencyVM>();
            currency = DALHelper.CreateListFromTable<CurrencyVM>(dt);

            return currency;
        }

        #endregion
    }
}