﻿using Mooc.Data.Context;
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
            return RedirectToAction("Login");
        }

       
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string username, string password, string checkme)
        {

            User user = _dataContext.Users.FirstOrDefault(m => m.UserName == username);

            if (user != null)
            {
                string Password = MD5Help.MD5Decrypt(user.PassWord, ConfigurationManager.AppSettings["sKey"].ToString());
                if (Password == password)
                {
                    Response.Cookies.Add(new HttpCookie("username")
                    {
                        Value = user.UserName,
                        Expires = DateTime.Now.AddDays(7)
                    });

                }
                else
                {
                    return Json(new { code = 1, msg = "用户名密码不存在" });
                }
                //if (checkme == "checked")
                //{
                

                //}
                //else
                //{
                //    Session["username"] = user.UserName;
                //    Session["userid"] = user.ID;
                //}


                //Session["username"] = user.UserName;
                //Session["userid"] = user.ID;

                return Redirect("~/Admin/User/Index");
            }
            else
            {
                return Json(new { code = 1, msg = "用户名密码不存在" });
            }


        }

        public ActionResult DeleteCookie()
        {
  
            if (Request.Cookies["username"] != null)
            {
                Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            }
            return Redirect("~/Admin/Account/Index");
        }


    }
}