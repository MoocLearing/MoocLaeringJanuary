using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Service;
using Mooc.Data.ViewModels;
using Mooc.Utils.Enums;
using Mooc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class RegisterController : Controller
    {
        private TeacherOption _option = new TeacherOption();
        private IUserService _userService;
        private DataContext _dataContext;

        //Inject EF operation class
        public RegisterController(IUserService userService, DataContext dataContext)
        {
            _userService = userService;
            _dataContext = dataContext;
        }

        // GET: Regist
        public ActionResult Index()
       {
            //List<SelectListItem> enumsList = ((RoleTypeEnum[])Enum.GetValues(typeof(SelectListItem))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();

            //ViewBag.RoleTypeList = enumsList;
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult Register(UserViewModel userView)
        {

            ViewBag.TeacherList = _option.GetTeacherSelectOptions();

            if (ModelState.IsValid)
            {

                //ViewBag.passwordCheckError = "";
                //int i = _userService.Regist(new User()
                //{
                //    UserName = userView.User.UserName,
                //    PassWord = userView.User.PassWord,
                //    Email = useView.User.Email,
                //    StudentNum = userView.User.StudentNum,
                //    Gender = userView.User.Gender,
                //    AddTime = DateTime.Now

                //});
                var model=this._dataContext.Users.FirstOrDefault(p => p.UserName == userView.UserName);
                if (model != null)
                {

                    ModelState.AddModelError("","用户已存在");
                    return View("Index", userView);
                }
                User user = AutoMapper.Mapper.Map<User>(userView);
                user.UserState = 0;
                user.TeacherId =Convert.ToInt32( userView.TeacherIds);
                user.RoleType = (int)RoleTypeEnum.student;
                int i = _userService.Regist(user);

                if (i > 0)
                        return RedirectToAction("Index", "Login");
                
            }


            return View("Index", userView);//不走action,走view 页面信息不会清空

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