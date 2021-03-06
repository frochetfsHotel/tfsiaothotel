﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;

namespace SuccessHotelierHub.Controllers
{
    [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
    public class PropertyCalendarController : Controller
    {
        #region Declaration

        private CalendarNotesRepository calendarNotesRepository = new CalendarNotesRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private HolidayRepository holidayRepository = new HolidayRepository();

        #endregion

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

            //Room Details.
            var results = roomRepository.GetTotalRoomAvailableByCalendar(month, year, LogInManager.LoggedInUserId);
            ViewData["RoomDetails"] = results;

            //Holidays.
            var holidays = holidayRepository.GetHolidaysOfCurrentMonth(month, year);
            ViewData["Holidays"] = holidays;

            //Calendar Notes.
            var calendarNotes = calendarNotesRepository.GetCalendarNotesOfCurrentMonth(month, year, LogInManager.LoggedInUserId);
            ViewData["CalendarNotes"] = calendarNotes;

            return PartialView("_Calendar");
        }

        [HttpPost]
        public ActionResult AddCalendarNotes(CalendarNotesVM model)
        {
            try
            {
                string notesId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                notesId = calendarNotesRepository.AddCalendarNotes(model);

                if (!string.IsNullOrWhiteSpace(notesId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            NotesId = notesId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Notes details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "AddCalendarNotes");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult EditCalendarNotes(CalendarNotesVM model)
        {
            try
            {
                string notesId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                notesId = calendarNotesRepository.UpdateCalendarNotes(model);

                if (!string.IsNullOrWhiteSpace(notesId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            NotesId = notesId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Notes details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "EditCalendarNotes");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string notesId = string.Empty;

                notesId = calendarNotesRepository.DeleteCalendarNotes(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(notesId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            NotesId = notesId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Notes details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpGet]
        public ActionResult GetAvailableRoomInfo(string date)
        {
            //Room Details.
            var results = roomRepository.GetAvailableRoomInfoByDate(date, LogInManager.LoggedInUserId);
            ViewData["RoomInfo"] = results;

            return PartialView("_RoomInfo");
        }
    }
}