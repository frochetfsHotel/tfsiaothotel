using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class PreferenceGroupRepository
    {

        #region Preference Group

        public List<PreferenceGroupVM> GetPreferenceGroup()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreferenceGroup");

            var preferenceGroups = new List<PreferenceGroupVM>();
            preferenceGroups = DALHelper.CreateListFromTable<PreferenceGroupVM>(dt);

            return preferenceGroups;
        }

        #endregion
    }
}