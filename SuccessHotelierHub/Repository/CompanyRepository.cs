using SuccessHotelierHub.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Repository
{
    public class CompanyRepository
    {
        public List<CompanyVM> GetCompanyById(Guid companyId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = companyId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCompanyById", parameters);


            var company = new List<CompanyVM>();
            company = DALHelper.CreateListFromTable<CompanyVM>(dt);

            return company;
        }

        public string AddCompany(CompanyVM company)
        {
            string companyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CompanyName", Value = company.CompanyName },
                    new SqlParameter { ParameterName = "@CompanyAddress", Value = company.CompanyAddress},
                    new SqlParameter { ParameterName = "@IsActive", Value = company.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = company.CreatedBy }
                };

            companyId = Convert.ToString(DALHelper.ExecuteScalar("AddCompany", parameters));

            return companyId;
        }

        public string UpdateCompany(CompanyVM company)
        {
            string companyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = company.Id },
                    new SqlParameter { ParameterName = "@CompanyName", Value = company.CompanyName},
                    new SqlParameter { ParameterName = "@CompanyAddress", Value = company.CompanyAddress},
                    new SqlParameter { ParameterName = "@IsActive", Value = company.IsActive},
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = company.UpdatedBy }
                };

            companyId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCompany", parameters));

            return companyId;
        }

        public string DeleteCompany(Guid id, int updatedBy)
        {
            string companyId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            companyId = Convert.ToString(DALHelper.ExecuteScalar("DeleteCompany", parameters));

            return companyId;
        }

        public List<SearchCompanyResultVM> SearchHoliday(SearchCompanyParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CompanyName", Value = model.CompanyName },
                    new SqlParameter { ParameterName = "@CompanyAddress", Value = model.CompanyAddress },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCompany", parameters);

            var Companies = new List<SearchCompanyResultVM>();
            Companies = DALHelper.CreateListFromTable<SearchCompanyResultVM>(dt);

            return Companies;
        }

        public List<CompanyVM> GetCompanyList()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCompanyList");

            var CompanyList = new List<CompanyVM>();
            CompanyList = DALHelper.CreateListFromTable<CompanyVM>(dt);

            return CompanyList;
        }
    }
}