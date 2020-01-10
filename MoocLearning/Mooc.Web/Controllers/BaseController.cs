using Mooc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class BaseController : Controller
    {
        public User user;
        public BaseController()
        {
            if (Session["User"] == null)
            {
                Response.Redirect("/Login/Index", true);
                return;
            }
            else
            {
                user = Session["User"] as User;
               
            }
        }
       
    }
}