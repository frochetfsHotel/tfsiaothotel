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

        #endregion

        #region Individual Profile 

        [HttpGet]
        // GET: Profile/Create
        public ActionResult CreateIndividualProfile()
        {
            var countryList = new SelectList(countryRepository.GetCountries(), "Id", "Name").ToList();
            var titleList = new SelectList(titleRepository.GetTitle(), "Id", "Title").ToList();
            var vipList = new SelectList(vipRepository.GetVip(), "Id", "Description").ToList();
            var nationalityList = new SelectList(nationalityRepository.GetNationality(), "Id", "Name").ToList();

            var profileTypeId = profileTypeRepository.GetProfileType(ProfileTypeName.INDIVIDUAL).FirstOrDefault().Id;

            var preferenceList = preferenceRepository.GetPreferences();

            IndividualProfileVM model = new IndividualProfileVM();
            model.ProfileTypeId = profileTypeId;


            ViewBag.TitleList = titleList;
            ViewBag.VipList = vipList;
            ViewBag.CountryList = countryList;
            ViewBag.NationalityList = nationalityList;
            ViewBag.PreferenceList = preferenceList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIndividualProfile(IndividualProfileVM model)
        {
            try
            {
                string profileId = string.Empty;

                model.CreatedBy = LogInManager.LoggedInUserId;
                profileId = profileRepository.AddIndividualProfile(model);

                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    #region Save Profile Preference Mapping
                    var preferenceItems = model.PreferenceItems;

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                    {
                        var preferenceItemsArr = preferenceItems.Split(',');

                        if (preferenceItemsArr != null)
                        {
                            foreach (var item in preferenceItemsArr)
                            {
                                //Save Preference Profile Mapping.
                                ProfilePreferenceMappingVM profilePreferenceMappingVM = new ProfilePreferenceMappingVM();
                                profilePreferenceMappingVM.ProfileTypeId = model.ProfileTypeId;
                                profilePreferenceMappingVM.PreferenceId = Guid.Parse(item);
                                profilePreferenceMappingVM.ProfileId = Guid.Parse(profileId);
                                profilePreferenceMappingVM.CreatedBy = LogInManager.LoggedInUserId;

                                preferenceRepository.AddProfilePreferenceMapping(profilePreferenceMappingVM);
                            }
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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult EditIndividualProfile(Guid id)
        {
            IndividualProfileVM model = new IndividualProfileVM();
            var profile = profileRepository.GetIndividualProfileById(id);

            if (profile != null && profile.Count > 0)
            {
                model = profile[0];

                //Get Preference Mapping
                var selectedPreferences = preferenceRepository.GetProfilePreferenceMapping(model.ProfileTypeId, model.Id, null);
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
                var nationalityList = new SelectList(nationalityRepository.GetNationality(), "Id", "Name").ToList();
                var preferenceList = preferenceRepository.GetPreferences();

                ViewBag.TitleList = titleList;
                ViewBag.VipList = vipList;
                ViewBag.CountryList = countryList;
                ViewBag.NationalityList = nationalityList;
                ViewBag.PreferenceList = preferenceList;

                //Get State
                var stateList = new SelectList(stateRepository.GetStates(model.CountryId), "Id", "Name").ToList();

                //Get City
                var cityList = new SelectList(cityRepository.GetCities(model.StateId), "Id", "Name").ToList();

                ViewBag.StateList = stateList;
                ViewBag.CityList = cityList;

                return View(model);
            }

            return RedirectToAction("IndividualProfileList");
        }

        [HttpPost]
        public ActionResult EditIndividualProfile(IndividualProfileVM model)
        {
            try
            {
                string profileId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;
                profileId = profileRepository.UpdateIndividualProfile(model);

                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    #region Save Profile Preference Mapping

                    var preferenceItems = model.PreferenceItems;

                    //Delete Existing Preference Profile Mapping.
                    preferenceRepository.DeleteProfilePreferenceMappingByProfile(model.ProfileTypeId, model.Id, LogInManager.LoggedInUserId);

                    if (!string.IsNullOrWhiteSpace(preferenceItems))
                    {
                        var preferenceItemsArr = preferenceItems.Split(',');

                        if (preferenceItemsArr != null)
                        {
                            foreach (var item in preferenceItemsArr)
                            {
                                //Save Preference Profile Mapping.
                                ProfilePreferenceMappingVM profilePreferenceMappingVM = new ProfilePreferenceMappingVM();
                                profilePreferenceMappingVM.ProfileTypeId = model.ProfileTypeId;
                                profilePreferenceMappingVM.PreferenceId = Guid.Parse(item);
                                profilePreferenceMappingVM.ProfileId = Guid.Parse(profileId);
                                profilePreferenceMappingVM.CreatedBy = LogInManager.LoggedInUserId;

                                preferenceRepository.AddProfilePreferenceMapping(profilePreferenceMappingVM);
                            }
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
                profileId = profileRepository.DeleteIndividualProfile(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(profileId))
                {
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
                var profiles = profileRepository.SearchIndividualProfile(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = profiles.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

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

