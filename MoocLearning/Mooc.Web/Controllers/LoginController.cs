using Mooc.Data.Context;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly DataContext _dataContext;

        public LoginController(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        // GET: Login
        public ActionResult Index()
        {
            //if (!string.IsNullOrEmpty(GetSession<string>(loginSessionName)))
            //{
            //    return RedirectToAction("Index", "Home");

            //}
           return View();
        }


        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            var user = _dataContext.Users.FirstOrDefault(m => m.UserName == username);

            if (user != null)
            {
                string pwd = MD5Help.MD5Encrypt(password, ConfigurationManager.AppSettings["sKey"].ToString());
                if (user.PassWord == pwd)
                {
                    Session["username"] = user.UserName;
                    Session["userid"] = user.ID;
                    return Json(new { code = 0 });
                }
                else
                {
                    return Json(new { code = 1, msg = "密码不正确" });
                }

            }
            else
            {
                return Json(new { code = 1, msg = "用户不存在" });
            }


        }


        [HttpPost]
        public ActionResult LoginResult(LoginViewModel user)
        {
            //这里加ModelState.Isvalid永远都返false,y因为传的过来什么就验证什么，有空项就报错
            if (ModelState.IsValid)
            {
                var userModel =
                    this._dataContext.Users.FirstOrDefault(x =>
                        x.UserName == user.UserName && x.PassWord == user.PassWord);

                if (userModel != null)
                {

                    //HttpCookie cookie = new HttpCookie("userinfo");
                    //cookie.Value = userModel.UserName;
                    //cookie.Expires = DateTime.Now.AddDays(1);
                    //Response.Cookies.Add(cookie);


                    //cookied 用法 看helper 类
                    CookieHelper.SetCookie(loginSessionId, userModel.ID.ToString());
                    CookieHelper.SetCookie(loginSessionName, userModel.UserName);

                   // SetSession(loginSessionName, userModel.UserName);
                    
                    
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码不存在");
                    return View("Index", user);
                }

            }
            return View("Index", user);

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

        public ActionResult LoginResult2(LoginViewModel user)
        {
            //里面自己实现
            return Json(new { code = 200, error = 300 });
        }
    }
}
