﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SuccessHotelierHub.Controllers
{
    //[HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
    public class ReservationController : Controller
    {
        #region Declaration

        //private const Int64 DefaultConfirmationNo = 100001;
        //private const Int64 DefaultFolioNumber = 101;

        private ProfileRepository profileRepository = new ProfileRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private FloorRepository floorRepository = new FloorRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private CountryRepository countryRepository = new CountryRepository();
        private TitleRepository titleRepository = new TitleRepository();
        private VipRepository vipRepository = new VipRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();
        private PreferenceGroupRepository preferenceGroupRepository = new PreferenceGroupRepository();
        private ReservationCancellationReasonRepository reservationCancellationReasonRepository = new ReservationCancellationReasonRepository();
        private AdditionalChargeRepository additionalChargeRepository = new AdditionalChargeRepository();
        private ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();
        private ReservationTypeRepository reservationTypeRepository = new ReservationTypeRepository();
        private PackageRepository packageRepository = new PackageRepository();
        private MarketRepository marketRepository = new MarketRepository();
        private ReservationSourceRepository reservationSourceRepository = new ReservationSourceRepository();
        private PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
        private RoomFeatureRepository roomFeatureRepository = new RoomFeatureRepository();
        private ReservationLogRepository reservationLogRepository = new ReservationLogRepository();
        private OriginRepository originRepository = new OriginRepository();
        private DiscountApprovedRepository discountApprovedRepository = new DiscountApprovedRepository();
        private RTCRepository rtcRepository = new RTCRepository();
        private AddOnsRepository addOnsRepository = new AddOnsRepository();
        private RateRepository rateRepository = new RateRepository();
        private UserRepository userRepository = new UserRepository();
        private UserGroupRepository userGroupRepository = new UserGroupRepository();
        private CurrencyRepository currencyRepository = new CurrencyRepository();
        private CompanyRepository CompanyRepository = new CompanyRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

        #endregion

        #region Reservation 

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult Create()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
            
            var titleList = new SelectList(titleRepository.GetTitle()
                .Select(
                    m => new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = (m.Title)
                    }
                ), "Value", "Text").ToList();

            var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var rateTypeList = new SelectList(
                    rateTypeRepository.GetRateType(string.Empty)
                    .Select(
                        m => new SelectListItem()
                        {
                            Value = m.Id.ToString(),
                            Text = m.RateTypeCode
                        }
                    ), "Value", "Text").ToList();

            var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();
            var reservationTypeList = new SelectList(reservationTypeRepository.GetReservationTypes(), "Id", "Name").ToList();
            var packageList = packageRepository.GetPackages();

            var marketList = new SelectList(
                              marketRepository.GetMarkets()
                              .Select(
                                  m => new SelectListItem()
                                  {
                                      Value = m.Id.ToString(),
                                      Text = !string.IsNullOrWhiteSpace(m.Description) ? m.Description : m.Name
                                  }
                      ), "Value", "Text").ToList();

            var reservationSourceList = new SelectList(
                             reservationSourceRepository.GetReservationSources()
                             .Select(
                                 m => new SelectListItem()
                                 {
                                     Value = m.Id.ToString(),
                                     Text = !string.IsNullOrWhiteSpace(m.Description) ? m.Description : m.Name
                                 }
                     ), "Value", "Text").ToList();

            var paymentMethodList = paymentMethodRepository.GetPaymentMethods();

            var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

            var discountApprovedList = new SelectList(discountApprovedRepository.GetDiscountApprovedDetail().Select
                       (
                              m => new SelectListItem()
                              {
                                  Value = m.Id.ToString(),
                                  Text = (m.Code + " - " + m.Description)
                              }
                       ), "Value", "Text").ToList();

            var rtcList = new SelectList(rtcRepository.GetRTC(), "Id", "Code").ToList();
            var currencyList = new SelectList(CurrencyManager.GetCurrencyInfo(), "Id", "Code").ToList();
            
            var companyList = CompanyRepository.GetCompanyList();
            ViewBag.CompanyList = companyList;

            ReservationVM model = new ReservationVM();
            RateQueryVM rateQuery = new RateQueryVM();
            if (Session["RateQueryVM"] != null)
            {
                rateQuery = (RateQueryVM)Session["RateQueryVM"];

                var profile = new IndividualProfileVM();
                if (rateQuery.ProfileId.HasValue)
                    profile = profileRepository.GetIndividualProfileById(rateQuery.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                model.ArrivalDate = rateQuery.ArrivalDate;
                model.NoOfNight = rateQuery.NoOfNight.HasValue ? rateQuery.NoOfNight.Value : 0;
                model.DepartureDate = rateQuery.DepartureDate;
                model.NoOfAdult = rateQuery.NoOfAdult;
                model.NoOfChildren = rateQuery.NoOfChildren;
                model.NoOfInfant = rateQuery.NoOfInfant;
                model.NoOfRoom = rateQuery.NoOfRoom;

                model.LastName = rateQuery.LastName;
                model.FirstName = rateQuery.FirstName;
                model.ProfileId = rateQuery.ProfileId;
                model.TitleId = profile.TitleId;
                model.Remarks = profile.Remarks;

                model.MemberTypeId = rateQuery.MemberTypeId;
                model.CompanyId = rateQuery.CompanyId;
                model.AgentId = rateQuery.AgentId;
                model.SourceId = rateQuery.SourceId;
                model.BlockCodeId = rateQuery.BlockCodeId;

                model.MemberNo = rateQuery.MemberNo;
                model.RateCodeId = rateQuery.RateTypeId; //RateTypeId
                model.RateTypeCode = rateQuery.RateTypeCode; //RateTypeCode
                model.RoomTypeId = rateQuery.RoomTypeId; //RoomTypeId
                model.RoomTypeCode = rateQuery.RoomTypeCode; //RoomTypeCode
                model.Rate = rateQuery.Amount; //Rate
                model.IsWeekEndPrice = rateQuery.IsWeekEndPrice; // Week End Price.

                //model.PackageId = rateQuery.PackageId;
                model.CompanyId = rateQuery.CompanyId;

                double? dblWeekEndPrice = model.Rate;
                if (model.RoomTypeId.HasValue && model.RateCodeId.HasValue)
                {
                    var weekEndPrice = rateRepository.GetWeekEndPrice(model.RoomTypeId.Value, model.RateCodeId.Value).FirstOrDefault();

                    if (weekEndPrice != null)
                    {
                        dblWeekEndPrice = weekEndPrice.Amount;
                    }
                }

                ViewBag.WeekEndPrice = CurrencyManager.ParseAmountToUserCurrency(dblWeekEndPrice, LogInManager.CurrencyCode);

            }

            #region Profile Info From Profile Page

            var profileId = (string)TempData["ProfileId"];
            var firstName = (string)TempData["FirstName"];
            var lastName = (string)TempData["LastName"];
            var titleId = (Guid?)TempData["TitleId"];
            var countryId = (int?)TempData["CountryId"];
            var telephoneNo = (string)TempData["TelephoneNo"];
            var remarks = (string)TempData["Remarks"];

            if (!string.IsNullOrWhiteSpace(profileId))
            {
                model.ProfileId = Guid.Parse(profileId);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                model.FirstName = firstName;
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                model.LastName = lastName;
            }

            if (titleId.HasValue)
            {
                model.TitleId = titleId;
            }

            if (countryId.HasValue)
            {
                model.CountryId = countryId;
            }
            if (!string.IsNullOrWhiteSpace(telephoneNo))
            {
                model.PhoneNo = telephoneNo;
            }
            if (!string.IsNullOrWhiteSpace(remarks))
            {
                model.Remarks = remarks;
            }

            #endregion

            ViewBag.TitleList = titleList;
            ViewBag.VipList = vipList;
            ViewBag.CountryList = countryList;
            ViewBag.PreferenceGroupList = preferenceGroupList;
            ViewBag.RateTypeList = rateTypeList;
            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.ReservationTypeList = reservationTypeList;
            ViewBag.PackageList = packageList;
            ViewBag.MarketList = marketList;
            ViewBag.ReservationSourceList = reservationSourceList;
            ViewBag.PaymentMethodList = paymentMethodList;
            ViewBag.RoomFeaturesList = roomFeaturesList;
            ViewBag.DiscountApprovedList = discountApprovedList;
            ViewBag.RtcList = rtcList;
            ViewBag.CurrencyList = currencyList;

            model.CurrencyId = LogInManager.CurrencyId;

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.RESERVATION, "Goes to create new reservation page.");
            #endregion

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.RoomIds))
                {
                    string strETAText = "10:00";
                    if (!string.IsNullOrWhiteSpace(model.ETAText))
                    {
                        strETAText = model.ETAText;
                    }
                    var checkReservationOrNot = reservationRepository.GetPreviousReservationOrNot(Guid.Parse(model.RoomIds), model.ArrivalDate, TimeSpan.Parse(strETAText), null, LogInManager.LoggedInUserId);
                    if (checkReservationOrNot != null && checkReservationOrNot.Count > 0)
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            IsReservation = true,
                            errorMessage = "Selected room already checked in by another user. please select check in (ETA) time after " + checkReservationOrNot.Select(m => m.DepartureDate != null ? m.DepartureDate.Value.ToString("dd MMM yyyy") : null).FirstOrDefault() + " " + checkReservationOrNot.Select(m => m.CheckOutTime != null ? m.CheckOutTime : null).FirstOrDefault() + ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                string confirmationNo = string.Empty;

                confirmationNo = CreateReservation(model);

                if (!string.IsNullOrWhiteSpace(confirmationNo))
                {

                    //Clear Session Object.
                    Session["RateQueryVM"] = null;

                    #region Send Email 

                    if (model.IsEmailConfirmation)
                    {
                        if (model.ProfileId.HasValue)
                        {
                            var profile = profileRepository.GetIndividualProfileById(model.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                            double rate = 0;
                            string currencyCode = string.Empty, currencySymbol = string.Empty;

                            if (profile != null)
                            {
                                var userDetail = userRepository.GetUserDetailByUserId(LogInManager.LoggedInUserId).FirstOrDefault();
                                var userGroupDetail = new UserGroupVM();

                                if (userDetail != null)
                                {
                                    var collegeGroup = collegeGroupRepository.GetCollegeGroupById(userDetail.CollegeGroupId.Value);

                                    if (collegeGroup != null)
                                    {
                                        userGroupDetail = userGroupRepository.GetUserGroupById(collegeGroup.UserGroupId.Value);
                                    }
                                }

                                if (userGroupDetail != null && userGroupDetail.Id != null)
                                {
                                    var currencyDetail = currencyRepository.GetCurrencyInfoById(userGroupDetail.CurrencyId).FirstOrDefault();

                                    if (currencyDetail != null)
                                    {
                                        currencyCode = currencyDetail.Code;
                                        currencySymbol = currencyDetail.CurrencySymbol;
                                    }
                                }

                                var profileName = (profile.LastName + ' ' + profile.FirstName);
                                var email = profile.Email;

                                if (model.Rate.HasValue)
                                {
                                    rate = CurrencyManager.ParseAmountToUserCurrency(model.Rate.Value, currencyCode);
                                }

                                if (!string.IsNullOrWhiteSpace(email))
                                {
                                    ReservationConfirmationReportVM obj = new ReservationConfirmationReportVM();
                                    obj.UserName = profile.FirstName;
                                    obj.GuestName = profileName;
                                    obj.ConfirmationNumber = model.ConfirmationNumber;
                                    obj.ArrivalDate = model.ArrivalDate.Value.ToString("dd MMM yyyy");
                                    obj.DepartureDate = model.DepartureDate.Value.ToString("dd MMM yyyy");

                                    obj.NoOfNight = model.NoOfNight;
                                    obj.NoOfAdult = model.NoOfAdult == null ? 0 : model.NoOfAdult;
                                    obj.NoOfChildren = model.NoOfChildren == null ? 0 : model.NoOfChildren;
                                    obj.CashierName = LogInManager.UserName;

                                    obj.Rate = rate;                                    

                                    obj.RatePerNight = Utility.Utility.FormatAmountWithTwoDecimal(rate);

                                    //Method Of Payment.
                                    if (model.PaymentMethodId.HasValue)
                                    {
                                        var paymentMethod = paymentMethodRepository.GetPaymentMethodById(model.PaymentMethodId.Value).FirstOrDefault();

                                        if (paymentMethod != null)
                                        {
                                            obj.MethodOfPayment = paymentMethod.Name;
                                        }
                                        else
                                        {
                                            obj.MethodOfPayment = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        obj.MethodOfPayment = string.Empty;
                                    }

                                    //Accommodation
                                    if (model.RoomTypeId.HasValue)
                                    {
                                        var roomType = roomTypeRepository.GetRoomTypeById(model.RoomTypeId.Value).FirstOrDefault();

                                        if (roomType != null)
                                        {
                                            obj.Accommodation = (Convert.ToString(model.NoOfRoom) + " " + roomType.Description);
                                        }
                                        else
                                        {
                                            obj.Accommodation = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        obj.Accommodation = string.Empty;
                                    }

                                    if (userDetail != null)
                                    {
                                        //Package
                                        var reservationPackageMapping = reservationRepository.GetReservationPackageMapping(model.Id, null, userDetail.UserId);

                                        if (reservationPackageMapping != null && reservationPackageMapping.Count > 0)
                                        {
                                            obj.IsBreakFast = reservationPackageMapping.Where(i => i.PackageName == "Full Irish Breakfast").Any();
                                        }
                                    }

                                    //obj.DepositPaid = "EUR";
                                    obj.DepositPaid = currencyCode;
                                    obj.CurrencySymbol = currencySymbol;

                                    //HTML generation.
                                    string html = Utility.Utility.RenderPartialViewToString((Controller)this, "ReservationConfirmation", obj);

                                    //HTML to PDF.
                                    byte[] pdfBytes = Utility.Utility.GetPDF(html);

                                    StringBuilder bodyMsg = new StringBuilder();
                                    using (var sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlTemplates/ReservationConfirmationEmail.html")))
                                    {
                                        bodyMsg.Append(sr.ReadToEnd());

                                        bodyMsg.Replace("[@UserName]", obj.UserName);
                                        bodyMsg.Replace("[@CashierName]", LogInManager.UserName);

                                        //File Name.
                                        string fileName = string.Format("Confirm-Reservation-{0}.pdf", model.ConfirmationNumber);

                                        //Send Email.                                        
                                        string EmailSubject = string.Format("Confirmation of your reservation at Roche International Hotel and Spa. {0} ", model.ConfirmationNumber);

                                        bool blnMailSend = SuccessHotelierHub.Utility.Email.sendMail(email, EmailSubject, Convert.ToString(bodyMsg), fileName, true, pdfBytes);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = model.Id,
                            ConfirmationNo = confirmationNo
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Create");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult Edit(Guid id)
        {
            ReservationVM model = new ReservationVM();
            var reservation = reservationRepository.GetReservationById(id, LogInManager.LoggedInUserId);

            if (reservation != null && reservation.Count > 0)
            {
                model = reservation[0];
                model.Remarks = string.Empty;

                //Get Profile Detail
                var profileDetail = profileRepository.GetIndividualProfileById(model.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                if (!model.CurrencyId.HasValue)
                {
                    model.CurrencyId = LogInManager.CurrencyId;
                }

                if (model.ArrivalDate.HasValue)
                {
                    if (model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Friday || model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        model.IsWeekEndPrice = true;
                    }
                }

                
                //Decrypt Credit Card#
                model.CreditCardNo = Utility.Utility.Decrypt(model.CreditCardNo, Utility.Utility.EncryptionKey);

                //Original Credit Card#
                ViewBag.OriginalCreditCardNo = model.CreditCardNo;

                //Mast Credit Card#
                model.CreditCardNo = Utility.Utility.MaskCreditCardNo(model.CreditCardNo);

                #region Room Type
                //Get Room Type Details.
                if (model.RoomTypeId.HasValue)
                {
                    var roomType = roomTypeRepository.GetRoomTypeById(model.RoomTypeId.Value).FirstOrDefault();

                    if (roomType != null)
                    {
                        model.RoomTypeCode = roomType.RoomTypeCode;
                    }
                }
                #endregion

                #region Room Mapping

                //Get Room Mapping
                var selectedRooms = roomRepository.GetReservationRoomMapping(model.Id, null, LogInManager.LoggedInUserId);

                ViewBag.SelectedRooms = selectedRooms;

                #endregion

                #region Preference Mapping

                //Get Preference Mapping
                var selectedPreferences = preferenceRepository.GetReservationPreferenceMapping(model.Id, null, LogInManager.LoggedInUserId);

                ViewBag.SelectedPreferences = selectedPreferences;

                var profilePreferences = new List<ProfilePreferenceMappingVM>();
                if (profileDetail != null && profileDetail.Id != null)
                {
                    //Get Profile Preference Mapping
                    profilePreferences = preferenceRepository.GetProfilePreferenceMapping(profileDetail.ProfileTypeId, profileDetail.Id, null, LogInManager.LoggedInUserId);
                }
                ViewBag.ProfilePreferences = profilePreferences;

                #endregion

                #region Add Ons Mapping

                //Get AddOns Mapping
                var selectedAddOns = reservationRepository.GetReservationAddOnsMapping(model.Id, null, LogInManager.LoggedInUserId);

                ViewBag.SelectedAddOns = selectedAddOns;

                #endregion

                #region Package Mapping

                //Get Package Mapping
                var selectedPackages = reservationRepository.GetReservationPackageMapping(model.Id, null, LogInManager.LoggedInUserId);
                

                ViewBag.PackageMapping = selectedPackages;

                #endregion

                #region Profile Info From Edit Profile Page

                var profileId = (string)TempData["ProfileId"];
                var firstName = (string)TempData["FirstName"];
                var lastName = (string)TempData["LastName"];
                var titleId = (Guid?)TempData["TitleId"];
                var countryId = (int?)TempData["CountryId"];
                var telephoneNo = (string)TempData["TelephoneNo"];
                var remarks = (string)TempData["Remarks"];

                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    model.ProfileId = Guid.Parse(profileId);
                }

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    model.FirstName = firstName;
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    model.LastName = lastName;
                }

                if (titleId.HasValue)
                {
                    model.TitleId = titleId;
                }

                if (countryId.HasValue)
                {
                    model.CountryId = countryId;
                }
                if (!string.IsNullOrWhiteSpace(telephoneNo))
                {
                    model.PhoneNo = telephoneNo;
                }
                if (!string.IsNullOrWhiteSpace(remarks))
                {
                    model.Remarks = remarks;
                }

                #endregion

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                
                var titleList = new SelectList(titleRepository.GetTitle()
                            .Select(
                                m => new SelectListItem()
                                {
                                    Value = m.Id.ToString(),
                                    Text = (m.Title)
                                }
                            ), "Value", "Text").ToList();
                var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
                var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty)
                                        .Select(
                                            m => new SelectListItem()
                                            {
                                                Value = m.Id.ToString(),
                                                Text = m.RateTypeCode
                                            }
                                        ), "Value", "Text").ToList();
                var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();
                var reservationTypeList = new SelectList(reservationTypeRepository.GetReservationTypes(), "Id", "Name").ToList();
                var packageList = packageRepository.GetPackages();

                var marketList = new SelectList(
                              marketRepository.GetMarkets()
                              .Select(
                                  m => new SelectListItem()
                                  {
                                      Value = m.Id.ToString(),
                                      Text = !string.IsNullOrWhiteSpace(m.Description) ? m.Description : m.Name
                                  }
                      ), "Value", "Text").ToList();


                var reservationSourceList = new SelectList(
                              reservationSourceRepository.GetReservationSources()
                              .Select(
                                  m => new SelectListItem()
                                  {
                                      Value = m.Id.ToString(),
                                      Text = !string.IsNullOrWhiteSpace(m.Description) ? m.Description : m.Name
                                  }
                      ), "Value", "Text").ToList();

                
                var paymentMethodList = paymentMethodRepository.GetPaymentMethods();

                var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

                var discountApprovedList = new SelectList(discountApprovedRepository.GetDiscountApprovedDetail().Select
                       (
                              m => new SelectListItem()
                              {
                                  Value = m.Id.ToString(),
                                  Text = (m.Code + " - " + m.Description)
                              }
                       ), "Value", "Text").ToList();

                var rtcList = new SelectList(rtcRepository.GetRTC(), "Id", "Code").ToList();
                var currencyList = new SelectList(CurrencyManager.GetCurrencyInfo(), "Id", "Code").ToList();

                var companyList = CompanyRepository.GetCompanyList();
                ViewBag.CompanyList = companyList;

                ViewBag.TitleList = titleList;
                ViewBag.VipList = vipList;
                ViewBag.CountryList = countryList;
                ViewBag.PreferenceGroupList = preferenceGroupList;
                ViewBag.RateTypeList = rateTypeList;
                ViewBag.RoomTypeList = roomTypeList;
                ViewBag.ReservationTypeList = reservationTypeList;
                ViewBag.PackageList = packageList;
                ViewBag.MarketList = marketList;
                ViewBag.ReservationSourceList = reservationSourceList;
                ViewBag.PaymentMethodList = paymentMethodList;
                ViewBag.RoomFeaturesList = roomFeaturesList;
                ViewBag.DiscountApprovedList = discountApprovedList;
                ViewBag.RtcList = rtcList;
                ViewBag.CurrencyList = currencyList;

                double? dblWeekEndPrice = model.Rate;
                if (model.RoomTypeId.HasValue && model.RateCodeId.HasValue)
                {
                    var weekEndPrice = rateRepository.GetWeekEndPrice(model.RoomTypeId.Value, model.RateCodeId.Value).FirstOrDefault();

                    if (weekEndPrice != null)
                    {
                        dblWeekEndPrice = weekEndPrice.Amount;
                    }
                }

                ViewBag.WeekEndPrice = CurrencyManager.ParseAmountToUserCurrency(dblWeekEndPrice, LogInManager.CurrencyCode);

                #region Update Price Value to User's Currency Conversion

                if (model.Rate.HasValue)
                {
                    model.Rate = CurrencyManager.ParseAmountToUserCurrency(model.Rate.Value, LogInManager.CurrencyCode);
                }

                if (model.ApprovalAmount.HasValue)
                {
                    model.ApprovalAmount = CurrencyManager.ParseAmountToUserCurrency(model.ApprovalAmount.Value, LogInManager.CurrencyCode);
                }

                if (model.DiscountAmount.HasValue)
                {
                    model.DiscountAmount = CurrencyManager.ParseAmountToUserCurrency(model.DiscountAmount.Value, LogInManager.CurrencyCode, 0);
                }

                if (model.GuestBalance.HasValue)
                {
                    model.GuestBalance = CurrencyManager.ParseAmountToUserCurrency(model.GuestBalance.Value, LogInManager.CurrencyCode);
                }

                if (model.TotalPrice.HasValue)
                {
                    model.TotalPrice = CurrencyManager.ParseAmountToUserCurrency(model.TotalPrice.Value, LogInManager.CurrencyCode);
                }

                #endregion                

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationVM model)
        {
            try
            {

                #region "check selected room is already check in by another user or not"
                if (!string.IsNullOrWhiteSpace(model.RoomIds))
                {
                    string strETAText = "10:00";
                    if (!string.IsNullOrWhiteSpace(model.ETAText))
                    {
                        strETAText = model.ETAText;
                    }

                    var checkReservationOrNot = reservationRepository.GetPreviousReservationOrNot(Guid.Parse(model.RoomIds), model.ArrivalDate, TimeSpan.Parse(strETAText), model.Id, LogInManager.LoggedInUserId);
                    if (checkReservationOrNot != null && checkReservationOrNot.Count > 0)
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            IsReservation = true,
                            errorMessage = "Selected room already checked in by another user. please select check in (ETA) time after " + checkReservationOrNot.Select(m => m.DepartureDate != null ? m.DepartureDate.Value.ToString("dd MMM yyyy") : null).FirstOrDefault() + " " + checkReservationOrNot.Select(m => m.CheckOutTime != null ? m.CheckOutTime : null).FirstOrDefault() + ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion

                string reservationId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;
                model.IsActive = true;

                #region "convert ETA string to time"
                string ETAText = model.ETAText;
                if (!string.IsNullOrWhiteSpace(ETAText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + ETAText);
                    DateTime time = Convert.ToDateTime(date);

                    model.ETA = time.TimeOfDay;
                }
                #endregion

                #region "update reservation log details using reservation"
                var GetReservationLogDetail = reservationLogRepository.GetReservationLogByReservationId(model.Id, LogInManager.LoggedInUserId).FirstOrDefault();
                if (GetReservationLogDetail != null)
                {
                    ReservationLogVM reservationLog = new ReservationLogVM();
                    reservationLog.Id = GetReservationLogDetail.Id;
                    reservationLog.IsActive = GetReservationLogDetail.IsActive;
                    reservationLog.ReservationId = GetReservationLogDetail.ReservationId;
                    reservationLog.ProfileId = GetReservationLogDetail.ProfileId;
                    reservationLog.RoomId = model.RoomIds != null ? Guid.Parse(model.RoomIds) : GetReservationLogDetail.RoomId;
                    reservationLog.RoomStatusId = GetReservationLogDetail.RoomStatusId;
                    reservationLog.CheckInDate = model.ArrivalDate;
                    reservationLog.CheckInTime = model.ETA;
                    reservationLog.CheckOutDate = model.DepartureDate;
                    reservationLog.CheckOutTime = GetReservationLogDetail.CheckOutTime;
                    reservationLog.UpdatedBy = LogInManager.LoggedInUserId;
                    reservationLog.IsActive = GetReservationLogDetail.IsActive;

                    reservationLogRepository.UpdateReservationLog(reservationLog);
                }
                #endregion

                double totalBalance = 0, totalPrice = 0;

                totalPrice = Utility.Utility.CalculateRoomRentCharges(model.NoOfNight, (model.Rate.HasValue ? model.Rate.Value : 0), model.NoOfChildren, model.DiscountAmount, model.DiscountPercentage, (model.DiscountPercentage.HasValue ? true : false));

                totalBalance = totalPrice;

                //model.GuestBalance = totalBalance;                
                model.TotalPrice = totalPrice;

                #region Update Price Value to Euro Conversion

                if (model.Rate.HasValue)
                {
                    model.Rate = CurrencyManager.ConvertAmountToBaseCurrency(model.Rate.Value, LogInManager.CurrencyCode);
                }

                if (model.ApprovalAmount.HasValue)
                {
                    model.ApprovalAmount = CurrencyManager.ConvertAmountToBaseCurrency(model.ApprovalAmount.Value, LogInManager.CurrencyCode);
                }

                if (model.DiscountAmount.HasValue)
                {
                    model.DiscountAmount = CurrencyManager.ConvertAmountToBaseCurrency(model.DiscountAmount.Value, LogInManager.CurrencyCode);
                }

                if (model.TotalPrice.HasValue)
                {
                    model.TotalPrice = CurrencyManager.ConvertAmountToBaseCurrency(model.TotalPrice.Value, LogInManager.CurrencyCode);
                }

                #endregion


                //Encrypt Credit Card#.
                model.CreditCardNo = Utility.Utility.Encrypt(model.CreditCardNo, Utility.Utility.EncryptionKey);

                reservationId = reservationRepository.UpdateReservation(model);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    var source = string.Empty;
                    if (Request.Form["Source"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.Form["Source"])))
                    {
                        source = Convert.ToString(Request.Form["Source"]);
                    }

                    #region Save Reservation Room Mapping

                    var roomIds = model.RoomIds;

                    string[] roomIdsArr = null;
                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        roomIdsArr = roomIds.Split(',');
                        if (roomIdsArr != null)
                        {
                            //Remove Duplication.
                            roomIdsArr = roomIdsArr.Distinct().ToArray();
                        }
                    }


                    #region Delete Room Mapping

                    var roomMappings = roomRepository.GetReservationRoomMapping(model.Id, null, LogInManager.LoggedInUserId);

                    if (roomMappings != null && roomMappings.Count > 0)
                    {
                        List<Guid> roomMappingIds = new List<Guid>();

                        foreach (var roomMapping in roomMappings)
                        {
                            if (roomMapping.RoomId.HasValue)
                            {
                                if (roomIdsArr == null || !roomIdsArr.Contains(roomMapping.RoomId.Value.ToString()))
                                {
                                    roomMappingIds.Add(roomMapping.Id);
                                }
                            }
                        }

                        //Delete Room Mapping
                        if (roomMappingIds != null && roomMappingIds.Count > 0)
                        {
                            foreach (var mappingId in roomMappingIds)
                            {
                                roomRepository.DeleteReservationRoomMapping(mappingId, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }
                        }
                    }

                    #endregion

                    if (roomIdsArr != null)
                    {
                        foreach (var item in roomIdsArr)
                        {
                            //Save Reservation Room Mapping.
                            ReservationRoomMappingVM reservationRoomMapping = new ReservationRoomMappingVM();
                            reservationRoomMapping.RoomId = Guid.Parse(item.Trim());
                            reservationRoomMapping.ReservationId = Guid.Parse(reservationId);
                            reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                            reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);


                            #region Remove Existing reservation if room status are dirty.

                            var reservationLog = reservationLogRepository.GetReservationLogByRoom(Guid.Parse(item.Trim()), model.Id, Guid.Parse(RoomStatusType.DIRTY), model.ArrivalDate, model.DepartureDate, LogInManager.LoggedInUserId).FirstOrDefault();

                            if (reservationLog != null)
                            {
                                //Delete Reservation.
                                reservationRepository.DeleteReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Room Mapping.
                                roomRepository.DeleteReservationRoomMappingByReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Log.
                                reservationLogRepository.DeleteReservationLog(reservationLog.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }

                            #endregion Remove Existing reservation if room status are dirty.

                        }
                    }

                    #endregion

                    #region Save Reservation Preference Mapping

                    var preferenceItems = model.PreferenceItems;

                    string[] preferenceItemsArr = null;

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                    {
                        preferenceItemsArr = preferenceItems.Split(',');

                        if (preferenceItemsArr != null)
                        {
                            //Remove Duplication.
                            preferenceItemsArr = preferenceItemsArr.Distinct().ToArray();
                        }
                    }


                    #region Delete Preference Mapping

                    var preferenceMappings = preferenceRepository.GetReservationPreferenceMapping(model.Id, null, LogInManager.LoggedInUserId);

                    if (preferenceMappings != null && preferenceMappings.Count > 0)
                    {
                        List<Guid> preferenceMappingIds = new List<Guid>();

                        foreach (var preferenceMapping in preferenceMappings)
                        {
                            if (preferenceMapping.PreferenceId.HasValue)
                            {
                                if (preferenceItemsArr == null || !preferenceItemsArr.Contains(preferenceMapping.PreferenceId.Value.ToString()))
                                {
                                    preferenceMappingIds.Add(preferenceMapping.Id);
                                }
                            }
                        }

                        //Delete Preference Mapping
                        if (preferenceMappingIds != null && preferenceMappingIds.Count > 0)
                        {
                            foreach (var mappingId in preferenceMappingIds)
                            {
                                preferenceRepository.DeleteReservationPreferenceMapping(mappingId, LogInManager.LoggedInUserId);
                            }
                        }
                    }

                    #endregion

                    if (preferenceItemsArr != null)
                    {
                        foreach (var item in preferenceItemsArr)
                        {
                            //Save Reservation Preference Mapping.
                            ReservationPreferenceMappingVM reservationPreferenceMapping = new ReservationPreferenceMappingVM();
                            reservationPreferenceMapping.PreferenceId = Guid.Parse(item);
                            reservationPreferenceMapping.ReservationId = Guid.Parse(reservationId);
                            reservationPreferenceMapping.CreatedBy = LogInManager.LoggedInUserId;

                            preferenceRepository.AddReservationPreferenceMapping(reservationPreferenceMapping);
                        }
                    }

                    #endregion

                    #region Save Reservation Add Ons Mapping      

                    #region Delete Add Ons Mapping

                    var addOnsMappings = reservationRepository.GetReservationAddOnsMapping(Guid.Parse(reservationId), null, LogInManager.LoggedInUserId);

                    if (addOnsMappings != null && addOnsMappings.Count > 0)
                    {
                        List<Guid> addOnsMappingIds = new List<Guid>();

                        foreach (var addOnsMapping in addOnsMappings)
                        {
                            if (addOnsMapping.AddOnsId.HasValue)
                            {
                                if (model.AddOnsList == null || !model.AddOnsList.Select(m => m.Id).Contains(addOnsMapping.AddOnsId.Value))
                                {
                                    addOnsMappingIds.Add(addOnsMapping.Id);
                                }
                            }
                        }

                        //Delete AddOns Mapping
                        if (addOnsMappingIds != null && addOnsMappingIds.Count > 0)
                        {
                            foreach (var id in addOnsMappingIds)
                            {
                                reservationRepository.DeleteReservationAddOnsMapping(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }
                        }
                    }
                    #endregion        

                    if (model.AddOnsList != null && model.AddOnsList.Count > 0)
                    {
                        foreach (var addOns in model.AddOnsList)
                        {
                            var addOnsDetail = addOnsRepository.GetAddOnsById(addOns.Id).FirstOrDefault();

                            if (addOnsDetail != null)
                            {
                                //Save Reservation Add Ons Mapping.
                                ReservationAddOnsMappingVM reservationAddOnsMapping = new ReservationAddOnsMappingVM();
                                reservationAddOnsMapping.AddOnsId = addOnsDetail.Id;
                                reservationAddOnsMapping.ReservationId = Guid.Parse(reservationId);
                                reservationAddOnsMapping.CreatedBy = LogInManager.LoggedInUserId;
                                reservationAddOnsMapping.UpdatedBy = LogInManager.LoggedInUserId;

                                var reservationAddOnsMappingId = reservationRepository.AddUpdateReservationAddOnsMapping(reservationAddOnsMapping);
                            }
                        }
                    }
                    #endregion

                    #region Save Reservation Package Mapping     

                    if (model.PackageMappingList != null && model.PackageMappingList.Count > 0)
                    {
                        #region Delete Package Mapping

                        var packageMappings = reservationRepository.GetReservationPackageMapping(Guid.Parse(reservationId), null, LogInManager.LoggedInUserId);

                        if (packageMappings != null && packageMappings.Count > 0)
                        {
                            List<Guid> pacakgeMappingIds = new List<Guid>();

                            foreach (var packageMapping in packageMappings)
                            {
                                if (packageMapping.PackageId.HasValue)
                                {
                                    if (model.PackageMappingList == null || !model.PackageMappingList.Select(m => m.PackageId).Contains(packageMapping.PackageId.Value))
                                    {
                                        pacakgeMappingIds.Add(packageMapping.Id);
                                    }
                                }
                            }

                            //Delete Package Mapping
                            if (pacakgeMappingIds != null && pacakgeMappingIds.Count > 0)
                            {
                                foreach (var id in pacakgeMappingIds)
                                {
                                    reservationRepository.DeleteReservationPackageMapping(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                                }
                            }
                        }
                        #endregion

                        foreach (var reservationPackageMapping in model.PackageMappingList)
                        {
                            if (reservationPackageMapping != null)
                            {
                                var packageDetail = packageRepository.GetPackageById(reservationPackageMapping.PackageId.Value).FirstOrDefault();
                                if (packageDetail != null)
                                {
                                    reservationPackageMapping.PackagePrice = packageDetail.Price;
                                }

                                reservationPackageMapping.TotalAmount = CurrencyManager.ConvertAmountToBaseCurrency(reservationPackageMapping.TotalAmount, LogInManager.CurrencyCode);

                                reservationPackageMapping.ReservationId = Guid.Parse(reservationId);
                                reservationPackageMapping.CreatedBy = LogInManager.LoggedInUserId;
                                reservationPackageMapping.UpdatedBy = LogInManager.LoggedInUserId;

                                reservationRepository.AddUpdateReservationPackageMapping(reservationPackageMapping);
                            }
                        }

                        //Nilesh: 23-JAN-2019 - Due to functionality change now user can able to select multiple packages - START
                        //var reservationPackageMapping = model.PackageMappingList[0];

                        //if (reservationPackageMapping != null)
                        //{
                        //    var packageDetail = packageRepository.GetPackageById(reservationPackageMapping.PackageId.Value).FirstOrDefault();

                        //    if (packageDetail != null)
                        //    {
                        //        reservationPackageMapping.PackagePrice = packageDetail.Price;
                        //    }

                        //    reservationPackageMapping.TotalAmount = CurrencyManager.ConvertAmountToBaseCurrency(reservationPackageMapping.TotalAmount, LogInManager.CurrencyCode);

                        //    //Save Reservation Package Mapping.
                        //    reservationPackageMapping.ReservationId = Guid.Parse(reservationId);
                        //    reservationPackageMapping.CreatedBy = LogInManager.LoggedInUserId;
                        //    reservationPackageMapping.UpdatedBy = LogInManager.LoggedInUserId;

                        //    reservationRepository.AddUpdateReservationPackageMapping(reservationPackageMapping);
                        //}
                        //Nilesh: 23-JAN-2019 - Due to functionality change now user can able to select multiple packages - END
                    }
                    else
                    {
                        var packageDetails = reservationRepository.GetReservationPackageMapping(Guid.Parse(reservationId), null, LogInManager.LoggedInUserId);

                        //If Package mapping exist then delete.
                        if (packageDetails != null && packageDetails.Count > 0)
                        {
                            reservationRepository.DeleteReservationPackageMappingByReservation(Guid.Parse(reservationId), LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                        }
                    }

                    #endregion

                    #region Update Reservation Total Price

                    totalPrice = Utility.Utility.CalculateReservationTotalPrice(Guid.Parse(reservationId));

                    //Update Total Price.
                    reservationRepository.UpdateReservationTotalPrice(Guid.Parse(reservationId), totalPrice, LogInManager.LoggedInUserId);

                    #endregion

                    #region Update Room Rent Charges 

                    var roomRentCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.ROOM_RENT).FirstOrDefault();

                    var reservationCharge = reservationChargeRepository.GetReservationCharges(Guid.Parse(reservationId), roomRentCharge.Id, LogInManager.LoggedInUserId).FirstOrDefault();

                    if (reservationCharge != null)
                    {
                        reservationCharge.ReservationId = Guid.Parse(reservationId);
                        reservationCharge.AdditionalChargeId = roomRentCharge.Id;
                        reservationCharge.AdditionalChargeSource = AdditionalChargeSource.ADDITIONAL_CHARGE;
                        reservationCharge.Code = roomRentCharge.Code;
                        reservationCharge.Description = roomRentCharge.Description;
                        reservationCharge.TransactionDate = model.ArrivalDate;
                        reservationCharge.Amount = totalPrice;
                        reservationCharge.Qty = 1;
                        reservationCharge.IsActive = true;
                        reservationCharge.UpdatedBy = LogInManager.LoggedInUserId;

                        reservationChargeRepository.UpdateReservationCharges(reservationCharge);
                    }

                    #endregion

                    #region Update Reservation Total Balance.

                    if (model.IsCheckOut == false)
                    {
                        double totalGuestBalance = Utility.Utility.CalculateTotalBalance(Guid.Parse(reservationId));

                        //Update Total Balance.
                        reservationRepository.UpdateReservationTotalBalance(Guid.Parse(reservationId), totalGuestBalance, LogInManager.LoggedInUserId);
                    }

                    #endregion

                    #region Add Reservation Remarks 

                    if (!string.IsNullOrWhiteSpace(model.Remarks))
                    {
                        ReservationRemarkVM remark = new ReservationRemarkVM();
                        remark.ReservationId = Guid.Parse(reservationId);
                        remark.Remarks = model.Remarks;
                        remark.CreatedBy = LogInManager.LoggedInUserId;
                        if (!remark.CreatedOn.HasValue)
                        {
                            remark.CreatedOn = DateTime.Now;
                        }

                        reservationRepository.AddReservationRemark(remark);
                    }

                    #endregion

                    #region Update TrackReservationLog Status

                    reservationRepository.UpdateTrackReservationLogStatusByReservationId(Guid.Parse(reservationId), true);

                    #endregion

                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.RESERVATION, string.Format("Updated reservation, Confirmation# : {0}", model.ConfirmationNumber));
                    #endregion

                    //Clear Session Object.
                    Session["RateQueryVM"] = null;

                    #region  Check Source Parameters
                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        string url = string.Empty;
                        string qid = string.Empty;

                        if (source == "RoomPlan")
                        {
                            url = Url.Action("RoomPlan", "Reservation");
                        }
                        else if (source == "SearchArrivals")
                        {
                            url = Url.Action("Arrivals", "FrontDesk");
                        }
                        else if (source == "RegistrationCard")
                        {
                            url = Url.Action("RegistrationCard", "Report");
                        }
                        else if (source == "BreakfastReport")
                        {
                            url = Url.Action("Breakfast", "Report");
                        }

                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            return Json(new
                            {
                                IsSuccess = true,
                                IsExternalUrl = true,
                                data = new
                                {
                                    Url = url,
                                    ReservationId = model.Id
                                }
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion

                    #region Send Email 

                    if (model.IsEmailConfirmation)
                    {
                        if (model.ProfileId.HasValue)
                        {
                            var profile = profileRepository.GetIndividualProfileById(model.ProfileId.Value, LogInManager.LoggedInUserId).FirstOrDefault();

                            double rate = 0;
                            string currencyCode = string.Empty, currencySymbol = string.Empty;

                            if (profile != null)
                            {
                                var userDetail = userRepository.GetUserDetailByUserId(LogInManager.LoggedInUserId).FirstOrDefault();
                                var userGroupDetail = new UserGroupVM();

                                if (userDetail != null)
                                {
                                    var collegeGroup = collegeGroupRepository.GetCollegeGroupById(userDetail.CollegeGroupId.Value);

                                    if (collegeGroup != null)
                                    {
                                        userGroupDetail = userGroupRepository.GetUserGroupById(collegeGroup.UserGroupId.Value);
                                    }
                                }

                                if (userGroupDetail != null && userGroupDetail.Id != null)
                                {
                                    var currencyDetail = currencyRepository.GetCurrencyInfoById(userGroupDetail.CurrencyId).FirstOrDefault();

                                    if (currencyDetail != null)
                                    {
                                        currencyCode = currencyDetail.Code;
                                        currencySymbol = currencyDetail.CurrencySymbol;
                                    }
                                }

                                var profileName = (profile.LastName + ' ' + profile.FirstName);
                                var email = profile.Email;

                                if (model.Rate.HasValue)
                                {
                                    rate = CurrencyManager.ParseAmountToUserCurrency(model.Rate.Value, currencyCode);
                                }

                                if (!string.IsNullOrWhiteSpace(email))
                                {
                                    ReservationConfirmationReportVM obj = new ReservationConfirmationReportVM();
                                    obj.UserName = profile.FirstName;
                                    obj.GuestName = profileName;
                                    obj.ConfirmationNumber = model.ConfirmationNumber;
                                    obj.ArrivalDate = model.ArrivalDate.Value.ToString("dd MMM yyyy");
                                    obj.DepartureDate = model.DepartureDate.Value.ToString("dd MMM yyyy");

                                    obj.NoOfNight = model.NoOfNight;
                                    obj.NoOfAdult = model.NoOfAdult == null ? 0 : model.NoOfAdult;
                                    obj.NoOfChildren = model.NoOfChildren == null ? 0 : model.NoOfChildren;
                                    obj.CashierName = LogInManager.UserName;

                                    obj.Rate = rate;

                                    obj.RatePerNight = Utility.Utility.FormatAmountWithTwoDecimal(rate);

                                    //Method Of Payment.
                                    if (model.PaymentMethodId.HasValue)
                                    {
                                        var paymentMethod = paymentMethodRepository.GetPaymentMethodById(model.PaymentMethodId.Value).FirstOrDefault();

                                        if (paymentMethod != null)
                                        {
                                            obj.MethodOfPayment = paymentMethod.Name;
                                        }
                                        else
                                        {
                                            obj.MethodOfPayment = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        obj.MethodOfPayment = string.Empty;
                                    }

                                    //Accommodation
                                    if (model.RoomTypeId.HasValue)
                                    {
                                        var roomType = roomTypeRepository.GetRoomTypeById(model.RoomTypeId.Value).FirstOrDefault();

                                        if (roomType != null)
                                        {
                                            obj.Accommodation = (Convert.ToString(model.NoOfRoom) + " " + roomType.Description);
                                        }
                                        else
                                        {
                                            obj.Accommodation = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        obj.Accommodation = string.Empty;
                                    }

                                    if (userDetail != null)
                                    {
                                        //Package
                                        var reservationPackageMapping = reservationRepository.GetReservationPackageMapping(model.Id, null, userDetail.UserId);

                                        if (reservationPackageMapping != null && reservationPackageMapping.Count > 0)
                                        {
                                            obj.IsBreakFast = reservationPackageMapping.Where(i => i.PackageName == "Full Irish Breakfast").Any();
                                        }
                                    }

                                    //obj.DepositPaid = "EUR";
                                    obj.DepositPaid = currencyCode;
                                    obj.CurrencySymbol = currencySymbol;

                                    //HTML generation.
                                    string html = Utility.Utility.RenderPartialViewToString((Controller)this, "ReservationConfirmation", obj);

                                    //HTML to PDF.
                                    byte[] pdfBytes = Utility.Utility.GetPDF(html);

                                    StringBuilder bodyMsg = new StringBuilder();
                                    using (var sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlTemplates/ReservationConfirmationEmail.html")))
                                    {
                                        bodyMsg.Append(sr.ReadToEnd());

                                        bodyMsg.Replace("[@UserName]", obj.UserName);
                                        bodyMsg.Replace("[@CashierName]", LogInManager.UserName);

                                        //File Name.
                                        string fileName = string.Format("Confirm-Of-Changes-Of-Reservation-{0}.pdf", model.ConfirmationNumber);

                                        //Send Email.                                        
                                        string EmailSubject = string.Format("Confirmation of changes in your reservation at Roche International Hotel and Spa. {0} ", model.ConfirmationNumber);

                                        bool blnMailSend = SuccessHotelierHub.Utility.Email.sendMail(email, EmailSubject, Convert.ToString(bodyMsg), fileName, true, pdfBytes);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Edit");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string reservationId = string.Empty;

                //Delete Reservation.
                reservationId = reservationRepository.DeleteReservation(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.RESERVATION, "Deleted reservation");
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult List()
        {
            var reservationCancellationReasonList = reservationCancellationReasonRepository.GetReservationCancellationReasons();

            ViewBag.ReservationCancellationReasonList = reservationCancellationReasonList;

            var companyList = new SelectList(CompanyRepository.GetCompanyList(), "Id", "CompanyName").ToList();
            ViewBag.CompanyList = companyList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult SearchReservation(SearchReservationParametersVM model)
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

                var reservations = reservationRepository.SearchReservation(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.RESERVATION, "Searched reservation");
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
                Utility.Utility.LogError(e, "SearchReservation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult CancelReservation(Guid id, Guid cancellationReasonId, string comment)
        {
            try
            {
                string reservationId = string.Empty;

                //Cancel Reservation.
                reservationId = reservationRepository.CancelReservation(id, cancellationReasonId, comment, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.RESERVATION, string.Format("Cancelled reservation due to {0}", comment));
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation not cancelled successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "CancelReservation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public string CreateReservation(ReservationVM model)
        {
            string reservationId = string.Empty;
            string confirmationNo = string.Empty;
            long folioNo = 0;

            model.CreatedBy = LogInManager.LoggedInUserId;
            model.IsActive = true;

            string ETAText = model.ETAText;
            if (!string.IsNullOrWhiteSpace(ETAText))
            {
                string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                string date = (todayDate + " " + ETAText);
                DateTime time = Convert.ToDateTime(date);

                model.ETA = time.TimeOfDay;
            }


            var lastReservation = reservationRepository.GetLastReservationByDate(null);

            #region Generate Confirmation No            
            Int64 confirmationSuffix = 1;

            if (lastReservation != null)
            {
                string lastConfirmationNo = lastReservation.ConfirmationNumber;
                if (!string.IsNullOrWhiteSpace(lastConfirmationNo))
                {
                    confirmationSuffix = !string.IsNullOrWhiteSpace(lastConfirmationNo) ? (Convert.ToInt64(lastConfirmationNo) + 1) : Constants.DEFAULT_CONFIRMATION_NO;

                    confirmationNo = confirmationSuffix.ToString();
                }
                else
                {
                    confirmationNo = Constants.DEFAULT_CONFIRMATION_NO.ToString();
                }
            }
            else
            {
                //Default confirmation no.
                confirmationNo = Constants.DEFAULT_CONFIRMATION_NO.ToString();
            }

            model.ConfirmationNumber = confirmationNo;

            #endregion

            #region Generate Folio Number
            Int64 folioSuffix = 1;

            if (lastReservation != null)
            {
                long lastFolioNumber = lastReservation.FolioNumber;
                if (lastFolioNumber > 0)
                {
                    folioSuffix = Convert.ToInt64(lastFolioNumber) + 1;

                    folioNo = folioSuffix;
                }
                else
                {
                    folioNo = Constants.DEFAULT_FOLIO_NUMBER;
                }
            }
            else
            {
                //Default folio no.
                folioNo = Constants.DEFAULT_FOLIO_NUMBER;
            }

            model.FolioNumber = folioNo;

            #endregion

            double totalBalance = 0, totalPrice = 0;

            totalPrice = Utility.Utility.CalculateRoomRentCharges(model.NoOfNight, (model.Rate.HasValue ? model.Rate.Value : 0), model.NoOfChildren, model.DiscountAmount, model.DiscountPercentage, (model.DiscountPercentage.HasValue ? true : false));

            totalBalance = totalPrice;

            model.GuestBalance = totalBalance;
            model.TotalPrice = totalPrice;

            #region Update Price Value to Euro Conversion

            if (model.Rate.HasValue)
            {
                model.Rate = CurrencyManager.ConvertAmountToBaseCurrency(model.Rate.Value, LogInManager.CurrencyCode);
            }

            if (model.ApprovalAmount.HasValue)
            {
                model.ApprovalAmount = CurrencyManager.ConvertAmountToBaseCurrency(model.ApprovalAmount.Value, LogInManager.CurrencyCode);
            }

            if (model.DiscountAmount.HasValue)
            {
                model.DiscountAmount = CurrencyManager.ConvertAmountToBaseCurrency(model.DiscountAmount.Value, LogInManager.CurrencyCode);
            }

            if (model.GuestBalance.HasValue)
            {
                model.GuestBalance = CurrencyManager.ConvertAmountToBaseCurrency(model.GuestBalance.Value, LogInManager.CurrencyCode);
            }

            if (model.TotalPrice.HasValue)
            {
                model.TotalPrice = CurrencyManager.ConvertAmountToBaseCurrency(model.TotalPrice.Value, LogInManager.CurrencyCode);
            }

            #endregion

            //Encrypt Credit Card #.
            model.CreditCardNo = Utility.Utility.Encrypt(model.CreditCardNo, Utility.Utility.EncryptionKey);

            reservationId = reservationRepository.AddReservation(model);

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                model.Id = Guid.Parse(reservationId);

                #region Save Reservation Room Mapping
                var roomIds = model.RoomIds;

                if (!string.IsNullOrWhiteSpace(roomIds))
                {
                    var roomIdsArr = roomIds.Split(',');

                    if (roomIdsArr != null)
                    {
                        //Remove Duplication.
                        roomIdsArr = roomIdsArr.Distinct().ToArray();

                        foreach (var item in roomIdsArr)
                        {
                            //Save Reservation Room Mapping.
                            ReservationRoomMappingVM reservationRoomMapping = new ReservationRoomMappingVM();
                            reservationRoomMapping.RoomId = Guid.Parse(item.Trim());
                            reservationRoomMapping.ReservationId = model.Id;
                            reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                            reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);


                            #region Remove Existing reservation if room status are dirty.

                            var reservationLog = reservationLogRepository.GetReservationLogByRoom(Guid.Parse(item.Trim()), model.Id, Guid.Parse(RoomStatusType.DIRTY), model.ArrivalDate, model.DepartureDate, LogInManager.LoggedInUserId).FirstOrDefault();

                            if (reservationLog != null)
                            {
                                //Delete Reservation.
                                reservationRepository.DeleteReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Room Mapping.
                                roomRepository.DeleteReservationRoomMappingByReservation(reservationLog.ReservationId.Value, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                                //Delete Reservation Log.
                                reservationLogRepository.DeleteReservationLog(reservationLog.Id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);
                            }

                            #endregion Remove Existing reservation if room status are dirty.
                        }
                    }
                }
                #endregion

                #region Save Reservation Preference Mapping
                var preferenceItems = model.PreferenceItems;

                if (!string.IsNullOrWhiteSpace(preferenceItems))
                {
                    var preferenceItemsArr = preferenceItems.Split(',');

                    if (preferenceItemsArr != null)
                    {
                        //Remove Duplication.
                        preferenceItemsArr = preferenceItemsArr.Distinct().ToArray();

                        foreach (var item in preferenceItemsArr)
                        {
                            //Save Reservation Preference Mapping.
                            ReservationPreferenceMappingVM reservationPreferenceMapping = new ReservationPreferenceMappingVM();
                            reservationPreferenceMapping.PreferenceId = Guid.Parse(item);
                            reservationPreferenceMapping.ReservationId = model.Id;
                            reservationPreferenceMapping.CreatedBy = LogInManager.LoggedInUserId;

                            preferenceRepository.AddReservationPreferenceMapping(reservationPreferenceMapping);
                        }
                    }
                }
                #endregion

                #region Save Reservation Add Ons Mapping               

                if (model.AddOnsList != null && model.AddOnsList.Count > 0)
                {
                    foreach (var addOns in model.AddOnsList)
                    {
                        //Save Reservation AddOns Mapping.
                        ReservationAddOnsMappingVM reservationAddOnsMapping = new ReservationAddOnsMappingVM();
                        reservationAddOnsMapping.AddOnsId = addOns.Id;
                        reservationAddOnsMapping.ReservationId = model.Id;
                        reservationAddOnsMapping.CreatedBy = LogInManager.LoggedInUserId;
                        reservationAddOnsMapping.UpdatedBy = LogInManager.LoggedInUserId;

                        reservationRepository.AddUpdateReservationAddOnsMapping(reservationAddOnsMapping);
                    }
                }
                #endregion

                #region Save Reservation Package Mapping               
                
                if (model.PackageMappingList != null && model.PackageMappingList.Count > 0)
                {
                    foreach(var packageMapping in model.PackageMappingList)
                    {
                        if (packageMapping != null)
                        {
                            var packageDetail = packageRepository.GetPackageById(packageMapping.PackageId.Value).FirstOrDefault();
                            if (packageDetail != null)
                            {
                                packageMapping.PackagePrice = packageDetail.Price;
                            }

                            packageMapping.TotalAmount = CurrencyManager.ConvertAmountToBaseCurrency(packageMapping.TotalAmount, LogInManager.CurrencyCode);

                            packageMapping.ReservationId = model.Id;
                            packageMapping.CreatedBy = LogInManager.LoggedInUserId;
                            packageMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            reservationRepository.AddUpdateReservationPackageMapping(packageMapping);
                        }
                    }

                    //Nilesh: 23-JAN-2019 - Due to functionality change now user can able to select multiple packages - START
                    //var packageMapping = model.PackageMappingList[0];

                    //if (packageMapping != null)
                    //{
                    //    var packageDetail = packageRepository.GetPackageById(packageMapping.PackageId.Value).FirstOrDefault();
                    //    if (packageDetail != null)
                    //    {
                    //        packageMapping.PackagePrice = packageDetail.Price;
                    //    }

                    //    packageMapping.TotalAmount = CurrencyManager.ConvertAmountToBaseCurrency(packageMapping.TotalAmount, LogInManager.CurrencyCode);

                    //    packageMapping.ReservationId = model.Id;
                    //    packageMapping.CreatedBy = LogInManager.LoggedInUserId;
                    //    packageMapping.UpdatedBy = LogInManager.LoggedInUserId;

                    //    reservationRepository.AddUpdateReservationPackageMapping(packageMapping);
                    //}
                    //Nilesh: 23-JAN-2019 - Due to functionality change now user can able to select multiple packages - END
                }
                #endregion

                #region Reservation Remarks 

                if (model.RemarksList != null && model.RemarksList.Count > 0)
                {
                    foreach (var remark in model.RemarksList)
                    {
                        remark.ReservationId = model.Id;
                        remark.CreatedBy = LogInManager.LoggedInUserId;
                        if (!remark.CreatedOn.HasValue)
                        {
                            remark.CreatedOn = DateTime.Now;
                        }

                        reservationRepository.AddReservationRemark(remark);
                    }
                }

                #endregion

                #region Update Reservation Total Price

                totalPrice = Utility.Utility.CalculateReservationTotalPrice(model.Id);

                //Update Total Price.
                reservationRepository.UpdateReservationTotalPrice(model.Id, totalPrice, LogInManager.LoggedInUserId);

                #endregion

                #region Update Reservation Total Balance.

                double totalGuestBalance = Utility.Utility.CalculateTotalBalance(model.Id);

                //Update Total Balance.
                reservationRepository.UpdateReservationTotalBalance(model.Id, totalGuestBalance, LogInManager.LoggedInUserId);

                #endregion

                #region Update TrackReservationLog Status

                reservationRepository.UpdateTrackReservationLogStatusByReservationId(model.Id, true);

                #endregion

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.RESERVATION, string.Format("Created new reservation, Confirmation# : {0}", model.ConfirmationNumber));
                #endregion

            }

            return confirmationNo;
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult DeleteAll()
        {
            try
            {
                //Delete all Reservation.
                reservationRepository.DeleteAllReservation(LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                return Json(new
                {
                    IsSuccess = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteAll");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT,TUTOR")]
        public ActionResult ReservationConfirmationReport(Guid? Id)
        {
            try
            {
                if (Id == null)
                {
                    return HttpNotFound();
                }

                var reservation = reservationRepository.GetReservationById(Id.Value, null).FirstOrDefault();

                ReservationConfirmationReportVM model = new ReservationConfirmationReportVM();

                string confirmationNo = string.Empty;
                double rate = 0;
                string currencyCode = string.Empty, currencySymbol = string.Empty;
                string cashierNumber = string.Empty;

                if (reservation != null)
                {
                    if (reservation.ProfileId.HasValue)
                    {
                        var profile = profileRepository.GetIndividualProfileById(reservation.ProfileId.Value, null).FirstOrDefault();

                        var userDetail = userRepository.GetUserDetailByUserId(reservation.CreatedBy.Value).FirstOrDefault();
                        var userGroupDetail = new UserGroupVM();

                        if (userDetail != null)
                        {
                            var collegeGroup = collegeGroupRepository.GetCollegeGroupById(userDetail.CollegeGroupId.Value);

                            if (collegeGroup != null)
                            {
                                userGroupDetail = userGroupRepository.GetUserGroupById(collegeGroup.UserGroupId.Value);
                            }
                            cashierNumber = userDetail.CashierNumber;
                        }

                        if (userGroupDetail != null && userGroupDetail.Id != null)
                        {
                            var currencyDetail = currencyRepository.GetCurrencyInfoById(userGroupDetail.CurrencyId).FirstOrDefault();

                            if (currencyDetail != null)
                            {
                                currencyCode = currencyDetail.Code;
                                currencySymbol = currencyDetail.CurrencySymbol;
                            }
                        }

                        if (profile != null)
                        {
                            var profileName = (profile.LastName + ' ' + profile.FirstName);
                            var email = profile.Email;

                            if (reservation.Rate.HasValue)
                            {
                                rate = CurrencyManager.ParseAmountToUserCurrency(reservation.Rate.Value, currencyCode);
                            }

                            model.UserName = profile.FirstName;
                            model.GuestName = profileName;
                            model.ConfirmationNumber = reservation.ConfirmationNumber;
                            model.ArrivalDate = reservation.ArrivalDate.Value.ToString("dd MMM yyyy");
                            model.DepartureDate = reservation.DepartureDate.Value.ToString("dd MMM yyyy");

                            model.NoOfNight = reservation.NoOfNight;
                            model.NoOfAdult = reservation.NoOfAdult.HasValue ? reservation.NoOfAdult.Value : 0;
                            model.NoOfChildren = reservation.NoOfChildren.HasValue ? reservation.NoOfChildren.Value : 0;
                            model.Rate = rate;

                            model.CashierName = cashierNumber;

                            model.RatePerNight = Utility.Utility.FormatAmountWithTwoDecimal(rate);

                            //Method Of Payment.
                            if (reservation.PaymentMethodId.HasValue)
                            {
                                var paymentMethod = paymentMethodRepository.GetPaymentMethodById(reservation.PaymentMethodId.Value).FirstOrDefault();

                                if (paymentMethod != null)
                                {
                                    model.MethodOfPayment = paymentMethod.Name;
                                }
                                else
                                {
                                    model.MethodOfPayment = string.Empty;
                                }
                            }
                            else
                            {
                                model.MethodOfPayment = string.Empty;
                            }

                            //Accommodation
                            if (reservation.RoomTypeId.HasValue)
                            {
                                var roomType = roomTypeRepository.GetRoomTypeById(reservation.RoomTypeId.Value).FirstOrDefault();

                                if (roomType != null)
                                {
                                    model.Accommodation = (Convert.ToString(reservation.NoOfRoom) + " " + roomType.Description);
                                }
                                else
                                {
                                    model.Accommodation = string.Empty;
                                }
                            }
                            else
                            {
                                model.Accommodation = string.Empty;
                            }


                            if(userDetail != null)
                            {
                                //Package
                                var reservationPackageMapping = reservationRepository.GetReservationPackageMapping(reservation.Id, null, userDetail.UserId);

                                if(reservationPackageMapping != null && reservationPackageMapping.Count > 0)
                                {
                                    model.IsBreakFast = reservationPackageMapping.Where(i => i.PackageName == "Full Irish Breakfast").Any();
                                }
                            }
                            

                            model.DepositPaid = currencyCode;
                            model.CurrencySymbol = currencySymbol;

                        }
                    }
                }

                //HTML generation.
                string html = Utility.Utility.RenderPartialViewToString((Controller)this, "ReservationConfirmation", model);

                //HTML to PDF.
                byte[] pdfBytes = Utility.Utility.GetPDF(html);

                //return File(pdfBytes, "application/pdf", string.Format("bill_{0}.pdf", model.Id));
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ReservationConfirmationReport");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult GetPriceDetails(Guid roomTypeId, Guid rateTypeId)
        {
            try
            {
                var weekDayPrice = rateRepository.GetWeekDayPrice(roomTypeId, rateTypeId).FirstOrDefault();

                var weekEndPrice = rateRepository.GetWeekEndPrice(roomTypeId, rateTypeId).FirstOrDefault();

                if (weekDayPrice != null || weekEndPrice != null)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            WeekDayPrice = (weekDayPrice != null ? CurrencyManager.ParseAmountToUserCurrency(weekDayPrice.Amount, LogInManager.CurrencyCode) : 0),
                            WeekEndPrice = (weekEndPrice != null ? CurrencyManager.ParseAmountToUserCurrency(weekEndPrice.Amount, LogInManager.CurrencyCode) : 0)
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Price details not found."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetPriceDetails");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Rate Query

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult RateQuery()
        {
            var profileId = (string)TempData["ProfileId"];
            var firstName = (string)TempData["FirstName"];
            var lastName = (string)TempData["LastName"];

            var rateSheetRoomTypeList = roomTypeRepository.GetRoomTypeDetailsForRateSheet(string.Empty, DateTime.Now.ToString("MM/dd/yyyy"), LogInManager.LoggedInUserId);

            var rateTypeList = rateTypeRepository.GetRateType(string.Empty);
            var packageList = packageRepository.GetPackages();
            //var packageList = new SelectList(
            //     packageRepository.GetPackages()
            //     .Select(
            //         m => new SelectListItem()
            //         {
            //             Value = m.Id.ToString(),
            //             Text = (m.Name + (!string.IsNullOrWhiteSpace(m.Description) ? " - " + m.Description : ""))
            //         }
            //     ), "Value", "Text").ToList();

            var companyList = new SelectList(CompanyRepository.GetCompanyList(), "Id", "CompanyName").ToList();
            ViewBag.CompanyList = companyList;

            ViewBag.RateTypeList = rateTypeList;
            ViewBag.RateSheetRoomTypeList = rateSheetRoomTypeList;
            ViewBag.PackageList = packageList;

            RateQueryVM model = new RateQueryVM();

            if (!string.IsNullOrWhiteSpace(profileId))
            {
                model.ProfileId = Guid.Parse(profileId);
            }

            model.FirstName = firstName;
            model.LastName = lastName;

            if (!string.IsNullOrWhiteSpace(lastName) || !string.IsNullOrWhiteSpace(firstName))
            {
                model.Name = (lastName + " " + firstName);
            }

            //Default Values.
            model.NoOfAdult = 1;
            model.NoOfNight = 1;
            model.NoOfRoom = 1;

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.RATEQUERY, "Goes to rate query page.");
            #endregion

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult ViewRateSheet(RateQueryVM model)
        {
            try
            {
                bool blnShowWeekEndPrice = false;

                if (model.ArrivalDate.HasValue)
                {
                    if (model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Friday || model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        blnShowWeekEndPrice = true;
                    }
                }

                var rateSheetRoomTypeList = roomTypeRepository.GetRoomTypeDetailsForRateSheet(string.Empty, model.ArrivalDate.Value.ToString("MM/dd/yyyy"), LogInManager.LoggedInUserId);
                var rateTypeList = rateTypeRepository.GetRateType(model.RateTypeCode);

                ViewData["RateType"] = rateTypeList;
                ViewData["RateSheetRoomType"] = rateSheetRoomTypeList;
                ViewData["IsShowWeekEndPrice"] = blnShowWeekEndPrice;

                if (model.CompanyId != null && model.CompanyId.HasValue)
                {
                    ViewData["ShowCorporateRate"] = true;
                }
                else
                {
                    ViewData["ShowCorporateRate"] = false;
                }


                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.RATEQUERY, "Searched rate sheet matrix.");
                #endregion

                return PartialView("_RateSheet");
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ViewRateSheet");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult ShowReservation(RateQueryVM model)
        {
            try
            {
                Session["RateQueryVM"] = model;

                return Json(new
                {
                    IsSuccess = true,
                    data = Url.Action("Create", "Reservation")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ShowReservation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Room Plan

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult RoomPlan()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.FloorList = floorList;

            #region Record Activity Log
            RecordActivityLog.RecordActivity(Pages.ROOMPLAN, "Goes to room plan");
            #endregion

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult SearchRoomPlan(RoomPlanVM model)
        {
            try
            {
                string startDate = "", endDate = "";
                string firstDate = "", lastDate = "", nextDate = "", prevDate = "";
                List<RoomPlanDateRangeVM> dates = new List<RoomPlanDateRangeVM>();

                if (model.StartDate.HasValue)
                {

                    DateTime dtEndDate = model.StartDate.Value.AddDays(6);

                    startDate = model.StartDate.Value.ToString("MM/dd/yyyy");
                    endDate = dtEndDate.ToString("MM/dd/yyyy");

                    for (DateTime dt = model.StartDate.Value; dt <= dtEndDate; dt = dt.AddDays(1))
                    {
                        dates.Add(new RoomPlanDateRangeVM()
                        {
                            Date = dt,
                            DateString = dt.ToString("dd/MM/yyyy"),
                            DayOfTheWeek = dt.DayOfWeek.ToString()
                        });
                    }

                    DateTime firstDayOfMonth = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, 1);

                    firstDate = firstDayOfMonth.ToString("dd/MM/yyyy");
                    lastDate = firstDayOfMonth.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");

                    nextDate = dtEndDate.AddDays(1).ToString("dd/MM/yyyy");
                    prevDate = model.StartDate.Value.AddDays(-7).ToString("dd/MM/yyyy");
                }

                var roomDetails = roomRepository.GetRoomDetailsForRoomPlan(model.RoomTypeId, model.FloorId, model.Rooms, startDate, endDate, LogInManager.LoggedInUserId);
                ViewBag.RoomDetail = roomDetails;

                var allocationFullDetail = reservationRepository.GetRoomAllocationDetailsForRoomPlan(null, startDate, endDate, LogInManager.LoggedInUserId);
                var getRecordCountHaveMoreThenone = allocationFullDetail.GroupBy(x => x.RoomId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                ViewBag.allocationFullDetail = JsonConvert.SerializeObject(getRecordCountHaveMoreThenone);

                ViewBag.Dates = dates;
                ViewBag.FirstDate = firstDate;
                ViewBag.LastDate = lastDate;
                ViewBag.NextDate = nextDate;
                ViewBag.PrevDate = prevDate;

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.ROOMPLAN, "Searched room plan detail");
                #endregion

                return PartialView("_RoomPlanView");
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchRoomPlan");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult ChangeRoomAllocation(Guid reservationId, Guid roomId, DateTime? arrivalDate, DateTime? departureDate)
        {
            try
            {
                string id = string.Empty;

                //Change Room Allocation.
                id = reservationRepository.ChangeRoomAllocation(reservationId, roomId, arrivalDate, departureDate, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(id))
                {
                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.ROOMPLAN, "Changed room allocation of reservation.");
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ReservationId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room Allocation not changed successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ChangeRoomAllocation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Reservation  Remarks

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult AddReservationRemark(ReservationRemarkVM model)
        {
            try
            {
                string remarkId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.CreatedOn = DateTime.Now;

                remarkId = reservationRepository.AddReservationRemark(model);

                if (!string.IsNullOrWhiteSpace(remarkId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RemarkId = remarkId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Remarks not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "AddReservationRemark");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult UpdateReservationRemark(ReservationRemarkVM model)
        {
            try
            {
                string remarkId = string.Empty;
                model.UpdatedOn = DateTime.Now;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                remarkId = reservationRepository.UpdateReservationRemark(model);

                if (!string.IsNullOrWhiteSpace(remarkId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RemarkId = remarkId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Remarks not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "UpdateReservationRemark");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult DeleteReservationRemark(Guid id)
        {
            try
            {
                string remarkId = string.Empty;

                remarkId = reservationRepository.DeleteReservationRemark(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(remarkId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RemarkId = remarkId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Remark not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteReservationRemark");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        [HttpPost]
        public ActionResult GetReservationRemarks(Guid reservationId)
        {
            try
            {
                var remarks = reservationRepository.GetReservationRemarks(reservationId, null, LogInManager.LoggedInUserId);


                return Json(new
                {
                    IsSuccess = true,
                    data = remarks
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetReservationRemarks");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        #endregion
    }
}
