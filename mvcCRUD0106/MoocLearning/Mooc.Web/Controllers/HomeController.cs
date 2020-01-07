using Mooc.DataAccess.Context;
using Mooc.DataAccess.Service;
using Mooc.Models.ViewModels;
using Mooc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubjectCrud _subjectCrud;
        private readonly IUserService _userService;
        private readonly DataContext _context;
        public HomeController(IUserService userService, DataContext context, ISubjectCrud subjectCrud)
        {
            this._subjectCrud = subjectCrud;
            this._userService = userService;
            this._context = context;
        }
        public ActionResult Index()
        {
            //TestModel testModel = new TestModel();
            //testModel.GetList();

            var list2 = this._subjectCrud.ListAsync();
            var userlist = this._context.Users.ToList();
            using (DataContext db = new DataContext())
            {
              var li=  db.Subjects.ToList();
            }

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            var subjectList = this._context.Subjects.ToList();
            var list = this._userService.GetList();
            List<UserViewModel> viewList = new List<UserViewModel>();
            UserViewModel model = null;
            foreach (var item in list)
            { 
                //model = new UserViewModel {
                //   UserName=item.UserName
                //};
                model = new UserViewModel();
                model.UserName = item.UserName;
                viewList.Add(model);
            }
           viewList = AutoMapper.Mapper.Map<List<UserViewModel>>(list);
            return View();
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