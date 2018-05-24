using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;
using System.Text;


namespace SuccessHotelierHub.Controllers
{
    [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
    public class ReportController : Controller
    {
        #region Declaration

        private ProfileRepository profileRepository = new ProfileRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private CheckInCheckOutRepository checkInCheckOutRepository = new CheckInCheckOutRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private ReservationLogRepository reservationLogRepository = new ReservationLogRepository();
        private AdditionalChargeRepository additionalChargeRepository = new AdditionalChargeRepository();
        private ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();
        private PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
        private RoomFeatureRepository roomFeatureRepository = new RoomFeatureRepository();
        private TitleRepository titleRepository = new TitleRepository();
        private CountryRepository countryRepository = new CountryRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();

        #endregion

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        #region Registration Card

        public ActionResult RegistrationCard()
        {
            var companyList = new SelectList(Utility.CompanyInfo.CompanyList, "Id", "CompanyName").ToList();
            ViewBag.CompanyList = companyList;

            return View();
        }

        [HttpPost]
        public ActionResult SearchPendingCheckInReservation(SearchPendingCheckInReservationParametersVM model)
        {
            try
            {
                object sortColumn = "";
                object sortDirection = "";

                if (model.order.Count == 0)
                {
                    sortColumn = "CreatedOn";
                    sortDirection = "desc";
                }
                else
                {
                    sortColumn = model.columns[Convert.ToInt32(model.order[0].column)].data ?? (object)DBNull.Value;
                    sortDirection = model.order[0].dir ?? (object)DBNull.Value;
                }

                model.PageSize = Constants.PAGESIZE;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.IsAdminUser = LogInManager.HasRights("ADMIN");

                var reservations = reservationRepository.SearchPendingCheckInReservation(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.SEARCH_REGISTRATION_CARD, "Searched data in registration card page.");
                #endregion

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservations
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchPendingCheckInReservation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult PreviewRegistrationCard(Guid? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }

            GuestRegistrationCardVM model = new GuestRegistrationCardVM();

            var reservation = reservationRepository.GetReservationById(Id.Value, LogInManager.LoggedInUserId).FirstOrDefault();

            #region Room Mapping

            //Get Room Mapping
            var selectedRooms = roomRepository.GetReservationRoomMapping(Id, null, LogInManager.LoggedInUserId);
            var roomIds = string.Empty;
            var roomNumbers = string.Empty;

            if (selectedRooms != null && selectedRooms.Count > 0)
            {
                foreach (var room in selectedRooms)
                {
                    roomIds += string.Format("{0},", room.RoomId);
                    roomNumbers += string.Format("{0}, ", room.RoomNo);
                }

                if (!string.IsNullOrWhiteSpace(roomNumbers))
                {
                    //Remove Last Comma.
                    roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                }
            }
            #endregion


            #region Profile

            var profile = new IndividualProfileVM();

            if (reservation.ProfileId.HasValue)
                profile = profileRepository.GetIndividualProfileById(reservation.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

            #endregion

            #region Title 

            var title = new TitleVM();
            if (profile.TitleId.HasValue)
                title = titleRepository.GetTitlebyId(profile.TitleId.Value).FirstOrDefault();

            #endregion

            #region Room Type

            var roomType = new RoomTypeVM();
            if (reservation.RoomTypeId.HasValue)
            {
                roomType = roomTypeRepository.GetRoomTypeById(reservation.RoomTypeId.Value).FirstOrDefault();
            }

            #endregion

            #region Preference Mapping

            //Get Preference Mapping
            var selectedPreferences = preferenceRepository.GetReservationPreferenceMapping(reservation.Id, null, LogInManager.LoggedInUserId);

            var preferences = "";
            if (selectedPreferences != null && selectedPreferences.Count > 0)
            {
                preferences = String.Join(", ", selectedPreferences.Select(m => m.PreferenceDescription));

                if (!string.IsNullOrWhiteSpace(preferences))
                {
                    preferences = Utility.Utility.RemoveLastCharcter(preferences.Trim(), ',');
                }
            }

            model.Preferences = preferences;

            #endregion

            #region Package Mapping

            //Get Package Mapping
            var selectedPackage = reservationRepository.GetReservationPackageMapping(reservation.Id, null, LogInManager.LoggedInUserId).FirstOrDefault();

            if (selectedPackage != null)
            {
                model.PackageName = selectedPackage.PackageName;
                model.PackagePrice = CurrencyManager.ParseAmountToUserCurrency(selectedPackage.PackagePrice, LogInManager.CurrencyCode);
                model.PackageTotalAmount = CurrencyManager.ParseAmountToUserCurrency(selectedPackage.TotalAmount, LogInManager.CurrencyCode);
            }

            #endregion


            model.Id = reservation.Id;
            model.ConfirmationNo = reservation.ConfirmationNumber;
            model.ProfileId = reservation.ProfileId;
            model.Title = title.Title;
            model.Email = profile.Email;
            model.PhoneNo = profile.TelephoneNo;
            model.Name = (profile.FirstName + ' ' + profile.LastName);
            model.CarRegistrationNo = profile.CarRegistrationNo;

            #region Fetch Address
            var address = "";
            if (!string.IsNullOrWhiteSpace(profile.Address))
            {
                //address = profile.Address;
                address = profile.Address.Replace(",", Delimeter.SPACE);
            }
            else
            {
                //address = profile.HomeAddress;
                address = profile.HomeAddress.Replace(",", Delimeter.SPACE);
            }

            model.Address = address;

            if (!string.IsNullOrWhiteSpace(profile.CityName))
            {
                //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (Delimeter.SPACE + profile.CityName) : profile.CityName;
                model.City = profile.CityName;
            }

            if (!string.IsNullOrWhiteSpace(profile.StateName))
            {
                //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (Delimeter.SPACE + profile.StateName) : profile.StateName;
                model.State = profile.StateName;
            }

            if (profile.CountryId.HasValue)
            {
                var country = countryRepository.GetCountryById(profile.CountryId.Value).FirstOrDefault();

                if (country != null)
                {
                    model.Country = country.Name;
                }
            }

            //Split Address

            //if (!string.IsNullOrWhiteSpace(model.Address))
            //{
            //    //var splitAddress = ExtensionMethod.SplitString(model.Address, 40);

            //    var splitAddress  = model.Address.SplitStringChunks(60);

            //    if (splitAddress != null && splitAddress.Length > 0)
            //    {
            //        model.Address1 = splitAddress[0];

            //        if (splitAddress.Length > 1) { model.Address2 = splitAddress[1]; }
            //        if (splitAddress.Length > 2) { model.Address3 = splitAddress[2]; }
            //    }
            //}

            model.ZipCode = profile.ZipCode;
            #endregion

            model.RoomNumer = roomNumbers;
            model.ArrivalDate = reservation.ArrivalDate.HasValue ? reservation.ArrivalDate.Value.ToString("dd-MMM-yyyy") : "";
            model.DepartureDate = reservation.DepartureDate.HasValue ? reservation.DepartureDate.Value.ToString("dd-MMM-yyyy") : "";
            model.NoOfNights = reservation.NoOfNight;
            model.NoOfAdult = reservation.NoOfAdult;
            model.NoOfChildren = reservation.NoOfChildren;

            if (roomType != null)
            {
                model.RoomType = roomType.RoomTypeCode;
            }
            model.Rate = CurrencyManager.ParseAmountToUserCurrency(reservation.Rate, LogInManager.CurrencyCode);
            model.ConfirmationNo = reservation.ConfirmationNumber;

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.SEARCH_REGISTRATION_CARD, "Generated guest registration card report.");
            #endregion

            //HTML to PDF
            string html = Utility.Utility.RenderPartialViewToString((Controller)this, "PreviewRegistrationCard", model);

            byte[] pdfBytes = Utility.Utility.GetPDF(html);

            //return File(pdfBytes, "application/pdf", string.Format("RegistrationCard_{0}.pdf", model.Id));
            return File(pdfBytes, "application/pdf");

            //return View(model);
        }

        [HttpPost]
        public ActionResult SaveSelectedPrintRegistrationCard(List<Guid> ids)
        {
            try
            {
                if (ids != null)
                {
                    Session["ReservationIds"] = ids;

                    return Json(new
                    {
                        IsSuccess = true,
                        IsExternalUrl = true,
                        data = Url.Action("PrintMultipleRegistrationCard", "Report")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Please select at least one checkbox to print registration card."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SaveSelectedPrintRegistrationCard");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult PrintMultipleRegistrationCard()
        {
            if (Session["ReservationIds"] == null || string.IsNullOrWhiteSpace(Convert.ToString(Session["ReservationIds"])))
            {
                return HttpNotFound();
            }

            var reservationIds = (List<Guid>)Session["ReservationIds"];

            StringBuilder html = new StringBuilder();

            if (reservationIds != null && reservationIds.Count > 0)
            {
                foreach (var reservationId in reservationIds)
                {
                    var reservation = reservationRepository.GetReservationById(reservationId, LogInManager.LoggedInUserId).FirstOrDefault();

                    if (reservation != null)
                    {
                        GuestRegistrationCardVM model = new GuestRegistrationCardVM();

                        #region Room Mapping

                        //Get Room Mapping
                        var selectedRooms = roomRepository.GetReservationRoomMapping(reservationId, null, LogInManager.LoggedInUserId);
                        var roomIds = string.Empty;
                        var roomNumbers = string.Empty;

                        if (selectedRooms != null && selectedRooms.Count > 0)
                        {
                            foreach (var room in selectedRooms)
                            {
                                roomIds += string.Format("{0},", room.RoomId);
                                roomNumbers += string.Format("{0}, ", room.RoomNo);
                            }

                            if (!string.IsNullOrWhiteSpace(roomNumbers))
                            {
                                //Remove Last Comma.
                                roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                            }
                        }
                        #endregion


                        #region Profile

                        var profile = new IndividualProfileVM();

                        if (reservation.ProfileId.HasValue)
                            profile = profileRepository.GetIndividualProfileById(reservation.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                        #endregion

                        #region Title 

                        var title = new TitleVM();
                        if (profile.TitleId.HasValue)
                            title = titleRepository.GetTitlebyId(profile.TitleId.Value).FirstOrDefault();

                        #endregion

                        #region Room Type

                        var roomType = new RoomTypeVM();
                        if (reservation.RoomTypeId.HasValue)
                        {
                            roomType = roomTypeRepository.GetRoomTypeById(reservation.RoomTypeId.Value).FirstOrDefault();
                        }

                        #endregion

                        #region Preference Mapping

                        //Get Preference Mapping
                        var selectedPreferences = preferenceRepository.GetReservationPreferenceMapping(reservation.Id, null, LogInManager.LoggedInUserId);

                        var preferences = "";
                        if (selectedPreferences != null && selectedPreferences.Count > 0)
                        {
                            preferences = String.Join(", ", selectedPreferences.Select(m => m.PreferenceDescription));

                            if (!string.IsNullOrWhiteSpace(preferences))
                            {
                                preferences = Utility.Utility.RemoveLastCharcter(preferences.Trim(), ',');
                            }
                        }

                        model.Preferences = preferences;

                        #endregion

                        #region Package Mapping

                        //Get Package Mapping
                        var selectedPackage = reservationRepository.GetReservationPackageMapping(reservation.Id, null, LogInManager.LoggedInUserId).FirstOrDefault();

                        if (selectedPackage != null)
                        {
                            model.PackageName = selectedPackage.PackageName;
                            model.PackagePrice = CurrencyManager.ParseAmountToUserCurrency(selectedPackage.PackagePrice, LogInManager.CurrencyCode);
                            model.PackageTotalAmount = CurrencyManager.ParseAmountToUserCurrency(selectedPackage.TotalAmount, LogInManager.CurrencyCode);
                        }

                        #endregion

                        model.Id = reservation.Id;
                        model.ConfirmationNo = reservation.ConfirmationNumber;
                        model.ProfileId = reservation.ProfileId;
                        model.Title = title.Title;
                        model.Email = profile.Email;
                        model.PhoneNo = profile.TelephoneNo;
                        model.Name = (profile.FirstName + ' ' + profile.LastName);
                        model.CarRegistrationNo = profile.CarRegistrationNo;

                        #region Fetch Address
                        var address = "";
                        if (!string.IsNullOrWhiteSpace(profile.Address))
                        {
                            //address = profile.Address;
                            address = profile.Address.Replace(",", Delimeter.SPACE);
                        }
                        else
                        {
                            //address = profile.HomeAddress;
                            address = profile.HomeAddress.Replace(",", Delimeter.SPACE);
                        }

                        model.Address = address;

                        if (!string.IsNullOrWhiteSpace(profile.CityName))
                        {
                            //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (Delimeter.SPACE + profile.CityName) : profile.CityName;
                            model.City = profile.CityName;
                        }

                        if (!string.IsNullOrWhiteSpace(profile.StateName))
                        {
                            //model.Address += !string.IsNullOrWhiteSpace(model.Address) ? (Delimeter.SPACE + profile.StateName) : profile.StateName;
                            model.State = profile.StateName;
                        }

                        if (profile.CountryId.HasValue)
                        {
                            var country = countryRepository.GetCountryById(profile.CountryId.Value).FirstOrDefault();

                            if (country != null)
                            {
                                model.Country = country.Name;
                            }
                        }

                        //Split Address

                        //if (!string.IsNullOrWhiteSpace(model.Address))
                        //{
                        //    //var splitAddress = ExtensionMethod.SplitString(model.Address, 40);

                        //    var splitAddress = model.Address.SplitStringChunks(60);

                        //    if (splitAddress != null && splitAddress.Length > 0)
                        //    {
                        //        model.Address1 = splitAddress[0];

                        //        if (splitAddress.Length > 1) { model.Address2 = splitAddress[1]; }
                        //        if (splitAddress.Length > 2) { model.Address3 = splitAddress[2]; }
                        //    }
                        //}

                        model.ZipCode = profile.ZipCode;
                        #endregion

                        model.RoomNumer = roomNumbers;
                        model.ArrivalDate = reservation.ArrivalDate.HasValue ? reservation.ArrivalDate.Value.ToString("dd-MMM-yyyy") : "";
                        model.DepartureDate = reservation.DepartureDate.HasValue ? reservation.DepartureDate.Value.ToString("dd-MMM-yyyy") : "";
                        model.NoOfNights = reservation.NoOfNight;
                        model.NoOfAdult = reservation.NoOfAdult;
                        model.NoOfChildren = reservation.NoOfChildren;

                        if (roomType != null)
                        {
                            model.RoomType = roomType.RoomTypeCode;
                        }
                        model.Rate = CurrencyManager.ParseAmountToUserCurrency(reservation.Rate, LogInManager.CurrencyCode);
                        model.ConfirmationNo = reservation.ConfirmationNumber;

                        //HTML to PDF
                        html.Append(Utility.Utility.RenderPartialViewToString((Controller)this, "PreviewRegistrationCard", model));
                    }
                }
            }

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.SEARCH_REGISTRATION_CARD, "Generated multiple guest registration card report.");
            #endregion

            //Clear session.
            //Session["ReservationIds"] = null;

            byte[] pdfBytes = Utility.Utility.GetPDF(Convert.ToString(html));

            //return File(pdfBytes, "application/pdf", string.Format("RegistrationCard_{0}.pdf", model.Id));
            return File(pdfBytes, "application/pdf");
        }

        #endregion Registration Card

        #region Breakfast Report

        public ActionResult Breakfast()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty)
                                       .Select(
                                           m => new SelectListItem()
                                           {
                                               Value = m.Id.ToString(),
                                               Text = m.RateTypeCode
                                           }
                                       ), "Value", "Text").ToList();


            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RateTypeList = rateTypeList;

            return View();
        }

        [HttpPost]
        public ActionResult SearchBreakfastReport(SearchBreakFastReportParametersVM model)
        {
            try
            {
                object sortColumn = "";
                object sortDirection = "";

                if (model.order.Count == 0)
                {
                    sortColumn = "CreatedOn";
                    sortDirection = "desc";
                }
                else
                {
                    sortColumn = model.columns[Convert.ToInt32(model.order[0].column)].data ?? (object)DBNull.Value;
                    sortDirection = model.order[0].dir ?? (object)DBNull.Value;
                }

                model.PageSize = Constants.PAGESIZE;
                model.UserId = LogInManager.LoggedInUserId;                

                var reservations = reservationRepository.SearchBreakfastReport(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.BREAKFAST_REPORT, "Searched break fast report.");
                #endregion

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservations
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchBreakfastReport");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult PreviewBreakfastReport(string date, string showDate)
        {

            BreakFastReport model = new BreakFastReport();
            model.Date = showDate;

            var results = reservationRepository.GetBreakfastReport(date, LogInManager.LoggedInUserId);
            model.Results = results;

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.SEARCH_REGISTRATION_CARD, "Generated break fast pdf report.");
            #endregion

            //HTML to PDF
            string html = Utility.Utility.RenderPartialViewToString((Controller)this, "PreviewBreakfastReport", model);

            byte[] pdfBytes = Utility.Utility.GetPDF(html);

            //return File(pdfBytes, "application/pdf", "BreakFastReport.pdf");

            return File(pdfBytes, "application/pdf");

            //return View(model);
        }

        #endregion Breakfast Report
    }
}
