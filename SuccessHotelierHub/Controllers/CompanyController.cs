using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Repository;
using SuccessHotelierHub.Utility;

namespace SuccessHotelierHub.Controllers
{
    [HotelierHubAuthorize(Roles = "ADMIN")]
    public class CompanyController : Controller
    {
        #region Declaration

        private CompanyRepository CompanyRepository = new CompanyRepository();

        #endregion

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyVM model)
        {
            try
            {
                string companyId = string.Empty;
                model.CreatedBy = LogInManager.LoggedInUserId;

                companyId = CompanyRepository.AddCompany(model);

                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CompanyId = companyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Company details not saved successfully."
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
            var company = CompanyRepository.GetCompanyById(id);

            CompanyVM model = new CompanyVM();

            if (company != null && company.Count > 0)
            {
                model = company[0];

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyVM model)
        {
            try
            {
                string companyId = string.Empty;
                model.UpdatedBy = LogInManager.LoggedInUserId;
                companyId = CompanyRepository.UpdateCompany(model);

                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CompanyId = companyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "Company details not updated successfully."
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
                string companyId = string.Empty;

                companyId = CompanyRepository.DeleteCompany(id, LogInManager.LoggedInUserId);

                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        data = new
                        {
                            CompanyId = companyId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        errorMessage = "CompanyId details not deleted successfully."
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
        public ActionResult Search(SearchCompanyParametersVM model)
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
                var Companies = CompanyRepository.SearchHoliday(model, Convert.ToString(sortColumn), Convert.ToString(sortDirection));

                int totalRecords = 0;
                var dbRecords = Companies.Select(m => m.TotalCount).FirstOrDefault();

                if (dbRecords != 0)
                    totalRecords = Convert.ToInt32(dbRecords);

                return Json(new
                {
                    IsSuccess = true,
                    CurrentPage = model.PageNum,
                    PageSize = Constants.PAGESIZE,
                    TotalRecords = totalRecords,
                    data = Companies
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Utility.Utility.LogError(e, "Search");
                return Json(new { IsSuccess = false, errorMessage = e.Message });
            }
        }
    }
}