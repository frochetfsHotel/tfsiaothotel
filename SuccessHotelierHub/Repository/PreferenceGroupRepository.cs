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

        public List<PreferenceGroupVM> GetPreferenceGroupById(Guid preferenceGroupId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = preferenceGroupId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreferenceGroupById", parameters);


            var preferenceGroup = new List<PreferenceGroupVM>();

            preferenceGroup = DALHelper.CreateListFromTable<PreferenceGroupVM>(dt);

            return preferenceGroup;
        }

        public PreferenceGroupVM GetPreferenceGroupByName(string preferenceGroupName)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Name", Value = preferenceGroupName }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreferenceGroupByName", parameters);


            var preferenceGroupList = new List<PreferenceGroupVM>();
            preferenceGroupList = DALHelper.CreateListFromTable<PreferenceGroupVM>(dt);

            var preferenceGroup = new PreferenceGroupVM();
            if (preferenceGroupList != null && preferenceGroupList.Count > 0)
            {
                preferenceGroup = preferenceGroupList[0];
            }
            return preferenceGroup;
        }

        public string AddPreferenceGroup(PreferenceGroupVM preferenceGroup)
        {
            string preferenceGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = preferenceGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = preferenceGroup.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = preferenceGroup.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = preferenceGroup.CreatedBy }
                };

            preferenceGroupId = Convert.ToString(DALHelper.ExecuteScalar("AddPreferenceGroup", parameters));

            return preferenceGroupId;
        }

        public string UpdatePreferenceGroup(PreferenceGroupVM preferenceGroup)
        {
            string preferenceGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = preferenceGroup.Id },
                    new SqlParameter { ParameterName = "@Name", Value = preferenceGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = preferenceGroup.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = preferenceGroup.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = preferenceGroup.CreatedBy }
                };

            preferenceGroupId = Convert.ToString(DALHelper.ExecuteScalar("UpdatePreferenceGroup", parameters));

            return preferenceGroupId;
        }

        public string DeletePreferenceGroup(Guid id, int updatedBy)
        {
            string preferenceGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            preferenceGroupId = Convert.ToString(DALHelper.ExecuteScalar("DeletePreferenceGroup", parameters));

            return preferenceGroupId;
        }

        public List<SearchPreferenceGroupResultVM> SearchPreferenceGroup(SearchPreferenceGroupParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Description", Value = model.Description },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchPreferenceGroup", parameters);

            var preferenceGroups = new List<SearchPreferenceGroupResultVM>();
            preferenceGroups = DALHelper.CreateListFromTable<SearchPreferenceGroupResultVM>(dt);

            return preferenceGroups;
        }

        #endregion
    }
}