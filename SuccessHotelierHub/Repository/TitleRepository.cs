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
    }
}