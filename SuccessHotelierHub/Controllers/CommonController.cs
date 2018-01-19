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
    [HotelierHubAuthorize]
    public class CommonController : Controller
    {
        private CountryRepository countryRepository = new CountryRepository();
        private StateRepository stateRepository = new StateRepository();
        private CityRepository cityRepository = new CityRepository();

        public ActionResult GetCountry()
        {   
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();

            if (countryList != null && countryList.Count() > 0)
                return Json(new { IsSuccess = true, data = countryList.ToList() }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetState(int countryId)
        {
            var stateList = new SelectList(stateRepository.GetStates(countryId), "Id", "Name").ToList();

            if (stateList != null && stateList.Count() > 0)
                return Json(new { IsSuccess = true, data = stateList.ToList() }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCity(int? countryId, int? stateId)
        {
            var cityList = new SelectList(cityRepository.GetCities(countryId, stateId), "Id", "Name").ToList();

            if (cityList != null && cityList.Count() > 0)
                return Json(new { IsSuccess = true, data = cityList.ToList() }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}