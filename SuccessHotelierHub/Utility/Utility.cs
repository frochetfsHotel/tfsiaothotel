using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SuccessHotelierHub.Models;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace SuccessHotelierHub.Utility
{
    public class Utility
    {
        

        #region 'SCALE-IMAGE'
        /// <summary>
        /// change size of image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static string ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            try
            {
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

                ImageConverter converter = new ImageConverter();
                byte[] imgArray = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
                return Convert.ToBase64String(imgArray);
            }
            catch (Exception Ex)
            {
                Utility.LogError(Ex);
                return null;
            }
        }
        #endregion

        #region 'DATE_FORMAT'
        public static string MailDateFormat(DateTime date)
        {
            try
            {
                if ((date.Year == DateTime.Now.Year)
                    && (date.Day == DateTime.Now.Day))
                {
                    return string.Format("{0:HH:mm tt}", date);
                }
                else if ((date.Day == (DateTime.Now.Day - 1))
                    && (date.Year == DateTime.Now.Year))
                {
                    return string.Format("Yesterday, {0:HH:mm tt}", date);
                }
                else if (date.Year < DateTime.Now.Year)
                {
                    //return date.ToString("dd/MM/yyyy");
                    return string.Format("{0:MMM dd yyyy}", date);
                }
                else
                    return string.Format("{0:MMM dd}", date);
            }
            catch (Exception Ex)
            {
                Utility.LogError(Ex);
                return null;
            }
        }

        public static string MailDateFullFormat(DateTime date)
        {
            try
            {
                if ((date.Year == DateTime.Now.Year)
                    && (date.Day == DateTime.Now.Day))
                {
                    return string.Format("{0:dd MMMM, yyyy HH:mm}", date);
                }
                else if ((date.Day == (DateTime.Now.Day - 1))
                    && (date.Year == DateTime.Now.Year))
                {
                    return string.Format("Yesterday, {0:HH:mm tt}", date);
                }
                else if (date.Year < DateTime.Now.Year)
                {
                    //return date.ToString("dd/MM/yyyy");
                    return string.Format("{0:MMM dd yyyy}", date);
                }
                else
                    return string.Format("{0:u}", date); // UniversalSortableDateTime
            }
            catch (Exception Ex)
            {
                Utility.LogError(Ex);
                return null;
            }
        }
        #endregion

        #region ''

        /// <summary>
        /// Returns count in US format like 12346789 to 12,346,789
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static string FormatCount(string Count)
        {
            string formatedCount = String.Format(new System.Globalization.CultureInfo("en-US", false), "{0:n0}", Convert.ToDouble(Count));
            return formatedCount;
        }

        #endregion
        

        #region 'ERROR-LOG'
        /// <summary>
        /// Log Error for exception class
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            SqlParameter[] parameters = {
                                    new SqlParameter { ParameterName = "@InnerException",  Value = ex.InnerException != null ? ex.InnerException.ToString() : "" },
                                    new SqlParameter { ParameterName = "@Message",  Value = ex.Message != null ? ex.Message.ToString() : ""},
                                    new SqlParameter { ParameterName = "@StackTrace",  Value = ex.StackTrace != null ? ex.StackTrace.ToString() : "" },
                                    new SqlParameter { ParameterName = "@TargetSite",  Value = ex.TargetSite != null ? ex.TargetSite.ToString() : "" },
                                    new SqlParameter { ParameterName = "@Source",  Value = ex.Source != null ? ex.Source.ToString() : "" }
                                };

            DataSet ds = DALHelper.GetDataSetWithExtendedTimeOut("CreateNewErrorLogEntry", parameters);
            
        }

        #endregion


        #region 'Get List Object from DataTable'
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, Convert.ChangeType(row[c], p.PropertyType), null);
                }
            }
        }
        #endregion

        #region 'Get Select List Items'
        public static IEnumerable<SelectListItem> GetSelectListItems(object elements, string action)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            if (action == "Title")
            {
                var titleList = (List<TitleVM>)elements;
                foreach (var title in titleList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = Convert.ToString(title.Id),
                        Text = title.Title
                    });
                }
            }
            else if (action == "VIP")
            {
                var vipList = (List<VipVM>)elements;
                foreach (var vip in vipList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = Convert.ToString(vip.Id),
                        Text = vip.Description
                    });
                }
            }
            else if (action == "Country")
            {
                var countryList = (List<CountryVM>)elements;
                foreach (var country in countryList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = Convert.ToString(country.Id),
                        Text = country.Name
                    });
                }
            }
            else if (action == "State")
            {
                var stateList = (List<StateVM>)elements;
                foreach (var state in stateList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = Convert.ToString(state.Id),
                        Text = state.Name
                    });
                }
            }
            else if (action == "City")
            {
                var cityList = (List<CityVM>)elements;
                foreach (var city in cityList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = Convert.ToString(city.Id),
                        Text = city.Name
                    });
                }
            }

            return selectList;
        }
        #endregion

        #region Escape String
        /// <summary>
        ///This method is used to remove the single and double quotes from the string
        /// </summary>
        /// <param name="strContent">The content</param>
        /// <returns>String</returns>
        public static string EscapeString(string strContent)
        {
            if ((strContent != null) & !string.IsNullOrEmpty(strContent))
            {
                strContent = strContent.Replace("'", "''");
                return strContent.Trim();
            }
            else
            {
                return strContent;
            }

        }
        #endregion

        #region 'Generate Confirmation Number'

        public static string GenerateConfirmationNo(Int64 suffix)
        {
            return (suffix.ToString("00000"));
        }

        #endregion

        #region Remove First & Last Character.

        public static string RemoveLastCharcter(string item, char charcter)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                return item.Trim().TrimEnd(charcter);
            }
            return "";
        }

        public static string RemoveFirstCharcter(string item, char charcter)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                return item.Trim().TrimStart(charcter);
            }
            return "";
        }

        #endregion

        public static string WeekdayName(int dayOfWeek)
        {
            string weekDay = string.Empty;
            switch (dayOfWeek)
            {
                case 0:
                    weekDay = "Sunday";
                    break;
                case 1:
                    weekDay = "Monday";
                    break;
                case 2:
                    weekDay = "Tuesday";
                    break;
                case 3:
                    weekDay = "Wednesday";
                    break;
                case 4:
                    weekDay = "Thursday";
                    break;
                case 5:
                    weekDay = "Friday";
                    break;
                case 6:
                    weekDay = "Saturday";
                    break;
            }
            return weekDay;
        }

        public static Guid LanguageId = Guid.Parse("0490AE29-FC46-42EA-BB8A-9674B4E8CCAE");
    }


    #region 'LOGIN-SESSION'
    public class LogInManager
    {
        public static int LoggedInUserId
        {
            get
            {
                return 1;
            }

        }
    }
    #endregion

    #region 'SESSION-MANAGER'
    public class SessionManager
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
            }
        }

        /// <summary>
        /// MasterResponseGUID of feedback request
        /// </summary>
        public static Guid ManagerFeedbackGUID
        {
            get
            {
                if (HttpContext.Current.Session["ManagerFeedbackGUID"] != null)
                {
                    return (Guid)HttpContext.Current.Session["ManagerFeedbackGUID"];
                }
                else
                {
                    return new Guid();
                }
            }
            set
            {
                HttpContext.Current.Session["ManagerFeedbackGUID"] = value;
            }
        }
    }
    #endregion

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
        /// Login blocked
        /// </summary>
        Blocked = 4,
        /// <summary>
        /// Login blocked
        /// </summary>
        Unknown = 5,
    }

    public static class ProfileTypeName
    {
        public const string INDIVIDUAL = "Individual";
        public const string COMPANY = "Company";
        public const string TRAVELAGENT = "Travel Agent";
        public const string GROUP = "Group";  
    }

    public static class RoomStatusType
    {
        public const string CLEAN = "B7063C63-EB3F-47AB-8383-BBAE17A8F407";
        public const string DIRTY = "28B74F64-569D-4EB8-A044-74BE6DC131AD";
    }

    public static class Constants
    {
        public const int PAGESIZE = 10;
    }

    public static class AdditionalChargeCode
    {
        public const string ROOM_RENT = "1000";
        public const string CHECK_OUT = "9004";
    }
}