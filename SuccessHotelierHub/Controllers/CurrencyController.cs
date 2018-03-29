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
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class CurrencyController : Controller
    {
        #region Declaration
        
        private CurrencyRepository currencyRepository = new CurrencyRepository();

        #endregion

        // GET: Currency
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
        public ActionResult Create(CurrencyVM model)
        {
            try
            {
                string currencyId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                #region Check Currency Code Exist.

                if (this.CheckCurrencyCodeExist(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Check Currency Name Exist.

                if (this.CheckCurrencyNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                currencyId = currencyRepository.AddCurrency(model);

                if (!string.IsNullOrWhiteSpace(currencyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CurrencyId = currencyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Currency details not saved successfully."
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
            var currency = currencyRepository.GetCurrencyInfoById(id);

            CurrencyVM model = new CurrencyVM();

            if (currency != null && currency.Count > 0)
            {
                model = currency[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurrencyVM model)
        {
            try
            {
                string currencyId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;

                #region Check Currency Code Exist.

                if (this.CheckCurrencyCodeExist(model.Id, model.Code) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Code : {0} already exist.", model.Code)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Check Currency Name Exist.

                if (this.CheckCurrencyNameExist(model.Id, model.Name) == false)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = string.Format("Name : {0} already exist.", model.Name)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                currencyId = currencyRepository.UpdateCurrency(model);

                if (!string.IsNullOrWhiteSpace(currencyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CurrencyId = currencyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Currency details not updated successfully."
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

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                string currencyId = string.Empty;

                currencyId = currencyRepository.DeleteCurrency(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(currencyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CurrencyId = currencyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Currency details not deleted successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Delete");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCurrencyParametersVM model)
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
                var currencyList = currencyRepository.SearchCurrency(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = currencyList.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = currencyList
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }

        public bool CheckCurrencyCodeExist(Guid? currencyId, string code)
        {
            bool blnAvailable = true;

            var currency = currencyRepository.CheckCurrencyCodeExist(currencyId, code);

            if (currency.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }

        public bool CheckCurrencyNameExist(Guid? currencyId, string name)
        {
            bool blnAvailable = true;

            var currency = currencyRepository.CheckCurrencyNameExist(currencyId, name);

            if (currency.Any())
            {
                blnAvailable = false;
            }

            return blnAvailable;
        }
    }
}