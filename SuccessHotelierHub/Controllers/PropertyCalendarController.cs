using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuccessHotelierHub.Controllers
{
    public class PropertyCalendarController : Controller
    {
        // GET: PropertyCalendar
        public ActionResult Index()
        {   
            return View();
        }

        [HttpPost]
        public ActionResult ShowCalendar(int year, int month)
        {
            ViewData["Month"] = month;
            ViewData["Year"] = year;
            return PartialView("_Calendar");
        }
    }
}