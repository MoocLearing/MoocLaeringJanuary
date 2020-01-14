using Mooc.Data.Context;
using Mooc.Data.Service;
using Mooc.Data.ViewModels;
using Mooc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
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
            List<UserViewModel> list = new List<UserViewModel>();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
            return View();
        }
    }
}