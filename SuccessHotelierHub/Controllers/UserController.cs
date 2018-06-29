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
    public class UserController : Controller
    {
        #region Declaration

        private UserRoleRepository userRoleRepository = new UserRoleRepository();
        private UserRepository userRepository = new UserRepository();
        private UserPageRepository userPageRepository = new UserPageRepository();
        private PageRepository pageRepository = new PageRepository();
        private UserGroupRepository userGroupRepository = new UserGroupRepository();

        #endregion

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            var userRoles = userRoleRepository.GetUserRoles();

            var userRoleList = new SelectList(userRoles, "Id", "Name").ToList();

            var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                                .Select(
                                    m => new SelectListItem()
                                    {
                                        Value = m.Id.ToString(),
                                        Text = (m.Name + " - " + m.CurrencyCode)
                                    }
                                ), "Value", "Text").ToList();

            var studentRoleId = userRoles.Where(m => m.Code == "STUDENT").FirstOrDefault().Id;

            ViewBag.UserRoleList = userRoleList;
            ViewBag.UserGroupList = userGroupList;

            UserVM model = new UserVM();
            if (studentRoleId != null)
            {
                model.UserRoleId = studentRoleId;
                model.Password = Utility.Utility.GenerateRandomPassword(8);
            }
            model.UserGroupId = userGroupRepository.GetUserGroupByName().FirstOrDefault().ID;
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

                #region Check User Email Exist.

                if (this.CheckUserEmailExist(model.Id, model.Email) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Email : {0} already exist.", model.Email)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion


                //Get Max. User Id.
                var newUserId = (userRepository.GetMaxUserId()) + 1;
                model.UserId = newUserId;

                #region Generate Cashier Number

                var firstTwoCharactersOfName = !string.IsNullOrWhiteSpace(model.Name) ? model.Name.Substring(0, 2) : "";
                model.CashierNumber = (firstTwoCharactersOfName + newUserId).ToUpper();

                #endregion Generate Cashier Number

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
                        errorMessage = "User details not saved successfully."
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

                var userRoleList = new SelectList(userRoleRepository.GetUserRoles(), "Id", "Name").ToList();
                var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                                .Select(
                                    m => new SelectListItem()
                                    {
                                        Value = m.Id.ToString(),
                                        Text = (m.Name + " - " + m.CurrencyCode)
                                    }
                                ), "Value", "Text").ToList();

                ViewBag.UserRoleList = userRoleList;
                ViewBag.UserGroupList = userGroupList;

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

                #region Check User Email Exist.

                if (this.CheckUserEmailExist(model.Id, model.Email) == false)
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
                        errorMessage = "User details not updated successfully."
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
                        errorMessage = "User details not deleted successfully."
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
            var userRoleList = new SelectList(userRoleRepository.GetUserRoles(), "Id", "Name").ToList();

            var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                                .Select(
                                    m => new SelectListItem()
                                    {
                                        Value = m.Id.ToString(),
                                        Text = (m.Name + " - " + m.CurrencyCode)
                                    }
                                ), "Value", "Text").ToList();

            ViewBag.UserRoleList = userRoleList;
            ViewBag.UserGroupList = userGroupList;

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

        public bool CheckUserEmailExist(Guid? userId, string email)
        {
            bool blnAvailable = true;

            var user = userRepository.CheckUserEmailExist(userId, email);

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