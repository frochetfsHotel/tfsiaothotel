using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using SuccessHotelierHub.Utility;

namespace SuccessHotelierHub.Controllers
{
    
    public class CommonController : Controller
    {
        #region Declaration

        private CountryRepository countryRepository = new CountryRepository();
        private StateRepository stateRepository = new StateRepository();
        private CityRepository cityRepository = new CityRepository();
        private ProfileRepository profileRepository = new ProfileRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RoomFeatureRepository roomFeatureRepository = new RoomFeatureRepository();

        private ReservationTypeRepository reservationTypeRepository = new ReservationTypeRepository();
        private MarketRepository marketRepository = new MarketRepository();
        private ReservationSourceRepository reservationSourceRepository = new ReservationSourceRepository();
        private PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();


        #endregion

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult GetCountry()
        {
            try
            {
                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();

                if (countryList != null && countryList.Count() > 0)
                    return Json(new { IsSuccess = true, data = countryList.ToList() }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetCountry");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }

        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult GetState(int countryId)
        {
            try
            {
                var stateList = new SelectList(stateRepository.GetStates(countryId), "Id", "Name").ToList();

                if (stateList != null && stateList.Count() > 0)
                    return Json(new { IsSuccess = true, data = stateList.ToList() }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetState");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult GetCity(int? countryId, int? stateId)
        {
            try
            {
                var cityList = new SelectList(cityRepository.GetCities(countryId, stateId), "Id", "Name").ToList();

                if (cityList != null && cityList.Count() > 0)
                    return Json(new { IsSuccess = true, data = cityList.ToList() }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsSuccess = false, data = "" },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetCity");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult BulkReservation()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RoomFeaturesList = roomFeaturesList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult BulkReservation(List<BulkReservationVM> models)
        {

            try
            {
                if (models != null && models.Count > 0)
                {
                    var reservationType = reservationTypeRepository.GetReservationTypes().FirstOrDefault();
                    var market = marketRepository.GetMarkets().FirstOrDefault();
                    var reservationSource = reservationSourceRepository.GetReservationSources().FirstOrDefault();

                    var paymentMethods = paymentMethodRepository.GetPaymentMethods();
                    var paymentMethod = paymentMethods.Where(m => m.Name == "Cash").FirstOrDefault();

                    foreach (var model in models)
                    {
                        ReservationVM reservation = new ReservationVM();
                        reservation.ProfileId = model.ProfileId;
                        reservation.LastName = model.LastName;
                        reservation.FirstName = model.FirstName;
                        reservation.TitleId = model.TitleId;
                        reservation.CountryId = model.CountryId;
                        
                        reservation.ArrivalDate = model.ArrivalDate;
                        reservation.ETA = DateTime.Now.TimeOfDay;
                        reservation.DepartureDate = model.DepartureDate;
                        reservation.NoOfNight = model.NoOfNight;
                        reservation.NoOfRoom = 1;
                        reservation.NoOfAdult = 1;

                        reservation.RoomNumbers = model.RoomNumber;
                        reservation.RoomIds = Convert.ToString(model.RoomId);

                        reservation.RoomTypeId = model.RoomTypeId;
                        reservation.RateCodeId = model.RateTypeId;

                        reservation.IsWeekEndPrice = model.IsWeekEndPrice;
                        reservation.Rate = model.Rate;
                        reservation.IsActive = model.IsActive;

                        reservation.ReservationTypeId = reservationType.Id;
                        reservation.MarketId = market.Id;
                        reservation.ReservationSourceId = reservationSource.Id;
                        reservation.PaymentMethodId = paymentMethod.Id;

                        reservation.PreferenceItems = model.PreferenceItems;
                        

                        var result = new ReservationController().CreateReservation(reservation);
                    }

                    return Json(new
                    {
                        IsSuccess = true                        
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Please select at lease one profile to create reservation."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "BulkReservation");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult SearchBulkReservationList(string lastName, string firstName)
        {
            try
            {
                var profiles = profileRepository.GetIndividualProfiles(lastName, firstName);

                ViewData["Source"] = "BulkReservation";
                ViewData["ProfileList"] = profiles;                

                return PartialView("_ShowBulkReservationList");                
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchBulkReservationList");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}