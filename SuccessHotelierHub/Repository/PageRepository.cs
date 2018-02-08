using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class PageRepository
    {
        #region Page

        public List<PagesVM> GetPages()
        {
            var ds = DALHelper.GetDataTableWithExtendedTimeOut("GetPages");

            var pages = new List<PagesVM>();
            pages = DALHelper.CreateListFromTable<PagesVM>(ds);

            return pages;
        }

        public List<PagesVM> GetPageById(Guid pageId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = pageId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPageById", parameters);
            
            var page = new List<PagesVM>();
            page = DALHelper.CreateListFromTable<PagesVM>(dt);

            return page;
        }

        public List<PagesVM> GetPageDetailByPageCode(string pageCode)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@PageCode", Value = pageCode }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetPageDetailByPageCode", parameters);

            var page = new List<PagesVM>();
            page = DALHelper.CreateListFromTable<PagesVM>(dt);

            return page;
        }

        #endregion
    }
}