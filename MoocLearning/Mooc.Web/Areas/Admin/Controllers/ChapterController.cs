using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;


//一旦引用的类库不对，系统也不给纠正
namespace Mooc.Web.Areas.Admin.Controllers
{
    // [CheckAdminLogin]

    public class ChapterController : Controller
    {

        private SelectOptions selectOptions = new SelectOptions();

        private readonly DataContext _dataContext;
        public ChapterController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: Admin/Chapter
        public ActionResult Index()
        {
            return View("List");
        }

        //改变呈现，groupby courseID
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetChapterList(int pageIndex, int pageSize)
        {
            PageResult<ChapterView> result = new PageResult<ChapterView>() { data = new List<ChapterView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;

            var list = (from a in _dataContext.Chapters
                        join b in _dataContext.Courses on a.CourseId equals b.ID
                        select new ChapterView
                        {
                            ID = a.ID,
                            ChapterName = a.ChapterName,
                            ChapterDetails = a.ChapterDetails,
                            CourseName = b.CourseName,
                            UpdateTime = a.UpdateTime,
                            VideoGuid = a.VideoGuid

                        });
            var viewList = list.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            //var chapterview = AutoMapper.Mapper.Map<List<ChapterView>>(list);
            result.data = viewList;
            result.Count = list.Count();

            return Json(result);
        }



        public ActionResult CreateNew()
        {
            // ViewBag.CourseList = selectOptions.GetCourseSelectOptions();
            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");
            //ViewBag.CourseId = new SelectList(_dataContext.Courses, "ID", "CourseName");
            var model = new ChapterView() { ID = 0 };
            return View(model);
        }


        [HttpPost]
        public ActionResult SaveNew(ChapterView model)
        {
            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");

            //if (ModelState.IsValid)
            //{
            var file = model.Video;
            string fileExtension = Path.GetExtension(file.FileName);
            string[] filetype = { ".mp4", ".flv", ".rmvb", ".mpeg", ".mov", ".wmv" }; //文件允许格式    rmvb、mpeg、mov、wmv、avi、mkv、mp4、flv、vob、f4v
            bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
            if (checkType)
            {
                ModelState.AddModelError("", "视频格式错误");
                return View("CreateNew", model);
            }

            if (file.ContentLength >= 1000 * 1024 * 1024)//1000MB
            {

                ModelState.AddModelError("", "上传视频大小不能超过1000MB");
                return View("CreateNew", model);
            }

            string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
            try
            {
                string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Video");
                if (!Directory.Exists(savaFile))
                {
                    Directory.CreateDirectory(savaFile);
                }
                var filePath = Path.Combine(savaFile, fileName);
                file.SaveAs(filePath);

                Chapter chapter = new Chapter
                {
                    ChapterName = model.ChapterName,
                    ChapterDetails = model.ChapterDetails,
                    VideoGuid = fileName,
                    CourseId = model.CourseId,
                    UpdateTime = DateTime.Now
                };
                _dataContext.Chapters.Add(chapter);
                _dataContext.SaveChanges();
            }
            catch (Exception)
            {

                return View("CreateNew", model);
            }

            return RedirectToAction("List");
            //}
            //return View("CreateNew", model);
        }

        public ActionResult EditNew(long? id)
        {
            Chapter chapter = _dataContext.Chapters.Find(id);
            if (chapter == null)
            {
                return Json(new { code = 1, msg = "更新的chapter不存在" });
            }
            // ViewBag.VideoId = new SelectList(_dataContext.Videos, "ID", "VideoName");
            // ViewBag.CourseId = new SelectList(_dataContext.Courses, "ID", "CourseName");
            var chapterview = AutoMapper.Mapper.Map<ChapterView>(chapter);
            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");
            return View(chapterview);
        }

        public ActionResult SaveEdit(ChapterView model)
        {
            //if (ModelState.IsValid)
            //{}
            Chapter chapter = _dataContext.Chapters.Find(model.ID);

            if (chapter != null)
            {
                if (model.Video==null)
                {
                    chapter.ChapterName = model.ChapterName;
                    chapter.ChapterDetails = model.ChapterDetails;
                    chapter.CourseId = model.CourseId;
                    chapter.UpdateTime = DateTime.Now;
                    _dataContext.SaveChanges();
                    return RedirectToAction("List");
                }

                if (model.Video!=null)
                {
                    var file = model.Video;
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] filetype = { ".mp4", ".flv", ".rmvb", ".mpeg", ".mov", ".wmv" }; //文件允许格式    rmvb、mpeg、mov、wmv、avi、mkv、mp4、flv、vob、f4v
                    bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
                    if (checkType)
                    {
                        ModelState.AddModelError("", "视频格式错误");
                        return View("EditNew", model);
                    }

                    if (file.ContentLength >= 1000 * 1024 * 1024)//1000MB
                    {

                        ModelState.AddModelError("", "上传视频大小不能超过1000MB");
                        return View("CreateNew", model);
                    }

                    string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
                    try
                    {
                        string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Video");
                        if (!Directory.Exists(savaFile))
                        {
                            Directory.CreateDirectory(savaFile);
                        }
                        var filePath = Path.Combine(savaFile, fileName);
                        file.SaveAs(filePath);

                        chapter.ChapterName = model.ChapterName;
                        chapter.ChapterDetails = model.ChapterDetails;
                        chapter.CourseId = model.CourseId;
                        chapter.UpdateTime = DateTime.Now;
                        chapter.VideoGuid = model.VideoGuid;

                        _dataContext.SaveChanges();
                        return RedirectToAction("List");

                    }
                    catch (Exception)
                    {

                        return View("EditNew", model);
                    }
                }



            }
            return RedirectToAction("List");
        }



        [Route("Video/Show/{fileName}")]
        public ActionResult Show(string fileName)
        {
            string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Video");
            var filePath = Path.Combine(savaFile, fileName);
            if (!System.IO.File.Exists(filePath))
                return Content("视频不存在");
            return File(FileToStream(filePath), "video/mp4", fileName);
        }

        public Stream FileToStream(string fileName)
        {

            // 打开文件

            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[]

            byte[] bytes = new byte[fileStream.Length];

            fileStream.Read(bytes, 0, bytes.Length);

            fileStream.Close();

            // 把 byte[] 转换成 Stream

            Stream stream = new MemoryStream(bytes);

            return stream;

        }

        public ActionResult Create()
        {
            // ViewBag.VideoId = new SelectList(_dataContext.Videos, "ID", "VideoName");
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
                ChapterDetails = model.ChapterDetails,
                CourseId = model.CourseId,
                //VideoId = model.VideoId,
                UpdateTime = DateTime.Now
            };


            if (model != null)
            {
                var chapterpercourse = _dataContext.Chapters.Count(c => c.CourseId == model.CourseId);
                if (chapterpercourse >= 5 || chapterpercourse < 1)
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
            // ViewBag.VideoId = new SelectList(_dataContext.Videos, "ID", "VideoName");
            // ViewBag.CourseId = new SelectList(_dataContext.Courses, "ID", "CourseName");
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
                model.ChapterDetails = chapter.ChapterDetails;
                model.CourseId = chapter.CourseId;
                //model.VideoId = chapter.VideoId;
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
                if (chapterpercourse <= 1)
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