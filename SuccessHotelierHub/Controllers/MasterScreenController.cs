using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuccessHotelierHub.Controllers
{
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class MasterScreenController : Controller
    {
        // GET: MasterScreen
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult CreateTitle()
        {
            return View();
        }
        public ActionResult ListingTitle()
        {
            return View();
        }
        public ActionResult CreateNationality()
        {
            return View();
        }
        public ActionResult ListingNationality()
        {
            return View();
        }
        public ActionResult CreateVip()
        {
            return View();
        }
        public ActionResult ListingVip()
        {
            return View();
        }
        public ActionResult CreateCountry()
        {
            return View();
        }
        public ActionResult ListingCountry()
        {
            return View();
        }
        public ActionResult CreateState()
        {
            return View();
        }
        public ActionResult ListingState()
        {
            return View();
        }
        public ActionResult CreateCity()
        {
            return View();
        }
        public ActionResult ListingCity()
        {
            return View();
        }
    }
}