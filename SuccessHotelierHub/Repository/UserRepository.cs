using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;


namespace SuccessHotelierHub.Repository
{
    public class UserRepository
    {

        #region User

        public UserVM GetUserLogin(string email, string password)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Email", Value = email },
                    new SqlParameter { ParameterName = "@Password", Value = password }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserLogin", parameters);

            var user = new UserVM();
            user = DALHelper.CreateListFromTable<UserVM>(dt).FirstOrDefault();

            return user;
        }

        public List<UserVM> CheckUserEmailExist(Guid? id, string email)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Email", Value = email }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckUserEmailExist", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public string AddUserDetail(UserVM user)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserRoleId", Value = user.UserRoleId },
                    new SqlParameter { ParameterName = "@Name", Value = user.Name },
                    new SqlParameter { ParameterName = "@Email", Value = user.Email },
                    new SqlParameter { ParameterName = "@Password", Value = user.Password },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = user.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = user.CreatedBy }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("AddUserDetail", parameters));

            return userId;
        }

        public List<CurrentUserRoleVM> GetUserRoleByUserId(Guid? userId, Guid? userRoleId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@UserRoleId", Value = userRoleId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserRoleByUserId", parameters);

            var currentUserRoles = new List<CurrentUserRoleVM>();
            currentUserRoles = DALHelper.CreateListFromTable<CurrentUserRoleVM>(dt);

            return currentUserRoles;
        }

        #endregion
    }
}