using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub.Repository
{
    public class CollegeGroupRepository
    {
        #region College Group

        public List<CollegeGroupVM> GetCollegeGroups()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCollegeGroups");

            var collegeGroupList = new List<CollegeGroupVM>();
            collegeGroupList = DALHelper.CreateListFromTable<CollegeGroupVM>(dt);

            return collegeGroupList;
        }        

        public CollegeGroupVM GetCollegeGroupById(Guid? collegeGroupId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter { ParameterName = "@Id", Value = collegeGroupId }
            };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCollegeGroupById", parameters);

            var collegeGroup = new CollegeGroupVM();
            collegeGroup = DALHelper.CreateListFromTable<CollegeGroupVM>(dt).FirstOrDefault();

            return collegeGroup;
        }

        public string AddCollegeGroup(CollegeGroupVM collegeGroup)
        {
            string collegeGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = collegeGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = collegeGroup.Description },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = collegeGroup.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = collegeGroup.CreatedBy }
                };

            collegeGroupId = Convert.ToString(DALHelper.ExecuteScalar("AddCollegeGroup", parameters));

            return collegeGroupId;
        }

        public string UpdateCollegeGroup(CollegeGroupVM collegeGroup)
        {
            string collegeGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = collegeGroup.Id },
                    new SqlParameter { ParameterName = "@Name", Value = collegeGroup.Name },
                    new SqlParameter { ParameterName = "@Description", Value = collegeGroup.Description },                    
                    new SqlParameter { ParameterName = "@IsActive", Value = collegeGroup.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = collegeGroup.UpdatedBy }
                };

            collegeGroupId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCollegeGroup", parameters));

            return collegeGroupId;
        }

        public string DeleteCollegeGroup(Guid id, int updatedBy)
        {
            string collegeGroupId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            collegeGroupId = Convert.ToString(DALHelper.ExecuteScalar("DeleteCollegeGroup", parameters));

            return collegeGroupId;
        }

        public List<SearchCollegeGroupResultVM> SearchCollegeGroup(SearchCollegeGroupParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = model.Name },                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchCollegeGroup", parameters);

            var collegeGroups = new List<SearchCollegeGroupResultVM>();
            collegeGroups = DALHelper.CreateListFromTable<SearchCollegeGroupResultVM>(dt);

            return collegeGroups;
        }

        public List<CollegeGroupVM> CheckCollegeGroupNameExist(Guid? id, string name)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@Name", Value = name }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("CheckCollegeGroupNameExist", parameters);

            var collegeGroup = new List<CollegeGroupVM>();
            collegeGroup = DALHelper.CreateListFromTable<CollegeGroupVM>(dt);

            return collegeGroup;
        }

        #endregion
    }
}