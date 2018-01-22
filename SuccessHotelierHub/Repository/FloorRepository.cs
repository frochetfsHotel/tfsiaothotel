using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class FloorRepository
    {
        #region Floor

        public List<FloorVM> GetFloors()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetFloors");

            var floors = new List<FloorVM>();
            floors = DALHelper.CreateListFromTable<FloorVM>(ds);

            return floors;
        }

        public List<FloorVM> GetFloorById(Guid floorId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = floorId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetFloorById", parameters);

            var floor = new List<FloorVM>();
            floor = DALHelper.CreateListFromTable<FloorVM>(dt);

            return floor;
        }

        public string AddFloor(FloorVM floor)
        {
            string floorId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = floor.Name },
                    new SqlParameter { ParameterName = "@Code", Value = floor.Code },
                    new SqlParameter { ParameterName = "@FloorNo", Value = floor.FloorNo },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = floor.NoOfRoom },
                    new SqlParameter { ParameterName = "@Description", Value = floor.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = floor.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = floor.CreatedBy }
                };

            floorId = Convert.ToString(DALHelper.ExecuteScalar("AddFloor", parameters));

            return floorId;
        }

        public string UpdateFloor(FloorVM floor)
        {
            string floorId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = floor.Id },
                    new SqlParameter { ParameterName = "@Name", Value = floor.Name },
                    new SqlParameter { ParameterName = "@Code", Value = floor.Code },
                    new SqlParameter { ParameterName = "@FloorNo", Value = floor.FloorNo },
                    new SqlParameter { ParameterName = "@NoOfRoom", Value = floor.NoOfRoom },
                    new SqlParameter { ParameterName = "@Description", Value = floor.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = floor.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = floor.UpdatedBy }
                };

            floorId = Convert.ToString(DALHelper.ExecuteScalar("UpdateFloor", parameters));

            return floorId;
        }

        public string DeleteFloor(Guid id, int updatedBy)
        {
            string floorId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            floorId = Convert.ToString(DALHelper.ExecuteScalar("DeleteFloor", parameters));

            return floorId;
        }

        public List<SearchFloorResultVM> SearchFloor(SearchFloorParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },
                    new SqlParameter { ParameterName = "@Code", Value = model.Code },
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchFloor", parameters);

            var floors = new List<SearchFloorResultVM>();
            floors = DALHelper.CreateListFromTable<SearchFloorResultVM>(dt);

            return floors;
        }

        public List<FloorVM> CheckFloorCodeAvailable(Guid? floorId, string code)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = floorId },
                    new SqlParameter { ParameterName = "@Code", Value = code }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckFloorCodeAvailable", parameters);

            var floor = new List<FloorVM>();
            floor = DALHelper.CreateListFromTable<FloorVM>(dt);

            return floor;
        }

        public int GetMaxFloorNo()
        {
            var floorNo = DALHelper.ExecuteScalar("GetMaxFloorNo");

            return (floorNo != null && !string.IsNullOrWhiteSpace(Convert.ToString(floorNo))) ? Convert.ToInt32(floorNo) : 0;
        }

        #endregion
    }
}
