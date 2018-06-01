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
    [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
    public class ProfileController : Controller
    {
        #region Declaration 

        private CountryRepository countryRepository = new CountryRepository();
        private StateRepository stateRepository = new StateRepository();
        private CityRepository cityRepository = new CityRepository();
        private TitleRepository titleRepository = new TitleRepository();
        private VipRepository vipRepository = new VipRepository();
        private ProfileTypeRepository profileTypeRepository = new ProfileTypeRepository();
        private ProfileRepository profileRepository = new ProfileRepository();
        private PreferenceRepository preferenceRepository = new PreferenceRepository();
        private NationalityRepository nationalityRepository = new NationalityRepository();
        private PreferenceGroupRepository preferenceGroupRepository = new PreferenceGroupRepository();

        #endregion

        #region Individual Profile 

        [HttpGet]
        // GET: Profile/Create
        public ActionResult CreateIndividualProfile()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
            //var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
            var titleList = new SelectList(titleRepository.GetTitle()
                            .Select(
                                m => new SelectListItem()
                                {
                                    Value = m.Id.ToString(),
                                    Text = (m.Title)
                                }
                            ), "Value", "Text").ToList();
            var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
            var nationalityList = new SelectList(nationalityRepository.GetNationality(), "Id", "Name").ToList();

            var profileTypeId = profileTypeRepository.GetProfileType(ProfileTypeName.INDIVIDUAL).FirstOrDefault().Id;
                            
            var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();
                        
            IndividualProfileVM model = new IndividualProfileVM();
            model.ProfileTypeId = profileTypeId;
            model.IsActive = true;

            ViewBag.TitleList = titleList;
            ViewBag.VipList = vipList;
            ViewBag.CountryList = countryList;
            ViewBag.NationalityList = nationalityList;            
            ViewBag.PreferenceGroupList = preferenceGroupList;            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIndividualProfile(IndividualProfileVM model)
        {
            try
            {
                string profileId = string.Empty;

                model.CarRegistrationNo = Utility.Utility.ToUpperCase(model.CarRegistrationNo);
                model.CreatedBy = LogInManager.LoggedInUserId;


                profileId = profileRepository.AddIndividualProfile(model);

                if (!string.IsNullOrWhiteSpace(profileId))
                {

                    model.Id = Guid.Parse(profileId);

                    #region Save Profile Preference Mapping
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
                                //Save Profile Preference Mapping.
                                ProfilePreferenceMappingVM profilePreferenceMapping = new ProfilePreferenceMappingVM();
                                profilePreferenceMapping.ProfileTypeId = model.ProfileTypeId;
                                profilePreferenceMapping.PreferenceId = Guid.Parse(item);
                                profilePreferenceMapping.ProfileId = Guid.Parse(profileId);
                                profilePreferenceMapping.CreatedBy = LogInManager.LoggedInUserId;

                                preferenceRepository.AddProfilePreferenceMapping(profilePreferenceMapping);
                            }
                        }
                    }
                    #endregion

                    #region Profile Remarks 

                    if (model.RemarksList != null && model.RemarksList.Count > 0)
                    {
                        foreach (var remark in model.RemarksList)
                        {
                            remark.ProfileId = model.Id;
                            remark.CreatedBy = LogInManager.LoggedInUserId;
                            if (!remark.CreatedOn.HasValue)
                            {
                                remark.CreatedOn = DateTime.Now;
                            }

                            profileRepository.AddProfileRemark(remark);
                        }
                    }

                    #endregion

                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.INDIVIDUAL_PROFILE, string.Format("Created new profile of {0} {1}.", model.LastName, model.FirstName));
                    #endregion

                    #region  Check Source Parameters
                    if (Request.Form["Source"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.Form["Source"])))
                    {
                        string source = string.Empty;
                        string url = string.Empty;
                        string qid = string.Empty;

                        source = Convert.ToString(Request.Form["Source"]);
                        
                        if (source == "RateQuery")
                        {
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("RateQuery", "Reservation");                            
                        }
                        else if (source == "CreateReservation")
                        {
                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Create", "Reservation");
                        }
                        else if (source == "EditReservation")
                        {
                            qid = Convert.ToString(Request.Form["Qid"]);

                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Edit", "Reservation", new { Id = qid });
                        }
                        else if (source == "SearchArrivals")
                        {
                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Arrivals", "FrontDesk");
                        }

                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            return Json(new
                            {
                                IsSuccess = true,
                                IsExternalUrl = true,
                                data = url
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ProfileId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Individual profile not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "CreateIndividualProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult EditIndividualProfile(Guid id)
        {
            IndividualProfileVM model = new IndividualProfileVM();
            var profile = profileRepository.GetIndividualProfileById(id, LogInManager.LoggedInUserId);

            if (profile != null && profile.Count > 0)
            {
                model = profile[0];

                model.Remarks = string.Empty;

                model.CarRegistrationNo = Utility.Utility.ToUpperCase(model.CarRegistrationNo);

                #region Preference Mapping
                //Get Preference Mapping
                var selectedPreferences = preferenceRepository.GetProfilePreferenceMapping(model.ProfileTypeId, model.Id, null, LogInManager.LoggedInUserId);
                
                ViewBag.SelectedPreferences = selectedPreferences;
                #endregion

                var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
                //var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
                var titleList = new SelectList(titleRepository.GetTitle()
                            .Select(
                                m => new SelectListItem()
                                {
                                    Value = m.Id.ToString(),
                                    Text = (m.Title)
                                }
                            ), "Value", "Text").ToList();
                var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
                var nationalityList = new SelectList(nationalityRepository.GetNationality(), "Id", "Name").ToList();
                var preferenceGroupList = new SelectList(preferenceGroupRepository.GetPreferenceGroup(), "Id", "Name").ToList();

                ViewBag.TitleList = titleList;
                ViewBag.VipList = vipList;
                ViewBag.CountryList = countryList;
                ViewBag.NationalityList = nationalityList;
                ViewBag.PreferenceGroupList = preferenceGroupList;

                ////Get State
                //var stateList = new SelectList(new List<StateVM>(), "Id", "Name").ToList();

                ////Get City
                //var cityList = new SelectList(new List<CityVM>(), "Id", "Name").ToList();

                //if (model.CountryId.HasValue)
                //{
                //    stateList = new SelectList(stateRepository.GetStates(model.CountryId), "Id", "Name").ToList();

                //    cityList = new SelectList(cityRepository.GetCities(model.CountryId,model.StateId), "Id", "Name").ToList();
                //}

                //ViewBag.StateList = stateList;
                //ViewBag.CityList = cityList;                

                return View(model);
            }

            return RedirectToAction("IndividualProfileList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIndividualProfile(IndividualProfileVM model)
        {
            try
            {
                string profileId = string.Empty;

                model.CarRegistrationNo = Utility.Utility.ToUpperCase(model.CarRegistrationNo);
                model.UpdatedBy = LogInManager.LoggedInUserId;

                profileId = profileRepository.UpdateIndividualProfile(model);

                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    #region Save Profile Preference Mapping

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

                    //Delete Existing Profile Preference Mapping.
                    //preferenceRepository.DeleteProfilePreferenceMappingByProfile(model.ProfileTypeId, model.Id);

                    #region Delete Preference Mapping

                    var preferenceMappings = preferenceRepository.GetProfilePreferenceMapping(model.ProfileTypeId, model.Id, null, LogInManager.LoggedInUserId);

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
                                preferenceRepository.DeleteProfilePreferenceMapping(mappingId, LogInManager.LoggedInUserId);
                            }
                        }
                    }

                    #endregion

                    if (preferenceItemsArr != null)
                    {
                        foreach (var item in preferenceItemsArr)
                        {
                            //Save Profile Preference Mapping.
                            ProfilePreferenceMappingVM profilePreferenceMappingVM = new ProfilePreferenceMappingVM();
                            profilePreferenceMappingVM.ProfileTypeId = model.ProfileTypeId;
                            profilePreferenceMappingVM.PreferenceId = Guid.Parse(item);
                            profilePreferenceMappingVM.ProfileId = Guid.Parse(profileId);
                            profilePreferenceMappingVM.CreatedBy = LogInManager.LoggedInUserId;

                            preferenceRepository.AddProfilePreferenceMapping(profilePreferenceMappingVM);
                        }
                    }

                    #endregion

                    #region Add Profile Remarks 

                    if (!string.IsNullOrWhiteSpace(model.Remarks))
                    {
                        ProfileRemarkVM remark = new ProfileRemarkVM();
                        remark.ProfileId = Guid.Parse(profileId);
                        remark.Remarks = model.Remarks;
                        remark.CreatedBy = LogInManager.LoggedInUserId;
                        if (!remark.CreatedOn.HasValue)
                        {
                            remark.CreatedOn = DateTime.Now;
                        }

                        profileRepository.AddProfileRemark(remark);
                    }

                    #endregion

                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.INDIVIDUAL_PROFILE, string.Format("Updated profile of {0} {1}.", model.LastName, model.FirstName));
                    #endregion

                    #region  Check Source Parameters
                    if (Request.Form["Source"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.Form["Source"])))
                    {
                        string source = string.Empty;
                        string url = string.Empty;
                        string qid = string.Empty;

                        source = Convert.ToString(Request.Form["Source"]);

                        if (source == "RateQuery")
                        {   
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("RateQuery", "Reservation");
                        }
                        else if (source == "CreateReservation")
                        {
                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Create", "Reservation");
                        }
                        else if (source == "EditReservation")
                        {
                            qid = Convert.ToString(Request.Form["Qid"]);

                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Edit", "Reservation", new { Id = qid });
                        }
                        else if (source == "SearchArrivals")
                        {
                            TempData["TitleId"] = model.TitleId;
                            TempData["ProfileId"] = profileId;
                            TempData["FirstName"] = model.FirstName;
                            TempData["LastName"] = model.LastName;
                            TempData["CountryId"] = model.CountryId;
                            TempData["TelephoneNo"] = model.TelephoneNo;
                            //TempData["Remarks"] = model.Remarks;

                            url = Url.Action("Arrivals", "FrontDesk");
                        }

                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            return Json(new
                            {
                                IsSuccess = true,
                                IsExternalUrl = true,
                                data = url
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ProfileId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Individual profile not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "EditIndividualProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteIndividualProfile(Guid id)
        {
            try
            {
                string profileId = string.Empty;
                
                //Delete Individual Profile.
                profileId = profileRepository.DeleteIndividualProfile(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    #region Record Activity Log
                    RecordActivityLog.RecordActivity(Pages.INDIVIDUAL_PROFILE, "Deleted profile.");
                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            ProfileId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Individual profile not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteIndividualProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult IndividualProfileList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchIndividualProfile(SearchIndividualProfileParametersVM model)
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

                var profiles = profileRepository.SearchIndividualProfile(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = profiles.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                #region Record Activity Log
                RecordActivityLog.RecordActivity(Pages.INDIVIDUAL_PROFILE, "Searched profile.");
                #endregion

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = profiles
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchIndividualProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult SearchAdvanceProfile(SearchAdvanceProfileParametersVM model)
        {
            try
            {
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.IsAdminUser = LogInManager.HasRights("ADMIN");
                var profiles = profileRepository.SearchAdvanceProfile(model);
                
                return Json(new
                {
                    IsSuccess = true,
                    data = profiles
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchAdvanceProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        [HttpGet]
        public ActionResult GetPreferencesByProfile(Guid profileId)
        {
            
            try
            {
                var profile = profileRepository.GetIndividualProfileById(profileId, LogInManager.LoggedInUserId).FirstOrDefault();

                if (profile != null)
                {
                    //Get Preference Mapping
                    var preferences = preferenceRepository.GetProfilePreferenceMapping(profile.ProfileTypeId, profileId, null, LogInManager.LoggedInUserId);

                    return Json(new
                    {
                        IsSuccess = true,
                        data = preferences
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = false, errorMessage = "Profile and preferences not exist."});
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetPreferencesByProfile");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            try
            {
                //Delete all Reservation.
                profileRepository.DeleteAllProfile(LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

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

        #endregion

        #region Profile  Remarks

        [HttpPost]
        public ActionResult AddProfileRemark(ProfileRemarkVM model)
        {
            try
            {
                string remarkId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.CreatedOn = DateTime.Now;

                remarkId = profileRepository.AddProfileRemark(model);

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
                Utility.Utility.LogError(e, "AddProfileRemark");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult UpdateProfileRemark(ProfileRemarkVM model)
        {
            try
            {
                string remarkId = string.Empty;
                model.UpdatedOn = DateTime.Now;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                remarkId = profileRepository.UpdateProfileRemark(model);

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
                Utility.Utility.LogError(e, "UpdateProfileRemark");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteProfileRemark(Guid id)
        {
            try
            {
                string remarkId = string.Empty;

                remarkId = profileRepository.DeleteProfileRemark(id, LogInManager.LoggedInUserId, LogInManager.LoggedInUserId);

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
                Utility.Utility.LogError(e, "DeleteProfileRemark");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        [HttpPost]
        public ActionResult GetProfileRemarks(Guid profileId)
        {
            try
            {
                var remarks = profileRepository.GetProfileRemarks(profileId, null, LogInManager.LoggedInUserId);


                return Json(new
                {
                    IsSuccess = true,
                    data = remarks
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetProfileRemarks");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }


        #endregion

        #region Company Profile 

        public ActionResult Company()
        {
            return View();
        }

        #endregion

      
    }
}

