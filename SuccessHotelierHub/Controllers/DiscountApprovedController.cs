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
    public class DiscountApprovedController : Controller
    {
        #region Declaration

        private DiscountApprovedRepository discountApprovedRepository = new DiscountApprovedRepository();

        #endregion

        // GET: DiscountApproved
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
        public ActionResult Create(DiscountApprovedVM model)
        {
            try
            {
                string discountApprovedId = string.Empty;

                model.Code = Utility.Utility.ToUpperCase(model.Code);
                model.CreatedBy = LogInManager.LoggedInUserId;

                discountApprovedId = discountApprovedRepository.AddDiscountApproved(model);

                if (!string.IsNullOrWhiteSpace(discountApprovedId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            DiscountApprovedId = discountApprovedId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Discount Approved details not saved successfully."
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
            var discountApproved = discountApprovedRepository.GetDiscountApprovedById(id);

            DiscountApprovedVM model = new DiscountApprovedVM();

            if (discountApproved != null && discountApproved.Count > 0)
            {
                model = discountApproved[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiscountApprovedVM model)
        {
            try
            {
                string discountApprovedId = string.Empty;

                model.Code = Utility.Utility.ToUpperCase(model.Code);
                model.UpdatedBy = LogInManager.LoggedInUserId;

                discountApprovedId = discountApprovedRepository.UpdateDiscountApproved(model);

                if (!string.IsNullOrWhiteSpace(discountApprovedId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            DiscountApprovedId = discountApprovedId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Discount Approved details not updated successfully."
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
                string discountApprovedId = string.Empty;

                discountApprovedId = discountApprovedRepository.DeleteDiscountApproved(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(discountApprovedId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            DiscountApprovedId = discountApprovedId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Discount Approved details not deleted successfully."
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
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchDiscountApprovedParametersVM model)
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
                var discountApprovedDetails = discountApprovedRepository.SearchDiscountApproved(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = discountApprovedDetails.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = discountApprovedDetails
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}