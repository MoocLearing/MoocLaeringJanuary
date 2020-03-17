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
using Mooc.Utils;
using Mooc.Web.Attribute;
using Mooc.Web.Models;

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
            string s = DecryptStringAES.DecryptByAES("3XpsTtxo5oO3AKeihGorxg==");

            return View();
        }


        public ActionResult _PartCourseList()
        {
            var courses = _dataContext.Courses.Where(x => x.Status == 1).ToList();
            return PartialView(courses);
        }

        //return ajax course list
        [HttpPost]
        public JsonResult GetCourse()
        {

            var courses = _dataContext.Courses.ToList();
            if (courses.Count > 0)
            {
                var addCourses = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);
                return Json(addCourses);
            }

            return Json(new { code = 1, msg = "not found any course" });
        }

        [HttpPost]
        public JsonResult IsLogin()
        {
            bool isLogin = LoginHelper.UserId > 0 && !string.IsNullOrWhiteSpace(LoginHelper.UserName);
            return Json(isLogin ? 1 : 0);
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
            List<CourseAddView> courseList = new List<CourseAddView>();
            Category category = _dataContext.Categorys.FirstOrDefault(x => x.CategoryName == categoryName);
            if (category == null)
            {
                // return Json(new { code = 1, msg = "课程类别不存在" });

                return PartialView(courseList);
            }
            var courses = _dataContext.Courses.Where(x => x.CategoryId == category.ID && x.Status == 1).ToList();

            if (courses.Count == 0)
            {
                // return Json(new { code = 1, msg = "课程名称不存在" });
                return PartialView(courseList);
            }
            courseList = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);

            return PartialView(courseList);
        }

        public ActionResult ShowChapter(long courseId)
        {
            if (courseId <= 0)
            {
                return Json(new { code = 1, msg = "error" });
            }

            var chapterList = _dataContext.Chapters.Where(x => x.CourseId == courseId).ToList();
            return View(chapterList);
        }

        public ActionResult Play(long? id)
        {
            if (Session["userid"] == null)
            {
                return HttpNotFound("未登录");
            }

            if (id == null)
                return HttpNotFound("参数错误");
            var model = _dataContext.Chapters.Find(id);

            if (model == null || string.IsNullOrEmpty(model.VideoGuid))
                return HttpNotFound("未上传视频");
            var userid = (long)Session["userid"];
            if (userid > 0)
            {
                var appliedCourse = _dataContext.CourseApplies.Where(x => x.UserId == userid && x.CourseId == model.CourseId).ToList();

                foreach (var item in appliedCourse)
                {
                    var schedule = _dataContext.Schedules.Find(item.ScheduleId);
                    if (schedule != null)
                    {
                        if (schedule.StartTime < DateTime.Now.Date && schedule.EndTime > DateTime.Now.Date)
                        {
                            return View(model);
                            break;
                        }
                    }

                }
            }


            return HttpNotFound("没有权限");
        }

        //测试attribute的工作
        //[CheckLoginAttribute]
        public ActionResult _IOSCourseList()
        {
            List<CourseAddView> courseList = new List<CourseAddView>();
            Category category = _dataContext.Categorys.FirstOrDefault(x => x.CategoryName == "IOS");
            if (category == null)
            {
                return PartialView(courseList);
            }
            var courses = _dataContext.Courses.Where(x => x.CategoryId == category.ID && x.Status == 1).ToList();

            if (courses.Count > 0)
            {
                courseList = AutoMapper.Mapper.Map<List<Course>, List<CourseAddView>>(courses);

                return PartialView(courseList);

            }

            return PartialView(courseList);
        }

        //public JsonResult GetScheduleList(long? courseid)
        //{
        //    var list = _dataContext.Schedules.Where(x => x.CourseId == courseid).ToList();
        //    return Json(list);
        //}

        public JsonResult GetScheduleList()
        {
            var list = _dataContext.Schedules.ToList();
            return Json(list);
        }

        public ActionResult SetCourseApply(long id)
        {
            //在teacheroption类写了一个一样的list表达式但没有引用，就算没引用这里写r.starttime.tostring会一直报错
            var list = _dataContext.Schedules.OrderByDescending(p => p.AddTime).Where(x => x.CourseId == id).Select(r => new SelectListItem()
            {
                Text = r.StartTime.ToString() + "  至  " + r.EndTime.ToString(),
                Value = r.ID.ToString(),

            }).ToList();
            ViewBag.Schedulelist = list;
            CourseApply apply = new CourseApply();
            apply.CourseId = id;
            if (Session["userid"] != null)
            {
                apply.UserId = (long)Session["userid"];
            }

            apply.ID = 0;
            apply.AddTime = DateTime.Now;
            return View(apply);
        }



        [HttpPost]
        public JsonResult SaveCourseApply(long CourseId, long ScheduleId)
        {
            if (CourseId <= 0 || ScheduleId <= 0)
                return Json(new { code = 1, msg = "参数错误" });

            long userId = LoginHelper.UserId;
            if (userId <= 0)
                return Json(new { code = 2, msg = "请登录后再报名" });

            var schedule = _dataContext.Schedules.FirstOrDefault(p => p.ID == ScheduleId);
            if (schedule == null)
                return Json(new { code = 1, msg = "该课程未开课" });

            if (schedule.EndTime < DateTime.Now)
                return Json(new { code = 1, msg = "该课程已结课" });

            CourseApply model = new CourseApply() { UserId = userId, CourseId = CourseId, ScheduleId = ScheduleId };
            int count = _dataContext.CourseApplies.Count(p => p.UserId == userId && p.ScheduleId == ScheduleId && p.CourseId == CourseId);
            if (count > 0)
                return Json(new { code = 1, msg = "该课程已报名" });

            _dataContext.CourseApplies.Add(model);
            _dataContext.SaveChanges();
            return Json(new { code = 0 });




            //if (apply != null)
            //{
            //    if (apply.UserId > 0)
            //    {
            //        if (apply.CourseId > 0 && apply.ScheduleId > 0)
            //        {
            //            _dataContext.CourseApplies.Add(apply);
            //            _dataContext.SaveChanges();
            //            return Json(new { code = 0 });
            //        }

            //    }

            //    return Json(new {code = 2, msg = "用户无登录"});
            //}
            //return Json(new{code=1,msg="错误空申请"});
        }
    }
}