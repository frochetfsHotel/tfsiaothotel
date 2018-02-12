using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class MarketRepository
    {
        #region Market

        public List<MarketVM> GetMarkets()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetMarkets");

            var markets = new List<MarketVM>();
            markets = DALHelper.CreateListFromTable<MarketVM>(ds);

            return markets;
        }

        public List<MarketVM> GetMarketById(Guid marketId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = marketId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetMarketById", parameters);

            var market = new List<MarketVM>();
            market = DALHelper.CreateListFromTable<MarketVM>(dt);

            return market;
        }

        public string AddMarket(MarketVM market)
        {
            string marketId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = market.Name },
                    new SqlParameter { ParameterName = "@Description", Value = market.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = market.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = market.CreatedBy }
                };

            marketId = Convert.ToString(DALHelper.ExecuteScalar("AddMarket", parameters));

            return marketId;
        }

        public string UpdateMarket(MarketVM market)
        {
            string marketId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = market.Id },
                    new SqlParameter { ParameterName = "@Name", Value = market.Name },
                    new SqlParameter { ParameterName = "@Description", Value = market.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = market.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = market.UpdatedBy }
                };

            marketId = Convert.ToString(DALHelper.ExecuteScalar("UpdateMarket", parameters));

            return marketId;
        }

        public string DeleteMarket(Guid id, int updatedBy)
        {
            string marketId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            marketId = Convert.ToString(DALHelper.ExecuteScalar("DeleteMarket", parameters));

            return marketId;
        }

        public List<SearchMarketResultVM> SearchMarket(SearchMarketParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchMarket", parameters);

            var markets = new List<SearchMarketResultVM>();
            markets = DALHelper.CreateListFromTable<SearchMarketResultVM>(dt);

            return markets;
        }

        public List<MarketVM> GetMarketByName(string marketName)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Name", Value = marketName }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetMarketByName", parameters);

            var market = new List<MarketVM>();
            market = DALHelper.CreateListFromTable<MarketVM>(dt);

            return market;
        }

        #endregion
    }
}