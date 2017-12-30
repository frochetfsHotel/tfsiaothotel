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

    public class RateTypeController : Controller
    {
        #region Declaration

        private RateTypeRepository rateTypeRepository = new RateTypeRepository();

        #endregion

        // GET: RateType
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RateTypeVM model)
        {
            try
            {
                string rateTypeId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Rate Type Code Available.

                if (this.CheckRateTypeCodeAvailable(model.Id, model.RateTypeCode) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Rate Type Code : {0} already exist.", model.RateTypeCode)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                rateTypeId = rateTypeRepository.AddRateType(model);

                if (!string.IsNullOrWhiteSpace(rateTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RateTypeId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate Type details not saved successfully."
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
            var rateType = rateTypeRepository.GetRateTypeById(id);

            RateTypeVM model = new RateTypeVM();

            if (rateType != null && rateType.Count > 0)
            {
                model = rateType[0];

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RateTypeVM model)
        {
            try
            {
                string rateTypeId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Rate Type Code Available.

                if (this.CheckRateTypeCodeAvailable(model.Id, model.RateTypeCode) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Rate Type Code : {0} already exist.", model.RateTypeCode)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                rateTypeId = rateTypeRepository.UpdateRateType(model);

                if (!string.IsNullOrWhiteSpace(rateTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RateTypeId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate Type details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string rateTypeId = string.Empty;

                rateTypeId = rateTypeRepository.DeleteRateType(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(rateTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RateTypeId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate Type not deleted successfully."
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
        public ActionResult Search(SearchRateTypeParametersVM model)
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
                var rateTypes = rateTypeRepository.SearchRateType(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = rateTypes.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = rateTypes
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckRateTypeCodeAvailable(Guid? rateTypeId, string rateTypeCode)
        {
            bool blnAvailable = true;

            var rateType = rateTypeRepository.CheckRateTypeCodeAvailable(rateTypeId, rateTypeCode);

            if (rateType.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}