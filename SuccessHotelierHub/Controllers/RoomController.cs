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
    [HotelierHubAuthorize]
    public class RoomController : Controller
    {
        #region Declaration 

        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private FloorRepository floorRepository = new FloorRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private RoomStatusRepository roomStatusRepository = new RoomStatusRepository();

        #endregion

        #region Room

        public ActionResult Index()
        {
            var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);

            ViewBag.RoomTypeList = roomTypeList;

            return View();
        }

        public ActionResult ShowRoomList(Guid roomTypeId)
        {
            var roomType = roomTypeRepository.GetRoomTypeById(roomTypeId).FirstOrDefault();

            if (roomType != null)
            {
                var roomList = roomRepository.GetRoomByRoomTypeId(roomType.Id);                
                var floorList = floorRepository.GetFloors();
                var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);
                

                ViewBag.RoomList = roomList;                
                ViewBag.FloorList = floorList;
                ViewBag.RoomTypeList = roomTypeList;

                return PartialView("_RoomList");
            }

            return Json(new
            {
                IsSuccess = false,
                errorMessage = "Not found any rooms details."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string roomId = string.Empty;

                var roomDetail = roomRepository.GetRoomById(id).FirstOrDefault();

                roomId = roomRepository.DeleteRoom(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(roomId))
                {
                    #region Update Floor (No. Of Rooms)

                    //Decrease the no. of rooms quantity from floor.
                    var floor = floorRepository.GetFloorById(roomDetail.FloorId).FirstOrDefault();

                    if (floor != null)
                    {
                        floor.NoOfRoom = floor.NoOfRoom > 0 ? (floor.NoOfRoom - 1) : 0;

                        floorRepository.UpdateFloor(floor);
                    }

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomId = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
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
                        var roomDetail = roomRepository.GetRoomById(id).FirstOrDefault();

                        var roomId = roomRepository.DeleteRoom(id, LogInManager.LoggedInUserId);

                        #region Update Floor (No. Of Rooms)

                        //Decrease the no. of rooms quantity from floor.
                        var floor = floorRepository.GetFloorById(roomDetail.FloorId).FirstOrDefault();

                        if (floor != null)
                        {
                            floor.NoOfRoom = floor.NoOfRoom > 0 ? (floor.NoOfRoom - 1) : 0;

                            floorRepository.UpdateFloor(floor);
                        }

                        #endregion

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
                        errorMessage = "Rooms not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateRooms(List<RoomVM> models)
        {
            try
            {
                string roomId = string.Empty;

                if (models != null && models.Count > 0)
                {
                    foreach (var room in models)
                    {
                        room.UpdatedBy = LogInManager.LoggedInUserId;

                        #region Check Room No Available.

                        if (this.CheckRoomNoAvailable(room.Id, room.RoomNo) == false)
                        {
                            return Json(new
                            {
                                IsSuccess = false,
                                errorMessage = string.Format("Room No : {0} already exist.", room.RoomNo)
                            }, JsonRequestBehavior.AllowGet);
                        }

                        #endregion

                        room.StatusId = Guid.Parse(RoomStatusType.CLEAN);

                        roomId = roomRepository.UpdateRoom(room);
                    }

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomTypeId = roomId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room details not updated successfully."
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

        [HttpPost]
        public ActionResult SearchAdvanceRoom(SearchAdvanceRoomParametersVM model)
        {
            try
            {
                var rooms = roomRepository.SearchAdvanceRoom(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = rooms
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult ShowAddRoom(Guid? roomTypeId)
        {
            try
            {
                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
                var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

                ViewBag.RoomTypeList = roomTypeList;
                ViewBag.FloorList = floorList;

                RoomVM model = new RoomVM();
                model.IsActive = true;

                if (roomTypeId.HasValue)
                {
                    model.RoomTypeId = roomTypeId.Value;
                }

                return PartialView("_AddRoom", model);
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

        public ActionResult Create()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.FloorList = floorList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomVM model)
        {
            try
            {
                string roomId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Room No Available.

                if (this.CheckRoomNoAvailable(model.Id, model.RoomNo) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Room No : {0} already exist.", model.RoomNo)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                model.StatusId = Guid.Parse(RoomStatusType.CLEAN);
                roomId = roomRepository.AddRoom(model);

                if (!string.IsNullOrWhiteSpace(roomId))
                {
                    model.Id = Guid.Parse(roomId);

                    #region Update Floor (No. Of Rooms)

                    //Increase the no. of rooms quantity in floor.
                    var floor = floorRepository.GetFloorById(model.FloorId).FirstOrDefault();

                    if (floor != null)
                    {
                        floor.NoOfRoom = floor.NoOfRoom > 0 ? (floor.NoOfRoom + 1) : 0;

                        floorRepository.UpdateFloor(floor);
                    }

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomId = roomId,
                            Room = model
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room details not saved successfully."
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
            var room = roomRepository.GetRoomById(id);

            RoomVM model = new RoomVM();

            if (room != null && room.Count > 0)
            {
                model = room[0];

                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
                var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

                ViewBag.RoomTypeList = roomTypeList;
                ViewBag.FloorList = floorList;

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomVM model)
        {
            try
            {
                string roomId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Room No Available.

                if (this.CheckRoomNoAvailable(model.Id, model.RoomNo) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Room No : {0} already exist.", model.RoomNo)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                model.StatusId = Guid.Parse(RoomStatusType.CLEAN);
                roomId = roomRepository.UpdateRoom(model);

                if (!string.IsNullOrWhiteSpace(roomId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomId = roomId,
                            Room = model
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room details not updated successfully."
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

        public ActionResult List()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.FloorList = floorList;

            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchRoomParametersVM model)
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

                var rooms = roomRepository.SearchRoom(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = rooms.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = rooms
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckRoomNoAvailable(Guid? roomId, string roomNo)
        {
            bool blnAvailable = true;

            var room = roomRepository.CheckRoomNoAvailable(roomId, roomNo);

            if (room.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        #endregion
    }
}