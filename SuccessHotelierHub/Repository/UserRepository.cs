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

        public UserVM GetUserLoginByEmail(string email)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Email", Value = email }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserLoginByEmail", parameters);

            var user = new UserVM();
            user = DALHelper.CreateListFromTable<UserVM>(dt).FirstOrDefault();

            return user;
        }

        public List<UserVM> CheckUserEmailExist(Guid? id, Guid? userRoleId, string email)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserRoleId", Value = userRoleId },
                    new SqlParameter { ParameterName = "@Email", Value = email }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckUserEmailExist", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> CheckCashierNumberExist(Guid? id, string cashierNumber)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@CashierNumber", Value = cashierNumber }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckCashierNumberExist", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> GetUserDetailById(Guid userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserDetailById", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> GetUserDetailByUserId(int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserDetailByUserId", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> GetUserDetailByCollegeGroupId(Guid collegeGroupId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = collegeGroupId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserDetailByCollegeGroupId", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public string AddUserDetail(UserVM user)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {                    
                    new SqlParameter { ParameterName = "@Name", Value = user.Name },
                    new SqlParameter { ParameterName = "@Email", Value = user.Email },
                    new SqlParameter { ParameterName = "@Password", Value = user.Password },
                    new SqlParameter { ParameterName = "@IsRecordActivity", Value = user.IsRecordActivity },
                    new SqlParameter { ParameterName = "@UserId", Value = user.UserId },
                    new SqlParameter { ParameterName = "@CashierNumber", Value = user.CashierNumber },
                    new SqlParameter { ParameterName = "@Address", Value = user.Address },
                    new SqlParameter { ParameterName = "@TelephoneNo", Value = user.TelephoneNo },
                    new SqlParameter { ParameterName = "@IsFromRegistrationPage", Value = user.IsFromRegistrationPage },
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = user.CollegeGroupId },
                    new SqlParameter { ParameterName = "@IsActive", Value = user.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = user.CreatedBy }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("AddUserDetail", parameters));

            return userId;
        }

        public string UpdateUserDetail(UserVM user)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {   
                    new SqlParameter { ParameterName = "@Id", Value = user.Id },
                    new SqlParameter { ParameterName = "@Name", Value = user.Name },
                    new SqlParameter { ParameterName = "@Email", Value = user.Email },
                    new SqlParameter { ParameterName = "@IsRecordActivity", Value = user.IsRecordActivity },
                    new SqlParameter { ParameterName = "@CashierNumber", Value = user.CashierNumber },
                    new SqlParameter { ParameterName = "@Address", Value = user.Address },
                    new SqlParameter { ParameterName = "@TelephoneNo", Value = user.TelephoneNo },
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = user.CollegeGroupId },
                    new SqlParameter { ParameterName = "@IsActive", Value = user.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = user.UpdatedBy }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUserDetail", parameters));

            return userId;
        }

        public string DeleteUserDetail(Guid id, int updatedBy)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserDetail", parameters));

            return userId;
        }

        public List<SearchUserDetailResultVM> SearchUserDetail(SearchUserDetailParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = model.CollegeGroupId },
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },                    
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },
                    new SqlParameter { ParameterName = "@IsFromRegistrationPage", Value = model.IsFromRegistrationPage },
                    new SqlParameter { ParameterName = "@IsLoginCredentialSent", Value = model.IsLoginCredentialSent },
                    new SqlParameter { ParameterName = "@IsViewAll", Value = model.IsViewAll },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchUserDetail", parameters);

            var users = new List<SearchUserDetailResultVM>();
            users = DALHelper.CreateListFromTable<SearchUserDetailResultVM>(dt);

            return users;
        }

        public string ChangePassword(ChangePasswordVM model)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = model.UserId },
                    new SqlParameter { ParameterName = "@Password", Value = model.Password },                    
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("ChangePassword", parameters));

            return userId;
        }

        public int GetMaxUserId()
        {
            int userId = 0;

            var objId = DALHelper.ExecuteScalar("GetMaxUserId");

            int.TryParse(Convert.ToString(objId), out userId);

            return userId;
        }

        public string UpdateUsersLastLoginTime(Guid id)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id }
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUsersLastLoginTime", parameters));

            return userId;
        }

        public string UpdateIsLoggedInFlag(Guid id, bool isLoggedIn = false)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@IsLoggedIn", Value = isLoggedIn },
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("UpdateIsLoggedInFlag", parameters));

            return userId;
        }

        public List<UserVM> GetTutorDetails()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTutorDetails");

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> GetTutorDetailByCollegeGroupId(Guid? collegeGroupId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = collegeGroupId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTutorDetailByCollegeGroupId", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public List<UserVM> GetUserDetailByRoleId(Guid userRoleId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@UserRoleId", Value = userRoleId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserDetailByRoleId", parameters);

            var user = new List<UserVM>();
            user = DALHelper.CreateListFromTable<UserVM>(dt);

            return user;
        }

        public string UpdateUserStatus(Guid id, bool isActive = false)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@IsActive", Value = isActive },
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("UpdateUserStatus", parameters));

            return userId;
        }

        public string UpdateLoginCredentialSentFlag(Guid id, bool isLoginCredentialSent = false)
        {
            string userId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@IsLoginCredentialSent", Value = isLoginCredentialSent },
                };

            userId = Convert.ToString(DALHelper.ExecuteScalar("UpdateLoginCredentialSentFlag", parameters));

            return userId;
        }

        public List<SearchTutorDetailResultVM> SearchTutorDetail(SearchTutorDetailParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = model.CollegeGroupId },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchTutorDetail", parameters);

            var tutors = new List<SearchTutorDetailResultVM>();
            tutors = DALHelper.CreateListFromTable<SearchTutorDetailResultVM>(dt);

            return tutors;
        }

        public List<SearchAdminDetailResultVM> SearchAdminDetail(SearchAdminDetailParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = model.CollegeGroupId },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdminDetail", parameters);

            var admins = new List<SearchAdminDetailResultVM>();
            admins = DALHelper.CreateListFromTable<SearchAdminDetailResultVM>(dt);

            return admins;
        }

        public List<SearchLoggedInUserInfoResultVM> GetLoggedInStudentInfo(Guid? tutorId = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@TutorId", Value = tutorId }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetLoggedInStudentInfo", parameters);

            var users = new List<SearchLoggedInUserInfoResultVM>();
            users = DALHelper.CreateListFromTable<SearchLoggedInUserInfoResultVM>(dt);

            return users;
        }

        public List<SearchLoggedInUserInfoResultVM> GetLoggedInUserInfo(SearchLoggedInUserInfoParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserRoleId", Value = model.UserRoleId },
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = model.CollegeGroupId },
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetLoggedInUserInfo", parameters);

            var users = new List<SearchLoggedInUserInfoResultVM>();
            users = DALHelper.CreateListFromTable<SearchLoggedInUserInfoResultVM>(dt);

            return users;
        }

        public List<SearchLoggedInUserInfoResultVM> SearchLoggedInUserInfo(SearchLoggedInUserInfoParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserRoleId ", Value = model.UserRoleId },
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = model.CollegeGroupId },
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },                    
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },
                    new SqlParameter { ParameterName = "@IsViewAll", Value = model.IsViewAll },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchLoggedInUserInfo", parameters);

            var users = new List<SearchLoggedInUserInfoResultVM>();
            users = DALHelper.CreateListFromTable<SearchLoggedInUserInfoResultVM>(dt);

            return users;
        }

        #endregion

        #region User Role Mapping

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

        public string AddUpdateUserRoleMapping(UserRoleMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId },
                    new SqlParameter { ParameterName = "@UserRoleId", Value = model.UserRoleId } ,
                    new SqlParameter { ParameterName = "@IsActive", Value = model.IsActive } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateUserRoleMapping", parameters));

            return id;
        }

        public string DeleteUserRoleMappingByUserId(Guid userId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UserId", Value = userId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserRoleMappingByUserId", parameters));

            return id;
        }

        public string DeleteUserRoleMapping(Guid mappingId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = mappingId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserRoleMapping", parameters));

            return id;
        }

        #endregion

        #region Tutor Student Mapping
        
        public List<SearchStudentDetailResultVM> SearchStudentDetail(SearchStudentDetailParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchStudentDetail", parameters);

            var students = new List<SearchStudentDetailResultVM>();
            students = DALHelper.CreateListFromTable<SearchStudentDetailResultVM>(dt);

            return students;
        }


        public string AddUpdateTutorStudentMapping(TutorStudentMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },
                    new SqlParameter { ParameterName = "@StudentId", Value = model.StudentId } ,
                    new SqlParameter { ParameterName = "@UserId", Value = model.UserId } ,
                    new SqlParameter { ParameterName = "@IsActive", Value = model.IsActive } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateTutorStudentMapping", parameters));

            return id;
        }


        public string DeleteTutorStudentMappingByTutor(Guid tutorId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = tutorId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteTutorStudentMappingByTutor", parameters));

            return id;
        }

        public string DeleteTutorStudentMapping(Guid mappingId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = mappingId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteTutorStudentMapping", parameters));

            return id;
        }

        public List<TutorStudentMappingByTutorResultVM> GetTutorStudentMappingByTutor(Guid? tutorId, Guid? studentId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = tutorId },
                    new SqlParameter { ParameterName = "@StudentId", Value = studentId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTutorStudentMappingByTutor", parameters);

            var results = new List<TutorStudentMappingByTutorResultVM>();
            results = DALHelper.CreateListFromTable<TutorStudentMappingByTutorResultVM>(dt);

            return results;
        }


        public List<StudentDetailsForTutorMappingResultVM> GetStudentDetailsForTutorMapping(Guid? tutorId, Guid? collegeGroupId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = tutorId },
                    new SqlParameter { ParameterName = "@CollegeGroupId", Value = collegeGroupId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetStudentDetailsForTutorMapping", parameters);

            var results = new List<StudentDetailsForTutorMappingResultVM>();
            results = DALHelper.CreateListFromTable<StudentDetailsForTutorMappingResultVM>(dt);

            return results;
        }

        #endregion

        #region User Login Time

        public string AddUpdateUserLoginTime(UserLoginTimeVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = model.TutorId },
                    new SqlParameter { ParameterName = "@ConfigurationType", Value = model.ConfigurationType },
                    new SqlParameter { ParameterName = "@LoginStartTime", Value = model.LoginStartTime } ,
                    new SqlParameter { ParameterName = "@LoginEndTime", Value = model.LoginEndTime } ,                    
                    new SqlParameter { ParameterName = "@IsActive", Value = model.IsActive } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = model.UpdatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddUpdateUserLoginTime", parameters));

            return id;
        }

        public string DeleteUserLoginTimeByTutor(Guid tutorId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = tutorId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserLoginTimeByTutor", parameters));

            return id;
        }

        public string DeleteUserLoginTime(Guid id, int updatedBy)
        {
            string loginTimeId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            loginTimeId = Convert.ToString(DALHelper.ExecuteScalar("DeleteUserLoginTime", parameters));

            return loginTimeId;
        }
        
        public List<UserLoginTimeVM> GetUserLoginTimeByTutor(Guid tutorId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@TutorId", Value = tutorId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetUserLoginTimeByTutor", parameters);

            var results = new List<UserLoginTimeVM>();
            results = DALHelper.CreateListFromTable<UserLoginTimeVM>(dt);

            return results;
        }

        public void RestrictStudentLogin(Guid tutorId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@TutorId", Value = tutorId }
            };

            DALHelper.ExecuteScalar("RestrictStudentLogin", parameters);
        }

        #endregion
    }
}