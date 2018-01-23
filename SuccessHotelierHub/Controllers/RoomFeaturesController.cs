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
    public class RoomFeaturesController : Controller
    {
        #region Declaration

        private RoomFeatureRepository roomFeatureRepository = new RoomFeatureRepository();

        #endregion

        // GET: RoomFeatures
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
        public ActionResult Create(RoomFeatureVM model)
        {
            try
            {
                string roomFeatureId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                roomFeatureId = roomFeatureRepository.AddRoomFeatures(model);

                if (!string.IsNullOrWhiteSpace(roomFeatureId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomFeatureId = roomFeatureId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room Features details not saved successfully."
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
            var roomFeature = roomFeatureRepository.GetRoomFeaturesById(id);

            RoomFeatureVM model = new RoomFeatureVM();

            if (roomFeature != null && roomFeature.Count > 0)
            {
                model = roomFeature[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomFeatureVM model)
        {
            try
            {
                string roomFeatureId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                roomFeatureId = roomFeatureRepository.UpdateRoomFeatures(model);

                if (!string.IsNullOrWhiteSpace(roomFeatureId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomFeatureId = roomFeatureId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room Features details not updated successfully."
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
        public ActionResult Delete(Guid id)
        {
            try
            {
                string roomFeatureId = string.Empty;

                roomFeatureId = roomFeatureRepository.DeleteRoomFeatures(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(roomFeatureId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            RoomFeatureId = roomFeatureId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Room Features details not deleted successfully."
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
        public ActionResult Search(SearchRoomFeatureParametersVM model)
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
                var roomFeatures = roomFeatureRepository.SearchRoomFeatures(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = roomFeatures.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = roomFeatures
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}