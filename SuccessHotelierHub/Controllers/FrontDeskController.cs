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
    public class FrontDeskController : Controller
    {
        #region Declaration

        private ProfileRepository profileRepository = new ProfileRepository();
        private RoomTypeRepository roomTypeRepository = new RoomTypeRepository();  
        private CheckInCheckOutRepository checkInCheckOutRepository = new CheckInCheckOutRepository();
        private RoomRepository roomRepository = new RoomRepository();
        private ReservationRepository reservationRepository = new ReservationRepository();
        private ReservationLogRepository reservationLogRepository = new ReservationLogRepository();
        private AdditionalChargeRepository additionalChargeRepository = new AdditionalChargeRepository();
        private ReservationChargeRepository reservationChargeRepository = new ReservationChargeRepository();

        #endregion

        // GET: FrontDesk
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckIn()
        {
            return View();
        }

        public ActionResult Arrivals()
        {
            var roomTypeList = new SelectList(roomTypeRepository.GetRoomType(string.Empty), "Id", "RoomTypeCode").ToList();

            ViewBag.RoomTypeList = roomTypeList;

            return View();
        }

        [HttpPost]
        public ActionResult SearchArrivals(SearchArrivalsParametersVM model)
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
                var reservations = checkInCheckOutRepository.SearchArrivals(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = reservations.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = reservations
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult ShowCheckInPaymentMethod(Guid reservationId, string source = "")
        {
            try
            {
                var reservation = reservationRepository.GetReservationById(reservationId).FirstOrDefault();

                #region Room Mapping

                //Get Room Mapping
                var selectedRooms = roomRepository.GetReservationRoomMapping(reservationId, null);
                var roomIds = string.Empty;
                var roomNumbers = string.Empty;

                if (selectedRooms != null && selectedRooms.Count > 0)
                {
                    foreach (var room in selectedRooms)
                    {
                        roomIds += string.Format("{0},", room.RoomId);
                        roomNumbers += string.Format("{0}, ", room.RoomNo);
                    }

                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        //Remove Last Comma.
                        roomIds = Utility.Utility.RemoveLastCharcter(roomIds, ',');
                    }

                    if (!string.IsNullOrWhiteSpace(roomNumbers))
                    {
                        //Remove Last Comma.
                        roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                    }
                }
                else
                {
                    SearchAdvanceRoomParametersVM searchRoomModel = new SearchAdvanceRoomParametersVM();
                    searchRoomModel.RoomTypeId = reservation.RoomTypeId;
                    searchRoomModel.ArrivalDate = reservation.ArrivalDate;
                    searchRoomModel.NoOfNight = reservation.NoOfNight;
                    searchRoomModel.DepartureDate = reservation.DepartureDate;
                    searchRoomModel.RoomNo = string.Empty;
                    searchRoomModel.Type = string.Empty;

                    var availableRoomList = roomRepository.SearchAdvanceRoom(searchRoomModel);

                    if(availableRoomList != null && availableRoomList.Count > 0)
                    {
                        //Get default available rooms by top.
                        availableRoomList = availableRoomList.Take(reservation.NoOfRoom).ToList();

                        foreach (var room in availableRoomList)
                        {
                            roomIds += string.Format("{0},", room.RoomId);
                            roomNumbers += string.Format("{0}, ", room.RoomNo);
                        }

                        if (!string.IsNullOrWhiteSpace(roomIds))
                        {
                            //Remove Last Comma.
                            roomIds = Utility.Utility.RemoveLastCharcter(roomIds, ',');
                        }

                        if (!string.IsNullOrWhiteSpace(roomNumbers))
                        {
                            //Remove Last Comma.
                            roomNumbers = Utility.Utility.RemoveLastCharcter(roomNumbers, ',');
                        }
                    }
                }

                #endregion
                
                CheckInPaymentMethodVM model = new CheckInPaymentMethodVM();

                model.ReservationId = reservation.Id;
                model.ProfileId = reservation.ProfileId;

                model.CheckInDate = reservation.ArrivalDate;
                model.CheckInTime = reservation.ETA;                

                model.NoOfRoom = reservation.NoOfRoom;
                model.Name = Convert.ToString(reservation.LastName + " " + reservation.FirstName).Trim();
                model.PaymentMethodId = reservation.PaymentMethodId;
                model.CreditCardNo = reservation.CreditCardNo;
                model.CardExpiryDate = reservation.CardExpiryDate;
                model.RoomNumbers = roomNumbers;
                model.RoomIds = roomIds;
                model.RoomTypeId = reservation.RoomTypeId;

                ViewData["Source"] = source;
                ViewData["ArrivalDate"] = reservation.ArrivalDate.HasValue ? reservation.ArrivalDate.Value.ToString("dd/MM/yyyy") : "";
                ViewData["DepartureDate"] = reservation.DepartureDate.HasValue ? reservation.DepartureDate.Value.ToString("dd/MM/yyyy") : "";

                return PartialView("_PaymentMethod", model);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        [HttpPost]
        public ActionResult CheckIn(CheckInPaymentMethodVM model)
        {
            try
            {
                CheckInCheckOutVM checkIn = new CheckInCheckOutVM();

                //Get Reservation detail.
                var reservation = reservationRepository.GetReservationById(model.ReservationId).FirstOrDefault();

                if(reservation != null)
                {
                    reservation.PaymentMethodId = model.PaymentMethodId;
                    reservation.CreditCardNo = model.CreditCardNo;
                    reservation.CardExpiryDate = model.CardExpiryDate;
                    reservation.ArrivalDate = model.CheckInDate;

                    DateTime dtDepartureDate;
                    dtDepartureDate = model.CheckInDate.Value.AddDays(reservation.NoOfNight);
                    reservation.DepartureDate = dtDepartureDate;

                    string CheckInTimeText = model.CheckInTimeText;
                    TimeSpan checkInTime = new TimeSpan();
                    if (!string.IsNullOrWhiteSpace(CheckInTimeText))
                    {
                        string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
                        string date = (todayDate + " " + CheckInTimeText);
                        DateTime time = Convert.ToDateTime(date);
                        checkInTime = time.TimeOfDay;

                        reservation.ETA = checkInTime;
                    }

                    reservation.UpdatedBy = LogInManager.LoggedInUserId;
                    reservationRepository.UpdateReservation(reservation);

                    #region Save Room Rent Charges 

                    double totalPrice = (double)(reservation.Rate * reservation.NoOfNight);

                    var roomRentCharge = additionalChargeRepository.GetAdditionalChargesByCode(AdditionalChargeCode.ROOM_RENT).FirstOrDefault();

                    ReservationChargeVM reservationCharge = new ReservationChargeVM();
                    reservationCharge.ReservationId = reservation.Id;
                    reservationCharge.AdditionalChargeId = roomRentCharge.Id;
                    reservationCharge.Code = roomRentCharge.Code;
                    reservationCharge.Description = roomRentCharge.Description;
                    reservationCharge.TransactionDate = reservation.ArrivalDate;
                    reservationCharge.Amount = totalPrice;
                    reservationCharge.Qty = 1;
                    reservationCharge.IsActive = true;
                    reservationCharge.CreatedBy = LogInManager.LoggedInUserId;

                    reservationChargeRepository.AddReservationCharges(reservationCharge);

                    #endregion

                    #region Save Reservation Room Mapping & Update Room Occupied Flag
                    var roomIds = model.RoomIds;

                    //Delete Existing Reservation Room Mapping.
                    roomRepository.DeleteReservationRoomMappingByReservation(reservation.Id, LogInManager.LoggedInUserId);

                    if (!string.IsNullOrWhiteSpace(roomIds))
                    {
                        var roomIdsArr = roomIds.Split(',');

                        if (roomIdsArr != null)
                        {
                            //Remove Duplication.
                            roomIdsArr = roomIdsArr.Distinct().ToArray();

                            foreach (var item in roomIdsArr)
                            {
                                //Save Reservation Room Mapping.
                                ReservationRoomMappingVM reservationRoomMapping = new ReservationRoomMappingVM();
                                reservationRoomMapping.RoomId = Guid.Parse(item.Trim());
                                reservationRoomMapping.ReservationId = reservation.Id;
                                reservationRoomMapping.CreatedBy = LogInManager.LoggedInUserId;
                                reservationRoomMapping.UpdatedBy = LogInManager.LoggedInUserId;

                                roomRepository.AddUpdateReservationRoomMapping(reservationRoomMapping);

                                ////Update Room Occupied Flag.
                                //roomRepository.UpdateRoomOccupiedFlag(Guid.Parse(item.Trim()), true, LogInManager.LoggedInUserId);

                                ////Update Room Status CLEAN to DIRTY.
                                //roomRepository.UpdateRoomCheckInStatus(Guid.Parse(item.Trim()), Guid.Parse(RoomStatusType.DIRTY), true, LogInManager.LoggedInUserId);

                                #region Add Reservation Log

                                var lstReservationLog = reservationLogRepository.GetReservationLogDetails(model.ReservationId, Guid.Parse(item.Trim()), null).FirstOrDefault();

                                if (lstReservationLog != null)
                                {
                                    lstReservationLog.ReservationId = model.ReservationId;
                                    lstReservationLog.ProfileId = model.ProfileId;
                                    lstReservationLog.RoomId = Guid.Parse(item.Trim());
                                    lstReservationLog.CheckInDate = model.CheckInDate.Value;
                                    lstReservationLog.CheckInTime = checkInTime;
                                    lstReservationLog.CheckOutDate = reservation.DepartureDate;
                                    lstReservationLog.RoomStatusId = Guid.Parse(RoomStatusType.DIRTY);
                                    lstReservationLog.IsActive = true;                                    
                                    lstReservationLog.UpdatedBy = LogInManager.LoggedInUserId;

                                    reservationLogRepository.UpdateReservationLog(lstReservationLog);
                                }
                                else
                                {
                                    ReservationLogVM reservationLog = new ReservationLogVM();
                                    reservationLog.ReservationId = model.ReservationId;
                                    reservationLog.ProfileId = model.ProfileId;
                                    reservationLog.RoomId = Guid.Parse(item.Trim());
                                    reservationLog.CheckInDate = model.CheckInDate.Value;
                                    reservationLog.CheckInTime = checkInTime;
                                    reservationLog.CheckOutDate = reservation.DepartureDate;
                                    reservationLog.RoomStatusId = Guid.Parse(RoomStatusType.DIRTY);
                                    reservationLog.IsActive = true;
                                    reservationLog.CreatedBy = LogInManager.LoggedInUserId;

                                    reservationLogRepository.AddReservationLog(reservationLog);
                                }
                                
                                #endregion

                            }
                        }
                    }
                    #endregion

                    #region Save CheckIn Details

                    checkIn.ReservationId = model.ReservationId;
                    checkIn.ProfileId = model.ProfileId;
                    checkIn.CheckInDate = model.CheckInDate.Value;
                    checkIn.CheckInTime = checkInTime;
                    checkIn.IsActive = true;
                    checkIn.CreatedBy = LogInManager.LoggedInUserId;

                    var checkInId = checkInCheckOutRepository.AddCheckInDetail(checkIn);

                    #endregion
                    
                    #region Update Reservation Check In Flag

                    reservationRepository.UpdateReservationCheckInFlag(model.ReservationId, true, LogInManager.LoggedInUserId);

                    #endregion

                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CheckInId = checkInId,
                            Name = model.Name
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Check In details not saved successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}
