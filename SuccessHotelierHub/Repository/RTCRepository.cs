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
        

        #endregion
    }
}