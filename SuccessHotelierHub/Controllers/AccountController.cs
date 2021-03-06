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
    public class AccountController : Controller
    {
        #region Declaration

        private UserRepository userRepository = new UserRepository();
        private UserGroupRepository userGroupRepository = new UserGroupRepository();
        private PageRepository pageRepository = new PageRepository();
        private UserPageRepository userPageRepository = new UserPageRepository();
        private UserRoleRepository userRoleRepository = new UserRoleRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

        #endregion

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            var cookieEmail = Utility.Utility.ReadCookie("HotelierHubUserEmail");

            try
            {
                if (cookieEmail != null)
                {
                    var result = LogInManager.LoginWithCookie(cookieEmail);

                    switch (result)
                    {
                        case LoginStatus.Success:

                            if (LogInManager.HasRights("STUDENT"))
                            {
                                //Create Dummy Reservation.
                                TempReservation.CreateDummyReservation();
                            }

                            var trackActivityStatus = RecordActivityLog.TrackUsersActivityByUserId(LogInManager.LoggedInUser.Id);

                            if(trackActivityStatus == TrackActivityStatus.Success)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                //Remove Cookie
                                Utility.Utility.RemoveCookie("HotelierHubUserEmail");

                                return RedirectToAction("Login", "Account");
                            }
                        case LoginStatus.Failure:
                        default:
                            //Remove Cookie
                            Utility.Utility.RemoveCookie("HotelierHubUserEmail");

                            //Abandon current sessions.
                            System.Web.HttpContext.Current.Session.Abandon();

                            return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Login");

                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
            return View();
        }

        public ActionResult SignOut()
        {
            RecordActivityLog.RecordActivity(Pages.LOGOUT, "Loggedout successfully.");

            //Here system going to call Session_End event from Global.asax file.
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Session.Abandon();            

            Utility.Utility.RemoveCookie("HotelierHubUserEmail");

            //Clear Session cookie from browser. So it will generate new session id after session expire.
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = "";
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["SFToken"] != null)
            {
                Response.Cookies["SFToken"].Value = "";
                Response.Cookies["SFToken"].Expires = DateTime.Now.AddMonths(-20);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = LogInManager.Login(model.Email, Utility.Utility.Encrypt(model.Password, Utility.Utility.EncryptionKey));

                switch (result)
                {
                    case LoginStatus.Success:
                        RecordActivityLog.RecordActivity(Pages.LOGIN, "Loggedin successfully.");

                        if (LogInManager.HasRights("STUDENT"))
                        {
                            //Create Dummy Reservation.
                            TempReservation.CreateDummyReservation();
                        }

                        if (model.RememberMe)
                        {
                            // Create a new cookie,
                            Utility.Utility.WriteCookie("HotelierHubUserEmail", model.Email, 24); //24 Hours expiration time. 
                        }

                        return Json(new
                        {
                            IsSuccess = true,
                            data = new
                            {
                                UserId = LogInManager.LoggedInUserId
                            }
                        }, JsonRequestBehavior.AllowGet);
                    case LoginStatus.AlreadyLoggedIn:
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "User already logged in!"
                        }, JsonRequestBehavior.AllowGet);
                    case LoginStatus.InvalidLoginTime:         
                                       
                        var msg = "Please note your course is restricted; therefore, you can only access it during the times & day set by your Tutor.";
                        
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = msg
                        }, JsonRequestBehavior.AllowGet);
                    case LoginStatus.Failure:                        
                    default:
                        RecordActivityLog.RecordActivity(Pages.LOGIN, "Login fail.");
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "Invalid Email and Password."
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Login POST");

                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                                .Select(
                                    m => new SelectListItem()
                                    {
                                        Value = m.Id.ToString(),
                                        Text = (m.Name + " - " + m.CurrencyCode)
                                    }
                                ), "Value", "Text").ToList();
            ViewBag.UserGroupList = userGroupList;

            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            UserVM model = new UserVM();

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserVM model)
        {
            try
            {
                #region Get Student Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var studentRoleId = userRoles.Where(m => m.Code == "STUDENT").FirstOrDefault().Id;

                #endregion

                //Check Email Exist.
                #region Check User Email Exist

                if (this.CheckUserEmailExist(model.Id, studentRoleId, model.Email) == true)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Email : {0} already registered.", model.Email)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Max. User Id

                //Get Max. User Id.
                var newUserId = (userRepository.GetMaxUserId()) + 1;
                model.UserId = newUserId;

                #endregion

                #region Generate Cashier Number

                var firstTwoCharactersOfName = !string.IsNullOrWhiteSpace(model.Name) ? model.Name.Substring(0, 2) : "";
                model.CashierNumber = (firstTwoCharactersOfName + newUserId).ToUpper();

                #endregion Generate Cashier Number

                #region Generate New Password
                
                //Generate new password.
                string newPassword = Utility.Utility.GenerateRandomPassword(8);
                model.Password = Utility.Utility.Encrypt(newPassword, Utility.Utility.EncryptionKey);

                #endregion

                #region Get Admin User Id

                int createdBy = 0;

                var adminRoleId = userRoles.Where(m => m.Code == "ADMIN").FirstOrDefault().Id;
                var adminUser = userRepository.GetUserDetailByRoleId(adminRoleId).FirstOrDefault();

                if(adminUser != null)
                {
                    createdBy = adminUser.UserId;
                }

                #endregion
                
                string userId = string.Empty;
                model.CreatedBy = createdBy;

                model.IsActive = false;
                model.IsRecordActivity = true;
                model.IsFromRegistrationPage = true;

                userId = userRepository.AddUserDetail(model);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Add User Role Mapping 

                    UserRoleMappingVM userRoleMapping = new UserRoleMappingVM();
                    userRoleMapping.UserId = Guid.Parse(userId);
                    userRoleMapping.UserRoleId = studentRoleId;
                    userRoleMapping.IsActive = true;
                    userRoleMapping.CreatedBy = createdBy;
                    userRoleMapping.UpdatedBy = createdBy;

                    userRepository.AddUpdateUserRoleMapping(userRoleMapping);

                    #endregion

                    #region Add User Page Access Rights For Profile Page

                    var profilePage = pageRepository.GetPageDetailByPageCode("INDIVIDUALPROFILE").FirstOrDefault();

                    if (profilePage != null)
                    {
                        UserPagesVM userPage = new UserPagesVM();
                        userPage.UserId = Guid.Parse(userId);
                        userPage.PageId = profilePage.Id;
                        if (model.IsAllowToDeleteProfile)
                        {
                            userPage.IsDelete = true;
                        }
                        else
                        {
                            userPage.IsDelete = false;
                        }
                        userPage.IsAdd = true;
                        userPage.IsEdit = true;
                        userPage.IsView = true;
                        userPage.IsActive = true;
                        userPage.CreatedBy = createdBy;

                        userPageRepository.AddUserPages(userPage);
                    }

                    #endregion

                    #region Add Tutor Student Mapping

                    TutorStudentMappingVM tutorStudentMapping = new TutorStudentMappingVM();
                    tutorStudentMapping.TutorId = model.TutorId.Value;
                    tutorStudentMapping.StudentId = Guid.Parse(userId);
                    tutorStudentMapping.UserId = newUserId;
                    tutorStudentMapping.CreatedBy = createdBy;
                    tutorStudentMapping.UpdatedBy = createdBy;
                    tutorStudentMapping.IsActive = true;

                    userRepository.AddUpdateTutorStudentMapping(tutorStudentMapping);

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
                        errorMessage = "You have not registered successfully. Something went wrong!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Register POST");

                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        public bool CheckUserEmailExist(Guid? id, Guid? userRoleId, string email)
        {
            bool blnExist = false;

            var user = userRepository.CheckUserEmailExist(id, userRoleId, email);

            if (user.Any())
            {
                blnExist = true;
            }

            return blnExist;
        }
    }
}