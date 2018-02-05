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

        #endregion

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            var userRoleList = new SelectList(userRoleRepository.GetUserRoles(), "Id", "Name").ToList();

            ViewBag.UserRoleList = userRoleList;

            return View();
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

                ViewBag.UserRoleList = userRoleList;

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

            ViewBag.UserRoleList = userRoleList;

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
    }
}