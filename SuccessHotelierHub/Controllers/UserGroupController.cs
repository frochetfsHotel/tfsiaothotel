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
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class UserGroupController : Controller
    {
        #region Declaration
        
        private UserGroupRepository userGroupRepository = new UserGroupRepository();
        private CurrencyRepository currencyRepository = new CurrencyRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();

        #endregion

        // GET: UserGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {   
            var currencyList = new SelectList(currencyRepository.GetCurrencyInfo()
                            .Select(
                                m => new SelectListItem()
                                {
                                    Value = m.Id.ToString(),
                                    Text = (m.Code + " (" + Server.HtmlDecode(m.CurrencySymbol) + ") - " + m.Name)
                                }
                            ), "Value", "Text").ToList();

            ViewBag.CurrencyList = currencyList;            
                     
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserGroupVM model)
        {
            try
            {
                string userGroupId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check User Group Name Exist.

                if (this.CheckUserGroupNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion
                
                userGroupId = userGroupRepository.AddUserGroup(model);

                if (!string.IsNullOrWhiteSpace(userGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserGroupId = userGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Group details not saved successfully."
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

        public ActionResult Edit(Guid id)
        {
            var userGroup = userGroupRepository.GetUserGroupById(id);

            UserGroupVM model = new UserGroupVM();

            if (userGroup != null)
            {
                model = userGroup;

                var currencyList = new SelectList(currencyRepository.GetCurrencyInfo()
                            .Select(
                                m => new SelectListItem()
                                {
                                    Value = m.Id.ToString(),
                                    Text = (m.Code + " (" + Server.HtmlDecode(m.CurrencySymbol) + ") - " + m.Name)
                                }
                            ), "Value", "Text").ToList();

                ViewBag.CurrencyList = currencyList;
                
                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserGroupVM model)
        {
            try
            {
                string userGroupId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check User Group Name Exist.

                if (this.CheckUserGroupNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion                

                userGroupId = userGroupRepository.UpdateUserGroup(model);

                if (!string.IsNullOrWhiteSpace(userGroupId))
                {   
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserGroupId = userGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Group details not updated successfully."
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

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string userGroupId = string.Empty;

                #region Check College Group Mapping Exist

                var collegeGroups = collegeGroupRepository.GetCollegeGroupByUserGroupId(id);

                if (collegeGroups != null && collegeGroups.Count > 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "You can not delete this user group because there are one or more colleges assigned with this user group."
                    }, JsonRequestBehavior.AllowGet);
                }
                #endregion

                userGroupId = userGroupRepository.DeleteUserGroup(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(userGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            UserGroupId = userGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "User Group details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            var currencyList = new SelectList(currencyRepository.GetCurrencyInfo()
                        .Select(
                            m => new SelectListItem()
                            {
                                Value = m.Id.ToString(),
                                Text = (m.Code + " (" + Server.HtmlDecode(m.CurrencySymbol) + ") - " + m.Name)
                            }
                        ), "Value", "Text").ToList();

            ViewBag.CurrencyList = currencyList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchUserGroupParametersVM model)
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
                var userGroups = userGroupRepository.SearchUserGroup(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = userGroups.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = userGroups
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckUserGroupNameExist(Guid? userGroupId, string name)
        {
            bool blnAvailable = true;

            var userGroup = userGroupRepository.CheckUserGroupNameExist(userGroupId, name);

            if (userGroup.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}