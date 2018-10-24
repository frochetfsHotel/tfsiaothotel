using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using System.Configuration;



namespace SuccessHotelierHub.Utility
{
    public class RecordActivityLog
    {
        public static void RecordActivity(string pageName, string description)
        {
            if (LogInManager.IsAllowToRecordActivity)
            {
                UsersActivityLogRepository usersActivityLogRepository = new UsersActivityLogRepository();
                UserRepository userRepository = new UserRepository();

                DateTime recordedOn = DateTime.Now;

                UsersActivityLogVM usersActivityLog = new UsersActivityLogVM();
                usersActivityLog.UserId = LogInManager.LoggedInUser.Id;
                usersActivityLog.Url = Utility.GetCurrentPageURL();
                usersActivityLog.PageName = pageName;
                usersActivityLog.Description = description;
                usersActivityLog.IpAddress = Utility.GetIpAddress_V3();
                usersActivityLog.CreatedBy = LogInManager.LoggedInUserId;
                usersActivityLog.IsActive = true;
                usersActivityLog.RecordedOn = recordedOn;
                usersActivityLog.SessionId = LogInManager.LoggedInUserSessionId;

                string newGUID = usersActivityLogRepository.AddUsersActivityLog(usersActivityLog);

                if (pageName == Pages.LOGOUT)
                {
                    //Update Flag IsLoggedIn = false.
                    userRepository.UpdateIsLoggedInFlag(LogInManager.LoggedInUser.Id, false);

                    var activities = usersActivityLogRepository.GetUsersActivityLogBySessionId(LogInManager.LoggedInUserSessionId);

                    if(activities != null && activities.Count > 0)
                    {
                        //Get Login Activity.
                        var loginActivity = activities.Where(m => m.PageName == Pages.LOGIN).FirstOrDefault();

                        if(loginActivity != null)
                        {
                            DateTime logoutTime = recordedOn;

                            var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                            //Calculate logged in duration seconds.
                            TimeSpan span = logoutTime.Subtract(loginTime);
                            double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                            //Update logged in duration in seconds.
                            usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(Guid.Parse(newGUID), loggedInDurationInSeconds, LogInManager.LoggedInUserId);
                        }
                    }                    
                }
            }
        }

        public static void UpdateLogOutTimeBySessionId(string sessionId)
        {
            UsersActivityLogRepository usersActivityLogRepository = new UsersActivityLogRepository();
            UserRepository userRepository = new UserRepository();

            var activities = usersActivityLogRepository.GetUsersActivityLogBySessionId(sessionId);

            if (activities != null && activities.Count > 0)
            {
                //Get Login Activity.
                var loginActivity = activities.Where(m => m.PageName == Pages.LOGIN).FirstOrDefault();

                //Check Login Activity Exist.
                if (loginActivity != null)
                {                    
                    //Get Logout Activity which tracked after current login.
                    var logoutActivity = activities.Where(m => m.PageName == Pages.LOGOUT && m.RecordedOn.Value > loginActivity.RecordedOn.Value).FirstOrDefault();

                    //Check Logout Activity Exist then update logged in duration.
                    if (logoutActivity != null)
                    {
                        //Check Logout Activity already updated with logged in duration. If No then update logged in duration.
                        if (logoutActivity.LoggedInDurationInSeconds.HasValue == false)
                        {
                            var userId = logoutActivity.CreatedBy.Value;
                            var userGUID = logoutActivity.UserId;

                            DateTime logoutTime = logoutActivity.RecordedOn.Value;

                            var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                            //Calculate logged in duration seconds.
                            TimeSpan span = logoutTime.Subtract(loginTime);
                            double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                            //Update logged in duration in seconds.
                            usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(logoutActivity.Id, loggedInDurationInSeconds, userId);

                            //Update Flag IsLoggedIn = false.
                            userRepository.UpdateIsLoggedInFlag(userGUID, false);                            
                        }   
                    }
                    else
                    {   
                        //If Logout Activity not exist then create logout activity and then calcualte logged in duration.
                        DateTime recordedOn = DateTime.Now;

                        var userId = loginActivity.CreatedBy.Value;
                        var userGUID = loginActivity.UserId;

                        var environment = Utility.GetConfigValue("Environment");
                        var siteDomainURL = Utility.GetSiteDomainURL(environment);

                        //Add logout activity.
                        UsersActivityLogVM usersActivityLog = new UsersActivityLogVM();
                        usersActivityLog.UserId = loginActivity.UserId;
                        usersActivityLog.Url = string.Format("{0}{1}", siteDomainURL, "Account/SignOut");
                        usersActivityLog.PageName = Pages.LOGOUT;
                        usersActivityLog.Description = "Loggedout successfully.";
                        usersActivityLog.IpAddress = loginActivity.IpAddress;
                        usersActivityLog.CreatedBy = userId;
                        usersActivityLog.IsActive = true;
                        usersActivityLog.RecordedOn = recordedOn;
                        usersActivityLog.SessionId = sessionId;

                        string newGUID = usersActivityLogRepository.AddUsersActivityLog(usersActivityLog);

                        //Calculate logged in duration and update value.
                        DateTime logoutTime = recordedOn;

                        var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                        //Calculate logged in duration seconds.
                        TimeSpan span = logoutTime.Subtract(loginTime);
                        double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                        //Update logged in duration in seconds.
                        usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(Guid.Parse(newGUID), loggedInDurationInSeconds, userId);

                        //Update Flag IsLoggedIn = false.
                        userRepository.UpdateIsLoggedInFlag(userGUID, false);
                    }
                }
            }
        }

        public static TrackActivityStatus TrackUsersActivityByUserId(Guid userGuid)
        {
            UsersActivityLogRepository usersActivityLogRepository = new UsersActivityLogRepository();
            UserRepository userRepository = new UserRepository();

            var activities = usersActivityLogRepository.GetUsersActivityLogByUserId(userGuid);

            if (activities != null && activities.Count > 0)
            {
                //Get Login Activity.
                var loginActivity = activities.Where(m => m.PageName == Pages.LOGIN).FirstOrDefault();

                //Check Login Activity Exist.
                if (loginActivity != null)
                {
                    var userId = LogInManager.LoggedInUserId;

                    //Get Logout Activity which tracked after current login.
                    var logoutActivity = activities.Where(m => m.PageName == Pages.LOGOUT 
                                            && m.SessionId == loginActivity.SessionId 
                                            && (m.RecordedOn.Value > loginActivity.RecordedOn.Value)
                                        ).FirstOrDefault();

                    //Check Logout Activity Exist then update logged in duration.
                    if (logoutActivity != null)
                    {
                        //Check Logout Activity already updated with logged in duration. If No then update logged in duration.
                        if (logoutActivity.LoggedInDurationInSeconds.HasValue == false)
                        {
                            DateTime logoutTime = logoutActivity.RecordedOn.Value;

                            var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                            //Calculate logged in duration seconds.
                            TimeSpan span = logoutTime.Subtract(loginTime);
                            double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                            //Update logged in duration in seconds.
                            usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(logoutActivity.Id, loggedInDurationInSeconds, userId);
                        }

                        //Update Flag IsLoggedIn = false.
                        userRepository.UpdateIsLoggedInFlag(userGuid, false);

                        return TrackActivityStatus.RedirectLogin;
                    }
                    else
                    {
                        //Update current session id to login activity.
                        usersActivityLogRepository.UpdateSessionIdInUsersActivity(loginActivity.Id, LogInManager.LoggedInUserSessionId, userId);

                        return TrackActivityStatus.Success;
                    }
                }
            }

            //Update Flag IsLoggedIn = false.
            userRepository.UpdateIsLoggedInFlag(userGuid, false);

            return TrackActivityStatus.RedirectLogin;
        }

        public static void TrackUsersActivityByUserId_V2(Guid userGuid)
        {
            UsersActivityLogRepository usersActivityLogRepository = new UsersActivityLogRepository();
            UserRepository userRepository = new UserRepository();

            var activities = usersActivityLogRepository.GetUsersActivityLogByUserId(userGuid);

            if (activities != null && activities.Count > 0)
            {
                //Get Login Activity.
                var loginActivity = activities.Where(m => m.PageName == Pages.LOGIN).FirstOrDefault();

                //Check Login Activity Exist.
                if (loginActivity != null)
                {
                    var userId = LogInManager.LoggedInUserId;

                    //Get Logout Activity which tracked after current login.
                    var logoutActivity = activities.Where(m => m.PageName == Pages.LOGOUT
                                            && m.SessionId == loginActivity.SessionId
                                            && (m.RecordedOn.Value > loginActivity.RecordedOn.Value)
                                        ).FirstOrDefault();

                    //Check Logout Activity Exist then update logged in duration.
                    if (logoutActivity != null)
                    {
                        //Check Logout Activity already updated with logged in duration. If No then update logged in duration.
                        if (logoutActivity.LoggedInDurationInSeconds.HasValue == false)
                        {
                            DateTime logoutTime = logoutActivity.RecordedOn.Value;

                            var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                            //Calculate logged in duration seconds.
                            TimeSpan span = logoutTime.Subtract(loginTime);
                            double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                            //Update logged in duration in seconds.
                            usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(logoutActivity.Id, loggedInDurationInSeconds, userId);
                        }

                        //Update Flag IsLoggedIn = false.
                        userRepository.UpdateIsLoggedInFlag(userGuid, false);
                    }
                    else
                    {
                        //If Logout Activity not exist then create logout activity and then calcualte logged in duration.
                        DateTime recordedOn = DateTime.Now;

                        var environment = Utility.GetConfigValue("Environment");
                        var siteDomainURL = Utility.GetSiteDomainURL(environment);

                        //Add logout activity.
                        UsersActivityLogVM usersActivityLog = new UsersActivityLogVM();
                        usersActivityLog.UserId = loginActivity.UserId;
                        usersActivityLog.Url = string.Format("{0}{1}", siteDomainURL, "Account/SignOut");
                        usersActivityLog.PageName = Pages.LOGOUT;
                        usersActivityLog.Description = "Loggedout successfully.";
                        usersActivityLog.IpAddress = loginActivity.IpAddress;
                        usersActivityLog.CreatedBy = loginActivity.CreatedBy.Value;
                        usersActivityLog.IsActive = true;
                        usersActivityLog.RecordedOn = recordedOn;
                        usersActivityLog.SessionId = loginActivity.SessionId;

                        string newGUID = usersActivityLogRepository.AddUsersActivityLog(usersActivityLog);

                        //Calculate logged in duration and update value.
                        DateTime logoutTime = recordedOn;

                        var loginTime = loginActivity.RecordedOn.HasValue ? loginActivity.RecordedOn.Value : logoutTime.AddMinutes(-1);

                        //Calculate logged in duration seconds.
                        TimeSpan span = logoutTime.Subtract(loginTime);
                        double? loggedInDurationInSeconds = Math.Abs(Math.Round(span.TotalSeconds, 0));

                        //Update logged in duration in seconds.
                        usersActivityLogRepository.UpdateLoggedInDurationInUsersActivity(Guid.Parse(newGUID), loggedInDurationInSeconds, loginActivity.CreatedBy.Value);

                        //Update Flag IsLoggedIn = false.
                        userRepository.UpdateIsLoggedInFlag(userGuid, false);
                    }
                }
            }
        }
    }

    public enum TrackActivityStatus
    {
        /// <summary>
        /// Activity tracked successfully
        /// </summary>
        Success = 0,        
        /// <summary>
        /// Track activity failed
        /// </summary>
        Failure = 1,
        /// <summary>
        /// Redirect to login logout successful
        /// </summary>
        RedirectLogin = 2,
    }
}