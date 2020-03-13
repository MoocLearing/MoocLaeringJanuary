using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public ActionResult Index()
        {
            return View("Login");
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult loginPost(string username, string password)
        {
            User user = _dataContext.Users.FirstOrDefault(m => m.UserName == username);

            if (user != null)
            {
                string pwd = MD5Help.MD5Encoding(password, ConfigurationManager.AppSettings["sKey"].ToString());
                if (user.PassWord == pwd)
                {
                    //Response.Cookies.Add(new HttpCookie("username")
                    //{
                    //    Value = user.UserName,
                    //    Expires = DateTime.Now.AddDays(7)
                    //});

                    CookieHelper.SetCookie("username", user.UserName, DateTime.Now.AddDays(7));
                    CookieHelper.SetCookie("userid", user.ID.ToString(), DateTime.Now.AddDays(7));
                    return Json(new { code = 0 });

                }
                return Json(new { code = 1, msg = "密码错误" });
            }
            return Json(new { code = 1, msg = "错误" });

        }




        public ActionResult DeleteCookie()
        {
            CookieHelper.DeleteCookie("username");
            CookieHelper.DeleteCookie("userid");
            //if (Request.Cookies["username"] != null)
            //{
            //    Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            //}
            return Redirect("~/Admin/Account/Index");
        }


    }
}