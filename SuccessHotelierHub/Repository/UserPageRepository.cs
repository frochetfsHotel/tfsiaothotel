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
    public class UserPageRepository
    {
        #region User Page

        public List<UserPagesVM> GetUserPages(Guid? userId, Guid? pageId)
        {
            SqlParameter[] parameters =
              {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@PageId", Value = pageId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserPages", parameters);

            var userPages = new List<UserPagesVM>();
            userPages = DALHelper.CreateListFromTable<UserPagesVM>(dt);

            return userPages;
        }

        public List<UserPagesVM> GetUserPageById(Guid pageId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = pageId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserPageById", parameters);

            var userPage = new List<UserPagesVM>();
            userPage = DALHelper.CreateListFromTable<UserPagesVM>(dt);

            return userPage;
        }


        public string AddUserPages(UserPagesVM userPage)
        {
            string userPageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userPage.UserId },
                    new SqlParameter { ParameterName = "@PageId", Value = userPage.PageId },
                    new SqlParameter { ParameterName = "@IsAdd", Value = userPage.IsAdd },
                    new SqlParameter { ParameterName = "@IsEdit", Value = userPage.IsEdit },
                    new SqlParameter { ParameterName = "@IsDelete", Value = userPage.IsDelete },
                    new SqlParameter { ParameterName = "@IsView", Value = userPage.IsView },
                    new SqlParameter { ParameterName = "@Description", Value = userPage.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = userPage.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = userPage.CreatedBy }
                };

            userPageId = Convert.ToString(DALHelper.ExecuteScalar("AddUserPages", parameters));

            return userPageId;
        }

        public string UpdateUserPages(UserPagesVM userPage)
        {
            string userPageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = userPage.Id },
                    new SqlParameter { ParameterName = "@UserId", Value = userPage.UserId },
                    new SqlParameter { ParameterName = "@PageId", Value = userPage.PageId },
                    new SqlParameter { ParameterName = "@IsAdd", Value = userPage.IsAdd },
                    new SqlParameter { ParameterName = "@IsEdit", Value = userPage.IsEdit },
                    new SqlParameter { ParameterName = "@IsDelete", Value = userPage.IsDelete },
                    new SqlParameter { ParameterName = "@IsView", Value = userPage.IsView },
                    new SqlParameter { ParameterName = "@Description", Value = userPage.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = userPage.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = userPage.UpdatedBy }
                };

            userPageId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUserPages", parameters));

            return userPageId;
        }

        public string DeleteUserPage(Guid id, int updatedBy)
        {
            string userPageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            userPageId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserPage", parameters));

            return userPageId;
        }

        public string DeleteUserPageByUserId(Guid userId, Guid? pageId, int updatedBy)
        {
            string userPageId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@PageId", Value = pageId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            userPageId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserPageByUserId", parameters));

            return userPageId;
        }

        public List<UserPageAccessRightsResultVM> GetUserPageAccessRights(Guid userId, string pageCode)
        {
            SqlParameter[] parameters =
              {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@PageCode", Value = pageCode }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserPageAccessRights", parameters);

            var userPages = new List<UserPageAccessRightsResultVM>();
            userPages = DALHelper.CreateListFromTable<UserPageAccessRightsResultVM>(dt);

            return userPages;
        }

        #endregion
    }
}