using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Enums;
using Mooc.Data.Service;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
  //  [CheckAdminLogin]
    public class UserController : Controller
    {
        private TeacherOption _option = new TeacherOption();
        private IUserService _userService;
        private DataContext _dataContext;

        //Inject EF operation class
        public UserController(IUserService userService, DataContext dataContext)
        {
            _userService = userService;
            _dataContext = dataContext;
        }


        // GET: Admin/User
        public ActionResult Index()
        {
            List<User> userList = _dataContext.Users.ToList();
            //List<UserViewModel> userViewModels = AutoMapper.Mapper.Map<List<User>,List<UserViewModel>>(users);
            List<UserViewModel> list = new List<UserViewModel>();
            if (userList != null && userList.Count > 0)
            {
                list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
            }

            ViewBag.username = Session["username"];
            return View(list);
        }

        public ActionResult AjaxList()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetUserList()
        {
            List<User> userList = _dataContext.Users.ToList();

            List<UserViewModel> list = new List<UserViewModel>();
            if (userList != null && userList.Count > 0)
            {
                list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
            }
            return Json(new { list = list });
        }

        public ActionResult AjaxPartialList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _PartUserList()
        {
            List<User> userList = _dataContext.Users.ToList();

            List<UserViewModel> list = new List<UserViewModel>();
            if (userList != null && userList.Count > 0)
            {
                list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
            }
            return PartialView(list);
        }

        public ActionResult Create()
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            var viewmodel = new UserViewModel() { ID = 0 };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserViewModel model)
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            if (!ModelState.IsValid)
            {
                User user = new User();

                if (model.ID > 0)
                {
                    user = _dataContext.Users.Find(model.ID);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "当前用户不存在");
                        return RedirectToAction("Create", model);
                    }
                    user = AutoMapper.Mapper.Map<User>(model);
                    user.UserState = (int)StatusEnum.Normal;
                    user.RoleType = model.RoleName.ToInt(3);
                }
                else
                {
                    user = _dataContext.Users.FirstOrDefault(c => c.UserName == model.UserName);
                    if (user != null)
                    {
                        ModelState.AddModelError("", "当前用户已存在");
                        return RedirectToAction("Create", model);
                    }
                    user = AutoMapper.Mapper.Map<User>(model);
                    user.UserState = (int)StatusEnum.Normal;
                    user.RoleType = model.RoleName.ToInt(model.RoleType);
                    _dataContext.Users.Add(user);

                }

                _dataContext.SaveChanges();
                return RedirectToAction("Index");

            }

            return RedirectToAction("Create", model);
            //ArrayList nameList = new ArrayList();
            //List<User> users = _dataContext.Users.ToList();
            //foreach (var user in users)
            //{
            //    nameList.Add(user.UserName);
            //}

            //if (!ModelState.IsValid)
            //{
            //    return View("Create", adminUserViewModel);
            //}

            //if (nameList.Contains(adminUserViewModel.UserName))
            //{
            //    var userInDb = _dataContext.Users.Single(c => c.UserName == adminUserViewModel.UserName);

            //    //User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);

            //    userInDb.UserName = adminUserViewModel.UserName;
            //    userInDb.PassWord = adminUserViewModel.PassWord;
            //    userInDb.Email = adminUserViewModel.Email;
            //    userInDb.TeacherId = adminUserViewModel.TeacherId;
            //}
            //else
            //{
            //    User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);
            //    _dataContext.Users.Add(user);
            //}

            //_dataContext.SaveChanges();
            // return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            /// var user = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            var user = _dataContext.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            // var adminUserViewModel = new UserViewModel();

            //System.InvalidOperationException: 'The ViewData item that has the key 'TeacherId' is of type 'System.Int64'
            //but must be of type 'IEnumerable<SelectListItem>'.'

            var adminUserViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);
            

            //adminUserViewModel.UserName = user.UserName;
            //adminUserViewModel.PassWord = user.PassWord;
            //adminUserViewModel.ConfirmPassword = user.PassWord;
            //adminUserViewModel.Email = user.Email;
            //ViewBag.TeacherList = _option.GetTeacherSelectOptions();


            return View("Create", adminUserViewModel);

        }

        public ActionResult Delete(int id)
        {
            var user = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            if (user != null)
            {
                //使用假删除，把更改userstate，前端做便利user显示时剔除userstate=2的对象
                //userdel.UserState = 2;
                //_dataContext.SaveChanges();
                //return RedirectToAction("Index");
                _dataContext.Users.Remove(user);
                _dataContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

       //[HttpPost]
       // public ActionResult AjaxEdit(int id)
       // {
       //     var user = _dataContext.Users.Find(id);

       //     return Json(new {code=0 });
       // }

        public ActionResult AjaxEdit(int id)
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();

            var user = _dataContext.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var adminUserViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);

            return View("Create", adminUserViewModel);

        }


    }
}