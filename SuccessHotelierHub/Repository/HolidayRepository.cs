using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class HolidayRepository
    {
        #region Holiday
        
        public List<HolidayVM> GetHolidayById(Guid holidayId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = holidayId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetHolidayById", parameters);


            var holiday = new List<HolidayVM>();
            holiday = DALHelper.CreateListFromTable<HolidayVM>(dt);

            return holiday;
        }

        public string AddHoliday(HolidayVM holiday)
        {
            string holidayId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = holiday.Date },
                    new SqlParameter { ParameterName = "@Title", Value = holiday.Title },
                    new SqlParameter { ParameterName = "@Description", Value = holiday.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = holiday.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = holiday.CreatedBy }
                };

            holidayId = Convert.ToString(DALHelper.ExecuteScalar("AddHoliday", parameters));

            return holidayId;
        }

        public string UpdateHoliday(HolidayVM holiday)
        {
            string holidayId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = holiday.Id },
                    new SqlParameter { ParameterName = "@Date", Value = holiday.Date },
                    new SqlParameter { ParameterName = "@Title", Value = holiday.Title },
                    new SqlParameter { ParameterName = "@Description", Value = holiday.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = holiday.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = holiday.UpdatedBy }
                };

            holidayId = Convert.ToString(DALHelper.ExecuteScalar("UpdateHoliday", parameters));

            return holidayId;
        }

        public string DeleteHoliday(Guid id, int updatedBy)
        {
            string holidayId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            holidayId = Convert.ToString(DALHelper.ExecuteScalar("DeleteHoliday", parameters));

            return holidayId;
        }

        public List<SearchHolidayResultVM> SearchHoliday(SearchHolidayParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = model.Date },
                    new SqlParameter { ParameterName = "@Title", Value = model.Title },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchHoliday", parameters);

            var holidays = new List<SearchHolidayResultVM>();
            holidays = DALHelper.CreateListFromTable<SearchHolidayResultVM>(dt);

            return holidays;
        }

        public List<HolidayVM> GetHolidaysOfCurrentMonth(int month, int year)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Month" , Value = month },
                    new SqlParameter { ParameterName = "@Year"  , Value = year }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetHolidaysOfCurrentMonth", parameters);


            var holiday = new List<HolidayVM>();
            holiday = DALHelper.CreateListFromTable<HolidayVM>(dt);

            return holiday;
        }

        #endregion
    }
}