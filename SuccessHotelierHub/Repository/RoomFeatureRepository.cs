using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class RoomFeatureRepository
    {
        #region Room Feature

        public List<RoomFeatureVM> GetRoomFeatures()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomFeatures");

            var roomFeatures = new List<RoomFeatureVM>();
            roomFeatures = DALHelper.CreateListFromTable<RoomFeatureVM>(ds);

            return roomFeatures;
        }

        public List<RoomFeatureVM> GetRoomFeaturesById(Guid roomFeatureId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = roomFeatureId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetRoomFeaturesById", parameters);

            var roomFeature = new List<RoomFeatureVM>();
            roomFeature = DALHelper.CreateListFromTable<RoomFeatureVM>(dt);

            return roomFeature;
        }

        public string AddRoomFeatures(RoomFeatureVM roomFeature)
        {
            string roomFeatureId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = roomFeature.Name },
                    new SqlParameter { ParameterName = "@Description", Value = roomFeature.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = roomFeature.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = roomFeature.CreatedBy }
                };

            roomFeatureId = Convert.ToString(DALHelper.ExecuteScalar("AddRoomFeatures", parameters));

            return roomFeatureId;
        }

        public string UpdateRoomFeatures(RoomFeatureVM roomFeature)
        {
            string roomFeatureId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = roomFeature.Id },
                    new SqlParameter { ParameterName = "@Name", Value = roomFeature.Name },
                    new SqlParameter { ParameterName = "@Description", Value = roomFeature.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = roomFeature.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = roomFeature.UpdatedBy }
                };

            roomFeatureId = Convert.ToString(DALHelper.ExecuteScalar("UpdateRoomFeatures", parameters));

            return roomFeatureId;
        }

        public string DeleteRoomFeatures(Guid id, int updatedBy)
        {
            string roomFeatureId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            roomFeatureId = Convert.ToString(DALHelper.ExecuteScalar("DeleteRoomFeatures", parameters));

            return roomFeatureId;
        }

        public List<SearchRoomFeatureResultVM> SearchRoomFeatures(SearchRoomFeatureParametersVM model, string sortColumn, string sortDirection)
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

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchRoomFeatures", parameters);

            var roomFeatures = new List<SearchRoomFeatureResultVM>();
            roomFeatures = DALHelper.CreateListFromTable<SearchRoomFeatureResultVM>(dt);

            return roomFeatures;
        }

        #endregion
    }
}