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
    public class RateController : Controller
    {
        #region Declaration
        
        private RateRepository rateRepository = new RateRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();

        #endregion

        public ActionResult Create()
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
                string mappingId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Rate Type Mapping Avaiable.

                if (this.CheckRoomTypeRateTypeMappingAvaiable(model.Id, model.RoomTypeId, model.RateTypeId) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Selected mapping already exist.")
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                mappingId = rateRepository.AddRoomTypeRateTypeMapping(model);

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
                string mappingId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Rate Type Mapping Avaiable.

                if (this.CheckRoomTypeRateTypeMappingAvaiable(model.Id, model.RoomTypeId, model.RateTypeId) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Selected mapping already exist.")
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                mappingId = rateRepository.UpdateRoomTypeRateTypeMapping(model);

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckRoomTypeRateTypeMappingAvaiable(Guid? mappingId, Guid roomTypeId, Guid ratetypeId)
        {
            bool blnAvailable = true;

            var mapping = rateRepository.CheckRoomTypeRateTypeMappingAvaiable(mappingId, roomTypeId, ratetypeId);

            if (mapping.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}