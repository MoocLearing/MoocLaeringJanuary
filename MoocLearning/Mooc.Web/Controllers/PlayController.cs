using Mooc.Data.Context;
using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;

namespace Mooc.Web.Controllers
{
    public class PlayController : Controller
    {
        private readonly DataContext _dataContext;

        public PlayController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // GET: Play
        public ActionResult Index(long id)
        {
            var model = _dataContext.Chapters.Find(id);
            if (model == null || string.IsNullOrEmpty(model.VideoGuid))
                return Content("当前视频不存在");

            var course = _dataContext.Courses.Find(model.CourseId);
            if (course == null || course.Status != 1)
                return Content("当前课程未上架");

            var courseApplyList = _dataContext.CourseApplies.Where(x => x.UserId == LoginHelper.UserId && x.CourseId == model.CourseId).ToList();
            if (courseApplyList.Count == 0)
                return Content("请先报名");

            ChapterDto viewmodel = new ChapterDto();
            //这里少这么一个逻辑，coureapply存在但所有schedule都没有在当前时间下，并且这个scheduleID还需要用来保存进study《=》chapter
            foreach (var applied in courseApplyList)
            {
                //只要找到一个schedule就可以，因为之前保存schedule的时候已经判定了日期之前的去重
                var schedule = _dataContext.Schedules.Find(applied.ScheduleId);
                if (schedule != null && schedule.StartTime <= DateTime.Now && schedule.EndTime >= DateTime.Now)
                {
                    viewmodel.ScheduleId = schedule.ID;
                    break;
                }
            }

            if (viewmodel.ScheduleId<1)
                return Content("当前课程安排时间未到");

            var curCourse = _dataContext.Courses.Find(model.CourseId);
            if (curCourse != null)
            {
                viewmodel.CoursePic = curCourse.CoverPic;
            }

            viewmodel.ChapterOne = model;

            return View(viewmodel);

        }

        [HttpPost]
        public JsonResult SaveStudy(Study study)
        {
            //验证来的数据
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
                //做一次study的去重处理
                var studyList = _dataContext.Studies.Where(x =>
                    x.UserId == newStudy.UserId && x.CourseId == newStudy.CourseId &&
                    x.ScheduleId == newStudy.ScheduleId && x.ChapterId == newStudy.ChapterId).ToList();

                if (studyList.Count>0)
                {
                    return Json(new {code = 1, msg = "章节已加入过学习列表"});
                }


                _dataContext.Studies.Add(newStudy);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }

            return Json(new { code = 1, msg = "该视频没有存入学习列表" });
        }
    }
}