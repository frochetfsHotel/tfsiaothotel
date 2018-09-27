using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub
{
    public class HotelierHubAuthorizeAttribute : AuthorizeAttribute
    {
        public string Roles { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (LogInManager.LoggedInUser != null)
            {

                #region Check Login Time

                if(LogInManager.UserRoleCode == "STUDENT")
                {
                    if (LogInManager.LoginStartTime.HasValue && LogInManager.LoginEndTime.HasValue)
                    {
                        var loginStartTime = LogInManager.LoginStartTime.Value;
                        var loginEndTime = LogInManager.LoginEndTime.Value;

                        var dtLoginStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginStartTime.Hours, loginStartTime.Minutes, loginStartTime.Seconds);
                        var dtLoginEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, loginEndTime.Hours, loginEndTime.Minutes, loginEndTime.Seconds);

                        var currentDateTime = DateTime.Now;

                        if (!((currentDateTime > dtLoginStartTime) && (currentDateTime < dtLoginEndTime)))
                        {
                            //Remove Cookie
                            Utility.Utility.RemoveCookie("HotelierHubUserEmail");

                            //Remove current sessions.
                            System.Web.HttpContext.Current.Session.Abandon();

                            return false;
                        }
                    }
                }

                #endregion

                if (string.IsNullOrEmpty(Roles))
                {
                    return true;
                }

                string[] split = Roles.Split(',');

                foreach (CurrentUserRoleVM role in LogInManager.UsersRoles)
                {
                    if (split.Contains(role.Code))
                    {
                        return true;
                    }
                }
                return false;

            }
            else
            {
                var cookieEmail = httpContext.Request.Cookies["HotelierHubUserEmail"];

                if (cookieEmail != null && !string.IsNullOrWhiteSpace(cookieEmail.Value))
                {
                    var result = LogInManager.LoginWithCookie(cookieEmail.Value);

                    if (result == LoginStatus.Failure)
                    {
                        //Remove Cookie
                        Utility.Utility.RemoveCookie("HotelierHubUserEmail");

                        //Remove current sessions.
                        System.Web.HttpContext.Current.Session.Abandon();

                        return false;
                    }

                    if (LogInManager.HasRights("STUDENT"))
                    {
                        //Create Dummy Reservation.
                        Utility.TempReservation.CreateDummyReservation();
                    }

                    var trackActivityStatus = Utility.RecordActivityLog.TrackUsersActivityByUserId(LogInManager.LoggedInUser.Id);

                    if (trackActivityStatus != Utility.TrackActivityStatus.Success)
                    {
                        //Remove Cookie
                        Utility.Utility.RemoveCookie("HotelierHubUserEmail");

                        //Remove current sessions.
                        System.Web.HttpContext.Current.Session.RemoveAll();

                        return false;
                    }

                    return true;
                }

                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            if (LogInManager.LoggedInUser != null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                }
                else
                {
                    filterContext.HttpContext.Items["UnauthorizeRequest"] = true;
                }
            }
            else
            {
                filterContext.HttpContext.Items["SessionExpired"] = true;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}