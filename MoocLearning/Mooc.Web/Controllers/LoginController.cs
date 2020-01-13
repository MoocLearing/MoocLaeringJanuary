using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mooc.DataAccess.Context;
using Mooc.DataAccess.Entities;

namespace Mooc.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _dataContext;

        public LoginController(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

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

            //return View();
        }


        [HttpPost]
        public ActionResult LoginResult(User user)
        {
            //这里加ModelState.Isvalid永远都返false
            if (true)
            {
                var userModel =
                    this._dataContext.Users.FirstOrDefault(x =>
                        x.UserName == user.UserName && x.PassWord == user.PassWord);

                if (userModel != null)
                {

                    HttpCookie cookie = new HttpCookie("userinfo");
                    cookie.Value = userModel.UserName;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码不存在");
                    return View("Index", user);
                }

            }

            //DataContext db = new DataContext();//using 或者dispose或者autofac

            //var userLoggedIn = db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.PassWord == user.PassWord);

            //if (userLoggedIn!=null)
            //{
            //    //ViewBag.message = "loggedin";
            //    //ViewBag.triedOnce = "yes";

            //    Session["username"] = user.UserName;

            //    return RedirectToAction("Index", "Home", new {username = user.UserName});
            //}
            //else
            //{
            //   // ViewBag.triedOnce = "yes";
            //    return RedirectToAction("Index");
            //}


        }
    }
}
