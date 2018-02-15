using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class RTCRepository
    {
        #region RTC

        public List<RtcVM> GetRTC()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRTC");

            var rtcList = new List<RtcVM>();
            rtcList = DALHelper.CreateListFromTable<RtcVM>(dt);

            return rtcList;
        }

        public List<RtcVM> GetRTCById(Guid rtcId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = rtcId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRTCById", parameters);

            var rtc = new List<RtcVM>();
            rtc = DALHelper.CreateListFromTable<RtcVM>(dt);

            return rtc;
        }

        public string AddRTC(RtcVM rtc)
        {
            string rtcId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = rtc.Code },
                    new SqlParameter { ParameterName = "@Description", Value = rtc.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = rtc.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = rtc.CreatedBy }
                };

            rtcId = Convert.ToString(DALHelper.ExecuteScalar("AddRTC", parameters));

            return rtcId;
        }

        public string UpdateRTC(RtcVM rtc)
        {
            string rtcId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = rtc.Id },
                    new SqlParameter { ParameterName = "@Code", Value = rtc.Code },
                    new SqlParameter { ParameterName = "@Description", Value = rtc.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = rtc.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = rtc.UpdatedBy }
                };

            rtcId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRTC", parameters));

            return rtcId;
        }

        public string DeleteRTC(Guid id, int updatedBy)
        {
            string rtcId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            rtcId = Convert.ToString(DALHelper.ExecuteScalar("Deletertc", parameters));

            return rtcId;
        }

        public List<SearchRTCResultVM> SearchRTC(SearchRTCParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRTC", parameters);

            var rtcList = new List<SearchRTCResultVM>();
            rtcList = DALHelper.CreateListFromTable<SearchRTCResultVM>(dt);

            return rtcList;
        }

        #endregion
    }
}