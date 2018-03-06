using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Models.StoredProcedure;

namespace SuccessHotelierHub.Repository
{
    public class ProfileRepository
    {
        #region Profile

        public string AddIndividualProfile(IndividualProfileVM profile)
        {
            string profileId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ProfileTypeId", Value = profile.ProfileTypeId },
                    new SqlParameter { ParameterName = "@FirstName", Value = profile.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = profile.LastName } ,
                    new SqlParameter { ParameterName = "@TitleId", Value = profile.TitleId },
                    new SqlParameter { ParameterName = "@TelephoneNo", Value = profile.TelephoneNo },
                    new SqlParameter { ParameterName = "@BusinessTelephoneNo", Value = profile.BusinessTelephoneNo },
                    new SqlParameter { ParameterName = "@Email", Value = profile.Email },
                    new SqlParameter { ParameterName = "@Address", Value = profile.Address },
                    new SqlParameter { ParameterName = "@HomeAddress", Value = profile.HomeAddress },
                    new SqlParameter { ParameterName = "@CountryId", Value = profile.CountryId },
                    new SqlParameter { ParameterName = "@StateId", Value = profile.StateId },
                    new SqlParameter { ParameterName = "@StateName", Value = profile.StateName },
                    new SqlParameter { ParameterName = "@CityId", Value = profile.CityId },
                    new SqlParameter { ParameterName = "@CityName", Value = profile.CityName },
                    new SqlParameter { ParameterName = "@ZipCode", Value = profile.ZipCode },
                    new SqlParameter { ParameterName = "@VipId", Value = profile.VipId },
                    new SqlParameter { ParameterName = "@NationalityId", Value = profile.NationalityId },
                    new SqlParameter { ParameterName = "@CarRegistrationNo", Value = profile.CarRegistrationNo },
                    new SqlParameter { ParameterName = "@PassportNo", Value = profile.PassportNo  },
                    new SqlParameter { ParameterName = "@DOB", Value = profile.DOB },
                    //new SqlParameter { ParameterName = "@Remarks", Value = profile.Remarks },
                    new SqlParameter { ParameterName = "@Remarks", Value = string.Empty },
                    new SqlParameter { ParameterName = "@IsMailingList", Value = profile.IsMailingList },
                    new SqlParameter { ParameterName = "@IsActive", Value = profile.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = profile.CreatedBy }
                };

            profileId = Convert.ToString(DALHelper.ExecuteScalar("AddIndividualProfile", parameters));

            return profileId;
        }

        public string UpdateIndividualProfile(IndividualProfileVM profile)
        {
            string profileId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = profile.Id },
                    new SqlParameter { ParameterName = "@ProfileTypeId", Value = profile.ProfileTypeId },
                    new SqlParameter { ParameterName = "@FirstName", Value = profile.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = profile.LastName } ,
                    new SqlParameter { ParameterName = "@TitleId", Value = profile.TitleId },
                    new SqlParameter { ParameterName = "@TelephoneNo", Value = profile.TelephoneNo },
                    new SqlParameter { ParameterName = "@BusinessTelephoneNo", Value = profile.BusinessTelephoneNo },
                    new SqlParameter { ParameterName = "@Email", Value = profile.Email },
                    new SqlParameter { ParameterName = "@Address", Value = profile.Address },
                    new SqlParameter { ParameterName = "@HomeAddress", Value = profile.HomeAddress },
                    new SqlParameter { ParameterName = "@CountryId", Value = profile.CountryId },
                    new SqlParameter { ParameterName = "@StateId", Value = profile.StateId },
                    new SqlParameter { ParameterName = "@StateName", Value = profile.StateName },
                    new SqlParameter { ParameterName = "@CityId", Value = profile.CityId },
                    new SqlParameter { ParameterName = "@CityName", Value = profile.CityName },
                    new SqlParameter { ParameterName = "@ZipCode", Value = profile.ZipCode },
                    new SqlParameter { ParameterName = "@VipId", Value = profile.VipId },
                    new SqlParameter { ParameterName = "@NationalityId", Value = profile.NationalityId },
                    new SqlParameter { ParameterName = "@CarRegistrationNo", Value = profile.CarRegistrationNo },
                    new SqlParameter { ParameterName = "@PassportNo", Value = profile.PassportNo  },
                    new SqlParameter { ParameterName = "@DOB", Value = profile.DOB },
                    //new SqlParameter { ParameterName = "@Remarks", Value = profile.Remarks },
                    new SqlParameter { ParameterName = "@Remarks", Value = string.Empty },
                    new SqlParameter { ParameterName = "@IsMailingList", Value = profile.IsMailingList },
                    new SqlParameter { ParameterName = "@IsActive", Value = profile.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = profile.UpdatedBy }
                };

            profileId = Convert.ToString(DALHelper.ExecuteScalar("UpdateIndividualProfile", parameters));

            return profileId;
        }

        public string DeleteIndividualProfile(Guid profileId, int updatedBy, int userId)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = profileId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteIndividualProfile", parameters));

            return id;
        }

        public List<IndividualProfileVM> GetIndividualProfileById(Guid profileId, int? userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = profileId },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetIndividualProfileById", parameters);


            var profile = new List<IndividualProfileVM>();
            profile = DALHelper.CreateListFromTable<IndividualProfileVM>(dt);

            return profile;
        }

        public List<SearchIndividualProfileResultVM> SearchIndividualProfile(SearchIndividualProfileParametersVM model, string sortColumn, string sortDirection)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@TelephoneNo", Value = model.TelephoneNo },
                    new SqlParameter { ParameterName = "@Email", Value = model.Email },                    
                    new SqlParameter { ParameterName = "@PageNum", Value = model.PageNum },
                    new SqlParameter { ParameterName = "@PageSize", Value = model.PageSize },
                    new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn },
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@IsAdminUser", Value = model.IsAdminUser }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchIndividualProfile", parameters);

            var profiles = new List<SearchIndividualProfileResultVM>();
            profiles = DALHelper.CreateListFromTable<SearchIndividualProfileResultVM>(dt);

            return profiles;
        }

        public List<SearchAdvanceProfileResultVM> SearchAdvanceProfile(SearchAdvanceProfileParametersVM model)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@LastName", Value = model.LastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = model.FirstName },
                    new SqlParameter { ParameterName = "@ViewBy", Value = model.ViewBy },
                    new SqlParameter { ParameterName = "@City", Value = model.City },
                    new SqlParameter { ParameterName = "@PostalCode", Value = model.PostalCode},
                    new SqlParameter { ParameterName = "@MemberTypeId", Value = model.MemberTypeId },                    
                    new SqlParameter { ParameterName = "@MemberNo", Value = model.MemberNo },
                    new SqlParameter { ParameterName = "@Keyword", Value = model.Keyword },
                    new SqlParameter { ParameterName = "@Communication", Value = model.Communication },
                    new SqlParameter { ParameterName = "@ClientID", Value = model.ClientID },
                    new SqlParameter { ParameterName = "@CorpNo", Value = model.CorpNo },
                    new SqlParameter { ParameterName = "@IATANo", Value = model.IATANo},
                    new SqlParameter { ParameterName = "@IsNegRates", Value = model.IsNegRates},
                    new SqlParameter { ParameterName = "@IsShowInActive", Value = model.IsShowInActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = model.CreatedBy },
                    new SqlParameter { ParameterName = "@IsAdminUser", Value = model.IsAdminUser }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceProfile", parameters);

            var profiles = new List<SearchAdvanceProfileResultVM>();
            profiles = DALHelper.CreateListFromTable<SearchAdvanceProfileResultVM>(dt);

            return profiles;
        }

        public List<IndividualProfileVM> GetIndividualProfiles(string lastName, string firstName, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@LastName", Value = lastName },
                    new SqlParameter { ParameterName = "@FirstName", Value = firstName },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetIndividualProfiles", parameters);


            var profiles = new List<IndividualProfileVM>();
            profiles = DALHelper.CreateListFromTable<IndividualProfileVM>(dt);

            return profiles;
        }

        public void DeleteAllProfile(int updatedBy, int userId)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            DALHelper.ExecuteScalar("DeleteAllProfile", parameters);
        }

        public List<IndividualProfileVM> GetIndividualProfileByUserId(int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetIndividualProfileByUserId", parameters);


            var profile = new List<IndividualProfileVM>();
            profile = DALHelper.CreateListFromTable<IndividualProfileVM>(dt);

            return profile;
        }

        #endregion

        #region Temp Bulk Profiles

        public void LoadDefaultIndividualProfile(int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            DALHelper.Execute("LoadDefaultIndividualProfile", parameters);
        }

        #endregion
                
        #region Profile Remarks

        public List<ProfileRemarksResultVM> GetProfileRemarks(Guid profileId, Guid? id, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@ProfileId", Value = profileId },
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetProfileRemarks", parameters);

            var remarks = new List<ProfileRemarksResultVM>();
            remarks = DALHelper.CreateListFromTable<ProfileRemarksResultVM>(dt);

            return remarks;
        }

        public List<ProfileRemarkVM> GetProfileRemarkById(Guid id, int userId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetProfileRemarkById", parameters);

            var remark = new List<ProfileRemarkVM>();
            remark = DALHelper.CreateListFromTable<ProfileRemarkVM>(dt);

            return remark;
        }

        public string AddProfileRemark(ProfileRemarkVM remark)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@ProfileId", Value = remark.ProfileId },
                    new SqlParameter { ParameterName = "@Remarks", Value = remark.Remarks },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = remark.CreatedBy },
                    new SqlParameter { ParameterName = "@CreatedOn", Value = remark.CreatedOn },
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("AddProfileRemark", parameters));

            return remarkId;
        }

        public string UpdateProfileRemark(ProfileRemarkVM remark)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = remark.Id },
                    new SqlParameter { ParameterName = "@ProfileId", Value = remark.ProfileId },
                    new SqlParameter { ParameterName = "@Remarks", Value = remark.Remarks },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = remark.UpdatedBy },
                    new SqlParameter { ParameterName = "@UpdatedOn", Value = remark.UpdatedOn },
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("UpdateProfileRemark", parameters));

            return remarkId;
        }

        public string DeleteProfileRemark(Guid id, int updatedBy, int userId)
        {
            string remarkId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy },
                    new SqlParameter { ParameterName = "@UserId", Value = userId }
                };

            remarkId = Convert.ToString(DALHelper.ExecuteScalar("DeleteProfileRemark", parameters));

            return remarkId;
        }

        #endregion
    }
}