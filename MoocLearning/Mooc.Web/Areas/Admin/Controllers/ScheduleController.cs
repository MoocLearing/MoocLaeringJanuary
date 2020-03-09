using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]
    public class ScheduleController : Controller
    {
        private readonly DataContext _dataContext;

        private SelectOptions selectOptions = new SelectOptions();
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


        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public JsonResult List(int pageIndex, int pageSize)
        {
            PageResult<ScheduleView> result = new PageResult<ScheduleView>() { data = new List<ScheduleView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;
            //var _course = _dataContext.Courses.ToList();//.Include(c => c.Teacher).Include(c => c.Category);
            var list = (from a in _dataContext.Schedules
                        join b in _dataContext.Courses on a.CourseId equals b.ID
                        select new ScheduleView
                        {
                            ID = a.ID,
                            CourseId = a.CourseId,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Remark = a.Remark,
                            CourseName = b.CourseName
                        });
            var viewList = list.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            result.data = viewList;
            result.Count = list.Count();
            return Json(result);
        }

        //public ActionResult Edit(long? id)
        //{
        //    Schedule schedule = _dataContext.Schedules.Find(id);
        //    if (schedule != null)
        //    {

        //        ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");
        //        //var scheduleView = AutoMapper.Mapper.Map<TeacherViewModel>(schedule);
        //        //[notmapped]就不能用mapper了
        //        ScheduleView view = new ScheduleView()
        //        {
        //            ID = schedule.ID,
        //            CourseId = schedule.CourseId,
        //            StartTime = schedule.StartTime,
        //            EndTime = schedule.EndTime,
        //            AddTime = schedule.AddTime,

        //        };
        //        return View(view);
        //    }
        //    return HttpNotFound();


        //}






        [HttpPost]
        public JsonResult DeleteSchedule(long? DeleteID)
        {
            Schedule schedule = _dataContext.Schedules.Find(DeleteID);
            if (schedule == null)
            {
                return Json(new { code = 1, msg = "当前课程安排不存在" });

            }

            _dataContext.Schedules.Remove(schedule);
            _dataContext.SaveChanges();

            return Json(new { code = 0 });
        }



        [HttpPost]
        public JsonResult SaveSchedule(Schedule schedule)
        {

            if (schedule == null || schedule.CourseId <= 0)
                return Json(new { code = 1, msg = "参数错误" });

            var course = _dataContext.Courses.Find(schedule.CourseId);
            if (course == null || course.Status != 1)
                return Json(new { code = 1, msg = "暂无课程信息" });


            //对开始和结束时间进行逻辑判断
            if (schedule.StartTime < DateTime.Now.Date)
                return Json(new { code = 1, msg = "开始时间不能小于当前时间" });
            if (schedule.StartTime > schedule.EndTime)
                return Json(new { code = 1, msg = "开始时间不能大于结课时间" });

            string sql = @" select * from [dbo].[Schedules] where CourseId={0} and 
                        ((StartTime<={1} and EndTime >={1})
                        or (StartTime<={2} and EndTime >={2})
                        or (StartTime>={1} and EndTime <={2})
                        ) ";

            var lists = _dataContext.Database.SqlQuery<Schedule>(sql, new object[] { schedule.CourseId, schedule.StartTime, schedule.EndTime }).ToList();
            if (lists != null && lists.Count > 0)
                return Json(new { code = 1, msg = "该课程在当前时间段已开课" });


            _dataContext.Schedules.Add(schedule);

            _dataContext.SaveChanges();
            return Json(new { code = 0 });

            //if (schedule.ID == 0)
            //{
            //    //TimeSpan startTime = schedule.StartTime.TimeOfDay;
            //    //TimeSpan endTime = schedule.EndTime.TimeOfDay;

            //    //起始结束时间必须都有
            //    if (schedule.StartTime == null || schedule.EndTime == null)
            //        return Json(new { code = 1, msg = "请填写开始时间和结束时间" });

            //    //开始时间要小于结束时间
            //    if (DateTime.Compare(schedule.StartTime, schedule.EndTime) >= 0)
            //        return Json(new { code = 1, msg = "结束日期不能小于或等于开始日期" });

            //    //开课时间要大于今天
            //    if (DateTime.Compare(schedule.StartTime, DateTime.Now) <= 0)
            //        return Json(new { code = 1, msg = "必须提前一天开课" });

            //    //和该courseID下其他安排比较有没有交集，AB是当前对象，XY是list里已有安排;
            //    //B <= X 或 Y <= A，对其取反，得到有交集的情况：X < B AND A < Y;

            //    var list = _dataContext.Schedules.Where(s => s.CourseId == schedule.CourseId).ToList();

            //    foreach (Schedule item in list)
            //    {
            //        if (DateTime.Compare(item.StartTime, schedule.EndTime) < 0 && DateTime.Compare(schedule.StartTime, item.EndTime) < 0)
            //        {
            //            return Json(new { code = 1, msg = "不能和此课程下的其他开课时间重叠！！" });
            //        };
            //    }


            //    _dataContext.Schedules.Add(schedule);
            //    //！！！必须在schduleview里面加[NotMapped],要不然就一直报错！！
            //    //但奇怪的是，整个环节都没有用到过这个modelview，莫非只要modelview引用了entity就要加[NotMapped]？
            //    //不加[notmapped]连通过linq进行多表查询返回scheduleview都直接一堆报错！！！
            //    _dataContext.SaveChanges();
            //    return Json(new { code = 0 });
            // }





            //下面是修改，貌似没有说需求这个功能
            //if (schedule.ID > 0)
            //{

            //    var s1 = _dataContext.Schedules.Find(schedule.ID);
            //    if (s1 != null)
            //    {
            //        s1.CourseId = schedule.CourseId;
            //        s1.StartTime = schedule.StartTime;
            //        s1.EndTime = schedule.EndTime;
            //        s1.Remark = schedule.Remark;
            //    _dataContext.SaveChanges();
            //        return Json(new { code = 0 });
            //    }
            //    else
            //    {
            //        return Json(new { code = 1, msg = "错误:更新的安排不存在" });
            //    }

            //}
            // return Json(new { code = 1, msg = "错误" });




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