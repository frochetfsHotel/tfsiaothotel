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
            return RedirectToAction("Index", "Home");

            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RoomFeaturesList = roomFeaturesList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult BulkReservation(List<TempBulkReservationVM> models)
        {
            try
            {
                return RedirectToAction("Index", "Home");

                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        //Get temp bulk reservation by id.
                        var tempReservation = reservationRepository.GetTempBulkReservationById(model.Id, LogInManager.LoggedInUserId);

                        if (tempReservation != null)
                        {
                            tempReservation.ProfileId = model.ProfileId;
                            tempReservation.LastName = model.LastName;
                            tempReservation.FirstName = model.FirstName;
                            tempReservation.TitleId = model.TitleId;
                            tempReservation.CountryId = model.CountryId;
                            tempReservation.ArrivalDate = model.ArrivalDate;
                            tempReservation.DepartureDate = model.DepartureDate;
                            tempReservation.NoOfNight = model.NoOfNight;
                            tempReservation.RoomNo = model.RoomNo;
                            tempReservation.RoomId = model.RoomId;
                            tempReservation.RoomTypeId = model.RoomTypeId;
                            tempReservation.IsWeekEndPrice = model.IsWeekEndPrice;
                            tempReservation.RateCodeId = model.RateCodeId;
                            tempReservation.Rate = model.Rate;
                            tempReservation.IsActive = model.IsActive;
                            tempReservation.UpdatedBy = LogInManager.LoggedInUserId;

                            //Update Temp Bulk Reservation.
                            reservationRepository.UpdateTempBulkReservation(tempReservation);
                        }
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
                        errorMessage = "Please select at lease one profile to create temp reservation."
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
                return RedirectToAction("Index", "Home");

                // var profiles = profileRepository.GetIndividualProfiles(lastName, firstName, LogInManager.LoggedInUserId);

                var reservations = reservationRepository.GetTempBulkReservation(LogInManager.LoggedInUserId);

                ViewData["Source"] = "BulkReservation";
                //ViewData["ProfileList"] = profiles;
                ViewData["TempBulkReservationList"] = reservations;

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