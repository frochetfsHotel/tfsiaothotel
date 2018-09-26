using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net;

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
        public static string ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
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
                Utility.LogError(Ex, "ScaleImage");
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
                Utility.LogError(Ex, "MailDateFormat");
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
                Utility.LogError(Ex, "MailDateFullFormat");
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
        public static void LogError(Exception ex, string methodName = "")
        {
            SuccessHotelierHub.Repository.ErrorLogRepository errorLogRepository = new SuccessHotelierHub.Repository.ErrorLogRepository();

            ErrorLogVM errorLog = new ErrorLogVM();
            errorLog.CreatedDateTime = DateTime.Now;
            errorLog.StackTrace = ex.StackTrace != null ? ex.StackTrace.ToString() : "Empty StackTrace.";
            errorLog.ErrorMessage = ex.Message != null ? ex.Message.ToString() : "Empty Message Exception.";
            errorLog.TargetSite = ex.TargetSite != null ? ex.TargetSite.ToString() : "Empty Target Site";
            errorLog.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "Empty InnerException";
            errorLog.PageUrl = GetCurrentPageURL();
            errorLog.MethodName = methodName;

            errorLogRepository.AddErrorLog(errorLog);

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

        public static string GetFullMonthName(int monthNo)
        {
            string monthName = string.Empty;
            switch (monthNo)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "";
                    break;
            }
            return monthName;
        }

        public static string GetShortMonthName(int monthNo)
        {
            string monthName = string.Empty;
            switch (monthNo)
            {
                case 1:
                    monthName = "Jan";
                    break;
                case 2:
                    monthName = "Feb";
                    break;
                case 3:
                    monthName = "Mar";
                    break;
                case 4:
                    monthName = "Apr";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "Jun";
                    break;
                case 7:
                    monthName = "Jul";
                    break;
                case 8:
                    monthName = "Aug";
                    break;
                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;
                case 11:
                    monthName = "Nov";
                    break;
                case 12:
                    monthName = "Dec";
                    break;
                default:
                    monthName = "";
                    break;
            }
            return monthName;
        }

        #region 'Format Amount with exact two decimal places'
        public static string FormatAmountWithTwoDecimal(double amount)
        {
            if (amount == 0)
                return "0.00";
            else
                return string.Format("{0:0.00}", amount);
        }
        #endregion

        public static Guid LanguageId = Guid.Parse("0490AE29-FC46-42EA-BB8A-9674B4E8CCAE");

        public static string EncryptionKey = System.Configuration.ConfigurationManager.AppSettings.Get("EncryptionKey");

        public static string Encrypt(string clearText, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText, string encryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }

        #region 'Get Pdf'

        public static byte[] GetPDF(string html)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(html);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            doc.Open();
            HTMLWorker htmlWorker = new HTMLWorker(doc);
            StringReader sr = new StringReader(html);
            XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, sr);
            // 4: we open document and start the worker on the document

            //htmlWorker.StartDocument();

            //// 5: parse the html into the document
            //htmlWorker.Parse(txtReader);

            //// 6: close the document and the worker
            //htmlWorker.EndDocument();
            //htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        #endregion

        public static string GetIpAddress()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string GetIpAddress_V2()
        {
            try
            {
                string ipAddressString = HttpContext.Current.Request.UserHostAddress;

                if (ipAddressString == null)
                    return null;

                IPAddress ipAddress;
                IPAddress.TryParse(ipAddressString, out ipAddress);

                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ipAddress = System.Net.Dns.GetHostEntry(ipAddress).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }

                return ipAddress.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogError(ex, "GetIpAddress_V2");
                return string.Empty;
            }
        }

        public static string GetIpAddress_V3()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }

        public static string GetCurrentPageURL()
        {
            return System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        }

        public static string ToUpperCase(string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) { return text.Trim().ToUpper(); }
            return "";
        }

        public static string ToLowerCase(string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) { return text.Trim().ToLower(); }
            return "";
        }

        public static double CalculateRoomRentCharges(int noOfNights, double rate, int? noOfChildren, double? discountAmount, double? discountPercentage, bool blnIsDisocuntInPercentage = false)
        {
            double dblTotalBalance = 0;
            double totalDiscount = 0;
            double childrenCharges = 0;

            if (blnIsDisocuntInPercentage)
            {
                if (discountPercentage.HasValue && discountPercentage.Value > 0)
                    totalDiscount = ((rate * discountPercentage.Value) / 100) * noOfNights;
            }
            else
            {
                if (discountAmount.HasValue)
                    totalDiscount = discountAmount.Value * noOfNights;
            }

            rate = (rate - totalDiscount);

            //Default 5 Euro for the one children.
            if (noOfChildren.HasValue)
            {
                //childrenCharges = (noOfChildren.Value *(double)Constants.CHILDREN_CHARGE);
                childrenCharges = (noOfChildren.Value * CurrencyManager.ParseAmountToUserCurrency((double)Constants.CHILDREN_CHARGE, LogInManager.CurrencyCode));
            }

            dblTotalBalance = (noOfNights * rate) + childrenCharges;

            return Math.Round(dblTotalBalance, 2);
        }

        public static double CalculateRoomRentCharges_V2(int noOfNights, double rate, int? noOfChildren, double? discountAmount, double? discountPercentage, bool blnIsDisocuntInPercentage = false)
        {
            double dblTotalBalance = 0;
            double totalDiscount = 0;
            double childrenCharges = 0;

            if (blnIsDisocuntInPercentage)
            {
                if (discountPercentage.HasValue && discountPercentage.Value > 0)
                    totalDiscount = ((rate * discountPercentage.Value) / 100) * noOfNights;
            }
            else
            {
                if (discountAmount.HasValue)
                    totalDiscount = discountAmount.Value * noOfNights;
            }

            rate = (rate - totalDiscount);

            //Default 5 Euro for the one children.
            if (noOfChildren.HasValue)
            {
                childrenCharges = (noOfChildren.Value * (double)Constants.CHILDREN_CHARGE);
            }

            dblTotalBalance = (noOfNights * rate) + childrenCharges;

            return Math.Round(dblTotalBalance, 2);
        }

        public static double CalculateReservationTotalPrice(Guid reservationId)
        {
            ReservationRepository reservationRepository = new ReservationRepository();
            RateRepository rateRepository = new RateRepository();

            var reservation = reservationRepository.GetReservationById(reservationId, LogInManager.LoggedInUserId).FirstOrDefault();

            double dblTotalPrice = 0;

            if (reservation != null)
            {
                int noOfNights;
                double rate;
                int? noOfChildren;
                double? discountAmount;
                double? discountPercentage;
                bool blnIsDisocuntInPercentage = false;

                double totalDiscount = 0;
                double childrenCharges = 0;

                noOfNights = reservation.NoOfNight;
                rate = reservation.Rate.HasValue ? reservation.Rate.Value : 0;
                noOfChildren = reservation.NoOfChildren;
                discountAmount = reservation.DiscountAmount;
                discountPercentage = reservation.DiscountPercentage;

                if (discountPercentage.HasValue) { blnIsDisocuntInPercentage = true; }

                double? dblWeekEndPrice = rate;

                if (reservation.RoomTypeId.HasValue && reservation.RateCodeId.HasValue)
                {
                    var weekEndPrice = rateRepository.GetWeekEndPrice(reservation.RoomTypeId.Value, reservation.RateCodeId.Value).FirstOrDefault();

                    if (weekEndPrice != null)
                    {
                        dblWeekEndPrice = weekEndPrice.Amount;
                    }
                }

                int totalNoOfDays = noOfNights;

                DateTime dtStartDate = reservation.ArrivalDate.Value;

                for (int i = 1; i <= totalNoOfDays; i++)
                {
                    double discount = 0;
                    rate = reservation.Rate.HasValue ? reservation.Rate.Value : 0;

                    if (dtStartDate.DayOfWeek == DayOfWeek.Friday || dtStartDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        rate = dblWeekEndPrice.HasValue ? dblWeekEndPrice.Value : rate;
                    }

                    if (blnIsDisocuntInPercentage)
                    {
                        if (discountPercentage.HasValue && discountPercentage.Value > 0)
                            discount = (rate * discountPercentage.Value) / 100;
                    }
                    else
                    {
                        if (discountAmount.HasValue)
                            discount = discountAmount.Value;
                    }

                    totalDiscount = totalDiscount + discount;

                    dblTotalPrice += (1 * rate); // Each night (1, 2, 3 etc...). Here 1 = current night

                    dtStartDate = dtStartDate.AddDays(1);
                }

                //Deduct Discount from total price.
                if (totalDiscount > 0)
                {
                    dblTotalPrice = (dblTotalPrice - totalDiscount);
                }


                //Default 5 Euro for the one children.
                if (noOfChildren.HasValue)
                    childrenCharges = (noOfChildren.Value * (double)Constants.CHILDREN_CHARGE);

                //dblTotalPrice = (noOfNights * rate) + childrenCharges;
                dblTotalPrice += childrenCharges;

                //Append Add-Ons Price.
                var addOnsDetails = reservationRepository.GetReservationAddOnsMapping(reservation.Id, null, LogInManager.LoggedInUserId);
                if (addOnsDetails != null && addOnsDetails.Count > 0)
                {
                    var totalAddOnsPrice = addOnsDetails.Where(m => m.AddOnsPrice.HasValue).Sum(m => m.AddOnsPrice);

                    dblTotalPrice += (totalAddOnsPrice.HasValue) ? totalAddOnsPrice.Value : 0;
                }

                //Append Package Price.
                var packageDetails = reservationRepository.GetReservationPackageMapping(reservation.Id, null, LogInManager.LoggedInUserId);
                if (packageDetails != null && packageDetails.Count > 0)
                {
                    var totalPackagePrice = packageDetails.Where(m => m.TotalAmount.HasValue).Sum(m => m.TotalAmount);

                    dblTotalPrice += (totalPackagePrice.HasValue) ? totalPackagePrice.Value : 0;
                }

            }

            return Math.Round(dblTotalPrice, 2);
        }

        public static double CalculateTotalBalance(Guid reservationId)
        {
            double dblTotalBalance = 0;

            ReservationRepository reservationRepository = new ReservationRepository();
            var reservation = reservationRepository.GetReservationById(reservationId, LogInManager.LoggedInUserId).FirstOrDefault();

            ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();

            if (reservation != null)
            {
                if (reservation.IsCheckIn == true)
                {
                    //Reservation Charges
                    var reservationCharges = reservationChargeRepository.GetReservationCharges(reservationId, null, LogInManager.LoggedInUserId);
                    if (reservationCharges != null && reservationCharges.Count > 0)
                    {
                        var totalReservationCharges = reservationCharges.Where(m => m.Amount.HasValue)
                                    .Sum(m => (m.Amount.Value * (m.Qty.HasValue ? m.Qty.Value : 1)));

                        dblTotalBalance += totalReservationCharges;

                        dblTotalBalance = CurrencyManager.ConvertAmountToBaseCurrency(dblTotalBalance, LogInManager.CurrencyCode);
                    }
                }
                else
                {
                    double totalRoomRentCharge = 0;
                    //totalRoomRentCharge = CalculateRoomRentCharges(reservation.NoOfNight, (reservation.Rate.HasValue ? reservation.Rate.Value : 0), reservation.NoOfChildren, reservation.DiscountAmount, reservation.DiscountPercentage, (reservation.DiscountPercentage.HasValue ? true : false));

                    totalRoomRentCharge = CalculateReservationTotalPrice(reservation.Id);

                    dblTotalBalance = totalRoomRentCharge;

                    ////Package Price
                    //var packageDetails = reservationRepository.GetReservationPackageMapping(reservation.Id, null);
                    //if (packageDetails != null && packageDetails.Count > 0)
                    //{
                    //    var totalPackagePrice = packageDetails.Where(m => m.PackagePrice.HasValue).Sum(m => m.PackagePrice);

                    //    dblTotalBalance += (totalPackagePrice.HasValue) ? totalPackagePrice.Value : 0;
                    //}
                }

            }

            return Math.Round(dblTotalBalance, 2);
        }

        public static double ConvertToNegative(double number)
        {
            return -Math.Abs(number);
        }

        #region 'Mask Credit Card Numbers'

        public static string MaskCreditCardNo(string number)
        {
            string text = "";

            if (!string.IsNullOrWhiteSpace(number) && number.Length > 4)
            {
                text = number.Substring(number.Length - 4, 4);
            }
            else
            {
                text = number;
            }

            return "XXXXXXXXXXXX" + text;
            //return (suffix.ToString("00000"));
        }

        public static string ExtractCreditCardNoLastFourDigits(string number)
        {
            string text = "";

            if (!string.IsNullOrWhiteSpace(number) && number.Length > 4)
            {
                text = number.Substring(number.Length - 4, 4);
            }
            else
            {
                text = number;
            }

            return text;
        }

        #endregion

        public static string GenerateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$#&?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        #region 'Manage Cookies'

        public static void WriteCookie(string cookieName, string cookieValue, int expireHours = 120)
        {
            string strApplicationPath = System.Web.HttpContext.Current.Request.Url.LocalPath;

            //Create a Cookie with a suitable Key.
            HttpCookie cookie = new HttpCookie(cookieName);

            //Set the Cookie value.            
            cookie.Value = cookieValue;

            //Set the Expiry date.
            cookie.Expires = DateTime.Now.AddHours(expireHours);

            string domainName = string.Empty;            
            domainName = HttpContext.Current.Request.Url.Host;

            HttpContext.Current.Trace.Warn("Domain Name : " + domainName);

            cookie.Domain = domainName;
            //cookie.Path = strApplicationPath;
            cookie.Path = "/";

            //Add the Cookie to Browser.
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void RemoveCookie(string cookieName)
        {
            //Fetch the Cookie using its Key.
            var cookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];

            if (cookie != null)
            {
                //Set the Expiry date to past date.
                cookie.Expires = DateTime.Now.AddMonths(-20);

                string domainName = string.Empty;
                domainName = HttpContext.Current.Request.Url.Host;

                HttpContext.Current.Trace.Warn("Domain Name : " + domainName);

                cookie.Domain = domainName;                
                cookie.Path = "/";

                //Update the Cookie in Browser.
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string ReadCookie(string cookieName)
        {
            //Fetch the Cookie using its Key.
            var cookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];

            string cookieValue = null;

            if (cookie != null)
            {
                //If Cookie exists fetch its value.
                cookieValue = cookie != null ? cookie.Value : null;                
            }

            return cookieValue;
        }

        #endregion 'Manage Cookies'

        public static string GetConfigValue(string key)
        {
            if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings.Get(key)))
            {
                return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get(key));
            }
            return "";
        }

        public static string GetSiteDomainURL(string environment)
        {
            if (!string.IsNullOrWhiteSpace(environment))
            {
                if(environment == "local")
                {
                    return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SiteLocalURL"));
                }
                else if (environment == "stage")
                {
                    return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SiteStageURL"));
                }
                else if (environment == "live")
                {
                    return Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SiteLiveURL"));
                }
            }
            return "";
        }

    }


    //#region 'LOGIN-SESSION'
    //public class LogInManager
    //{
    //    public static int LoggedInUserId
    //    {
    //        get
    //        {
    //            return 1;
    //        }

    //    }
    //}
    //#endregion

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

    public static class ReservationStatusName
    {
        public const string DUEOUT = "C019A362-1A34-4D4F-A793-5182406D4DB3";
        public const string CHECKEDIN = "FB5C5376-8872-4ED8-9A1C-CBF8BB0716AA";
        public const string CHECKEDOUT = "94488A00-864A-4A4D-B742-68FBB84ED5D9";
    }

    public static class Constants
    {
        public const int PAGESIZE = 10;
        public const int CHILDREN_CHARGE = 5; //Default 5 EURO 
        public const int DEFAULT_CONFIRMATION_NO = 100001;
        public const int DEFAULT_FOLIO_NUMBER = 101;
    }

    public static class AdditionalChargeCode
    {
        public const string ROOM_RENT = "1000";
        public const string CHECK_OUT = "9004";
        public const string PACKAGE = "9005";
    }

    public static class AdditionalChargeSource
    {
        public const string ADDITIONAL_CHARGE = "AC"; //Additional Charge
        public const string PACKAGE_MAPPING = "PKM"; //Package Mapping
        public const string ADD_ONS_MAPPING = "AOM"; //Add-Ons
    }

    public static class Pages
    {
        public const string LOGIN = "Login";
        public const string LOGOUT = "Logout";
        public const string INDIVIDUAL_PROFILE = "Individual Profile";
        public const string RATEQUERY = "Rate Query";
        public const string RESERVATION = "Reservation";
        public const string ROOMPLAN = "Room Plan";
        public const string FLOORPLAN = "Floor Plan";
        public const string CALENDAR = "Calendar";
        public const string SEARCH_ARRIVALS = "Search Arrivals";
        public const string SEARCH_REGISTRATION_CARD = "Search Pending Check In Reservation to Generate Registration Card";
        public const string BREAKFAST_REPORT = "Breakfast Report";
        public const string CHECKIN = "Check In";
        public const string SEARCH_GUESTS = "Search Guests";
        public const string BILLING_TRANSACTION = "Billing Transaction";
        public const string CHECKOUT = "Check Out";
        public const string SPLIT_PAYMENT = "Split-Payment and Check Out";
        public const string DAILY_CASHSHEET_REPORT = "Daily CashSheet Report";

        //Master
        public const string PREFERENCEGROUP = "Preference Group";
        public const string PREFERENCE = "Preference";
        public const string RATETYPE = "Rate Type";
        public const string FLOOR = "Floor";
        public const string ROOMTYPE = "Room Type";
        public const string ROOM = "Room";
        public const string ROOMFEATURES = "Room Features";
        public const string WEEKDAYPRICE = "Week Day Price";
        public const string WEEKENDPRICE = "Week End Price";
        public const string RESERVATIONTYPE = "Reservation Type";
        public const string PACKAGE = "Package";
        public const string MARKET = "Market";
        public const string RESERVATIONSOURCE = "Reservation Source";
        public const string PAYMENTMETHOD = "Payment Method";
        public const string CANCELLATION_REASON = "Cancellation Reason";
        public const string ADDITIONAL_CHARGES = "Additional Charges";
        public const string NATIONALITY = "Nationality";
        public const string COUNTRY = "Country";
        public const string STATE = "State";
        public const string CITY = "City";
        public const string USERROLE = "User Role";
        public const string User = "User";
        public const string TUTOR_STUDENT_MAPPING = "Tutor Student Mapping";
        public const string SEARCHTUTOR = "Search Tutor";
        public const string SEARCHSTUDENT = "Search Student";
        public const string VIEWSTUDENT_ACTIVITY = "View Student Activity";
    }

    public static class ExtensionMethod
    {
        public static List<List<T>> Split<T>(this IEnumerable<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var count = 0;
            var temp = new List<T>();

            foreach (var element in collection)
            {
                if (count++ == size)
                {
                    chunks.Add(temp);
                    temp = new List<T>();
                    count = 1;
                }
                temp.Add(element);
            }
            chunks.Add(temp);

            return chunks;
        }


        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 10)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }


        public static List<string> SplitString(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }

        public static string[] SplitStringChunks(this string toSplit, int chunkSize)
        {
            int stringLength = toSplit.Length;

            int chunksRequired = (int)Math.Ceiling((decimal)stringLength / (decimal)chunkSize);
            var stringArray = new string[chunksRequired];

            int lengthRemaining = stringLength;

            for (int i = 0; i < chunksRequired; i++)
            {
                int lengthToUse = Math.Min(lengthRemaining, chunkSize);
                int startIndex = chunkSize * i;
                stringArray[i] = toSplit.Substring(startIndex, lengthToUse);

                lengthRemaining = lengthRemaining - lengthToUse;
            }

            return stringArray;
        }
    }

    public static class Delimeter
    {
        public const string BREAKLINE = "<br/>";
        public const string NEWLINE = "\n";
        public const string COMMA = ",";
        public const string SEMICOLON = ";";
        public const string PIPE = "|";
        public const string DOUBLEPIPE = "||";
        public const string DOT = ".";
        public const string HASH = "#";
        public const string SPACE = " ";
    }

    public static class CompanyInfo
    {
        public static List<CompanyVM> CompanyList
        {
            get
            {
                return new List<CompanyVM>()
               {
                  new CompanyVM() { Id = Guid.Parse("AC3BD2C0-D75B-475E-97F1-5DFBAB9C4C7E"), CompanyName = "Bausch & Lomb", CompanyAddress = "424/425, Cork Road Industrial Estate, Cork Rd, Waterford, Ireland. " },
                  new CompanyVM() { Id = Guid.Parse("2AB20BA4-5915-42E3-8F98-F64EB10BBC9C"), CompanyName = "House of Waterford Crystal", CompanyAddress = "28 The Mall, Waterford, Ireland." },
                  new CompanyVM() { Id = Guid.Parse("EABEBA2A-AC17-458D-A23D-342142905858"), CompanyName = "Apple Ireland", CompanyAddress = "Hollyhill Industrial Estate, Hollyhill, Cork, Ireland." }
               };
            }
        }
    }

    public static class UserLoginTimeInfo
    {
        public const string DEFAULT_LOGIN_START_TIME = "11:00:00"; //11:00 AM
        public const string DEFAULT_LOGIN_END_TIME = "17:00:00"; //05:00 PM
    }
} 