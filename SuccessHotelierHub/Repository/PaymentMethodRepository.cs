using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class PaymentMethodRepository
    {
        #region Payment Method

        public List<PaymentMethodVM> GetPaymentMethods()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetPaymentMethods");

            var paymentMethods = new List<PaymentMethodVM>();
            paymentMethods = DALHelper.CreateListFromTable<PaymentMethodVM>(ds);

            return paymentMethods;
        }

        public List<PaymentMethodVM> GetPaymentMethodById(Guid paymentMethodId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = paymentMethodId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPaymentMethodById", parameters);

            var paymentMethod = new List<PaymentMethodVM>();
            paymentMethod = DALHelper.CreateListFromTable<PaymentMethodVM>(dt);

            return paymentMethod;
        }

        public string AddPaymentMethod(PaymentMethodVM paymentMethod)
        {
            string paymentMethodId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = paymentMethod.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = paymentMethod.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = paymentMethod.CreatedBy }
                };

            paymentMethodId = Convert.ToString(DALHelper.ExecuteScalar("AddPaymentMethod", parameters));

            return paymentMethodId;
        }

        public string UpdatePaymentMethod(PaymentMethodVM paymentMethod)
        {
            string paymentMethodId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = paymentMethod.Id },
                    new SqlParameter { ParameterName = "@Name", Value = paymentMethod.Name },
                    new SqlParameter { ParameterName = "@IsActive", Value = paymentMethod.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = paymentMethod.UpdatedBy }
                };

            paymentMethodId = Convert.ToString(DALHelper.ExecuteScalar("UpdatePaymentMethod", parameters));

            return paymentMethodId;
        }

        public string DeletePaymentMethod(Guid id, int updatedBy)
        {
            string paymentMethodId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            paymentMethodId = Convert.ToString(DALHelper.ExecuteScalar("DeletePaymentMethod", parameters));

            return paymentMethodId;
        }

        public List<SearchPaymentMethodResultVM> SearchPaymentMethod(SearchPaymentMethodParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchPaymentMethod", parameters);

            var paymentMethods = new List<SearchPaymentMethodResultVM>();
            paymentMethods = DALHelper.CreateListFromTable<SearchPaymentMethodResultVM>(dt);

            return paymentMethods;
        }

        #endregion
    }
}