using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ProfileRepository
    {
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
                    new SqlParameter { ParameterName = "@CityId", Value = profile.CityId },
                    new SqlParameter { ParameterName = "@ZipCode", Value = profile.ZipCode },
                    new SqlParameter { ParameterName = "@VipId", Value = profile.VipId },
                    new SqlParameter { ParameterName = "@NationalityId", Value = profile.NationalityId },
                    new SqlParameter { ParameterName = "@CarRegistrationNo", Value = profile.CarRegistrationNo },
                    new SqlParameter { ParameterName = "@PassportNo", Value = profile.PassportNo  },
                    new SqlParameter { ParameterName = "@DOB", Value = profile.DOB },
                    new SqlParameter { ParameterName = "@Remarks", Value = profile.Remarks },
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
                    new SqlParameter { ParameterName = "@CityId", Value = profile.CityId },
                    new SqlParameter { ParameterName = "@ZipCode", Value = profile.ZipCode },
                    new SqlParameter { ParameterName = "@VipId", Value = profile.VipId },
                    new SqlParameter { ParameterName = "@NationalityId", Value = profile.NationalityId },
                    new SqlParameter { ParameterName = "@CarRegistrationNo", Value = profile.CarRegistrationNo },
                    new SqlParameter { ParameterName = "@PassportNo", Value = profile.PassportNo  },
                    new SqlParameter { ParameterName = "@DOB", Value = profile.DOB },
                    new SqlParameter { ParameterName = "@Remarks", Value = profile.Remarks },
                    new SqlParameter { ParameterName = "@IsMailingList", Value = profile.IsMailingList },
                    new SqlParameter { ParameterName = "@IsActive", Value = profile.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = profile.UpdatedBy }
                };

            profileId = Convert.ToString(DALHelper.ExecuteScalar("UpdateIndividualProfile", parameters));

            return profileId;
        }

        public string DeleteIndividualProfile(Guid profileId, int updatedBy)
        {
            string id = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = profileId },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = updatedBy }
                };

            id = Convert.ToString(DALHelper.ExecuteScalar("DeleteIndividualProfile", parameters));

            return id;
        }

        public List<IndividualProfileVM> GetIndividualProfileById(Guid profileId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = profileId }
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
                    new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection }
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
                    new SqlParameter { ParameterName = "@IsShowInActive", Value = model.IsShowInActive }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("SearchAdvanceProfile", parameters);

            var profiles = new List<SearchAdvanceProfileResultVM>();
            profiles = DALHelper.CreateListFromTable<SearchAdvanceProfileResultVM>(dt);

            return profiles;
        }
    }
}