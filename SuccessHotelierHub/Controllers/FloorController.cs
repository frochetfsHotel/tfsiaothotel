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
    public class FloorController : Controller
    {
        #region Declaration

        private FloorRepository floorRepository = new FloorRepository();

        #endregion

        // GET: Floor
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
        public ActionResult Create(FloorVM model)
        {
            try
            {
                string floorId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Floor Code Available.

                if (this.CheckFloorCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Floor Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                floorId = floorRepository.AddFloor(model);

                if (!string.IsNullOrWhiteSpace(floorId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            FloorId = floorId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Floor details not saved successfully."
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
            var floor = floorRepository.GetFloorById(id);

            FloorVM model = new FloorVM();

            if (floor != null && floor.Count > 0)
            {
                model = floor[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FloorVM model)
        {
            try
            {
                string floorId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Floor Code Available.

                if (this.CheckFloorCodeAvailable(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Floor Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                floorId = floorRepository.UpdateFloor(model);

                if (!string.IsNullOrWhiteSpace(floorId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            FloorId = floorId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Floor details not updated successfully."
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
                string floorId = string.Empty;

                floorId = floorRepository.DeleteFloor(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(floorId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            FloorId = floorId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Floor details not deleted successfully."
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
        public ActionResult Search(SearchFloorParametersVM model)
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
                var floors = floorRepository.SearchFloor(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = floors.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = floors
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckFloorCodeAvailable(Guid? floorId, string code)
        {
            bool blnAvailable = true;

            var floor = floorRepository.CheckFloorCodeAvailable(floorId, code);

            if (floor.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}