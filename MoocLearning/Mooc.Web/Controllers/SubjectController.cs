using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Utils;

namespace Mooc.Web.Controllers
{
    public class SubjectController : Controller
    {
        private readonly DataContext _dataContext;

        public SubjectController(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }
        // GET: Subject
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult ShowSubject(long? courseId)
        {
            ViewBag.courseId = courseId;
            var course = _dataContext.Courses.Find(courseId);
            if (course != null)
            {
                ViewBag.coursename = course.CourseName;
                ViewBag.Coursedetail = course.CourseDetail;
            }

            var cookieusername = CookieHelper.GetCookie("username");
            var cookieuserid = CookieHelper.GetCookie("userid");
            if (!cookieuserid.IsNullOrWhiteSpace() && !cookieusername.IsNullOrWhiteSpace())
            {
                var userobj = _dataContext.Users.Find(long.Parse(cookieuserid));
                if (userobj != null)
                {
                    ViewBag.userObj = userobj;
                }
            }


            var chapters = _dataContext.Chapters.Where(x => x.CourseId == courseId).ToList();
            return View(chapters);
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
            if (CookieHelper.GetCookie("userid") != null)
            {
                apply.UserId = long.Parse(CookieHelper.GetCookie("userid"));
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

        }

        [HttpPost]
        public JsonResult Play(long? id)
        {
            //这里session集体该cookie了
            if (CookieHelper.GetCookie("username").IsNullOrWhiteSpace() || CookieHelper.GetCookie("userid").IsNullOrWhiteSpace())
            {
                return Json(new {code = 1, msg = "未登录"});
            }

            if (id == null)
                return Json(new {code = 1, msg = "参数错误"});
            var model = _dataContext.Chapters.Find(id);

            if (model == null || string.IsNullOrEmpty(model.VideoGuid))
                return Json(new {code = 1, msg = "未上传视频"});
            var userid = long.Parse(CookieHelper.GetCookie("userid"));
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
                            return Json(new{code=0,res=model});
                            break;
                        }
                    }

                }
            }


            return Json(new {code = 1, msg = "没有足够权限"});
        }
    }
}