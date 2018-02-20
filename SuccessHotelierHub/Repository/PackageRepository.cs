using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class PackageRepository
    {
        #region Package

        public List<PackageVM> GetPackages()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetPackages");

            var packages = new List<PackageVM>();
            packages = DALHelper.CreateListFromTable<PackageVM>(ds);

            return packages;
        }

        public List<PackageVM> GetPackageById(Guid packageId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = packageId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPackageById", parameters);

            var package = new List<PackageVM>();
            package = DALHelper.CreateListFromTable<PackageVM>(dt);

            return package;
        }

        public string AddPackage(PackageVM package)
        {
            string packageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = package.Name },
                    new SqlParameter { ParameterName = "@Description", Value = package.Description },
                    new SqlParameter { ParameterName = "@Price", Value = package.Price },
                    new SqlParameter { ParameterName = "@CalculationRatioId", Value = package.CalculationRatioId },
                    new SqlParameter { ParameterName = "@IsActive", Value = package.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = package.CreatedBy }
                };

            packageId = Convert.ToString(DALHelper.ExecuteScalar("AddPackage", parameters));

            return packageId;
        }

        public string UpdatePackage(PackageVM package)
        {
            string packageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = package.Id },
                    new SqlParameter { ParameterName = "@Name", Value = package.Name },
                    new SqlParameter { ParameterName = "@Description", Value = package.Description },
                    new SqlParameter { ParameterName = "@Price", Value = package.Price },
                    new SqlParameter { ParameterName = "@CalculationRatioId", Value = package.CalculationRatioId },
                    new SqlParameter { ParameterName = "@IsActive", Value = package.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = package.UpdatedBy }
                };

            packageId = Convert.ToString(DALHelper.ExecuteScalar("UpdatePackage", parameters));

            return packageId;
        }

        public string DeletePackage(Guid id, int updatedBy)
        {
            string packageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            packageId = Convert.ToString(DALHelper.ExecuteScalar("DeletePackage", parameters));

            return packageId;
        }

        public List<SearchPackageResultVM> SearchPackage(SearchPackageParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Price", Value = model.Price },
                    new SqlParameter { ParameterName = "@CalculationRatioId", Value = model.CalculationRatioId },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchPackage", parameters);

            var packages = new List<SearchPackageResultVM>();
            packages = DALHelper.CreateListFromTable<SearchPackageResultVM>(dt);

            return packages;
        }

        public List<SearchAdvancePackageResultVM> SearchAdvancePackage(SearchAdvancePackageParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@Price", Value = model.Price },
                    new SqlParameter { ParameterName = "@CalculationRatioId", Value = model.CalculationRatioId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvancePackage", parameters);

            var packages = new List<SearchAdvancePackageResultVM>();
            packages = DALHelper.CreateListFromTable<SearchAdvancePackageResultVM>(dt);

            return packages;
        }

        #endregion

        #region Package Calculation Ration

        public List<PackageCalculationRatioVM> GetPackageCalculationRatio()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetPackageCalculationRatio");

            var packageCalculationRatios = new List<PackageCalculationRatioVM>();
            packageCalculationRatios = DALHelper.CreateListFromTable<PackageCalculationRatioVM>(ds);

            return packageCalculationRatios;
        }

        #endregion

       
    }
}