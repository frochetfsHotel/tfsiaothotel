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

                UsersActivityLogVM usersActivityLog = new UsersActivityLogVM();
                usersActivityLog.UserId = LogInManager.LoggedInUser.Id;
                usersActivityLog.Url = Utility.GetCurrentPageURL();
                usersActivityLog.PageName = pageName;
                usersActivityLog.Description = description;                
                usersActivityLog.IpAddress = Utility.GetIpAddress();
                usersActivityLog.CreatedBy = LogInManager.LoggedInUserId;
                usersActivityLog.IsActive = true;
                usersActivityLog.RecordedOn = DateTime.Now;

                usersActivityLogRepository.AddUsersActivityLog(usersActivityLog);
            }
        }
    }
}