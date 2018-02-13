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
    public class RoomTypeController : Controller
    {
        #region Declaration

        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private RateTypeRepository rateTypeRepository = new RateTypeRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private RateRepository rateRepository = new RateRepository();
        #endregion

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomTypeVM model)
        {
            try
            {
                string roomTypeId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Code Available.

                if (this.CheckRoomTypeCodeAvailable(model.Id, model.RoomTypeCode) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Room Type Code : {0} already exist.", model.RoomTypeCode)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                roomTypeId = roomTypeRepository.AddRoomType(model);
                
                if (!string.IsNullOrWhiteSpace(roomTypeId))
                {

                    #region Add Rate (Default 100) for each Rate Type.

                    var rateTypes = rateTypeRepository.GetRateType(string.Empty);

                    if (rateTypes != null && rateTypes.Count > 0)
                    {
                        foreach (var rateType in rateTypes)
                        {
                            RoomTypeRateTypeMappingVM roomTypeRateTypeMapping = new RoomTypeRateTypeMappingVM();
                            roomTypeRateTypeMapping.RoomTypeId = Guid.Parse(roomTypeId);
                            roomTypeRateTypeMapping.RateTypeId = rateType.Id;
                            roomTypeRateTypeMapping.Amount = 100; //Default Price.
                            roomTypeRateTypeMapping.IsActive = true;
                            roomTypeRateTypeMapping.CreatedBy = LogInManager.LoggedInUserId;

                            rateRepository.AddRoomTypeRateTypeMapping(roomTypeRateTypeMapping);
                        }
                    }

                    #endregion

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
                        errorMessage = "Room Type details not saved successfully."
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
            var roomType = roomTypeRepository.GetRoomTypeById(id);

            RoomTypeVM model = new RoomTypeVM();

            if (roomType != null && roomType.Count > 0)
            {
                model = roomType[0];

                return View(model);
            }

            return RedirectToAction("List");

        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomTypeVM model)
        {
            try
            {
                string roomTypeId = string.Empty;

                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Room Type Code Available.

                if (this.CheckRoomTypeCodeAvailable(model.Id, model.RoomTypeCode) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Room Type Code : {0} already exist.", model.RoomTypeCode)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                roomTypeId = roomTypeRepository.UpdateRoomType(model);

                if (!string.IsNullOrWhiteSpace(roomTypeId))
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
                        errorMessage = "Room Type details not updated successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Edit");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string roomTypeId = string.Empty;

                roomTypeId = roomTypeRepository.DeleteRoomType(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(roomTypeId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomTypeId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room Type not deleted successfully."
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
                        roomTypeRepository.DeleteRoomType(id, LogInManager.LoggedInUserId);
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
                        errorMessage = "Room Types not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "DeleteSelected");
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
        public ActionResult Search(SearchRoomTypeParametersVM model)
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
                var roomTypes = roomTypeRepository.SearchRoomType(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = roomTypes.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = roomTypes
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckRoomTypeCodeAvailable(Guid? roomTypeId, string roomTypeCode)
        {
            bool blnAvailable = true;

            var roomType = roomTypeRepository.CheckRoomTypeCodeAvailable(roomTypeId, roomTypeCode);

            if (roomType.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        [HttpPost]
        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult SearchAdvanceRoomType(SearchAdvanceRoomTypeParametersVM model)
        {
            try
            {
                var roomTypes = roomTypeRepository.SearchAdvanceRoomType(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = roomTypes
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchAdvanceRoomType");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult RoomStatus(Guid roomTypeId)
        {
            try
            {
                var roomType = roomTypeRepository.GetRoomTypeById(roomTypeId).FirstOrDefault();

                ViewData["RoomTypeInfo"] = roomType;

                return PartialView("_RoomTypeStatus");
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "RoomStatus");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
            
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult GetRoomTypeInfo(Guid id)
        {
            try
            {

                var roomType = roomTypeRepository.GetRoomTypeById(id).FirstOrDefault();

                return Json(new
                {
                    IsSuccess = true,
                    data = roomType
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "GetRoomTypeInfo");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}