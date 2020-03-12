using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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
            var courses = _dataContext.Courses.Where(x=>x.Status==1).ToList();
            return PartialView(courses);
        }

        //return ajax course list
        [HttpPost]
        public JsonResult GetCourse()
        {
           
           var courses =  _dataContext.Courses.ToList();
            if (courses.Count>0)
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

        public ActionResult _PartJavaCourse(string categoryName)
        {

            Category category = _dataContext.Categorys.FirstOrDefault(x => x.CategoryName == categoryName);
            if (category==null)
            {
                return Json(new { code = 1, msg = "课程类别不存在" });
            }
            var courses = _dataContext.Courses.Where(x => x.CategoryId == category.ID && x.Status==1).ToList();

            if (courses.Count==0)
            {
                return Json(new { code = 1, msg = "课程名称不存在" });
            }
            var courseList = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);

            return PartialView(courseList);
        }

        public ActionResult ShowChapter(long courseId)
        {
            if (courseId<=0)
            {
                return Json(new {code = 1, msg = "error"});
            }

            var chapterList = _dataContext.Chapters.Where(x => x.CourseId == courseId).ToList();
            return View(chapterList);
        }

        public ActionResult Play(long? id)
        {
            if (id == null)
                return HttpNotFound();
            var model = _dataContext.Chapters.Find(id);
            if (model == null)
                return HttpNotFound();
            if (string.IsNullOrEmpty(model.VideoGuid))
                return Content("未上传视频");
            return View(model);
        }


        public ActionResult _IOSCourseList()
        {
            CourseChaptersView views = new CourseChaptersView();
            Category category = _dataContext.Categorys.FirstOrDefault(x => x.CategoryName == "IOS");
            if (category == null)
            {
                return Json(new { code = 1, msg = "IOS课程类别不存在" });
            }
            var courses = _dataContext.Courses.Where(x => x.CategoryId == category.ID && x.Status==1).ToList();

            if (courses.Count > 0)
            {
               List<CourseAddView> viewList = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);

               CourseAddView emptycCourseAddView = new CourseAddView(){ID=0,CourseName = "No Course Name",CourseDetail = "No CourseDetail",CoverPic = ""};

               while (viewList.Count<3)
               {
                   viewList.Add(emptycCourseAddView);
               }
               

                return PartialView(viewList);

            }

            return HttpNotFound();
        }
    }
}