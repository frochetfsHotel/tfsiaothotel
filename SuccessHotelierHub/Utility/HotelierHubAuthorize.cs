using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub
{
    public class HotelierHubAuthorizeAttribute : AuthorizeAttribute
    {
        public string Roles { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (LogInManager.LoggedInUser != null)
            {
                if (string.IsNullOrEmpty(Roles))
                {
                    return true;
                }

                string[] split = Roles.Split(',');

                foreach (CurrentUserRoleVM role in LogInManager.UsersRoles)
                {
                    if (split.Contains(role.Code))
                    {
                        return true;
                    }
                }
                return false;

            }
            else
                return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            if (LogInManager.LoggedInUser != null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                }
                else
                {
                    filterContext.HttpContext.Items["UnauthorizeRequest"] = true;
                }
            }
            else
            {
                filterContext.HttpContext.Items["SessionExpired"] = true;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}