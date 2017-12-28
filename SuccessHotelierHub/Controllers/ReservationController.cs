using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;

namespace SuccessHotelierHub.Controllers
{
    public class ReservationController : Controller
    {
        #region Declaration

        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private CountryRepository countryRepository = new CountryRepository();        
        private TitleRepository titleRepository = new TitleRepository();
        private VipRepository vipRepository = new VipRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();

        #endregion


        #region Reservation 

        public ActionResult Create()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
            var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
            var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();            
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "Id", "RateTypeCode").ToList();
            var preferenceList = preferenceRepository.GetPreferences();

            ReservationVM model = new ReservationVM();
            RateQueryVM rateQuery = new RateQueryVM();                        
            if (Session["RateQueryVM"] != null)
            {
                //rateQuery = (RateQueryVM)TempData["RateQueryVM"];
                rateQuery = (RateQueryVM)Session["RateQueryVM"];
                
                model.ArrivalDate = rateQuery.ArrivalDate;
                model.NoOfNight = rateQuery.NoOfNight;
                model.DepartureDate = rateQuery.DepartureDate;
                model.NoOfAdult = rateQuery.NoOfAdult;
                model.NoOfChildren = rateQuery.NoOfChildren;
                model.NoOfRoom = rateQuery.NoOfRoom;

                model.LastName = rateQuery.LastName;
                model.FirstName = rateQuery.FirstName;
                model.ProfileId = rateQuery.ProfileId;

                model.MemberTypeId = rateQuery.MemberTypeId;
                model.CompanyId = rateQuery.CompanyId;
                model.AgentId = rateQuery.AgentId;
                model.SourceId = rateQuery.SourceId;
                model.BlockCodeId = rateQuery.BlockCodeId;

                model.MemberNo = rateQuery.MemberNo;
                model.RateCodeId = rateQuery.RateTypeId; //RateTypeCode
                model.RoomTypeId = rateQuery.RoomTypeId; //RoomTypeCode
                model.Rate = rateQuery.Amount; //Rate

                model.PackageId= rateQuery.PackageId;

            }

            ViewBag.TitleList = titleList;
            ViewBag.VipList = vipList;
            ViewBag.CountryList = countryList;
            ViewBag.PreferenceList = preferenceList;
            ViewBag.RateTypeList = rateTypeList;
            ViewBag.RoomTypeList = roomTypeList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationVM model)
        {
            try
            {
                string reservationId = string.Empty;

                model.CreatedBy = LogInManager.LoggedInUserId;

                string ETAText = model.ETAText;
                if (!string.IsNullOrWhiteSpace(ETAText))
                {
                    string todayDate = DateTime.Now.ToString("MM/dd/yyyy");
                    string date = (todayDate + " " + ETAText);
                    DateTime time = Convert.ToDateTime(date);

                    model.ETA = time.TimeOfDay;
                }

                reservationId = reservationRepository.AddReservation(model);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    #region Save Reservation Preference Mapping
                    var preferenceItems = model.PreferenceItems;

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                    {
                        var preferenceItemsArr = preferenceItems.Split(',');

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
                    }
                    #endregion

                    //Clear Session Object.
                    Session["RateQueryVM"] = null;

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
                        errorMessage = "Reservation not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult Edit(Guid id)
        {
            ReservationVM model = new ReservationVM();
            var reservation = reservationRepository.GetReservationById(id);

            if (reservation != null && reservation.Count > 0)
            {
                model = reservation[0];

                //Get Preference Mapping
                var selectedPreferences = preferenceRepository.GetReservationPreferenceMapping(model.Id, null);
                string preferenceItems = string.Empty;

                if (selectedPreferences != null && selectedPreferences.Count > 0)
                {
                    preferenceItems += string.Join(",", selectedPreferences.Select(i => i.PreferenceId));

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                        preferenceItems = preferenceItems.Trim(',');
                }

                model.PreferenceItems = preferenceItems;

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
                var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
                var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "Id", "RateTypeCode").ToList();
                var preferenceList = preferenceRepository.GetPreferences();

                ViewBag.TitleList = titleList;
                ViewBag.VipList = vipList;
                ViewBag.CountryList = countryList;
                ViewBag.PreferenceList = preferenceList;
                ViewBag.RateTypeList = rateTypeList;
                ViewBag.RoomTypeList = roomTypeList;

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationVM model)
        {
            try
            {
                string reservationId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;

                string ETAText = model.ETAText;
                if(!string.IsNullOrWhiteSpace(ETAText))
                {
                    string todayDate = DateTime.Now.ToString("MM/dd/yyyy");
                    string date = (todayDate + " " + ETAText);
                    DateTime time = Convert.ToDateTime(date);

                    model.ETA = time.TimeOfDay;
                }

                reservationId = reservationRepository.UpdateReservation(model);


                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    #region Save Reservation Preference Mapping

                    var preferenceItems = model.PreferenceItems;

                    //Delete Existing Reservation Preference Mapping.
                    preferenceRepository.DeleteReservationPreferenceMappingByReservation(model.Id);

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                    {
                        var preferenceItemsArr = preferenceItems.Split(',');

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
                    }
                    #endregion

                    //Clear Session Object.
                    Session["RateQueryVM"] = null;

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string reservationId = string.Empty;

                //Delete Reservation.
                reservationId = reservationRepository.DeleteReservation(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            return View();
        }

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
                var reservations = reservationRepository.SearchReservation(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Rate Query

        public ActionResult RateQuery()
        {
            var profileId = (string)TempData["ProfileId"];
            var firstName = (string)TempData["FirstName"];
            var lastName = (string)TempData["LastName"];

            var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);
            var rateTypeList = rateTypeRepository.GetRateType(string.Empty);
            //var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "Id", "RateTypeCode").ToList();

            ViewBag.RateTypeList = rateTypeList;
            ViewBag.RoomTypeList = roomTypeList;

            RateQueryVM model = new RateQueryVM();
            model.NoOfAdult = 1;
            model.NoOfChildren = 1;
            model.NoOfNight = 1;
            model.NoOfRoom = 1;

            if(!string.IsNullOrWhiteSpace(profileId))
            {
                model.ProfileId = Guid.Parse(profileId);
            }

            if (!string.IsNullOrWhiteSpace(lastName) || !string.IsNullOrWhiteSpace(firstName))
            {
                model.Name = (lastName + " " + firstName);
            }
            

            return View(model);
        }

        [HttpPost]
        public ActionResult ViewRateSheet(RateQueryVM model)
        {
            try
            {
                var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);
                var rateTypeList = rateTypeRepository.GetRateType(model.RateTypeCode);

                ViewData["RateType"] = rateTypeList;
                ViewData["RoomType"] = roomTypeList;

                return PartialView("_RateSheet");
            }
            catch(Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult ShowReservation(RateQueryVM model)
        {
            try
            {
                //TempData["RateQueryVM"] = model;
                Session["RateQueryVM"] = model;

                return Json(new
                {
                    IsSuccess = true,                    
                    data = Url.Action("Create", "Reservation")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion
    }
}