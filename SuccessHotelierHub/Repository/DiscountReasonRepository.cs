using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class DiscountReasonRepository
    {
        #region Discount Reason

        public List<DiscountReasonVM> GetDiscountReasons()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetDiscountReasons");

            var discountReasons = new List<DiscountReasonVM>();
            discountReasons = DALHelper.CreateListFromTable<DiscountReasonVM>(ds);

            return discountReasons;
        }

        public List<DiscountReasonVM> GetDiscountReasonById(Guid discountReasonId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = discountReasonId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetDiscountReasonById", parameters);

            var discountReason = new List<DiscountReasonVM>();
            discountReason = DALHelper.CreateListFromTable<DiscountReasonVM>(dt);

            return discountReason;
        }

        public string AddDiscountReason(DiscountReasonVM discountReason)
        {
            string discountReasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = discountReason.Code },
                    new SqlParameter { ParameterName = "@Description", Value = discountReason.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = discountReason.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = discountReason.CreatedBy }
                };

            discountReasonId = Convert.ToString(DALHelper.ExecuteScalar("AddDiscountReason", parameters));

            return discountReasonId;
        }

        public string UpdateDiscountReason(DiscountReasonVM discountReason)
        {
            string discountReasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = discountReason.Id },
                    new SqlParameter { ParameterName = "@Code", Value = discountReason.Code },
                    new SqlParameter { ParameterName = "@Description", Value = discountReason.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = discountReason.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = discountReason.UpdatedBy }
                };

            discountReasonId = Convert.ToString(DALHelper.ExecuteScalar("UpdateDiscountReason", parameters));

            return discountReasonId;
        }

        public string DeleteDiscountReason(Guid id, int updatedBy)
        {
            string discountReasonId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            discountReasonId = Convert.ToString(DALHelper.ExecuteScalar("DeleteDiscountReason", parameters));

            return discountReasonId;
        }

        public List<SearchDiscountReasonResultVM> SearchDiscountReason(SearchDiscountReasonParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchDiscountReason", parameters);

            var discountReasons = new List<SearchDiscountReasonResultVM>();
            discountReasons = DALHelper.CreateListFromTable<SearchDiscountReasonResultVM>(dt);

            return discountReasons;
        }

        #endregion
    }
}