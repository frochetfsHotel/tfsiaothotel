using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;
using System.IO;

namespace SuccessHotelierHub.Controllers
{
    public class UserController : Controller
    {
        #region Declaration

        private UserRoleRepository userRoleRepository = new UserRoleRepository();
        private UserRepository userRepository = new UserRepository();
        private UserPageRepository userPageRepository = new UserPageRepository();
        private PageRepository pageRepository = new PageRepository();
        private UserGroupRepository userGroupRepository = new UserGroupRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

        #endregion

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        #region Student CRUD

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

                #region Get Student Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var studentRoleId = userRoles.Where(m => m.Code == "STUDENT").FirstOrDefault().Id;
                model.UserRoleId = studentRoleId;

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

                    #region Add User Page Access Rights For Profile

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
                        userPage.CreatedBy = LogInManager.LoggedInUserId;

                        userPageRepository.AddUserPages(userPage);
                    }


                    #endregion

                    #region Add Tutor Student Mapping

                    TutorStudentMappingVM tutorStudentMapping = new TutorStudentMappingVM();
                    tutorStudentMapping.TutorId = model.TutorId.Value;
                    tutorStudentMapping.StudentId = Guid.Parse(userId);
                    tutorStudentMapping.UserId = newUserId;
                    tutorStudentMapping.CreatedBy = LogInManager.LoggedInUserId;
                    tutorStudentMapping.UpdatedBy = LogInManager.LoggedInUserId;
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
                        errorMessage = "Student details not saved successfully."
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

                var tutorList = new SelectList(userRepository.GetTutorDetailByCollegeGroupId(model.CollegeGroupId.Value), "Id", "Name");
                ViewBag.TutorList = tutorList;

                //Get existing tutor student mapping.
                var tutorStudentMappings = userRepository.GetTutorStudentMappingByTutor(null, model.Id).FirstOrDefault();

                if (tutorStudentMappings != null)
                {
                    model.TutorId = tutorStudentMappings.TutorId;
                }

                //Check Delete Access For Profile Page
                var profilePageAccessRights = userPageRepository.GetUserPageAccessRights(id, "INDIVIDUALPROFILE").FirstOrDefault();

                if (profilePageAccessRights != null)
                {
                    model.IsAllowToDeleteProfile = profilePageAccessRights.IsDelete;
                }
                else
                {
                    model.IsAllowToDeleteProfile = false;
                }

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

                #region Get Student Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var studentRoleId = userRoles.Where(m => m.Code == "STUDENT").FirstOrDefault().Id;
                model.UserRoleId = studentRoleId;

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

                    #region Add/Update User Page Access Rights For Profile

                    var profilePage = pageRepository.GetPageDetailByPageCode("INDIVIDUALPROFILE").FirstOrDefault();

                    var profilePageAccessRights = userPageRepository.GetUserPageAccessRights(Guid.Parse(userId), "INDIVIDUALPROFILE").FirstOrDefault();

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
                        userPage.CreatedBy = LogInManager.LoggedInUserId;
                        userPage.UpdatedBy = LogInManager.LoggedInUserId;

                        if (profilePageAccessRights == null)
                        {
                            userPageRepository.AddUserPages(userPage);
                        }
                        else
                        {
                            userPage.Id = profilePageAccessRights.Id;
                            userPageRepository.AddUserPages(userPage);
                        }
                    }



                    #endregion

                    #region Add/Update Tutor Student Mapping

                    TutorStudentMappingVM tutorStudentMapping = new TutorStudentMappingVM();
                    tutorStudentMapping.TutorId = model.TutorId.Value;
                    tutorStudentMapping.StudentId = Guid.Parse(userId);
                    tutorStudentMapping.UserId = model.UserId;
                    tutorStudentMapping.CreatedBy = LogInManager.LoggedInUserId;
                    tutorStudentMapping.UpdatedBy = LogInManager.LoggedInUserId;
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
                        errorMessage = "Student details not updated successfully."
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

                userId = userRepository.DeleteUserDetail(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Delete User Role Mapping

                    userRepository.DeleteUserRoleMappingByUserId(id, LogInManager.LoggedInUserId);

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
                        errorMessage = "Student details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult List()
        {
            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        [HttpPost]
        public ActionResult Search(SearchUserDetailParametersVM model)
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
                var users = userRepository.SearchUserDetail(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = users.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        if (!string.IsNullOrWhiteSpace(user.Password))
                            user.Password = Utility.Utility.Decrypt(user.Password, Utility.Utility.EncryptionKey);
                    }
                }

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = users
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion
        
        #region Change Password

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult ChangePassword(Guid id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ChangePasswordVM model = new ChangePasswordVM();
            model.UserId = id;

            return View(model);
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var userDetail = userRepository.GetUserDetailById(model.UserId).FirstOrDefault();

                if (userDetail != null)
                {
                    #region Compare Old Password
                    if (userDetail.Password != Utility.Utility.Encrypt(model.OldPassword, Utility.Utility.EncryptionKey))
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            errorMessage = "Old password does not match with original password."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                    string userId = string.Empty;
                    model.UpdatedBy = LogInManager.LoggedInUserId;

                    model.Password = Utility.Utility.Encrypt(model.Password, Utility.Utility.EncryptionKey);

                    userId = userRepository.ChangePassword(model);

                    if (!string.IsNullOrWhiteSpace(userId))
                    {

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
                            errorMessage = "User password not changed successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User details not exist."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ChangePassword");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        #endregion

        #region Generate New Password

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult CreateNewPassword(Guid id)
        {
            try
            {
                var userDetail = userRepository.GetUserDetailById(id).FirstOrDefault();

                if (userDetail != null)
                {
                    string userId = "";

                    //Generate new password.
                    string newPassword = Utility.Utility.GenerateRandomPassword(8);

                    ChangePasswordVM model = new ChangePasswordVM();

                    model.UserId = userDetail.Id;
                    model.UpdatedBy = LogInManager.LoggedInUserId;

                    model.Password = Utility.Utility.Encrypt(newPassword, Utility.Utility.EncryptionKey);

                    userId = userRepository.ChangePassword(model);

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
                        errorMessage = "New password not created successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "CreateNewPassword");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        #endregion

        #region Get Tutor Info By College Group

        public ActionResult GetTutorDetailByCollegeGroupId(Guid collegeGroupId)
        {
            try
            {
                var tutorList = userRepository.GetTutorDetailByCollegeGroupId(collegeGroupId);

                if (tutorList != null && tutorList.Count() > 0)
                    return Json(new { IsSuccess = true, data = tutorList.ToList() }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { IsSuccess = false, data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetTutorDetailByCollegeGroupId");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
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

                        if(user != null)
                        {
                            string bodyMsg = "";
                            string email = string.Empty;
                            email = user.Email;

                            var firstName = string.Empty;

                            using (var sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/HtmlTemplates/WelcomeUser.html")))
                            {
                                bodyMsg = sr.ReadToEnd();
                                
                                if(!string.IsNullOrWhiteSpace(user.Name))
                                {
                                    firstName = user.Name.Split(' ')[0];
                                }
                                bodyMsg = bodyMsg.Replace("[@UserName]", firstName);
                                bodyMsg = bodyMsg.Replace("[@Email]", email);
                                bodyMsg = bodyMsg.Replace("[@Password]", Utility.Utility.Decrypt(user.Password, Utility.Utility.EncryptionKey));
                                bodyMsg = bodyMsg.Replace("[@CashierName]", LogInManager.UserName);
                            }

                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                //Send Email.
                                string EmailSubject = string.Format("Welcome to Hotelier Hub - {0}", firstName);

                                blnMailSend = SuccessHotelierHub.Utility.Email.sendMail(email, EmailSubject, bodyMsg);

                                if (blnMailSend)
                                {
                                    //Update flags : IsLoginCredentialSent = true, IsActive = true.
                                    userRepository.UpdateLoginCredentialSentFlag(user.Id, true);
                                }
                                else
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
        
        #region Admin CRUD

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult CreateAdmin()
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
        public ActionResult CreateAdmin(UserVM model)
        {
            try
            {
                string userId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;
                model.Password = Utility.Utility.Encrypt(model.Password, Utility.Utility.EncryptionKey);

                #region Get Admin Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var adminRoleId = userRoles.Where(m => m.Code == "ADMIN").FirstOrDefault().Id;
                model.UserRoleId = adminRoleId;

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
                        errorMessage = "Admin details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "CreateAdmin");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult EditAdmin(Guid id)
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
        public ActionResult EditAdmin(UserVM model)
        {
            try
            {
                string userId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                model.IsRecordActivity = true;

                #region Get Admin Role Id

                var userRoles = userRoleRepository.GetUserRoles();
                var adminRoleId = userRoles.Where(m => m.Code == "ADMIN").FirstOrDefault().Id;
                model.UserRoleId = adminRoleId;

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
                        errorMessage = "Admin details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "EditAdmin");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult ListAdmin()
        {
            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult SearchAdmin(SearchAdminDetailParametersVM model)
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
                var admins = userRepository.SearchAdminDetail(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = admins.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                if (admins != null && admins.Count > 0)
                {
                    foreach (var admin in admins)
                    {
                        if (!string.IsNullOrWhiteSpace(admin.Password))
                            admin.Password = Utility.Utility.Decrypt(admin.Password, Utility.Utility.EncryptionKey);
                    }
                }

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = admins
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchAdmin");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult DeleteAdmin(Guid id)
        {
            try
            {
                string userId = string.Empty;                

                userId = userRepository.DeleteUserDetail(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    #region Delete User Role Mapping

                    userRepository.DeleteUserRoleMappingByUserId(id, LogInManager.LoggedInUserId);

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
                        errorMessage = "Admin details not deleted successfully."
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

        #region Logged In User Info

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult LoggedInUserInfo()
        {
            var collegeGroupList = new SelectList(collegeGroupRepository.GetCollegeGroups(), "Id", "Name").ToList();
            ViewBag.CollegeGroupList = collegeGroupList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult LoggedInUserInfo(SearchLoggedInUserInfoParametersVM model)
        {
            try
            {
                object sortColumn = "";
                object sortDirection = "";

                if (model.order.Count == 0)
                {
                    sortColumn = "LastLoggedOn";
                    sortDirection = "desc";
                }
                else
                {
                    sortColumn = model.columns[Convert.ToInt32(model.order[0].column)].data ?? (object)DBNull.Value;
                    sortDirection = model.order[0].dir ?? (object)DBNull.Value;
                }

                model.PageSize = Constants.PAGESIZE;
                var users = userRepository.SearchLoggedInUserInfo(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));
                
                int totalRecords = 0;
                var dbRecords = users.Select(m => m.TotalCount).FirstOrDefault();
                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                //Get Total Users (student)
                var loggedInUsers = userRepository.GetLoggedInUserInfo(model);

                int totalUsers = 0;
                var dbTotalUsers = loggedInUsers.Count;
                if (dbTotalUsers != 0)
                    totalUsers = Convert.ToInt32(dbTotalUsers);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    TotalUsers = totalUsers,
                    data = users
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "LoggedInUserInfo");
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
    }
}