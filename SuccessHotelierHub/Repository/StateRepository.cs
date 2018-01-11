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
        #region State

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

        public List<StateVM> GetStateById(int stateId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = stateId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetStateById", parameters);

            var state = new List<StateVM>();
            state = DALHelper.CreateListFromTable<StateVM>(dt);

            return state;
        }

        public string AddState(StateVM state)
        {
            string stateId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CountryId", Value = state.CountryId },
                    new SqlParameter { ParameterName = "@Code", Value = state.Code },
                    new SqlParameter { ParameterName = "@Name", Value = state.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = state.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = state.CreatedBy }
                };

            stateId = Convert.ToString(DALHelper.ExecuteScalar("AddState", parameters));

            return stateId;
        }

        public string UpdateState(StateVM state)
        {
            string stateId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = state.Id },
                    new SqlParameter { ParameterName = "@CountryId", Value = state.CountryId },
                    new SqlParameter { ParameterName = "@Code", Value = state.Code },
                    new SqlParameter { ParameterName = "@Name", Value = state.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = state.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = state.UpdatedBy }
                };

            stateId = Convert.ToString(DALHelper.ExecuteScalar("UpdateState", parameters));

            return stateId;
        }

        public string DeleteState(int id, int updatedBy)
        {
            string stateId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            stateId = Convert.ToString(DALHelper.ExecuteScalar("DeleteState", parameters));

            return stateId;
        }

        public List<SearchStateResultVM> SearchState(SearchStateParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CountryId", Value = model.CountryId },
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchState", parameters);

            var states = new List<SearchStateResultVM>();
            states = DALHelper.CreateListFromTable<SearchStateResultVM>(dt);

            return states;
        }

        #endregion
    }
}