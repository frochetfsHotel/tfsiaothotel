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

            if(tutor != null)
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
                    return Json(new { IsSuccess = false, errorMessage = "User details not exist."});
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

            if(userDetail != null)
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
        public ActionResult UserLoginTime(Guid id)
        {
            var userLoginDetails = userRepository.GetUserLoginTimeByUser(id);

            UserLoginTimeVM model = new UserLoginTimeVM();

            if (userLoginDetails != null && userLoginDetails.Count > 0)
            {
                var userLoginDetail = userLoginDetails[0];
                
                model.UserId = userLoginDetail.UserId;
                model.LoginStartTime = userLoginDetail.LoginStartTime;
                model.LoginEndTime = userLoginDetail.LoginEndTime;
                model.UserName = userLoginDetail.UserName;
                
                if (model.LoginStartTime != null)
                {
                    DateTime startTime = DateTime.Today.Add(model.LoginStartTime);
                    model.LoginStartTimeText = startTime.ToString("HH:mm");
                }
                
                if (model.LoginEndTime != null)
                {
                    DateTime endTime = DateTime.Today.Add(model.LoginEndTime);
                    model.LoginEndTimeText = endTime.ToString("HH:mm");
                }

                return View(model);
            }
            else
            {
                var user = userRepository.GetUserDetailById(id).FirstOrDefault();

                if(user != null)
                {
                    model.UserId = user.Id;
                    model.UserName = user.Name;
                }

                model.LoginStartTimeText = UserLoginTimeInfo.DEFAULT_LOGIN_START_TIME;
                model.LoginEndTimeText = UserLoginTimeInfo.DEFAULT_LOGIN_END_TIME;
                
                if (!string.IsNullOrWhiteSpace(model.LoginStartTimeText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + model.LoginStartTimeText);
                    DateTime time = Convert.ToDateTime(date);

                    model.LoginStartTime = time.TimeOfDay;
                }

                if (!string.IsNullOrWhiteSpace(model.LoginEndTimeText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + model.LoginEndTimeText);
                    DateTime time = Convert.ToDateTime(date);

                    model.LoginEndTime = time.TimeOfDay;
                }

                return View(model);
            }
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

                if (!string.IsNullOrWhiteSpace(model.LoginStartTimeText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + model.LoginStartTimeText);
                    startTime = Convert.ToDateTime(date);

                    model.LoginStartTime = startTime.TimeOfDay;
                }

                if (!string.IsNullOrWhiteSpace(model.LoginEndTimeText))
                {
                    string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string date = (todayDate + " " + model.LoginEndTimeText);
                    endTime = Convert.ToDateTime(date);

                    model.LoginEndTime = endTime.TimeOfDay;
                }

                if(endTime < startTime)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Login start time must be less than login end time."
                    });
                }

                #region Add / Update User Login Time

                model.CreatedBy = LogInManager.LoggedInUserId;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                model.IsActive = true;

                userRepository.AddUpdateUserLoginTime(model);                

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

    }
}

