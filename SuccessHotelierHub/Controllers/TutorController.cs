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

        #endregion

        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult List()
        {
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
        public ActionResult Edit(Guid id)
        {
            var students = userRepository.GetStudentDetailsForTutorMapping(id);

            if (students != null && students.Count > 0)
            {
                ViewBag.Students = students;
                ViewBag.TutorId = id;

                return View();
            }
            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Edit(Guid tutorId, List<TutorStudentMappingVM> models)
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
                        if (!studentIds.Contains(tutorStudentMapping.StudentId))
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
                Utility.Utility.LogError(e, "Edit");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

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

        [HotelierHubAuthorize(Roles = "ADMIN,TUTOR")]
        public ActionResult ViewActivity(Guid id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            SearchUsersActivityLogParametersVM model = new SearchUsersActivityLogParametersVM();
            model.UserId = id;
            model.ActivityDate = DateTime.Now;

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

    }
}

