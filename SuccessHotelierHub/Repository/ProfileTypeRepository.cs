using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class ProfileTypeRepository
    {
        public List<ProfileTypeVM> GetProfileType(string name)
        {
            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Name", Value = name}
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetProfileType", parameters);

            var profileTypes = DALHelper.CreateListFromTable<ProfileTypeVM>(dt);

            return profileTypes;
        }
    }
}