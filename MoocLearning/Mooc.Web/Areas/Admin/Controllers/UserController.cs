using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Service;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]
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
            List<User> users = _dataContext.Users.ToList();
            List<UserViewModel> userViewModels = AutoMapper.Mapper.Map<List<User>,List<UserViewModel>>(users);
            List<UserViewModel> list = new List<UserViewModel>();
            ViewBag.username = Session["username"];
            return View(userViewModels);
        }

        public ActionResult Create()
        {
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            var viewmodel = new AdminUserViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Save(AdminUserViewModel adminUserViewModel)
        {
            ArrayList nameList = new ArrayList();
            List<User> users = _dataContext.Users.ToList();
            foreach (var user in users)
            {
                nameList.Add(user.UserName);
            }

            if (!ModelState.IsValid)
            {
                return View("Create", adminUserViewModel);
            }

            if (nameList.Contains(adminUserViewModel.UserName))
            {
                var userInDb = _dataContext.Users.Single(c => c.UserName == adminUserViewModel.UserName);

                //User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);

                userInDb.UserName = adminUserViewModel.UserName;
                userInDb.PassWord = adminUserViewModel.PassWord;
                userInDb.Email = adminUserViewModel.Email;
                userInDb.TeacherId = adminUserViewModel.TeacherId;
            }
            else
            {
                User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);
                _dataContext.Users.Add(user);
            }

            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var user = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var adminUserViewModel = new AdminUserViewModel();

            //System.InvalidOperationException: 'The ViewData item that has the key 'TeacherId' is of type 'System.Int64'
            //but must be of type 'IEnumerable<SelectListItem>'.'

            //adminUserViewModel = AutoMapper.Mapper.Map<AdminUserViewModel>(user);

            adminUserViewModel.UserName = user.UserName;
            adminUserViewModel.PassWord = user.PassWord;
            adminUserViewModel.ConfirmPassword = user.PassWord;
            adminUserViewModel.Email = user.Email;
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();


            return View("Create", adminUserViewModel);

        }

        public ActionResult Delete(int id)
        {
            var userdel = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            if (userdel!=null)
            {
                //使用假删除，把更改userstate，前端做便利user显示时剔除userstate=2的对象
                userdel.UserState = 2;
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}