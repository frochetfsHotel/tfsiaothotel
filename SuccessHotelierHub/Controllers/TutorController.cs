using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;
using Hangfire;
using System.IO;

namespace SuccessHotelierHub.Controllers
{

    public class TutorController : Controller
    {
        #region Declaration

        private UserRoleRepository userRoleRepository = new UserRoleRepository();
        private UserRepository userRepository = new UserRepository();
        private UsersActivityLogRepository usersActivityLogRepository = new UsersActivityLogRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

        #endregion

        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }

        #region  TUTOR CRUD
        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult List()
        {
            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Search(SearchTutorDetailParametersVM model)
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
                var tutors = userRepository.SearchTutorDetail(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = tutors.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                if (tutors != null && tutors.Count > 0)
                {
                    foreach (var tutor in tutors)
                    {
                        if (!string.IsNullOrWhiteSpace(tutor.Password))
                            tutor.Password = Utility.Utility.Decrypt(tutor.Password, Utility.Utility.EncryptionKey);
                    }
                }

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = tutors
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            UserVM model = new UserVM();
            model.Password = Utility.Utility.GenerateRandomPassword(8);

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserVM model)
        {
            try
            {
                string userId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.Password = Utility.Utility.Encrypt(model.Password, Utility.Utility.EncryptionKey);

                #region Get Tutor Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var tutorRoleId = userRoles.Where(m => m.Code == "TUTOR").FirstOrDefault().Id;
                model.UserRoleId = tutorRoleId;

                #endregion

                #region Check User Email Exist.

                if (this.CheckUserEmailExist(model.Id, model.UserRoleId, model.Email) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Email : {0} already exist.", model.Email)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Get Max. User Id

                //Get Max. User Id.
                var newUserId = (userRepository.GetMaxUserId()) + 1;
                model.UserId = newUserId;

                #endregion

                #region Generate Cashier Number

                var firstTwoCharactersOfName = !string.IsNullOrWhiteSpace(model.Name) ? model.Name.Substring(0, 2) : "";
                model.CashierNumber = (firstTwoCharactersOfName + newUserId).ToUpper();

                #endregion Generate Cashier Number

                model.IsFromRegistrationPage = false;
                model.IsRecordActivity = true;

                userId = userRepository.AddUserDetail(model);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Add User Role Mapping 

                    UserRoleMappingVM userRoleMapping = new UserRoleMappingVM();
                    userRoleMapping.UserId = Guid.Parse(userId);
                    userRoleMapping.UserRoleId = model.UserRoleId;
                    userRoleMapping.IsActive = true;
                    userRoleMapping.CreatedBy = LogInManager.LoggedInUserId;
                    userRoleMapping.UpdatedBy = LogInManager.LoggedInUserId;


                    userRepository.AddUpdateUserRoleMapping(userRoleMapping);

                    #endregion

                    #region Set Default Tutor Configuration Type to Restricted.

                    UserLoginTimeVM userLoginTime = new UserLoginTimeVM();

                    userLoginTime.ConfigurationType = UserLoginConfigurationType.RESTRICTED;
                    userLoginTime.LoginStartTime = null;
                    userLoginTime.LoginEndTime = null;
                    userLoginTime.TutorId = Guid.Parse(userId);
                    userLoginTime.CreatedBy = LogInManager.LoggedInUserId;
                    userLoginTime.UpdatedBy = LogInManager.LoggedInUserId;
                    userLoginTime.IsActive = true;

                    userRepository.AddUpdateUserLoginTime(userLoginTime);

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserId = userId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Tutor details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Create");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Edit(Guid id)
        {
            var user = userRepository.GetUserDetailById(id);

            UserVM model = new UserVM();

            if (user != null && user.Count > 0)
            {
                model = user[0];

                var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
                ViewBag.CollegeGroupList = collegeGroupList;

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            try
            {
                string userId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                model.IsRecordActivity = true;

                #region Get Tutor Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var tutorRoleId = userRoles.Where(m => m.Code == "TUTOR").FirstOrDefault().Id;
                model.UserRoleId = tutorRoleId;

                #endregion

                #region Check User Email Exist.

                if (this.CheckUserEmailExist(model.Id, model.UserRoleId, model.Email) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Email : {0} already exist.", model.Email)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Check Cashier Number Exist.

                if (!string.IsNullOrWhiteSpace(model.CashierNumber))
                {
                    if (this.CheckCashierNumberExist(model.Id, model.CashierNumber) == false)
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = string.Format("Cashier Number : {0} already exist.", model.Email)
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                #endregion

                userId = userRepository.UpdateUserDetail(model);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Add / Update User Role Mapping 

                    UserRoleMappingVM userRoleMapping = new UserRoleMappingVM();
                    userRoleMapping.UserId = Guid.Parse(userId);
                    userRoleMapping.UserRoleId = model.UserRoleId;
                    userRoleMapping.IsActive = true;
                    userRoleMapping.CreatedBy = LogInManager.LoggedInUserId;
                    userRoleMapping.UpdatedBy = LogInManager.LoggedInUserId;

                    userRepository.AddUpdateUserRoleMapping(userRoleMapping);

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserId = userId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Tutor details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Edit");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string userId = string.Empty;

                #region Check Student Mapping Exist

                var students = userRepository.GetTutorStudentMappingByTutor(id, null);

                if (students != null && students.Count > 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "You can not delete this tutor because there are one or more students assigned with this tutors."
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                userId = userRepository.DeleteUserDetail(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Delete User Role Mapping

                    userRepository.DeleteUserRoleMappingByUserId(id, LogInManager.LoggedInUserId);

                    #endregion

                    #region Delete Tutor Student Mapping

                    userRepository.DeleteTutorStudentMappingByTutor(id, LogInManager.LoggedInUserId);

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserId = userId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Tutor details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Tutor Student Mapping

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult StudentMapping(Guid id)
        {

            var tutor = userRepository.GetUserDetailById(id).FirstOrDefault();

            if (tutor != null)
            {
                var students = userRepository.GetStudentDetailsForTutorMapping(id, tutor.CollegeGroupId);

                if (students != null && students.Count > 0)
                {
                    var tutorDetail = userRepository.GetUserDetailById(id).FirstOrDefault();

                    ViewBag.Students = students;
                    ViewBag.TutorId = id;
                    ViewBag.TutorName = tutorDetail.Name;

                    return View();
                }
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult StudentMapping(Guid tutorId, List<TutorStudentMappingVM> models)
        {
            try
            {

                if (tutorId == null)
                {
                    return HttpNotFound();
                }

                #region Delete Tutor Student Mapping

                //Get existing tutor student mapping.
                var tutorStudentMappings = userRepository.GetTutorStudentMappingByTutor(tutorId, null);

                //Get distict student ids list from the models.
                var studentIds = models.Select(m => m.StudentId).Distinct().ToList();

                if (tutorStudentMappings != null && tutorStudentMappings.Count > 0)
                {
                    List<Guid> mappingIds = new List<Guid>();
                    foreach (var tutorStudentMapping in tutorStudentMappings)
                    {
                        if (studentIds == null || !studentIds.Contains(tutorStudentMapping.StudentId))
                        {
                            mappingIds.Add(tutorStudentMapping.Id);
                        }
                    }

                    //Delete Tutor Student Mapping
                    if (mappingIds != null && mappingIds.Count > 0)
                    {
                        foreach (var id in mappingIds)
                        {
                            userRepository.DeleteTutorStudentMapping(id, LogInManager.LoggedInUserId);
                        }
                    }
                }
                #endregion

                if (models != null && models.Count > 0)
                {
                    #region Add / Update Tutor Student Mapping

                    foreach (var model in models)
                    {
                        model.CreatedBy = LogInManager.LoggedInUserId;
                        model.UpdatedBy = LogInManager.LoggedInUserId;
                        model.IsActive = true;

                        userRepository.AddUpdateTutorStudentMapping(model);
                    }

                    #endregion

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
                        errorMessage = "Please select at least one student."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "StudentMapping");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        #endregion

        #region Student List

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult ViewStudent()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        public ActionResult SearchStudent(SearchStudentDetailParametersVM model)
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
                model.TutorId = LogInManager.LoggedInUser.Id;

                var students = userRepository.SearchStudentDetail(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = students.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                if (students != null && students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        if (!string.IsNullOrWhiteSpace(student.Password))
                            student.Password = Utility.Utility.Decrypt(student.Password, Utility.Utility.EncryptionKey);
                    }
                }

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = students
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchStudent");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region User's Activity Log


        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult ViewActivity(Guid id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            SearchUsersActivityLogParametersVM model = new SearchUsersActivityLogParametersVM();
            model.UserId = id;

            var userDetail = userRepository.GetUserDetailById(id).FirstOrDefault();

            if (userDetail != null)
            {
                model.UserName = userDetail.Name;
                model.UserEmail = userDetail.Email;
            }

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        public ActionResult SearchActivityLog(SearchUsersActivityLogParametersVM model)
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

                var activities = usersActivityLogRepository.SearchUsersActivityLog(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = activities.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = activities
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchActivityLog");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Reservation Log

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult ReservationLog(Guid id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            SearchUsersReservationLogParametersVM model = new SearchUsersReservationLogParametersVM();
            model.UserGUID = id;

            var userDetail = userRepository.GetUserDetailById(id).FirstOrDefault();

            if (userDetail != null)
            {
                model.UserName = userDetail.Name;
                model.UserEmail = userDetail.Email;
            }

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        public ActionResult SearchReservationLog(SearchUsersReservationLogParametersVM model)
        {
            try
            {
                var userDetail = userRepository.GetUserDetailById(model.UserGUID.Value).FirstOrDefault();

                if (userDetail == null)
                {
                    return Json(new { IsSuccess = false, errorMessage = "User details not exist." });
                }

                model.UserId = userDetail.UserId;

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

                var reservations = reservationRepository.SearchReservationByUserId(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

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
                Utility.Utility.LogError(e, "SearchReservationLog");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Folio Log

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult FolioLog(Guid id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            SearchUsersReservationFolioLogParametersVM model = new SearchUsersReservationFolioLogParametersVM();
            model.UserGUID = id;

            var userDetail = userRepository.GetUserDetailById(id).FirstOrDefault();

            if (userDetail != null)
            {
                model.UserName = userDetail.Name;
                model.UserEmail = userDetail.Email;
            }

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        public ActionResult SearchFolioLog(SearchUsersReservationFolioLogParametersVM model)
        {
            try
            {
                var userDetail = userRepository.GetUserDetailById(model.UserGUID.Value).FirstOrDefault();

                if (userDetail == null)
                {
                    return Json(new { IsSuccess = false, errorMessage = "User details not exist." });
                }

                model.UserId = userDetail.UserId;

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

                var reservations = reservationRepository.SearchCheckedOutReservationByUserId(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

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
                Utility.Utility.LogError(e, "SearchFolioLog");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region User Login Time        

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult UserLoginTime()
        {
            var userLoginDetails = userRepository.GetUserLoginTimeByTutor(LogInManager.LoggedInUser.Id);

            UserLoginTimeVM model = new UserLoginTimeVM();

            model.TutorId = LogInManager.LoggedInUser.Id;
            model.ConfigurationType = UserLoginConfigurationType.RESTRICTED;

            if (userLoginDetails != null && userLoginDetails.Count > 0)
            {
                var userLoginDetail = userLoginDetails[0];

                model.ConfigurationType = userLoginDetail.ConfigurationType;
                model.LoginStartTime = userLoginDetail.LoginStartTime;
                model.LoginEndTime = userLoginDetail.LoginEndTime;
                model.UserName = userLoginDetail.UserName;

                if (model.LoginStartTime != null && model.LoginStartTime.HasValue)
                {
                    DateTime startTime = DateTime.Today.Add(model.LoginStartTime.Value);
                    model.LoginStartTimeText = startTime.ToString("HH:mm");
                }

                if (model.LoginEndTime != null && model.LoginEndTime.HasValue)
                {
                    DateTime endTime = DateTime.Today.Add(model.LoginEndTime.Value);
                    model.LoginEndTimeText = endTime.ToString("HH:mm");
                }

                //Get user's login time configuration setting
                var loginTimeConfigurations = userRepository.GetUserLoginTimeConfigurationByTutor(LogInManager.LoggedInUser.Id, null);
                if (loginTimeConfigurations != null && loginTimeConfigurations.Count > 0)
                {
                    foreach(var item in loginTimeConfigurations)
                    {
                        if (item.IsAllowLogin)
                        {
                            DateTime startTime = DateTime.Today.Add(item.LoginStartTime.Value);
                            item.LoginStartTimeText = startTime.ToString("HH:mm");

                            DateTime endTime = DateTime.Today.Add(item.LoginEndTime.Value);
                            item.LoginEndTimeText = endTime.ToString("HH:mm");
                        }                        
                    }

                    model.Configurations = loginTimeConfigurations;                    
                }
                else
                {
                    List<UserLoginTimeConfigurationVM> configurations = new List<UserLoginTimeConfigurationVM>();
                    for (int i = 0; i < 7; i++) // Sunday to Saturday
                    {
                        UserLoginTimeConfigurationVM objTimeConfiguration = new UserLoginTimeConfigurationVM();
                        objTimeConfiguration.WeekDay = i;
                        objTimeConfiguration.IsAllowLogin = false;
                        objTimeConfiguration.LoginStartTimeText = string.Empty;
                        objTimeConfiguration.LoginEndTimeText = string.Empty;

                        configurations.Add(objTimeConfiguration);
                    }
                    model.Configurations = configurations;
                }
            }
            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLoginTime(UserLoginTimeVM model)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;

                if (model.ConfigurationType == UserLoginConfigurationType.SET_LIMIT)
                {
                    #region Add/Update Time Configuration Setting

                    if (model.Configurations != null && model.Configurations.Count > 0)
                    {
                        var loginTimeConfigurations = userRepository.GetUserLoginTimeConfigurationByTutor(LogInManager.LoggedInUser.Id, null);
                        if (loginTimeConfigurations != null && loginTimeConfigurations.Count > 0)
                        {
                            for (int i = 0; i < 7; i++)  // Sunday to Saturday
                            {
                                var existingTimeConfiguration = loginTimeConfigurations.Where(p => p.WeekDay == i).FirstOrDefault();
                                var newTimeConfiguration = model.Configurations.Where(p => p.WeekDay == i).FirstOrDefault();

                                if (existingTimeConfiguration != null)
                                {
                                    existingTimeConfiguration.IsAllowLogin = newTimeConfiguration.IsAllowLogin;

                                    if (existingTimeConfiguration.IsAllowLogin)
                                    {
                                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

                                        string dateStart = (todayDate + " " + newTimeConfiguration.LoginStartTimeText);
                                        startTime = Convert.ToDateTime(dateStart);

                                        string dateEnd = (todayDate + " " + newTimeConfiguration.LoginEndTimeText);
                                        endTime = Convert.ToDateTime(dateEnd);

                                        existingTimeConfiguration.LoginStartTime = startTime.TimeOfDay;
                                        existingTimeConfiguration.LoginEndTime = endTime.TimeOfDay;
                                    }
                                    else
                                    {
                                        existingTimeConfiguration.LoginStartTime = null;
                                        existingTimeConfiguration.LoginEndTime = null;
                                    }

                                    existingTimeConfiguration.TutorId = LogInManager.LoggedInUser.Id;
                                    existingTimeConfiguration.WeekDay = newTimeConfiguration.WeekDay;                                    
                                    existingTimeConfiguration.UpdatedBy = LogInManager.LoggedInUserId;

                                    //Update existing configuration
                                    userRepository.UpdateUserLoginTimeConfiguration(existingTimeConfiguration);
                                }
                                else
                                {
                                    if(newTimeConfiguration.IsAllowLogin)
                                    {
                                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

                                        string dateStart = (todayDate + " " + newTimeConfiguration.LoginStartTimeText);
                                        startTime = Convert.ToDateTime(dateStart);

                                        string dateEnd = (todayDate + " " + newTimeConfiguration.LoginEndTimeText);
                                        endTime = Convert.ToDateTime(dateEnd);

                                        newTimeConfiguration.LoginStartTime = startTime.TimeOfDay;
                                        newTimeConfiguration.LoginEndTime = endTime.TimeOfDay;
                                    }
                                    else
                                    {
                                        newTimeConfiguration.LoginStartTime = null;
                                        newTimeConfiguration.LoginEndTime = null;
                                    }

                                    newTimeConfiguration.TutorId = LogInManager.LoggedInUser.Id;
                                    newTimeConfiguration.CreatedBy = LogInManager.LoggedInUserId;

                                    //Add new configuration
                                    userRepository.AddUserLoginTimeConfiguration(newTimeConfiguration);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++) // Sunday to Saturday
                            {
                                var newTimeConfiguration = model.Configurations.Where(p => p.WeekDay == i).FirstOrDefault();

                                if (newTimeConfiguration != null)
                                {
                                    if(newTimeConfiguration.IsAllowLogin == true)
                                    {
                                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

                                        string dateStart = (todayDate + " " + newTimeConfiguration.LoginStartTimeText);
                                        startTime = Convert.ToDateTime(dateStart);

                                        string dateEnd = (todayDate + " " + newTimeConfiguration.LoginEndTimeText);
                                        endTime = Convert.ToDateTime(dateEnd);

                                        newTimeConfiguration.LoginStartTime = startTime.TimeOfDay;
                                        newTimeConfiguration.LoginEndTime = endTime.TimeOfDay;
                                    }
                                    else
                                    {
                                        newTimeConfiguration.LoginStartTime = null;
                                        newTimeConfiguration.LoginEndTime = null;
                                    }
                                    
                                    newTimeConfiguration.TutorId = LogInManager.LoggedInUser.Id;
                                    newTimeConfiguration.CreatedBy = LogInManager.LoggedInUserId;

                                    //Add new configuration
                                    userRepository.AddUserLoginTimeConfiguration(newTimeConfiguration);
                                }
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Delete Time Configuration Setting If Exist

                    userRepository.DeleteUserLoginTimeConfigurationByTutor(LogInManager.LoggedInUser.Id, LogInManager.LoggedInUserId);

                    #endregion
                }

                #region Add / Update User Login Time

                model.LoginStartTime = null;
                model.LoginEndTime = null;
                model.TutorId = LogInManager.LoggedInUser.Id;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                model.IsActive = true;

                userRepository.AddUpdateUserLoginTime(model);

                #endregion
                
                #region Set Configuration to Restricted After End Time Limit Reach

                //if (model.ConfigurationType == UserLoginConfigurationType.SET_LIMIT)
                //{
                //    DateTime currentTime = DateTime.Now;

                //    if(currentTime < startTime)
                //    {   
                //        double totalMinutes = ((endTime - currentTime).TotalMinutes + 1);

                //        //Start Background Schedule Job (Execute only once)
                //        BackgroundJob.Schedule(() => SetStudentLoginTimeToRestrict(model.TutorId), TimeSpan.FromMinutes(totalMinutes));
                //    }
                //    else if(currentTime > startTime && currentTime < endTime) //Between Start & End Time
                //    {
                //        double totalMinutes = ((endTime - currentTime).TotalMinutes + 1);

                //        //Start Background Schedule Job (Execute only once)
                //        BackgroundJob.Schedule(() => SetStudentLoginTimeToRestrict(model.TutorId), TimeSpan.FromMinutes(totalMinutes));
                //    }
                //    else if(endTime < currentTime)
                //    {
                //        DateTime scheduleDate = endTime.AddDays(1);
                //        scheduleDate = scheduleDate.AddMinutes(1);

                //        //Start Background Schedule Job (Execute only once)
                //        BackgroundJob.Schedule(() => SetStudentLoginTimeToRestrict(model.TutorId), scheduleDate.TimeOfDay);
                //    }
                //}

                #endregion

                return Json(new
                {
                    IsSuccess = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "UserLoginTime");

                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        #endregion

        #region Send Login Credentials to Users

        [HttpPost]
        public ActionResult SendLoginCredentials(List<Guid> ids)
        {
            try
            {
                if (ids != null)
                {
                    bool blnMailSend = false;
                    foreach (var id in ids)
                    {
                        var user = userRepository.GetUserDetailById(id).FirstOrDefault();

                        if (user != null)
                        {
                            string bodyMsg = "";
                            string email = string.Empty;
                            email = user.Email;

                            var firstName = string.Empty;

                            using (var sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlTemplates/WelcomeTutor.html")))
                            {
                                bodyMsg = sr.ReadToEnd();

                                if (!string.IsNullOrWhiteSpace(user.Name))
                                {
                                    firstName = user.Name.Split(' ')[0];
                                }
                                bodyMsg = bodyMsg.Replace("[@TutorName]", firstName);
                                bodyMsg = bodyMsg.Replace("[@Email]", email);
                                bodyMsg = bodyMsg.Replace("[@Password]", Utility.Utility.Decrypt(user.Password, Utility.Utility.EncryptionKey));
                                bodyMsg = bodyMsg.Replace("[@CashierName]", LogInManager.UserName);
                            }

                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                //Send Email.
                                string EmailSubject = string.Format("Welcome to Hotelier Hub - {0}", firstName);

                                blnMailSend = SuccessHotelierHub.Utility.Email.sendMail(email, EmailSubject, bodyMsg);

                                if (!blnMailSend)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (blnMailSend)
                    {
                        return Json(new
                        {
                            IsSuccess = true,
                            data = "Login credentials email send successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "Oops! Something went wrong. Email not send."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Please select at least one checkbox to send email."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SendLoginCredentials");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        public bool CheckUserEmailExist(Guid? userId, Guid? userRoleId, string email)
        {
            bool blnAvailable = true;

            var user = userRepository.CheckUserEmailExist(userId, userRoleId, email);

            if (user.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        public bool CheckCashierNumberExist(Guid? userId, string cashierNumber)
        {
            bool blnAvailable = true;

            var user = userRepository.CheckCashierNumberExist(userId, cashierNumber);

            if (user.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        public void SetStudentLoginTimeToRestrict(Guid tutorId)
        {
            UserRepository userRepository = new UserRepository();
            userRepository.RestrictStudentLogin(tutorId);
        }

    }
}

