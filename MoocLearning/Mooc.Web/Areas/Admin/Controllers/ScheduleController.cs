using Mooc.Data.Context;
using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly DataContext _dataContext;

        public ScheduleController(DataContext datacontext)
        {
            _dataContext = datacontext;
        }
        // GET: Admin/Schedule
        public ActionResult Index(long? id)
        {
            ViewBag.id = id;
            return View();
        }


        [HttpPost]
        public JsonResult SaveSchedule(Schedule schedule)
        {
            if (schedule == null || schedule.CourseId <= 0)
                return Json(new { code = 1, msg = "参数错误" });

            var course = _dataContext.Courses.Find(schedule.CourseId);
            if (course == null || course.Status != 1)
                return Json(new { code = 1, msg = "暂无课程信息" });

            _dataContext.Schedules.Add(schedule);
            _dataContext.SaveChanges();
            return Json(new { code = 0 });


            //if (schedule != null)
            //{

            //    if (schedule.CourseId != 0)
            //    {

            //        var course = _dataContext.Courses.Find(schedule.CourseId);
            //        if (course.Status == 1)
            //        {
            //            _dataContext.Schedules.Add(schedule);
            //            _dataContext.SaveChanges();
            //            return Json(new { code = 0 });
            //        }
            //        else
            //        {
            //            return Json(new { code = 1, msg = "课程必须上架才能安排时间" });
            //        }


            //    }
            //    return Json(new { code = 1, msg = "no course with ID can be schedule" });
            //}

            //return Json(new { code = 1, msg = "no schedule paras come" });
        }
    }

    #region homework
   //1>schedule list=> coursename startdate,enddate,delete
   //2>can't schdule classes during the same period
   //3>start date >=today
  //4>start date<end date
    #endregion



}