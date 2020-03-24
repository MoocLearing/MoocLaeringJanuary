using Mooc.Data.Context;
using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            return View(model);
        }
    }
}