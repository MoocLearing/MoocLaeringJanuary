using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mooc.DataAccess.Context;
using Mooc.DataAccess.Entities;
using Mooc.DataAccess.Service;
using Mooc.Models.ViewModels;

namespace Mooc.Web.Controllers
{
    public class RegistController : Controller
    {

        private IUserService _userService;

        //Inject EF operation class
        public RegistController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Regist
        public ActionResult Index()
        {
            ViewBag.passwordCheckError = "";
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult UserRegist(UserView userView)
        {
            if(ModelState.IsValid)
            {
                if (userView.PasswordCheck == userView.User.PassWord)
                {
                    ViewBag.passwordCheckError = "";
                    int i = _userService.Regist(new User()
                    {
                        UserName = userView.User.UserName,
                        PassWord = userView.User.PassWord,
                        Email = userView.User.Email,
                        StudentNum = userView.User.StudentNum,
                        Gender = userView.User.Gender,
                        AddTime = DateTime.Now
                        
                    });

                    if (i > 0)
                        return RedirectToAction("Index", "Login");

                }
                else
                {
                    ViewBag.passwordCheckError = "请保持密码和校验密码一致";
                }
            }

            return View("Index");//不走action,走view 页面信息不会清空

            //_userService.Regist(new User()
            //{
            //    UserName = user.UserName,
            //    PassWord = user.PassWord,
            //    Email = user.Email,
            //    AddTime = DateTime.Now,
            //    UserState = user.UserState,
            //    RoleType = user.RoleType
            //});

            //if (!ModelState.IsValid)
            //{
            //    // if not legal, return badpage mention!!!
            //    return new EmptyResult();
            //}

            //return RedirectToAction("Index", "Login");
        }
    }
}