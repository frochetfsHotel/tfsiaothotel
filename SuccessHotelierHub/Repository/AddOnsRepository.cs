using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class AddOnsRepository
    {
        #region Add Ons

        public List<AddOnsVM> GetAddOns()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetAddOns");

            var addOns = new List<AddOnsVM>();
            addOns = DALHelper.CreateListFromTable<AddOnsVM>(dt);

            return addOns;
        }

        public List<AddOnsVM> GetAddOnsById(Guid addOnsId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = addOnsId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetAddOnsById", parameters);

            var addOns = new List<AddOnsVM>();
            addOns = DALHelper.CreateListFromTable<AddOnsVM>(dt);

            return addOns;
        }

        public string AddAddOns(AddOnsVM addOns)
        {
            string addOnsId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = addOns.Name },
                    new SqlParameter { ParameterName = "@Description", Value = addOns.Description },
                    new SqlParameter { ParameterName = "@Price", Value = addOns.Price },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = addOns.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = addOns.CreatedBy }
                };

            addOnsId = Convert.ToString(DALHelper.ExecuteScalar("AddAddOns", parameters));

            return addOnsId;
        }

        public string UpdateAddOns(AddOnsVM addOns)
        {
            string addOnsId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = addOns.Id },
                    new SqlParameter { ParameterName = "@Name", Value = addOns.Name },
                    new SqlParameter { ParameterName = "@Description", Value = addOns.Description },
                    new SqlParameter { ParameterName = "@Price", Value = addOns.Price },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = addOns.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = addOns.UpdatedBy }
                };

            addOnsId = Convert.ToString(DALHelper.ExecuteScalar("UpdateAddOns", parameters));

            return addOnsId;
        }

        public string DeleteAddOns(Guid id, int updatedBy)
        {
            string addOnsId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            addOnsId = Convert.ToString(DALHelper.ExecuteScalar("DeleteAddOns", parameters));

            return addOnsId;
        }

        public List<SearchAddOnsResultVM> SearchAddOns(SearchAddOnsParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Price", Value = model.Price },                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAddOns", parameters);

            var addOns = new List<SearchAddOnsResultVM>();
            addOns = DALHelper.CreateListFromTable<SearchAddOnsResultVM>(dt);

            return addOns;
        }

        public List<SearchAdvanceAddOnsResultVM> SearchAdvanceAddOns(SearchAdvanceAddOnsParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Price", Value = model.Price },                    
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceAddOns", parameters);

            var addOns = new List<SearchAdvanceAddOnsResultVM>();
            addOns = DALHelper.CreateListFromTable<SearchAdvanceAddOnsResultVM>(dt);

            return addOns;
        }

        #endregion
    }
}