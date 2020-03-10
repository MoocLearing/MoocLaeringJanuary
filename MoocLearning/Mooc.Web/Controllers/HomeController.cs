using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Mongo;
using Mooc.Data.ViewModels;

namespace Mooc.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataContext _dataContext;


        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.username = Session["username"];
            return View();
        }


        public ActionResult _PartCourseList()
        {
            var courses = _dataContext.Courses.ToList();
            return PartialView(courses);
        }

        //return ajax course list
        [HttpPost]
        public JsonResult GetCourse()
        {
           
           var courses =  _dataContext.Courses.ToList();
            if (courses != null)
            {
                var addCourses = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);
                return Json(addCourses);
            }

            return Json(new { code=1, msg="not found any course"});
        }



        public ActionResult Show(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    return Content("参数错误");

                var bytes = MongoDBHelper.down(filename);
                if (bytes == null || bytes.Length == 0)
                    return Content("图片不存在");
                return File(bytes, "image/jpeg", filename);
            }
            catch (Exception e)
            {
                return Content("出错：" + e.Message);
            }
        }
    }
}