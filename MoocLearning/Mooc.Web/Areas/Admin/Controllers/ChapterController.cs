using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;


//一旦引用的类库不对，系统也不给纠正
namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]

    public class ChapterController : Controller
    {

        private DataContext _dataContext = new DataContext();

        public ChapterController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: Admin/Chapter
        public ActionResult Index()
        {
            return View();
        }

        //改变呈现，groupby courseID
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PageList(int pageIndex, int pageSize)
        {
            PageResult<ChapterView> result = new PageResult<ChapterView>() { data = new List<ChapterView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;
            var _chapter = _dataContext.Chapters.Include(m => m.Course).Include(m => m.Video);
            var list = _chapter.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            var chapterview = AutoMapper.Mapper.Map<List<ChapterView>>(list);
            result.data = chapterview;
            result.Count = _chapter.Count();

            return Json(result);
        }

        public ActionResult Create()
        {
            ViewBag.VideoId = new SelectList(_dataContext.Videos, "ID", "VideoName");
            ViewBag.CourseId = new SelectList(_dataContext.Courses, "ID", "CourseName");
            var model = new ChapterView() { ID = 0 };
            return View(model);
        }


        //验证：当前course有多少chapter？最少一个，最多5个
        [HttpPost]
        public JsonResult Save(ChapterView model)
        {
            Chapter chapter = new Chapter()
            {
                ChapterName = model.ChapterName,
                ChapterDetail = model.ChapterDetail,
                CourseId = model.CourseId,
                VideoId = model.VideoId,
                UpdateTime = DateTime.Now
            };
            

            if (model != null)
            {
                var chapterpercourse = _dataContext.Chapters.Count(c => c.CourseId == model.CourseId);
                if(chapterpercourse >= 5 || chapterpercourse < 1)
                {
                    return Json(new { code = 1, msg = "每个course的chapter不得超过5个也不得小于1个！！！" });
                }
                _dataContext.Chapters.Add(chapter);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "出错" });
        }


        public ActionResult Edit(long? id)
        {
            Chapter chapter = _dataContext.Chapters.Find(id);
            if (chapter == null)
            {
                return Json(new { code = 1, msg = "更新的chapter不存在" });
            }
            ViewBag.VideoId = new SelectList(_dataContext.Videos, "ID", "VideoName");
            ViewBag.CourseId = new SelectList(_dataContext.Courses, "ID", "CourseName");
            var chapterview = AutoMapper.Mapper.Map<ChapterView>(chapter);
            return View(chapterview);
        }


        //验证courseID下最多5个最少1个
        [HttpPost]
        public JsonResult EditChapter(Chapter chapter)
        {
            try
            {
                var model = _dataContext.Chapters.Find(chapter.ID);
                if (model == null)
                {
                    return Json(new { code = 1, msg = "当前chapter不存在" });
                }

                model.ChapterName = chapter.ChapterName;
                model.ChapterDetail = chapter.ChapterDetail;
                model.CourseId = chapter.CourseId;
                model.VideoId = chapter.VideoId;
                model.UpdateTime = DateTime.Now;
                _dataContext.SaveChanges();

                return Json(new { code = 0 });

            }
            catch (System.Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }

        //验证如果COURSEID下只有一个，不能删除
        [HttpGet]
        public ActionResult Delete(long? id)
        {
            var chapter = _dataContext.Chapters.Find(id);
            if (chapter != null)
            {
                var chapterpercourse = _dataContext.Chapters.Count(c => c.CourseId == chapter.CourseId);
                if (chapterpercourse <=1 )
                {
                    return Content("每个course至少有一个chapter");
                }
                
                _dataContext.Chapters.Remove(chapter);
                _dataContext.SaveChanges();

                return Redirect("~/Admin/Chapter/List");
            }
            return Content("此chapter不存在！！！");
        }
    }
}