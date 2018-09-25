using System;
using System.Linq;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;

namespace SuccessHotelierHub.Controllers
{
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class CollegeGroupController : Controller
    {
        #region Declaration

        private UserGroupRepository userGroupRepository = new UserGroupRepository();
        private CollegeGroupRepository collegeGroupRepository = new CollegeGroupRepository();
        private UserRepository userRepository = new UserRepository();

        #endregion

        // GET: CollegeGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
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

            CollegeGroupVM model = new CollegeGroupVM();
            model.UserGroupId = userGroupRepository.GetUserGroupByName().FirstOrDefault().ID;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollegeGroupVM model)
        {
            try
            {
                string collegeGroupId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check College Group Name Exist.

                if (this.CheckCollegeGroupNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                collegeGroupId = collegeGroupRepository.AddCollegeGroup(model);

                if (!string.IsNullOrWhiteSpace(collegeGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CollegeGroupId = collegeGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "College Group details not saved successfully."
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
            var collegeGroup = collegeGroupRepository.GetCollegeGroupById(id);

            CollegeGroupVM model = new CollegeGroupVM();

            if (collegeGroup != null)
            {
                model = collegeGroup;

                var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                                .Select(
                                    m => new SelectListItem()
                                    {
                                        Value = m.Id.ToString(),
                                        Text = (m.Name + " - " + m.CurrencyCode)
                                    }
                                ), "Value", "Text").ToList();

                ViewBag.UserGroupList = userGroupList;

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollegeGroupVM model)
        {
            try
            {
                string collegeGroupId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check College Group Name Exist.

                if (this.CheckCollegeGroupNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion                

                collegeGroupId = collegeGroupRepository.UpdateCollegeGroup(model);

                if (!string.IsNullOrWhiteSpace(collegeGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CollegeGroupId = collegeGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "College Group details not updated successfully."
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
                string collegeGroupId = string.Empty;

                #region Check User Mapping Exist

                var users = userRepository.GetUserDetailByCollegeGroupId(id);

                if (users != null && users.Count > 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "You can not delete this college group because there are one or more students / tutors assigned with this college group."
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                collegeGroupId = collegeGroupRepository.DeleteCollegeGroup(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(collegeGroupId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CollegeGroupId = collegeGroupId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "College Group details not deleted successfully."
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
            var userGroupList = new SelectList(userGroupRepository.GetUserGroupsWithCurrencyInfo()
                               .Select(
                                   m => new SelectListItem()
                                   {
                                       Value = m.Id.ToString(),
                                       Text = (m.Name + " - " + m.CurrencyCode)
                                   }
                               ), "Value", "Text").ToList();

            ViewBag.UserGroupList = userGroupList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCollegeGroupParametersVM model)
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
                var collegeGroups = collegeGroupRepository.SearchCollegeGroup(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = collegeGroups.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = collegeGroups
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckCollegeGroupNameExist(Guid? collegeGroupId, string name)
        {
            bool blnAvailable = true;

            var collegeGroup = collegeGroupRepository.CheckCollegeGroupNameExist(collegeGroupId, name);

            if (collegeGroup.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}