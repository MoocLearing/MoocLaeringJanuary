using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;

namespace Mooc.Web.Areas.Admin.Controllers
{
    public class RazorAccountController : Controller
    {
        private DataContext _dataContxt;

        public RazorAccountController(DataContext dataContext)
        {
            _dataContxt = dataContext;
        }
        // GET: Admin/RazorAccount
        public ActionResult Index()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserRazorView userRazorView)
        {

            if (ModelState.IsValid)
            {
                User user = _dataContxt.Users.FirstOrDefault(m => m.UserName == userRazorView.UserName && m.PassWord == userRazorView.PassWord);
                long userid = user.ID;

                if (userRazorView.RememberMe)
                {
                    Response.Cookies.Add(new HttpCookie("username")
                    {
                        Value = userRazorView.UserName,
                        Expires = DateTime.Now.AddDays(7)
                    });
                    Response.Cookies.Add(new HttpCookie("userid")
                    {
                        Value = userid.ToString(),
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                else
                {
                    Session["username"] = userRazorView.UserName;
                    Session["userid"] = userid;
                }

                return Redirect("~/Admin/RazorUser/Index");

            }
            else
            {
                ModelState.AddModelError("", "您的账号密码有误");
            }

            return View(userRazorView);
        }
    }
}