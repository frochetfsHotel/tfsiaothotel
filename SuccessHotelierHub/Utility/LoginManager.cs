using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using System.Configuration;
using System.Web.Mvc;
using System.Net;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub
{
    public class LogInManager
    {
        public static LoginStatus Login(string email, string password)
        {
            UserRepository userRepository = new UserRepository();
            UserRoleRepository userRoleRepository = new UserRoleRepository();
            UserPageRepository userPageRepository = new UserPageRepository();
            UserGroupRepository userGroupRepository = new UserGroupRepository();
            CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return LoginStatus.Failure;
            }

            var user = userRepository.GetUserLogin(email, password);

            if (user == null)
            {
                return LoginStatus.Failure;
            }
            else
            {
                if (user.IsLoggedIn.HasValue && user.IsLoggedIn.Value == true)
                {
                    return LoginStatus.AlreadyLoggedIn;
                }
            }

            var userRoles = userRepository.GetUserRoleByUserId(user.Id, null);
            var userRoleName = GetUserRoleName(userRoles);

            #region Check Student Login Time

            if (userRoles != null && userRoles.Count > 0)
            {
                if (userRoles[0].Code == "STUDENT")
                {
                    var tutorDetails = userRepository.GetTutorStudentMappingByTutor(null, user.Id);
                    if (tutorDetails != null && tutorDetails.Count > 0)
                    {
                        var userLoginTime = userRepository.GetUserLoginTimeByTutor(tutorDetails[0].TutorId).FirstOrDefault();

                        //If time limt set then compare current time and time limit
                        if (userLoginTime != null && !string.IsNullOrWhiteSpace(userLoginTime.ConfigurationType))
                        {
                            if (userLoginTime.ConfigurationType.Trim() == Utility.UserLoginConfigurationType.SET_LIMIT)
                            {
                                var loginStartTime = userLoginTime.LoginStartTime.Value;
                                var loginEndTime = userLoginTime.LoginEndTime.Value;

                                var dtLoginStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginStartTime.Hours, loginStartTime.Minutes, loginStartTime.Seconds);
                                var dtLoginEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginEndTime.Hours, loginEndTime.Minutes, loginEndTime.Seconds);

                                var currentDateTime = DateTime.Now;

                                if (!((currentDateTime > dtLoginStartTime) && (currentDateTime < dtLoginEndTime)))
                                {
                                    return LoginStatus.InvalidLoginTime;
                                }
                                else
                                {
                                    LogInManager.LoginStartTime = loginStartTime;
                                    LogInManager.LoginEndTime = loginEndTime;
                                }
                            }
                            else if (userLoginTime.ConfigurationType.Trim() == Utility.UserLoginConfigurationType.RESTRICTED)
                            {
                                return LoginStatus.InvalidLoginTime;
                            }
                        }
                    }
                }
            }
            #endregion

            var userPageAccessRights = userPageRepository.GetUserPageAccessRights(user.Id, string.Empty);

            var collegeGroup = collegeGroupRepository.GetCollegeGroupById(user.CollegeGroupId.Value);

            if (collegeGroup != null)
            {
                var userGroup = userGroupRepository.GetUserGroupById(collegeGroup.UserGroupId.Value);

                if (userGroup != null)
                {
                    LogInManager.UserGroup = userGroup;
                    LogInManager.CurrencyId = userGroup.CurrencyId;

                    var currencyInfo = CurrencyManager.GetCurrencyInfoById(userGroup.CurrencyId);
                    if (currencyInfo != null)
                    {
                        LogInManager.CurrencyCode = currencyInfo.Code;
                        LogInManager.CurrencyConversionRate = currencyInfo.ConversionRate;
                        LogInManager.CurrencySymbol = currencyInfo.CurrencySymbol;
                    }
                }
            }

            LogInManager.UserName = user.Name;
            LogInManager.CashierNumber = user.CashierNumber;
            LogInManager.LoggedInUserId = user.UserId;
            LogInManager.LoggedInUserSessionId = System.Web.HttpContext.Current.Session.SessionID;
            LogInManager.LoggedInUser = user;
            LogInManager.UsersRoles = userRoles;
            LogInManager.UserPageAccessRights = userPageAccessRights;
            LogInManager.UserRoleName = userRoleName;
            LogInManager.UserRoleCode = GetUserRoleCode(userRoles);

            //Update Last LoggedIn Date.
            userRepository.UpdateUsersLastLoginTime(user.Id);

            //Update Flag IsLoggedIn = true 
            userRepository.UpdateIsLoggedInFlag(user.Id, true);

            return LoginStatus.Success;
        }

        public static LoginStatus LoginWithCookie(string email)
        {
            UserRepository userRepository = new UserRepository();
            UserRoleRepository userRoleRepository = new UserRoleRepository();
            UserPageRepository userPageRepository = new UserPageRepository();
            UserGroupRepository userGroupRepository = new UserGroupRepository();
            CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

            if (string.IsNullOrEmpty(email))
            {
                return LoginStatus.Failure;
            }

            var user = userRepository.GetUserLoginByEmail(email);

            if (user == null)
            {
                return LoginStatus.Failure;
            }

            var userRoles = userRepository.GetUserRoleByUserId(user.Id, null);

            var userRoleName = GetUserRoleName(userRoles);

            #region Check Student Login Time

            if (userRoles != null && userRoles.Count > 0)
            {
                if (userRoles[0].Code == "STUDENT")
                {
                    var tutorDetails = userRepository.GetTutorStudentMappingByTutor(null, user.Id);

                    if (tutorDetails != null && tutorDetails.Count > 0)
                    {
                        var userLoginTime = userRepository.GetUserLoginTimeByTutor(tutorDetails[0].TutorId).FirstOrDefault();

                        //If time limt set then compare current time and time limit
                        if (userLoginTime != null && !string.IsNullOrWhiteSpace(userLoginTime.ConfigurationType))
                        {
                            if (userLoginTime.ConfigurationType.Trim() == Utility.UserLoginConfigurationType.SET_LIMIT)
                            {
                                var loginStartTime = userLoginTime.LoginStartTime.Value;
                                var loginEndTime = userLoginTime.LoginEndTime.Value;

                                var dtLoginStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginStartTime.Hours, loginStartTime.Minutes, loginStartTime.Seconds);
                                var dtLoginEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginEndTime.Hours, loginEndTime.Minutes, loginEndTime.Seconds);

                                var currentDateTime = DateTime.Now;

                                if (!((currentDateTime > dtLoginStartTime) && (currentDateTime < dtLoginEndTime)))
                                {
                                    return LoginStatus.Failure;
                                }
                                else
                                {
                                    LogInManager.LoginStartTime = loginStartTime;
                                    LogInManager.LoginEndTime = loginEndTime;
                                }
                            }
                            else if (userLoginTime.ConfigurationType.Trim() == Utility.UserLoginConfigurationType.RESTRICTED)
                            {
                                return LoginStatus.Failure;
                            }
                        }
                    }
                }
            }
            #endregion

            var userPageAccessRights = userPageRepository.GetUserPageAccessRights(user.Id, string.Empty);

            var collegeGroup = collegeGroupRepository.GetCollegeGroupById(user.CollegeGroupId.Value);

            if (collegeGroup != null)
            {
                var userGroup = userGroupRepository.GetUserGroupById(collegeGroup.UserGroupId.Value);

                if (userGroup != null)
                {
                    LogInManager.UserGroup = userGroup;
                    LogInManager.CurrencyId = userGroup.CurrencyId;

                    var currencyInfo = CurrencyManager.GetCurrencyInfoById(userGroup.CurrencyId);
                    if (currencyInfo != null)
                    {
                        LogInManager.CurrencyCode = currencyInfo.Code;
                        LogInManager.CurrencyConversionRate = currencyInfo.ConversionRate;
                        LogInManager.CurrencySymbol = currencyInfo.CurrencySymbol;
                    }
                }
            }

            LogInManager.UserName = user.Name;
            LogInManager.CashierNumber = user.CashierNumber;
            LogInManager.LoggedInUserId = user.UserId;
            LogInManager.LoggedInUserSessionId = System.Web.HttpContext.Current.Session.SessionID;
            LogInManager.LoggedInUser = user;
            LogInManager.UsersRoles = userRoles;
            LogInManager.UserPageAccessRights = userPageAccessRights;
            LogInManager.UserRoleName = userRoleName;
            LogInManager.UserRoleCode = GetUserRoleCode(userRoles);

            //Update Flag IsLoggedIn = true 
            userRepository.UpdateIsLoggedInFlag(user.Id, true);

            return LoginStatus.Success;
        }

        public static int LoggedInUserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return (int)HttpContext.Current.Session["UserId"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static UserVM LoggedInUser
        {
            get
            {
                if (HttpContext.Current.Session["LoggedInUser"] != null)
                {
                    return (UserVM)HttpContext.Current.Session["LoggedInUser"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["LoggedInUser"] = value;
            }
        }

        public static List<CurrentUserRoleVM> UsersRoles
        {
            get
            {
                if (HttpContext.Current.Session["UsersRoles"] != null)
                {
                    return (List<CurrentUserRoleVM>)HttpContext.Current.Session["UsersRoles"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UsersRoles"] = value;
            }
        }

        public static List<UserPageAccessRightsResultVM> UserPageAccessRights
        {
            get
            {
                if (HttpContext.Current.Session["UserPageAccessRights"] != null)
                {
                    return (List<UserPageAccessRightsResultVM>)HttpContext.Current.Session["UserPageAccessRights"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UserPageAccessRights"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    return (string)HttpContext.Current.Session["UserName"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static string CashierNumber
        {
            get
            {
                if (HttpContext.Current.Session["CashierNumber"] != null)
                {
                    return (string)HttpContext.Current.Session["CashierNumber"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["CashierNumber"] = value;
            }
        }

        public static string UserRoleName
        {
            get
            {
                if (HttpContext.Current.Session["UserRoleName"] != null)
                {
                    return (string)HttpContext.Current.Session["UserRoleName"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UserRoleName"] = value;
            }
        }

        public static string UserRoleCode
        {
            get
            {
                if (HttpContext.Current.Session["UserRoleCode"] != null)
                {
                    return (string)HttpContext.Current.Session["UserRoleCode"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UserRoleCode"] = value;
            }
        }

        public static string LoggedInUserSessionId
        {
            get
            {
                if (HttpContext.Current.Session["SessionId"] != null)
                {
                    return (string)HttpContext.Current.Session["SessionId"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["SessionId"] = value;
            }
        }

        public static UserPageAccessRightsResultVM HasPageAccessRights(string pageCode)
        {
            if (UserPageAccessRights != null && UserPageAccessRights.Count > 0)
            {
                var returnVal = UserPageAccessRights
                                .Where(m => m.PageCode == pageCode)
                                .FirstOrDefault();
                return returnVal;
            }
            return null;
        }

        public static bool HasRights(string code)
        {
            if (UsersRoles != null)
            {
                var returnVal = UsersRoles
                                .Where(m => m.Code == code)
                                .Any();
                return returnVal;
            }
            return false;
        }

        public static bool IsAllowToRecordActivity
        {
            get
            {
                if (LoggedInUser != null)
                {
                    return LoggedInUser.IsRecordActivity;
                }
                return false;
            }
        }

        public static string GetUserRoleName(List<CurrentUserRoleVM> currentUserRoles)
        {
            string userRoleName = string.Empty;

            if (currentUserRoles != null && currentUserRoles.Count > 0)
            {
                if (currentUserRoles.Where(m => m.Code == "ADMIN").FirstOrDefault() != null)
                {
                    userRoleName = currentUserRoles.Where(m => m.Code == "ADMIN").FirstOrDefault().Name;
                }
                else if (currentUserRoles.Where(m => m.Code == "TUTOR").FirstOrDefault() != null)
                {
                    userRoleName = currentUserRoles.Where(m => m.Code == "TUTOR").FirstOrDefault().Name;
                }
                else if (currentUserRoles.Where(m => m.Code == "STUDENT").FirstOrDefault() != null)
                {
                    userRoleName = currentUserRoles.Where(m => m.Code == "STUDENT").FirstOrDefault().Name;
                }
            }

            return userRoleName;
        }

        public static string GetUserRoleCode(List<CurrentUserRoleVM> currentUserRoles)
        {
            string userRoleCode = string.Empty;

            if (currentUserRoles != null && currentUserRoles.Count > 0)
            {
                userRoleCode = currentUserRoles.FirstOrDefault().Code;
            }

            return userRoleCode;
        }

        public static UserGroupVM UserGroup
        {
            get
            {
                if (HttpContext.Current.Session["UserGroup"] != null)
                {
                    return (UserGroupVM)HttpContext.Current.Session["UserGroup"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UserGroup"] = value;
            }
        }

        public static Guid? CurrencyId
        {
            get
            {
                if (HttpContext.Current.Session["CurrencyId"] != null)
                {
                    return (Guid?)HttpContext.Current.Session["CurrencyId"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["CurrencyId"] = value;
            }
        }

        public static string CurrencyCode
        {
            get
            {
                if (HttpContext.Current.Session["CurrencyCode"] != null)
                {
                    return (string)HttpContext.Current.Session["CurrencyCode"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["CurrencyCode"] = value;
            }
        }

        public static string CurrencySymbol
        {
            get
            {
                if (HttpContext.Current.Session["CurrencySymbol"] != null)
                {
                    return (string)HttpContext.Current.Session["CurrencySymbol"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["CurrencySymbol"] = value;
            }
        }

        public static double? CurrencyConversionRate
        {
            get
            {
                if (HttpContext.Current.Session["CurrencyConversionRate"] != null)
                {
                    return (double?)HttpContext.Current.Session["CurrencyConversionRate"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["CurrencyConversionRate"] = value;
            }
        }

        public static TimeSpan? LoginStartTime
        {
            get
            {
                if (HttpContext.Current.Session["LoginStartTime"] != null)
                {
                    return (TimeSpan?)HttpContext.Current.Session["LoginStartTime"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["LoginStartTime"] = value;
            }
        }

        public static TimeSpan? LoginEndTime
        {
            get
            {
                if (HttpContext.Current.Session["LoginEndTime"] != null)
                {
                    return (TimeSpan?)HttpContext.Current.Session["LoginEndTime"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["LoginEndTime"] = value;
            }
        }       
    }

    public enum LoginStatus
    {
        /// <summary>
        /// Logged in successfully
        /// </summary>
        Success = 0,
        /// <summary>
        /// User is locked out
        /// </summary>
        LockedOut = 1,
        /// <summary>
        /// Login in requires addition verification (i.e. two factor)
        /// </summary>
        RequiresVerification = 2,
        /// <summary>
        /// Login failed
        /// </summary>
        Failure = 3,
        /// <summary>
        /// User is already logged in.
        /// </summary>
        AlreadyLoggedIn = 4,
        /// <summary>
        /// User trying to do login at invalid login time.
        /// </summary>
        InvalidLoginTime = 5,
    }

}
