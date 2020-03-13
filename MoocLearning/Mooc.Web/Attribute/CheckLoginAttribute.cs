using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Attribute
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public bool isCheck { get; set; } = false;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (isCheck)
            {
                var auth = filterContext.HttpContext.Request.Cookies["username"];// filterContext.HttpContext.Session["username"];
                if (auth == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
            }
        }
    }
}