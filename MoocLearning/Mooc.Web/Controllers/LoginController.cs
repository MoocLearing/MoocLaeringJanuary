using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooc.DataAccess.Context;
using Mooc.DataAccess.Entities;

namespace Mooc.Web.Controllers
{
    public class LoginController : Controller
    {
        //加了session储存用户登录状态，验证跳转主页，没跑出这个功能，是不是需要在web-config里面进行session使用的配置还是直接默认就可以用session的功能？

        // GET: Login
        public ActionResult Index()
        {

            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "Home", new { username = Session["username"].ToString() });

            }
            else
            {
                return View();
            }
            return View();
        }

        //Create session if uid&pwd correct
        [HttpPost]
        public ActionResult LoginResult(User user)
        {
            DataContext db = new DataContext();

            var userLoggedIn = db.Users.SingleOrDefault(x => x.UserName == user.UserName && x.PassWord == user.PassWord);

            if (userLoggedIn!=null)
            {
                ViewBag.message = "loggedin";
                ViewBag.triedOnce = "yes";

                Session["username"] = user.UserName;

                return RedirectToAction("Index", "Home", new {username = user.UserName});
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return RedirectToAction("Index");
            }

            
        }
    }
}