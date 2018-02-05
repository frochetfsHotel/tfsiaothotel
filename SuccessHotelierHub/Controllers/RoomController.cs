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
    
    public class RoomController : Controller
    {
        #region Declaration 

        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();
        private FloorRepository floorRepository = new FloorRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private RoomStatusRepository roomStatusRepository = new RoomStatusRepository();
        private RoomFeatureRepository roomFeatureRepository = new RoomFeatureRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();

        #endregion

        #region Room

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Index()
        {
            var roomTypeList = roomTypeRepository.GetRoomType(string.Empty);

            ViewBag.RoomTypeList = roomTypeList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                    #region Delete Room Features

                    //Delete Room Features.
                    roomRepository.DeleteRoomFeaturesMappingByRoom(id, LogInManager.LoggedInUserId);

                    #endregion

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
                        var roomDetail = roomRepository.GetRoomById(id).FirstOrDefault();

                        var roomId = roomRepository.DeleteRoom(id, LogInManager.LoggedInUserId);

                        #region Delete Room Features

                        //Delete Room Features.
                        roomRepository.DeleteRoomFeaturesMappingByRoom(id, LogInManager.LoggedInUserId);

                        #endregion

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
                Utility.Utility.LogError(e, "DeleteSelected");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                Utility.Utility.LogError(e, "UpdateRooms");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }

        }

        [HttpPost]
        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult SearchAdvanceRoom(SearchAdvanceRoomParametersVM model)
        {
            try
            {
                if (model.RoomFeatures != null && model.RoomFeatures.Count > 0)
                {
                    model.RoomFeaturesIds = String.Join(",", model.RoomFeatures.ToArray());
                }

                var rooms = roomRepository.SearchAdvanceRoom(model);

                return Json(new
                {
                    IsSuccess = true,
                    data = rooms
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "SearchAdvanceRoom");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
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
                Utility.Utility.LogError(e, "ShowAddRoom");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");
            var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.FloorList = floorList;
            ViewBag.RoomFeaturesList = roomFeaturesList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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


                    #region Add Room Features

                    if (model.RoomFeatures != null && model.RoomFeatures.Count > 0)
                    {
                        foreach (var item in model.RoomFeatures)
                        {
                            RoomFeaturesMappingVM roomFeaturesMapping = new RoomFeaturesMappingVM();
                            roomFeaturesMapping.RoomId = Guid.Parse(roomId);
                            roomFeaturesMapping.RoomFeatureId = item;
                            roomFeaturesMapping.CreatedBy = LogInManager.LoggedInUserId;
                            roomFeaturesMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateRoomFeaturesMapping(roomFeaturesMapping);
                        }
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
            var room = roomRepository.GetRoomById(id);

            RoomVM model = new RoomVM();

            if (room != null && room.Count > 0)
            {
                model = room[0];

                var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
                var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

                var roomFeatures = roomRepository.GetRoomFeaturesMapping(id, null);

                if (roomFeatures != null && roomFeatures.Count > 0)
                {
                    List<Guid> roomFeatureIds = new List<Guid>();

                    foreach (var roomFeature in roomFeatures)
                    {
                        if(roomFeature.RoomFeatureId.HasValue)
                            roomFeatureIds.Add(roomFeature.RoomFeatureId.Value);
                    }

                    model.RoomFeatures = roomFeatureIds;
                }

                var roomFeaturesList = roomFeatureRepository.GetRoomFeatures();

                ViewBag.RoomTypeList = roomTypeList;
                ViewBag.FloorList = floorList;
                ViewBag.RoomFeaturesList = roomFeaturesList;

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                    #region Delete Room Features

                    var roomFeatures = roomRepository.GetRoomFeaturesMapping(Guid.Parse(roomId), null);

                    if (roomFeatures != null && roomFeatures.Count > 0)
                    {
                        List<Guid> mappingIds = new List<Guid>();
                        foreach (var roomFeature in roomFeatures)
                        {
                            if (roomFeature.RoomFeatureId.HasValue)
                            {
                                if (!model.RoomFeatures.Contains(roomFeature.RoomFeatureId.Value))
                                {
                                    mappingIds.Add(roomFeature.Id);
                                }
                            }
                        }

                        //Delete Room Features
                        if (mappingIds != null && mappingIds.Count > 0)
                        {
                            foreach (var id in mappingIds)
                            {
                                roomRepository.DeleteRoomFeaturesMapping(id, LogInManager.LoggedInUserId);
                            }
                        }
                    }
                    #endregion

                    #region Add / Update Room Features

                    if (model.RoomFeatures != null && model.RoomFeatures.Count > 0)
                    {
                        foreach (var item in model.RoomFeatures)
                        {
                            RoomFeaturesMappingVM roomFeaturesMapping = new RoomFeaturesMappingVM();
                            roomFeaturesMapping.RoomId = Guid.Parse(roomId);
                            roomFeaturesMapping.RoomFeatureId = item;
                            roomFeaturesMapping.CreatedBy = LogInManager.LoggedInUserId;
                            roomFeaturesMapping.UpdatedBy = LogInManager.LoggedInUserId;

                            roomRepository.AddUpdateRoomFeaturesMapping(roomFeaturesMapping);
                        }
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
                        errorMessage = "Room details not updated successfully."
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
        public ActionResult List()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode");
            var floorList = new SelectList(floorRepository.GetFloors(), "Id", "Name");

            ViewBag.RoomTypeList = roomTypeList;
            ViewBag.FloorList = floorList;

            return View();
        }

        [HotelierHubAuthorize(Roles = "ADMIN")]
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
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HotelierHubAuthorize(Roles = "ADMIN,STUDENT")]
        public ActionResult ChangeRoomAndReservationMapping(Guid reservationId, Guid roomId, Guid roomTypeId, DateTime? date)
        {
            try
            {
                var reservationDetail = reservationRepository.GetReservationById(reservationId).FirstOrDefault();

                if (reservationDetail != null)
                {
                    DateTime? arrivalDate = null, departureDate = null;
                    if (date.HasValue)
                    {
                        var noOfNight = reservationDetail.NoOfNight;

                        arrivalDate = date.Value;
                        departureDate = date.Value.AddDays(noOfNight);

                    }

                    //Update dates in Reservation, ReservationLog, CheckInCheckOutDetail Tables & RoomId into ReservationLog, ReservationRoomMapping Tables.
                    var id = roomRepository.ChangeRoomAndReservationMappingDetails(reservationId, roomId, roomTypeId, arrivalDate, departureDate, LogInManager.LoggedInUserId);

                    return Json(new
                    {
                        IsSuccess = true,
                        ReservationId = id
                    });
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Reservation details not exist"
                    });
                }

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "ChangeRoomAndReservationMapping");
                return Json(new
                {
                    IsSuccess = false,
                    errorMessage = e.Message
                });
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