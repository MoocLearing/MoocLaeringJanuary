using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using Mooc.Web.Attribute;

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


        //必须是登录用户-加个拦截器就行
        //注意命名--Details;index-显示到页面 注意名字
        [CheckLogin]
        public ActionResult ShowSubject(long? courseId)
        {

            var model = (from b in _dataContext.Courses
                         join c in _dataContext.Teachers on b.TeacherId equals c.ID
                         where b.ID == courseId
                         select new CourseDetails
                         {
                             CourseName = b.CourseName,
                             CourseDetail = b.CourseDetail,
                             ID = b.ID,
                             TeacherName = c.TeacherName,
                             TeacherId = c.ID,
                             CoverPic = b.CoverPic
                         }).FirstOrDefault();

            if (model == null)
                return HttpNotFound("当前课程不存在");

            model.chapters = _dataContext.Chapters.Where(p => p.CourseId == model.ID).ToList();



            //var course = _dataContext.Courses.Find(courseId);
            //if (course == null)
            //    return HttpNotFound("当前课程不存在");


            //ViewBag.courseId = courseId;
            //var course = _dataContext.Courses.Find(courseId);
            //if (course != null)
            //{
            //    ViewBag.coursename = course.CourseName;
            //    ViewBag.Coursedetail = course.CourseDetail;
            //}

            var cookieusername = LoginHelper.UserName;//CookieHelper.GetCookie("username");
            var cookieuserid = LoginHelper.UserId;//CookieHelper.GetCookie("userid");
            //if (cookieuserid > 0 && !cookieusername.IsNullOrWhiteSpace())
            //{
            //    var userobj = _dataContext.Users.Find(cookieuserid);
            //    if (userobj != null)
            //    {
            //        ViewBag.userObj = userobj;//ViewBag.User
            //    }
            //}
            ViewBag.userObj = _dataContext.Users.Find(cookieuserid);

           // var chapters = _dataContext.Chapters.Where(x => x.CourseId == courseId).ToList();
            return View(model);
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
        public ActionResult Play(long? id)
        {

            var model = _dataContext.Chapters.Find(id);
            if (model == null || string.IsNullOrEmpty(model.VideoGuid))
                return Json(new { code = 1, msg = "当前视频不存在" });

            var course = _dataContext.Courses.Find(model.CourseId);
            if (course == null || course.Status != 1)
                return Json(new { code = 1, msg = "当前课程未上架" });

            var courseApplyList = _dataContext.CourseApplies.Where(x => x.UserId == LoginHelper.UserId && x.CourseId == model.CourseId).ToList();

            if (courseApplyList.Count == 0)
                return Json(new { code = 1, msg = "请先报名" });

            foreach (var courseapplys in courseApplyList)
            {
                var currentschedule = _dataContext.Schedules.Find(courseapplys.ScheduleId);

                if (currentschedule != null && currentschedule.StartTime <= DateTime.Now.Date && currentschedule.EndTime >= DateTime.Now.Date)
                {
                    return PartialView(model);
                }

            }
            return Json(new { code = 1, msg = "该课程暂时未开课" });
        }

        

        //public ActionResult play(long? id)
        //{
        //    var model = _dataContext.Chapters.Find(id);
        //    if (model == null || string.IsNullOrEmpty(model.VideoGuid))
        //        return Content("当前视频不存在" );

        //    var course = _dataContext.Courses.Find(model.CourseId);
        //    if (course == null || course.Status != 1)
        //        return Content("当前课程未上架" );

        //    var courseApplyList = _dataContext.CourseApplies.Where(x => x.UserId == LoginHelper.UserId && x.CourseId == model.CourseId).ToList();

        //    if (courseApplyList.Count == 0)
        //        return Content("请先报名" );

        //    foreach (var courseapplys in courseApplyList)
        //    {
        //        var currentschedule = _dataContext.Schedules.Find(courseapplys.ScheduleId);

        //        if (currentschedule != null && currentschedule.StartTime <= DateTime.Now.Date && currentschedule.EndTime >= DateTime.Now.Date)
        //        {
        //            ViewBag.currentscheduleId = currentschedule.ID;
        //            return PartialView(model);
        //        }

        //    }
        //    return Content("该课程暂时未开课" );

        //}


        [HttpPost]
        public JsonResult SaveStudy(Study study)
        {
            if (study.ChapterId > 0 
                && study.CourseId > 0 
                && study.ScheduleId > 0 
                && LoginHelper.UserId > 0 
                && !LoginHelper.UserName.IsNullOrWhiteSpace())
            {
                Study newStudy = new Study()
                {
                    UserId = LoginHelper.UserId,
                    CourseId = study.CourseId,
                    ScheduleId = study.ScheduleId,
                    ChapterId = study.ChapterId
                };

                //已经学习的章节对每个user是唯一的，进行判断如果章节已经记录学习，不新建
                var studylist = _dataContext.Studies.Where(x => x.UserId == study.UserId).ToList();

                foreach (var item in studylist)
                {
                    if (item.ChapterId == study.ChapterId)
                    {
                        return Json(new {code = 0});
                    }   
                }

                _dataContext.Studies.Add(newStudy);
                _dataContext.SaveChanges();
                return Json(new{code=0});
            }

            return Json(new {code = 1, msg = "该视频没有存入学习列表"});
        }

        //var userid = long.Parse(CookieHelper.GetCookie("userid"));
        //if (userid > 0)
        //{
        //    var appliedCourse = _dataContext.CourseApplies.Where(x => x.UserId == userid && x.CourseId == model.CourseId).ToList();

        //    foreach (var item in appliedCourse)
        //    {

        //        var schedule = _dataContext.Schedules.Find(item.ScheduleId);
        //        if (schedule != null)
        //        {
        //            if (schedule.StartTime < DateTime.Now.Date && schedule.EndTime > DateTime.Now.Date)
        //            {
        //                return Json(new { code = 0, res = model });
        //                break;
        //            }
        //        }

        //    }
        //}



    }
}