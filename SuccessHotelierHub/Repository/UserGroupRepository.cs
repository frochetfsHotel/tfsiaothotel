using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;


namespace SuccessHotelierHub.Repository
{
    public class UserGroupRepository
    {
        #region User Group

        public List<UserGroupVM> GetUserGroups()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserGroups");

            var userGroupList = new List<UserGroupVM>();
            userGroupList = DALHelper.CreateListFromTable<UserGroupVM>(dt);

            return userGroupList;
        }

        public List<UserGroupsWithCurrencyInfoResultVM> GetUserGroupsWithCurrencyInfo()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserGroupsWithCurrencyInfo");

            var userGroupList = new List<UserGroupsWithCurrencyInfoResultVM>();
            userGroupList = DALHelper.CreateListFromTable<UserGroupsWithCurrencyInfoResultVM>(dt);

            return userGroupList;
        }

        public UserGroupVM GetUserGroupById(Guid? userGroupId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Id", Value = userGroupId }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserGroupById", parameters);

            var userGroup = new UserGroupVM();
            userGroup = DALHelper.CreateListFromTable<UserGroupVM>(dt).FirstOrDefault();

            return userGroup;
        }

        public string AddUserGroup(UserGroupVM userGroup)
        {
            string userGroupId = string.Empty;

            SqlParameter[] parameters =
                {   
                    new SqlParameter { ParameterName = "@Name", Value = userGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = userGroup.Description },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = userGroup.CurrencyId },
                    new SqlParameter { ParameterName = "@IsActive", Value = userGroup.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = userGroup.CreatedBy }
                };

            userGroupId = Convert.ToString(DALHelper.ExecuteScalar("AddUserGroup", parameters));

            return userGroupId;
        }

        public string UpdateUserGroup(UserGroupVM userGroup)
        {
            string userGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = userGroup.Id },
                    new SqlParameter { ParameterName = "@Name", Value = userGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = userGroup.Description },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = userGroup.CurrencyId },
                    new SqlParameter { ParameterName = "@IsActive", Value = userGroup.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = userGroup.UpdatedBy }
                };

            userGroupId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUserGroup", parameters));

            return userGroupId;
        }

        public string DeleteUserGroup(Guid id, int updatedBy)
        {
            string userGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            userGroupId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserGroup", parameters));

            return userGroupId;
        }

        public List<SearchUserGroupResultVM> SearchUserGroup(SearchUserGroupParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@CurrencyId", Value = model.CurrencyId },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchUserGroup", parameters);

            var userGroups = new List<SearchUserGroupResultVM>();
            userGroups = DALHelper.CreateListFromTable<SearchUserGroupResultVM>(dt);

            return userGroups;
        }

        public List<UserGroupVM> CheckUserGroupNameExist(Guid? id, string name)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Name", Value = name }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckUserGroupNameExist", parameters);

            var userGroup = new List<UserGroupVM>();
            userGroup = DALHelper.CreateListFromTable<UserGroupVM>(dt);

            return userGroup;
        }

        #endregion

        #region User Group Currency



        #endregion
    }
}