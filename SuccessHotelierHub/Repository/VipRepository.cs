using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class VipRepository
    {
        public List<VipVM> GetVip()
        {
            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetVip");

            var vips = new List<VipVM>();
            vips = DALHelper.CreateListFromTable<VipVM>(dt);

            return vips;
        }
    }
}