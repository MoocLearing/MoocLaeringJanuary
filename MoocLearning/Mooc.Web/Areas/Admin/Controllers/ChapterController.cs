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
using Mooc.Data.Mongo;
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
            return View("ListAll");
        }

        public ActionResult ListAll()
        {
            
            return View();
        }


        [HttpPost]
        public JsonResult ListAll(int pageIndex, int pageSize)
        {
            PageResult<ChapterView> result = new PageResult<ChapterView>() { data = new List<ChapterView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;

            var list = (from a in _dataContext.Chapters
                        join b in _dataContext.Courses on a.CourseId equals b.ID
                        orderby a.CourseId
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

        //改变呈现，groupby courseID
        public ActionResult List(long? id)
        {
            if (id == null)
                return HttpNotFound("参数错误");
            ViewBag.CourseId = id;
            var course = _dataContext.Courses.Find(id);
            if (course == null)
                return HttpNotFound("参数错误");

            ViewBag.CourseStatus = course.Status;
            return View();
        }

        [HttpPost]
        public JsonResult GetChapterList(long courseId, int pageIndex, int pageSize)
        {
            PageResult<ChapterView> result = new PageResult<ChapterView>() { data = new List<ChapterView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;

            var list = (from a in _dataContext.Chapters
                        join b in _dataContext.Courses on a.CourseId equals b.ID
                        where a.CourseId == courseId
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

        //本地文件夹保存文件
        [HttpPost]
        public ActionResult SaveNew(ChapterView model)
        {
            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");

            if (ModelState.IsValid)
            {
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

                return RedirectToAction("List", "Chapter", new { id = model.CourseId });
            }
            return View("CreateNew", model);
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
                if (model.Video == null)
                {
                    chapter.ChapterName = model.ChapterName;
                    chapter.ChapterDetails = model.ChapterDetails;
                    chapter.CourseId = model.CourseId;
                    chapter.UpdateTime = DateTime.Now;
                    _dataContext.SaveChanges();
                    return RedirectToAction("List");
                }

                if (model.Video != null)
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
                        // return RedirectToAction("List");
                        return RedirectToAction("List", "Chapter", new { id = model.CourseId });
                    }
                    catch (Exception)
                    {

                        return View("EditNew", model);
                    }
                }



            }
            return RedirectToAction("List");
        }


        // [Route("Video/play/{id}")]
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



        // [Route("Video/Show/{fileName}")]

        //本地文件保存视频
        //public ActionResult Show(string fileName)
        //{
        //    string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Video");
        //    var filePath = Path.Combine(savaFile, fileName);
        //    if (!System.IO.File.Exists(filePath))
        //        return Content("视频不存在");
        //    return File(FileToStream(filePath), "video/mp4", fileName);
        //}


        //mongoDB保存视频
        public ActionResult AjCreate()
        {
            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");
            var model = new ChapterView() { ID = 0 };
            return View(model);
        }

        [HttpPost]
        public ActionResult AjSave(Chapter chapter)
        {
            if (chapter != null)
            {
                if (chapter.ID == 0 || chapter.ID.ToString() == null)
                {
                    _dataContext.Chapters.Add(chapter);
                    _dataContext.SaveChanges();
                    return Json(new { code = 0 });
                }

            }
            return Json(new { code = 1, msg = "出错" });
        }

        public ActionResult AjEdit(long? id)
        {
            Chapter chapter = _dataContext.Chapters.Find(id);
            if (chapter == null)
            {
                return null;
            }

            ViewBag.CourseSelectOption = new SelectList(selectOptions.GetCourseSelectOptions(), "Value", "Text");
            var view = AutoMapper.Mapper.Map<ChapterView>(chapter);
            return View(view);

        }

        [HttpPost]
        public JsonResult AjSaveEdit(Chapter chapter)
        {
            try
            {
                var model = _dataContext.Chapters.Find(chapter.ID);
                if (model == null)
                    return Json(new { code = 1, msg = "当前章节不存在" });

                model.ChapterName = chapter.ChapterName;
                model.ChapterDetails = chapter.ChapterDetails;
                model.CourseId = chapter.CourseId;
                model.UpdateTime = DateTime.Now;
                model.VideoGuid = chapter.VideoGuid;
                _dataContext.SaveChanges();

                return Json(new { code = 0 });

            }
            catch (System.Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }



        [HttpPost]
        public JsonResult SaveBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return Json(new { code = 1, msg = "图片不存在" });

            try
            {
                string fileName = $"{Guid.NewGuid().ToString("N")}.mp4";

                string data = base64.Substring(base64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除
                byte[] arr = Convert.FromBase64String(data);

                string uploadId = MongoDBHelper.Upload(fileName, arr);
                if (string.IsNullOrEmpty(uploadId))
                    return Json(new { code = 1, msg = "图片不存在" });
                return Json(new { code = 0, msg = "上传成功", fileName = fileName, objectId = uploadId });
            }
            catch (Exception e)
            {

                return Json(new { code = 1, msg = e.Message });
            }
        }



        public ActionResult Show(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    return Content("参数错误");

                var bytes = MongoDBHelper.down(filename);
                if (bytes == null || bytes.Length == 0)
                    return Content("视频不存在");
                return File(bytes, "video/mp4", filename);
            }
            catch (Exception e)
            {
                return Content("出错：" + e.Message);
            }
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

                return Redirect("~/Admin/Course/List");
            }
            return Content("此chapter不存在！！！");
        }

        [HttpPost]
        public JsonResult UploadVideo()
        {
            HttpFileCollection Files = System.Web.HttpContext.Current.Request.Files;
            if (Files.Count > 0)
            {
                try
                {
                    //多个for循环
                    HttpPostedFile file = Files[0];
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] filetype = { ".mp4", ".flv", ".rmvb", ".mpeg", ".mov", ".wmv" }; //文件允许格式    rmvb、mpeg、mov、wmv、avi、mkv、mp4、flv、vob、f4v
                    bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
                    if (checkType)
                    {
                        return Json(new { code = 1, msg = "视频格式错误" });
                    }

                    if (file.ContentLength >= 500 * 1024 * 1024)
                    {
                        return Json(new { code = 1, msg = "上传视频大小不能超过500MB" });

                    }

                    string fileName = $"v_{Guid.NewGuid().ToString("N")}{fileExtension}";

                    string uploadId = MongoDBHelper.Upload(fileName, file.InputStream);
                    if (string.IsNullOrEmpty(uploadId))
                        return Json(new { code = 1, msg = "视频不存在" });
                    return Json(new { code = 0, msg = "上传成功", fileName = fileName, objectId = uploadId });
                }
                catch (Exception e)
                {
                    return Json(new { code = 1, msg = e.Message });
                }

            }

            return Json(new { code = 1, msg = "请选择视频" });

        }
    }
}