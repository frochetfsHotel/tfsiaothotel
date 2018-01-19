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
    [HotelierHubAuthorize]
    public class UserRoleController : Controller
    {
        #region Declaration

        private UserRoleRepository userRoleRepository = new UserRoleRepository();

        #endregion


        // GET: UserRole
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRoleVM model)
        {
            try
            {
                string userRoleId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check User Role Code Available.

                if (this.CheckUserRoleCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                userRoleId = userRoleRepository.AddUserRole(model);

                if (!string.IsNullOrWhiteSpace(userRoleId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserRoleId = userRoleId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Role details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        public ActionResult Edit(Guid id)
        {
            var userRole = userRoleRepository.GetUserRoleById(id);

            UserRoleVM model = new UserRoleVM();

            if (userRole != null && userRole.Count > 0)
            {
                model = userRole[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRoleVM model)
        {
            try
            {
                string userRoleId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check User Role Code Available.

                if (this.CheckUserRoleCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                userRoleId = userRoleRepository.UpdateUserRole(model);

                if (!string.IsNullOrWhiteSpace(userRoleId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserRoleId = userRoleId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Role details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string userRoleId = string.Empty;

                userRoleId = userRoleRepository.DeleteUserRole(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(userRoleId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserRoleId = userRoleId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Role details not deleted successfully."
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
        public ActionResult Search(SearchUserRoleParametersVM model)
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
                var userRoles = userRoleRepository.SearchUserRole(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = userRoles.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = userRoles
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckUserRoleCodeAvailable(Guid? userRoleId, string code)
        {
            bool blnAvailable = true;

            var userRole = userRoleRepository.CheckUserRoleCodeAvailable(userRoleId, code);

            if (userRole.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}