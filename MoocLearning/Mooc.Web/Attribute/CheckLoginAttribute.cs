using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mooc.Web.Attribute
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies[LoginHelper.loginCookieId] == null || filterContext.HttpContext.Request.Cookies[LoginHelper.loginCookieName] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    {"controller","Login" },
                    {"action","Index" }
                });
            }
        }
    }
}