using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuccessHotelierHub.Controllers
{
    public class FrontDeskController : Controller
    {
        // GET: FrontDesk
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckIn()
        {
            return View();
        }
    }
}