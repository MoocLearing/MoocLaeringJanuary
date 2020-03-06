using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Mooc.Data.Context;

namespace Mooc.Web.Areas.Admin.Attribute
{
    public class CheckAdminLoginAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);

            //当用户存储在cookie中且session数据为空时，把cookie的数据同步到session中
            //if (filterContext.HttpContext.Request.Cookies["username"] != null &&
            //    filterContext.HttpContext.Session["username"] == null)
            //{
            //    filterContext.HttpContext.Session["username"] = filterContext.HttpContext.Request.Cookies["username"].Value;
            //    filterContext.HttpContext.Session["userid"] = filterContext.HttpContext.Request.Cookies["userid"].Value;
            //}

            //没cookie没session验证，直接跳转到~/admin/account/login/
            //if (!(filterContext.HttpContext.Session["username"] != null ||
            //    filterContext.HttpContext.Request.Cookies["username"] != null))
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
            //    {
            //        {"controller","Account" },
            //        {"action","Index" }
            //    });
            //}

            if (filterContext.HttpContext.Request.Cookies["username"] == null||filterContext.HttpContext.Request.Cookies["userid"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    {"controller","Account" },
                    {"action","Login" }
                });
            }

        }

    }
}