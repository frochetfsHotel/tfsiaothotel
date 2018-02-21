using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class DiscountApprovedRepository
    {
        #region Discount Approved

        public List<DiscountApprovedVM> GetDiscountApprovedDetail()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetDiscountApprovedDetail");

            var discountApprovedDetails = new List<DiscountApprovedVM>();
            discountApprovedDetails = DALHelper.CreateListFromTable<DiscountApprovedVM>(ds);

            return discountApprovedDetails;
        }

        public List<DiscountApprovedVM> GetDiscountApprovedById(Guid discountApprovedId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = discountApprovedId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetDiscountApprovedById", parameters);

            var discountApproved = new List<DiscountApprovedVM>();
            discountApproved = DALHelper.CreateListFromTable<DiscountApprovedVM>(dt);

            return discountApproved;
        }

        public string AddDiscountApproved(DiscountApprovedVM discountApproved)
        {
            string discountApprovedId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = discountApproved.Code },
                    new SqlParameter { ParameterName = "@Description", Value = discountApproved.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = discountApproved.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = discountApproved.CreatedBy }
                };

            discountApprovedId = Convert.ToString(DALHelper.ExecuteScalar("AddDiscountApproved", parameters));

            return discountApprovedId;
        }

        public string UpdateDiscountApproved(DiscountApprovedVM discountApproved)
        {
            string discountApprovedId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = discountApproved.Id },
                    new SqlParameter { ParameterName = "@Code", Value = discountApproved.Code },
                    new SqlParameter { ParameterName = "@Description", Value = discountApproved.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = discountApproved.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = discountApproved.UpdatedBy }
                };

            discountApprovedId = Convert.ToString(DALHelper.ExecuteScalar("UpdateDiscountApproved", parameters));

            return discountApprovedId;
        }

        public string DeleteDiscountApproved(Guid id, int updatedBy)
        {
            string discountApprovedId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            discountApprovedId = Convert.ToString(DALHelper.ExecuteScalar("DeleteDiscountApproved", parameters));

            return discountApprovedId;
        }

        public List<SearchDiscountApprovedResultVM> SearchDiscountApproved(SearchDiscountApprovedParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchDiscountApproved", parameters);

            var discountApprovedDetails = new List<SearchDiscountApprovedResultVM>();
            discountApprovedDetails = DALHelper.CreateListFromTable<SearchDiscountApprovedResultVM>(dt);

            return discountApprovedDetails;
        }

        #endregion
    }
}