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
    public class RateTypeCategoryRepository
    {
        public List<RateTypeCategoryVM> GetRateTypeCategory()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeCategory");

            var rateTypeCategoryList = new List<RateTypeCategoryVM>();
            rateTypeCategoryList = DALHelper.CreateListFromTable<RateTypeCategoryVM>(dt);

            return rateTypeCategoryList;
        }

        public List<RateTypeCategoryVM> GetRateTypeCategoryByCode(string code)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Code", Value = code }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeCategoryByCode", parameters);

            var rateTypeCategoryList = new List<RateTypeCategoryVM>();
            rateTypeCategoryList = DALHelper.CreateListFromTable<RateTypeCategoryVM>(dt);

            return rateTypeCategoryList;
        }

        public List<RateTypeCategoryVM> GetRateTypeCategoryByName(string name)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Name", Value = name }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRateTypeCategoryByName", parameters);

            var rateTypeCategoryList = new List<RateTypeCategoryVM>();
            rateTypeCategoryList = DALHelper.CreateListFromTable<RateTypeCategoryVM>(dt);

            return rateTypeCategoryList;
        }
    }
}