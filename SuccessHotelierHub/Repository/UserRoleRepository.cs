using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class UserRoleRepository
    {

        #region User Role

        public List<UserRoleVM> GetUserRoles()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetUserRoles");

            var userRoles = new List<UserRoleVM>();
            userRoles = DALHelper.CreateListFromTable<UserRoleVM>(ds);

            return userRoles;
        }

        public List<UserRoleVM> GetUserRoleById(Guid userRoleId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = userRoleId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserRoleById", parameters);

            var userRole = new List<UserRoleVM>();
            userRole = DALHelper.CreateListFromTable<UserRoleVM>(dt);

            return userRole;
        }

        public string AddUserRole(UserRoleVM userRole)
        {
            string userRoleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = userRole.Code},
                    new SqlParameter { ParameterName = "@Name", Value = userRole.Name },
                    new SqlParameter { ParameterName = "@Description", Value = userRole.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = userRole.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = userRole.CreatedBy }
                };

            userRoleId = Convert.ToString(DALHelper.ExecuteScalar("AddUserRole", parameters));

            return userRoleId;
        }

        public string UpdateUserRole(UserRoleVM userRole)
        {
            string userRoleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = userRole.Id },
                    new SqlParameter { ParameterName = "@Code", Value = userRole.Code},
                    new SqlParameter { ParameterName = "@Name", Value = userRole.Name },
                    new SqlParameter { ParameterName = "@Description", Value = userRole.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = userRole.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = userRole.UpdatedBy }
                };

            userRoleId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUserRole", parameters));

            return userRoleId;
        }

        public string DeleteUserRole(Guid id, int updatedBy)
        {
            string userRoleId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            userRoleId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserRole", parameters));

            return userRoleId;
        }

        public List<SearchUserRoleResultVM> SearchUserRole(SearchUserRoleParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchUserRole", parameters);

            var userRoles = new List<SearchUserRoleResultVM>();
            userRoles = DALHelper.CreateListFromTable<SearchUserRoleResultVM>(dt);

            return userRoles;
        }

        public List<UserRoleVM> CheckUserRoleCodeAvailable(Guid? userRoleId, string Code)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = userRoleId },
                    new SqlParameter { ParameterName = "@Code", Value = Code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckUserRoleCodeAvailable", parameters);

            var userRole = new List<UserRoleVM>();
            userRole = DALHelper.CreateListFromTable<UserRoleVM>(dt);

            return userRole;
        }

        #endregion
    }
}