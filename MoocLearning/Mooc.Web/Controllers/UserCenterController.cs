using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class UserCenterController : Controller
    {
        // GET: UserCenter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Person(long? id)
        {

            return Content(id.ToString());
        }
    }
}