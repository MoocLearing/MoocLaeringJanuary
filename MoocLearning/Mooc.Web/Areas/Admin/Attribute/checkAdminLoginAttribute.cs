using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;

namespace Mooc.Web.Areas.Admin.Attribute
{
    public class CheckAdminLoginAttribute : ActionFilterAttribute
    {
        public bool IsCheck { get; set; } = true;

        private DataContext _dataContext;

        public CheckAdminLoginAttribute() { }
        public CheckAdminLoginAttribute(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck)
            {
                var auth = filterContext.HttpContext.Session["username"];
                
                //如果没有session的结果，是直接跳转login界面吗？
                if (auth == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                else
                {
                    //var user = _dataContext.Users.SingleOrDefault(c => c.UserName == auth);
                    ////判断user是admin权限
                    //if (user.RoleType == 2)
                    //{
                    //    //filter通过！！
                    //    base.OnActionExecuting(filterContext);
                    //}
                }

                    
      

                
            }
        }
    }
}