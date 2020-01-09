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
            return View();
        }

        [System.Web.Http.HttpPost]
        public async Task<ActionResult> Regist(User user)
        {

            _userService.Regist(new User()
            {
                UserName = user.UserName,
                PassWord = user.PassWord,
                Email = user.Email,
                AddTime = DateTime.Now,
                UserState = user.UserState,
                RoleType = user.RoleType
            });

            if (!ModelState.IsValid)
            {
                // if not legal, return badpage mention!!!
                return new EmptyResult();
            }

            return RedirectToAction("Index", "Login");
        }
    }
}