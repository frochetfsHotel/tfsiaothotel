﻿using System;
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
    public class RateController : Controller
    {
        #region Declaration
        
        private RateRepository rateRepository = new RateRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();

        #endregion

        public ActionResult Create(string source)
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var ratetypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "ID", "RateTypeCode");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RateTypeList = ratetypeList;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomTypeRateTypeMappingVM model)
        {
            try
            {
                string source = string.Empty;
                string url = string.Empty;
                string qid = string.Empty;
                string mappingId = string.Empty;

                model.IsWeekEndPrice = false;

                #region  Check Source Parameters
                if (Request.Form["Source"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.Form["Source"])))
                {
                    source = Convert.ToString(Request.Form["Source"]);

                    if (source == "WeekDayPrice")
                    {
                        TempData["TabName"] = "WeekDayPrice";
                        url = Url.Action("ManagePrice", "Rate");
                        model.IsWeekEndPrice = false;
                    }
                    else if (source == "WeekEndPrice")
                    {
                        TempData["TabName"] = "WeekEndPrice";
                        url = Url.Action("ManagePrice", "Rate");

                        model.IsWeekEndPrice = true;
                    }
                }
                #endregion

                
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Rate Type Mapping Available.

                if (this.CheckRoomTypeRateTypeMappingAvailable(model.Id, model.RoomTypeId, model.RateTypeId, model.IsWeekEndPrice) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Selected mapping already exist.")
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                mappingId = rateRepository.AddRoomTypeRateTypeMapping(model);

                #region Check External URL
                if (!string.IsNullOrWhiteSpace(url))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        IsExternalUrl = true,
                        data = url
                    }, JsonRequestBehavior.AllowGet);
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(mappingId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            MappingId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate details not saved successfully."
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
            var mapping = rateRepository.GetRoomTypeRateTypeMappingId(id);

            RoomTypeRateTypeMappingVM model = new RoomTypeRateTypeMappingVM();

            if (mapping != null && mapping.Count > 0)
            {
                model = mapping[0];

                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
                var ratetypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "ID", "RateTypeCode");

                ViewBag.RoomTypeList = roomTypeList;
                ViewBag.RateTypeList = ratetypeList;

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomTypeRateTypeMappingVM model)
        {
            try
            {
                string source = string.Empty;
                string url = string.Empty;
                string qid = string.Empty;
                string mappingId = string.Empty;

                model.IsWeekEndPrice = false;

                #region  Check Source Parameters
                if (Request.Form["Source"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.Form["Source"])))
                {
                    source = Convert.ToString(Request.Form["Source"]);

                    if (source == "WeekDayPrice")
                    {
                        TempData["TabName"] = "WeekDayPrice";
                        url = Url.Action("ManagePrice", "Rate");
                        model.IsWeekEndPrice = false;
                    }
                    else if (source == "WeekEndPrice")
                    {
                        TempData["TabName"] = "WeekEndPrice";
                        url = Url.Action("ManagePrice", "Rate");

                        model.IsWeekEndPrice = true;
                    }
                }
                #endregion

                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Rate Type Mapping Available.

                if (this.CheckRoomTypeRateTypeMappingAvailable(model.Id, model.RoomTypeId, model.RateTypeId, model.IsWeekEndPrice) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Selected mapping already exist.")
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                mappingId = rateRepository.UpdateRoomTypeRateTypeMapping(model);

                #region Check External URL
                if (!string.IsNullOrWhiteSpace(url))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        IsExternalUrl = true,
                        data = url
                    }, JsonRequestBehavior.AllowGet);
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(mappingId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomTypeId = model.Id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Edit");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string mappingId = string.Empty;

                mappingId = rateRepository.DeleteRoomTypeRateTypeMapping(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(mappingId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            MappingId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Rate not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(List<Guid> ids)
        {
            try
            {
                var isDelete = false;

                if (ids != null)
                {
                    foreach (var id in ids)
                    {
                        rateRepository.DeleteRoomTypeRateTypeMapping(id, LogInManager.LoggedInUserId);
                        isDelete = true;
                    }
                }

                if (isDelete)
                {
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
                        errorMessage = "Rates not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteSelected");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var ratetypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "ID", "RateTypeCode");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RateTypeList = ratetypeList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchRoomTypeRateTypeMappingParametersVM model)
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
                var mappings = rateRepository.SearchRoomTypeRateTypeMapping(model, 
                    Convert.ToString(sortColumn),Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = mappings.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = mappings
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckRoomTypeRateTypeMappingAvailable(Guid? mappingId, Guid roomTypeId, Guid ratetypeId, bool isWeekEndPrice)
        {
            bool blnAvailable = true;

            var mapping = rateRepository.CheckRoomTypeRateTypeMappingAvailable(mappingId, roomTypeId, ratetypeId, isWeekEndPrice);

            if (mapping.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        public ActionResult ManagePrice()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var ratetypeList = new SelectList(rateTypeRepository.GetRateType(string.Empty), "ID", "RateTypeCode");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.RateTypeList = ratetypeList;

            string tabName = string.Empty;

            if (TempData["TabName"] != null) {
                tabName = Convert.ToString(TempData["TabName"]);
            }

            ViewBag.TabName = tabName;

            return View();
        }

        public JsonResult GetWeekDayPrice(Guid roomTypeId, Guid rateTypeId)
        {
            try
            {
                //Get Week Day Price.
                var rate = rateRepository.GetWeekDayPrice(roomTypeId, rateTypeId).FirstOrDefault();

                return Json(new
                {
                    IsSuccess = true,
                    data = rate
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetWeekDayPrice");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}