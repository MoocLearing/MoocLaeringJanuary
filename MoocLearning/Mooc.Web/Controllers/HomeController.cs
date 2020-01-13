using Mooc.DataAccess.Entities;
using Mooc.DataAccess.Service;
using Mooc.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            this._userService = userService;
        }
        public ActionResult Index()
        {


            HttpCookie cookie = Request.Cookies["userinfo"];
            if (cookie!=null)
            {
                ViewBag.username = cookie.Value;
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            // var user = Session["User"];

            //if (Session["User"] ==null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            //else
            //{
            //    var user= Session["User"] as User;
            //    ViewBag.username = username;
            //    return View();
            //}

            //var list = this._userService.GetList();
            //List<UserViewModel> models = AutoMapper.Mapper.Map<List<UserViewModel>>(list);

        }

        public ActionResult DeleteCookie()
        {
            HttpCookie cookie = Request.Cookies["userinfo"];
            if(cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
            }
            return View("index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}