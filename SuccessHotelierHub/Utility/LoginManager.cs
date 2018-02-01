using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using System.Configuration;
using System.Web.Mvc;
using System.Net;

namespace SuccessHotelierHub
{
    public class LogInManager
    {
        public static LoginStatus Login(string email, string password)
        {
            UserRepository userRepository = new UserRepository();
            UserRoleRepository userRoleRepository = new UserRoleRepository();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return LoginStatus.Failure;
            }

            var user = userRepository.GetUserLogin(email, password);

            if (user == null)
            {
                return LoginStatus.Failure;
            }

            var userRoles = userRepository.GetUserRoleByUserId(user.Id, null);

            LogInManager.UserName = user.Name;
            LogInManager.LoggedInUserId = user.UserId;
            LogInManager.LoggedInUser = user;
            LogInManager.UsersRoles = userRoles;
            LogInManager.UserRoleName = GetUserRoleName(userRoles);

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

        public static bool HasRights(string userRightsCode)
        {
            if (UsersRoles != null)
            {
                var returnVal = UsersRoles
                                .Where(m => m.Code == userRightsCode)
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

            if(currentUserRoles != null && currentUserRoles.Count > 0 )
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
    }

}
