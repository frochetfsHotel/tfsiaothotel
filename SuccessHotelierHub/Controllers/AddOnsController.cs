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
    public class AddOnsController : Controller
    {
        #region Declaration

        private AddOnsRepository addOnsRepository = new AddOnsRepository();

        #endregion

        // GET: AddOns
        public ActionResult Index()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {   
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddOnsVM model)
        {
            try
            {
                string addOnsId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                addOnsId = addOnsRepository.AddAddOns(model);

                if (!string.IsNullOrWhiteSpace(addOnsId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            AddOnsId = addOnsId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Add Ons details not saved successfully."
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
            var addOns = addOnsRepository.GetAddOnsById(id);

            AddOnsVM model = new AddOnsVM();

            if (addOns != null && addOns.Count > 0)
            {
                model = addOns[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddOnsVM model)
        {
            try
            {
                string addOnsId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                addOnsId = addOnsRepository.UpdateAddOns(model);

                if (!string.IsNullOrWhiteSpace(addOnsId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            AddOnsId = addOnsId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Add Ons details not updated successfully."
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
                string addOnsId = string.Empty;

                addOnsId = addOnsRepository.DeleteAddOns(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(addOnsId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            AddOnsId = addOnsId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Add Ons details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult List()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Search(SearchAddOnsParametersVM model)
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
                var addOns = addOnsRepository.SearchAddOns(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = addOns.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = addOns
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult SearchAdvanceAddOns(SearchAdvanceAddOnsParametersVM model)
        {
            try
            {
                var addOns = addOnsRepository.SearchAdvanceAddOns(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = addOns
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchAdvanceAddOns");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}