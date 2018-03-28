using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using System.Configuration;
using System.Web.Mvc;
using System.Net;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub
{
    public class CurrencyManager
    {
        public static List<CurrencyVM> GetCurrencyInfo()
        {
            var currencyList = new List<CurrencyVM>();

            if (System.Web.HttpContext.Current.Application["CurrencyInfo"] != null)
            {
                currencyList = (List<CurrencyVM>)System.Web.HttpContext.Current.Application["CurrencyInfo"];
            }

            return currencyList;
        }

        /// <summary>
        /// This method is used to get curreny information by id.
        /// </summary>
        /// <param name="currencyId">The currency id.</param>
        /// <returns>CurrencyVM Object</returns>
        public static CurrencyVM GetCurrencyInfoById(Guid currencyId)
        {
            var currencyInfo = new CurrencyVM();
            
            if(System.Web.HttpContext.Current.Application["CurrencyInfo"] != null)
            {
                var currencyInfoList = (List<CurrencyVM>) System.Web.HttpContext.Current.Application["CurrencyInfo"];

                if(currencyInfoList != null && currencyInfoList.Count > 0)
                {
                    currencyInfo = currencyInfoList.Where(m => m.Id == currencyId).FirstOrDefault();
                }
            }

            return currencyInfo;
        }

        /// <summary>
        /// This method is used to get curreny information by code.
        /// </summary>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns>CurrencyVM Object</returns>
        public static CurrencyVM GetCurrencyInfoByCode(string currencyCode)
        {
            var currencyInfo = new CurrencyVM();

            if (System.Web.HttpContext.Current.Application["CurrencyInfo"] != null)
            {
                var currencyInfoList = (List<CurrencyVM>)System.Web.HttpContext.Current.Application["CurrencyInfo"];

                if (currencyInfoList != null && currencyInfoList.Count > 0)
                {
                    currencyInfo = currencyInfoList.Where(m => m.Code == currencyCode).FirstOrDefault();
                }
            }

            return currencyInfo;
        }

        /// <summary>
        /// This method is used to parse given amount to user's default currency.
        /// </summary>
        /// <param name="amount">The given amount</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="roundToNearest">The round to nearest value.</param>
        /// <returns>double</returns>
        public static double ParseAmountToUserCurrency(double? amount, string currencyCode, int roundToNearest = 2)
        {
            double parseAmount = 0;

            if(amount.HasValue && amount > 0)
            {
                var currencyInfo = GetCurrencyInfoByCode(currencyCode);

                if(currencyInfo != null)
                {
                    if (currencyInfo.ConversionRate.HasValue)
                    {
                        parseAmount = Math.Round(amount.Value * currencyInfo.ConversionRate.Value, roundToNearest);
                    }
                    else
                    {
                        parseAmount = Math.Round(amount.Value * 1, roundToNearest);
                    }
                }
            }
            return parseAmount;
        }

        /// <summary>
        /// This method is used to convert given amount to base (EURO) currency.
        /// </summary>
        /// <param name="amount">The given amount</param>
        /// <param name="currencyCode">The currency code</param>
        /// <returns>double</returns>
        public static double ConvertAmountToBaseCurrency(double? amount, string currencyCode)
        {
            double baseCurrencyAmount = 0;

            if (amount.HasValue && amount > 0)
            {
                var currencyInfo = GetCurrencyInfoByCode(currencyCode);

                if (currencyInfo != null)
                {
                    if (currencyInfo.ConversionRate.HasValue)
                    {
                        baseCurrencyAmount = Math.Round(amount.Value / currencyInfo.ConversionRate.Value, 2);
                    }
                    else
                    {
                        baseCurrencyAmount = Math.Round(amount.Value / 1, 2);
                    }
                }
            }
            return baseCurrencyAmount;
        }
    }
}