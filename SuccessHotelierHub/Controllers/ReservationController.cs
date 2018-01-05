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
    public class ReservationController : Controller
    {
        #region Declaration

        private const Int64 DefaultConfirmationNo = 100001;

        private ProfileRepository profileRepository = new ProfileRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private CountryRepository countryRepository = new CountryRepository();
        private TitleRepository titleRepository = new TitleRepository();
        private VipRepository vipRepository = new VipRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();
        private PreferenceGroupRepository preferenceGroupRepository = new PreferenceGroupRepository();
        private ReservationCancellationReasonRepository reservationCancellationReasonRepository = new ReservationCancellationReasonRepository();

        #endregion


        #region Reservation 

        public ActionResult Create()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
            var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
            var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
            var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "Id", "RateTypeCode").ToList();
            var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();

            ReservationVM model = new ReservationVM();
            RateQueryVM rateQuery = new RateQueryVM();
            if (Session["RateQueryVM"] != null)
            {
                rateQuery = (RateQueryVM)Session["RateQueryVM"];

                var profile = new IndividualProfileVM();
                if (rateQuery.ProfileId.HasValue)
                    profile = profileRepository.GetIndividualProfileById(rateQuery.ProfileId.Value).FirstOrDefault();

                model.ArrivalDate = rateQuery.ArrivalDate;
                model.NoOfNight = rateQuery.NoOfNight.HasValue ? rateQuery.NoOfNight.Value : 0;
                model.DepartureDate = rateQuery.DepartureDate;
                model.NoOfAdult = rateQuery.NoOfAdult.HasValue ? rateQuery.NoOfAdult.Value : 0;
                model.NoOfChildren = rateQuery.NoOfChildren.HasValue ? rateQuery.NoOfChildren.Value : 0;
                model.NoOfRoom = rateQuery.NoOfRoom.HasValue ? rateQuery.NoOfRoom.Value : 0;

                model.LastName = rateQuery.LastName;
                model.FirstName = rateQuery.FirstName;
                model.ProfileId = rateQuery.ProfileId;
                model.TitleId = profile.TitleId;

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

                model.PackageId = rateQuery.PackageId;
            }

            #region Profile Info From Profile Page

            var profileId = (string)TempData["ProfileId"];
            var firstName = (string)TempData["FirstName"];
            var lastName = (string)TempData["LastName"];
            var titleId = (Guid?)TempData["TitleId"];

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

            #endregion

            ViewBag.TitleList = titleList;
            ViewBag.VipList = vipList;
            ViewBag.CountryList = countryList;
            ViewBag.PreferenceGroupList = preferenceGroupList;
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
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + ETAText);
                    DateTime time = Convert.ToDateTime(date);

                    model.ETA = time.TimeOfDay;
                }

                #region Generate Confirmation No
                string confirmationNo = string.Empty;
                Int64 confirmationSuffix = 1;

                var lastReservation = reservationRepository.GetLastReservationByDate(null);

                if (lastReservation != null)
                {
                    string lastConfirmationNo = lastReservation.ConfirmationNumber;
                    if (!string.IsNullOrWhiteSpace(lastConfirmationNo))
                    {
                        confirmationSuffix = !string.IsNullOrWhiteSpace(lastConfirmationNo) ? (Convert.ToInt64(lastConfirmationNo) + 1) : DefaultConfirmationNo;

                        confirmationNo = confirmationSuffix.ToString();
                    }
                    else
                    {
                        confirmationNo = DefaultConfirmationNo.ToString();
                    }
                }
                else
                {
                    //Default confirmation no.
                    confirmationNo = DefaultConfirmationNo.ToString();
                }

                model.ConfirmationNumber = confirmationNo;

                #endregion

                reservationId = reservationRepository.AddReservation(model);

                if (!string.IsNullOrWhiteSpace(reservationId))
                {
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
                                reservationRoomMapping.ReservationId = Guid.Parse(reservationId);
                                reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                                reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                                roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);
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
                var selectedRooms = roomRepository.GetReservationRoomMapping(model.Id, null);

                ViewBag.SelectedRooms = selectedRooms;

                #endregion

                #region Preference Mapping

                //Get Preference Mapping
                var selectedPreferences = preferenceRepository.GetReservationPreferenceMapping(model.Id, null);

                ViewBag.SelectedPreferences = selectedPreferences;

                #endregion

                #region Profile Info From Edit Profile Page

                var profileId = (string)TempData["ProfileId"];
                var firstName = (string)TempData["FirstName"];
                var lastName = (string)TempData["LastName"];
                var titleId = (Guid?)TempData["TitleId"];

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

                #endregion

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
                var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();
                var rateTypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "Id", "RateTypeCode").ToList();
                var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();

                ViewBag.TitleList = titleList;
                ViewBag.VipList = vipList;
                ViewBag.CountryList = countryList;
                ViewBag.PreferenceGroupList = preferenceGroupList;
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
                if (!string.IsNullOrWhiteSpace(ETAText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + ETAText);
                    DateTime time = Convert.ToDateTime(date);

                    model.ETA = time.TimeOfDay;
                }

                reservationId = reservationRepository.UpdateReservation(model);


                if (!string.IsNullOrWhiteSpace(reservationId))
                {
                    #region Save Reservation Room Mapping
                    var roomIds = model.RoomIds;

                    //Delete Existing Reservation Room Mapping.
                    roomRepository.DeleteReservationRoomMappingByReservation(model.Id, LogInManager.LoggedInUserId);

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
                                reservationRoomMapping.ReservationId = Guid.Parse(reservationId);
                                reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                                reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                                roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);
                            }
                        }
                    }
                    #endregion

                    #region Save Reservation Preference Mapping

                    var preferenceItems = model.PreferenceItems;

                    //Delete Existing Reservation Preference Mapping.
                    preferenceRepository.DeleteReservationPreferenceMappingByReservation(model.Id);

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
            var reservationCancellationReasonList = reservationCancellationReasonRepository.GetReservationCancellationReasons();

            ViewBag.ReservationCancellationReasonList = reservationCancellationReasonList;

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

            ViewBag.RateTypeList = rateTypeList;
            ViewBag.RoomTypeList = roomTypeList;

            RateQueryVM model = new RateQueryVM();

            if (!string.IsNullOrWhiteSpace(profileId))
            {
                model.ProfileId = Guid.Parse(profileId);
            }

            if (!string.IsNullOrWhiteSpace(lastName) || !string.IsNullOrWhiteSpace(firstName))
            {
                model.Name = (lastName + " " + firstName);
            }

            //Default Values.
            model.NoOfAdult = 1;
            model.NoOfNight = 1;
            model.NoOfRoom = 1;


            return View(model);
        }

        [HttpPost]
        public ActionResult ViewRateSheet(RateQueryVM model)
        {
            try
            {
                bool blnShowWeekEndPrice = false;

                if (model.ArrivalDate.HasValue)
                {
                    if (model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Friday || model.ArrivalDate.Value.DayOfWeek == DayOfWeek.Sunday)
                    {
                        blnShowWeekEndPrice = true;
                    }
                }

                var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);
                var rateTypeList = rateTypeRepository.GetRateType(model.RateTypeCode);
                
                ViewData["RateType"] = rateTypeList;
                ViewData["RoomType"] = roomTypeList;
                ViewData["IsShowWeekEndPrice"] = blnShowWeekEndPrice;

                return PartialView("_RateSheet");
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion
    }
}