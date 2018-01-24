using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class TitleRepository
    {
        public List<TitleVM>  GetTitle()
        {   
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetTitle");

            var titles = new List<TitleVM>();
            titles = DALHelper.CreateListFromTable<TitleVM>(ds);

            return titles;
        }
        

        public List<TitleVM> GetTitlebyId(Guid profileId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = profileId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetTitlebyId", parameters);


            var title = new List<TitleVM>();
            title = DALHelper.CreateListFromTable<TitleVM>(dt);

            return title;
        }
    }
}