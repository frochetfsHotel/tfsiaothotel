using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;


namespace SuccessHotelierHub.Repository
{
    public class NationalityRepository
    {
        #region Nationality

        public List<NationalityVM> GetNationality()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetNationality");

            var nationalities = new List<NationalityVM>();
            nationalities = DALHelper.CreateListFromTable<NationalityVM>(ds);

            return nationalities;
        }

        public List<NationalityVM> GetNationalityById(int nationalityId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = nationalityId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetNationalityById", parameters);

            var nationality = new List<NationalityVM>();
            nationality = DALHelper.CreateListFromTable<NationalityVM>(dt);

            return nationality;
        }

        public string AddNationality(NationalityVM nationality)
        {
            string nationalityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = nationality.Name },
                    new SqlParameter { ParameterName = "@Description", Value = nationality.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = nationality.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = nationality.CreatedBy }
                };

            nationalityId = Convert.ToString(DALHelper.ExecuteScalar("AddNationality", parameters));

            return nationalityId;
        }

        public string UpdateNationality(NationalityVM nationality)
        {
            string nationalityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = nationality.Id },
                    new SqlParameter { ParameterName = "@Name", Value = nationality.Name },
                    new SqlParameter { ParameterName = "@Description", Value = nationality.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = nationality.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = nationality.UpdatedBy }
                };

            nationalityId = Convert.ToString(DALHelper.ExecuteScalar("UpdateNationality", parameters));

            return nationalityId;
        }

        public string DeleteNationality(int id, int updatedBy)
        {
            string nationalityId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            nationalityId = Convert.ToString(DALHelper.ExecuteScalar("DeleteNationality", parameters));

            return nationalityId;
        }

        public List<SearchNationalityResultVM> SearchNationality(SearchNationalityParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchNationality", parameters);

            var nationalities = new List<SearchNationalityResultVM>();
            nationalities = DALHelper.CreateListFromTable<SearchNationalityResultVM>(dt);

            return nationalities;
        }

        #endregion
    }
}