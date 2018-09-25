using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SuccessHotelierHub.Models;
using SuccessHotelierHub.Utility;
using SuccessHotelierHub.Repository;

namespace SuccessHotelierHub
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            // Date format overwrite - culture information
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;

            //Get All Currency Info
            if (System.Web.HttpContext.Current.Application["CurrencyInfo"] == null)
            {
                CurrencyRepository currencyRepository = new CurrencyRepository();
                var currencyInfo = currencyRepository.GetCurrencyInfo();
                System.Web.HttpContext.Current.Application["CurrencyInfo"] = currencyInfo;
            }

        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Application["CurrencyInfo"] != null)
            {
                System.Web.HttpContext.Current.Application["CurrencyInfo"] = null;
            }

            if (Context.Items["SessionExpired"] is bool)
            {
                Response.Redirect("~/Account/Login");
            }
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
            if (Context.Items["UnauthorizeRequest"] is bool)
            {
                Response.Redirect("~/Home/Unauthorize");
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            if (exc != null && exc.TargetSite.DeclaringType.FullName == "LogInManager.LoggedInUser") //When Session expired then redirect to Login Screen
            {
                Response.Redirect("~/Account/Login");
            }
            // Clear the error from the server
            Server.ClearError();

            //Response.Redirect("~/Home/Error");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            ////Set NoCache.
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
        }
        
        protected void Session_End(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();            

            #region  Update Logout Time based on Session Id.

            var sessionId = Session.SessionID;

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                RecordActivityLog.UpdateLogOutTimeBySessionId(sessionId);
            }

            #endregion
        }
    }
}
