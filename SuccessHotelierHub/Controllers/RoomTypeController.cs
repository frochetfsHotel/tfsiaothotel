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
    public class RoomTypeController : Controller
    {
        #region Declaration

        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();

        #endregion
        
        public ActionResult Create()
        {
            return View();
        }

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
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

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
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            return View();
        }

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
    }
}