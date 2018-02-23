using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class PreferenceRepository
    {
        #region Preference
        public List<PreferenceVM> GetPreferences()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreferences");

            var preferences = new List<PreferenceVM>();
            preferences = DALHelper.CreateListFromTable<PreferenceVM>(dt);

            return preferences;
        }

        public List<PreferenceVM> GetPreferenceById(Guid preferenceId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = preferenceId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPreferenceById", parameters);


            var preference = new List<PreferenceVM>();
            preference = DALHelper.CreateListFromTable<PreferenceVM>(dt);

            return preference;
        }

        public string AddPreferences(PreferenceVM preference)
        {
            string preferenceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@PreferenceGroupId", Value = preference.PreferenceGroupId },
                    new SqlParameter { ParameterName = "@Code", Value = preference.Code },
                    new SqlParameter { ParameterName = "@Description", Value = preference.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = preference.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = preference.CreatedBy }
                };

            preferenceId = Convert.ToString(DALHelper.ExecuteScalar("AddPreferences", parameters));

            return preferenceId;
        }

        public string UpdatePreferences(PreferenceVM preference)
        {
            string preferenceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = preference.Id },
                    new SqlParameter { ParameterName = "@PreferenceGroupId", Value = preference.PreferenceGroupId },
                    new SqlParameter { ParameterName = "@Code", Value = preference.Code },
                    new SqlParameter { ParameterName = "@Description", Value = preference.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = preference.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = preference.UpdatedBy }
                };

            preferenceId = Convert.ToString(DALHelper.ExecuteScalar("UpdatePreferences", parameters));

            return preferenceId;
        }

        public string DeletePreferences(Guid id, int updatedBy)
        {
            string preferenceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            preferenceId = Convert.ToString(DALHelper.ExecuteScalar("DeletePreferences", parameters));

            return preferenceId;
        }

        public List<SearchPreferenceResultVM> SearchPreferences(SearchPreferenceParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchPreferences", parameters);

            var preferences = new List<SearchPreferenceResultVM>();
            preferences = DALHelper.CreateListFromTable<SearchPreferenceResultVM>(dt);

            return preferences;
        }

        public List<SearchAdvancePreferenceResultVM> SearchAdvancePreference(SearchAdvancePreferenceParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@PreferenceGroupId", Value = model.PreferenceGroupId },
                    new SqlParameter { ParameterName = "@Preference", Value = model.Preference }                    
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvancePreference", parameters);

            var preferences = new List<SearchAdvancePreferenceResultVM>();
            preferences = DALHelper.CreateListFromTable<SearchAdvancePreferenceResultVM>(dt);

            return preferences;
        }

        #endregion Preference


        #region Profile Preference Mapping

        public List<ProfilePreferenceMappingVM> GetProfilePreferenceMapping(Guid? profileTypeId, Guid? profileId, Guid? preferenceId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ProfileTypeId", Value = (object) profileTypeId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@ProfileId", Value = (object) profileId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@PreferenceId", Value = (object) preferenceId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetProfilePreferenceMapping", parameters);

            var profilePreferenceMapping = new List<ProfilePreferenceMappingVM>();
            profilePreferenceMapping = DALHelper.CreateListFromTable<ProfilePreferenceMappingVM>(dt);

            return profilePreferenceMapping;
        }

        public string AddProfilePreferenceMapping(ProfilePreferenceMappingVM profilePreferenceMapping)
        {
            string profilePreferenceMappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ProfileTypeId", Value = profilePreferenceMapping.ProfileTypeId },
                    new SqlParameter { ParameterName = "@ProfileId", Value = profilePreferenceMapping.ProfileId },
                    new SqlParameter { ParameterName = "@PreferenceId", Value = profilePreferenceMapping.PreferenceId },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = profilePreferenceMapping.CreatedBy }
                };

            profilePreferenceMappingId = Convert.ToString(DALHelper.ExecuteScalar("AddProfilePreferenceMapping", parameters));

            return profilePreferenceMappingId;
        }

        public string DeleteProfilePreferenceMappingByProfile(Guid? profileTypeId, Guid? profileId, int userId)
        {
            string preferenceId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ProfileTypeId", Value = (object) profileTypeId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@ProfileId", Value = (object) profileId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            preferenceId = Convert.ToString(DALHelper.ExecuteScalar("DeleteProfilePreferenceMappingByProfile", parameters));

            return preferenceId;
        }

        public string DeleteProfilePreferenceMapping(Guid id, int userId)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("DeleteProfilePreferenceMapping", parameters));

            return mappingId;
        }

        #endregion #region Profile Preference Mapping

        #region Reservation Preference Mapping 

        public string AddReservationPreferenceMapping(ReservationPreferenceMappingVM model)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = model.ReservationId },
                    new SqlParameter { ParameterName = "@PreferenceId", Value = model.PreferenceId } ,
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("AddReservationPreferenceMapping", parameters));

            return id;
        }

        public string DeleteReservationPreferenceMappingByReservation(Guid reservationId, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = reservationId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationPreferenceMappingByReservation", parameters));

            return id;
        }

        public string DeleteReservationPreferenceMapping(Guid id, int userId)
        {
            string mappingId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            mappingId = Convert.ToString(DALHelper.ExecuteScalar("DeleteReservationPreferenceMapping", parameters));

            return mappingId;
        }

        public List<ReservationPreferenceMappingVM> GetReservationPreferenceMapping(Guid? reservationId, Guid? preferenceId, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ReservationId", Value = (object) reservationId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@PreferenceId", Value = (object) preferenceId ?? DBNull.Value },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetReservationPreferenceMapping", parameters);

            var reservationPreferenceMapping = new List<ReservationPreferenceMappingVM>();
            reservationPreferenceMapping = DALHelper.CreateListFromTable<ReservationPreferenceMappingVM>(dt);

            return reservationPreferenceMapping;
        }

        #endregion
    }

}