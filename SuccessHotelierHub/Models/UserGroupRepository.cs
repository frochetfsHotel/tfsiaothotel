using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;


namespace SuccessHotelierHub.Models
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

        #endregion

        #region User Group Currency



        #endregion
    }
}