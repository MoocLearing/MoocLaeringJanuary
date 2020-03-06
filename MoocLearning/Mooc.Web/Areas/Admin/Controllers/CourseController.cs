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
using Mooc.Data.Enums;
using Mooc.Data.Mongo;
using Mooc.Data.ViewModels;

using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;

namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]
    public class CourseController : Controller
    {

        private TeacherOption _teacheroption = new TeacherOption();

        private CategoryOption _categoryoption = new CategoryOption();

        private readonly DataContext _dataContext;//声明这个变量
        public CourseController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: Admin/Course
        public ActionResult Index()
        {
            // var courses = _dataContext.Courses.Include(c => c.Teacher).Include(c => c.Category).ToList();
            return View("List");
        }

        //索引
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCourseList(int pageIndex, int pageSize)
        {
            PageResult<CourseView> result = new PageResult<CourseView>() { data = new List<CourseView>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;
            //var _course = _dataContext.Courses.ToList();//.Include(c => c.Teacher).Include(c => c.Category);
            var list = (from a in _dataContext.Courses
                        join b in _dataContext.Teachers on a.TeacherId equals b.ID
                        join c in _dataContext.Categorys on a.CategoryId equals c.ID
                        select new CourseView
                        {
                            ID = a.ID,
                            CourseName = a.CourseName,
                            CourseDetail = a.CourseDetail,
                            TeacherName = b.TeacherName,
                            CategoryName = c.CategoryName,
                            AddTime = a.AddTime,
                            Status = a.Status,
                            CoverPic = a.CoverPic

                        });

            var viewList = list.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            // var courseview = AutoMapper.Mapper.Map<List<CourseView>>(list);
            result.data = viewList;
            result.Count = list.Count();

            return Json(result);
        }

        public ActionResult Create()
        {
            //dropdownlist还是要用正宗写法
            ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();
            ViewBag.CategoryList = _categoryoption.GetCategorySelectOptions();
            //ViewBag.CategoryId = new SelectList(_dataContext.Categorys, "ID", "CategoryName");
            //ViewBag.TeacherId = new SelectList(_dataContext.Teachers, "ID", "TeacherName");
            var model = new CourseView() { ID = 0 };
            return View(model);
        }


        //[HttpPost]
        //public ActionResult Save(string CourseName, string CourseDetail, int TeacherId, int CategoryId, int Status)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var course = AutoMapper.Mapper.Map<Course>(model);
        //            _dataContext.Courses.Add(course);
        //            _dataContext.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { code = 1, msg = e.Message });
        //    }

        //}

        [HttpPost]
        public JsonResult Save(Course course)
        {
            //Course course = new Course()
            //{
            //    CourseName = CourseName,
            //    CourseDetail = CourseDetail,
            //    TeacherId = TeacherId,
            //    CategoryId = CategoryId,
            //    //Status = Status
            //};
            if (course != null)
            {
                if (course.ID == 0 || course.ID.ToString() == null)
                {
                    course.Status = (int)CourseStatusEnum.未上架;
                    _dataContext.Courses.Add(course);
                    _dataContext.SaveChanges();
                    return Json(new { code = 0 });
                }

            }
            return Json(new { code = 1, msg = "出错" });
        }



        public ActionResult Edit(long? id)
        {
            Course course = _dataContext.Courses.Find(id);
            if (course == null)
                return HttpNotFound();

            //Ajax里面这用dropdownlist没错，但表单razor提交就完蛋
            ViewBag.CategoryId = new SelectList(_dataContext.Categorys, "ID", "CategoryName");
            ViewBag.TeacherId = new SelectList(_dataContext.Teachers, "ID", "TeacherName");
            var courseview = AutoMapper.Mapper.Map<CourseView>(course);
            return View(courseview);

        }

        [HttpPost]
        public JsonResult SaveEdit(Course course)
        {
            if (course != null)
            {
                var exist = _dataContext.Courses.Find(course.ID);
                if (exist != null)
                {
                    exist.CourseName = course.CourseName;
                    exist.CourseDetail = course.CourseDetail;
                    exist.TeacherId = course.TeacherId;
                    exist.CategoryId = course.CategoryId;
                    exist.Status = course.Status;
                    exist.CoverPic = course.CoverPic;
                    _dataContext.SaveChanges();
                    return Json(new { code = 0 });
                }

            }
            return Json(new { code = 1, msg = "错误" });

        }


        [HttpPost]
        public JsonResult EditCourse(Course course)
        {
            try
            {
                var model = _dataContext.Courses.Find(course.ID);
                if (model == null)
                    return Json(new { code = 1, msg = "当前课程不存在" });

                model.CourseName = course.CourseName;
                model.CourseDetail = course.CourseDetail;
                model.TeacherId = course.TeacherId;
                model.CategoryId = course.CategoryId;
                model.Status = course.Status;
                _dataContext.SaveChanges();

                return Json(new { code = 0 });

            }
            catch (System.Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }

        [HttpGet]
        public ActionResult Delete(long? id)
        {
            var course = _dataContext.Courses.Find(id);
            _dataContext.Courses.Remove(course);
            _dataContext.SaveChanges();

            return Redirect("~/Admin/Course/List");
        }

        [HttpPost]
        public JsonResult DeleteCourse(long? DeleteID)
        {
            if (DeleteID != null)
            {
                IEnumerable<Chapter> list = _dataContext.Chapters.ToList().Where(p => p.CourseId == DeleteID);
                foreach (var cap in list)
                {
                    _dataContext.Chapters.Remove(cap);
                }
                var course = _dataContext.Courses.Find(DeleteID);
                _dataContext.Courses.Remove(course);

                _dataContext.SaveChanges();

                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "该课程不存在" });

        }

        [HttpPost]
        public JsonResult ChangeStatus(long ID)
        {
            Course course = _dataContext.Courses.Find(ID);
            if (course == null)
                return Json(new { code = 1, msg = "参数错误" });
            if (course.Status != 1)
            {
                int iCount = _dataContext.Chapters.Count(p => p.CourseId == ID && !string.IsNullOrEmpty(p.VideoGuid));
                if (iCount <= 0)
                    return Json(new { code = 0, msg = "当前课程暂无课程视频" });
            }
            else
            {
                //暂时不加--如果有学生正在学习该课程并且未结课 不能下架
            }
            course.Status = course.Status == 1 ? 2 : 1;
            _dataContext.SaveChanges();
            return Json(new { code = 0, msg = course.Status == 1 ? "上架成功" : "下架成功" });

            //if (course.Status == 0)
            //{
            //    course.Status = 1;
            //    _dataContext.SaveChanges();
            //    return Json(new { code = 0, changestatue = 1 });
            //}

            //if (course.Status == 1)
            //{
            //    course.Status = 2;
            //    _dataContext.SaveChanges();
            //    return Json(new { code = 0, changestatue = 2 });
            //}

            //if (course.Status == 2)
            //{
            //    course.Status = 1;
            //    _dataContext.SaveChanges();
            //    return Json(new { code = 0, changestatue = 1 });
            //}

            //else
            //{
            //    return Json(new { code = 1, msg = "出错" });
            //}


        }


        [HttpPost]
        public JsonResult SearchChapter(int ID)
        {
            int id = ID;
            int chaptercount = _dataContext.Chapters.Count(c => c.CourseId == id);
            if (chaptercount > 0)
            {
                var chapters = _dataContext.Chapters.Where(m => m.CourseId == ID).ToList();
                return Json(chapters);
            }
            return Json(new { code = 1 });
        }

        [HttpPost]
        public JsonResult SaveBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return Json(new { code = 1, msg = "图片不存在" });

            try
            {
                //string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Cover");
                //if (!Directory.Exists(savaFile))
                //{
                //    Directory.CreateDirectory(savaFile);
                //}
                string fileName = $"{Guid.NewGuid().ToString("N")}.jpg";
                //var filePath = Path.Combine(savaFile, fileName);

                string data = base64.Substring(base64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除
                byte[] arr = Convert.FromBase64String(data);


                //System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
                //bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);//保存到服务器路径
                //return Json(new { code = 0, msg = "上传成功", fileName = fileName });


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
                    return Content("图片不存在");
                return File(bytes, "image/jpeg", filename);
            }
            catch (Exception e)
            {
                return Content("出错：" + e.Message);
            }
        }


        [HttpPost]
        public JsonResult UploadImg()
        {
            HttpFileCollection Files = System.Web.HttpContext.Current.Request.Files;
            if (Files.Count > 0)
            {
                try
                {
                    //多个for循环
                    HttpPostedFile file = Files[0];
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] filetype = { ".jpg", ".jpeg", ".gif", ".png" }; //文件允许格式jpg、jpeg、gif、png
                    bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
                    if (checkType)
                    {
                        return Json(new { code = 1, msg = "图片格式错误" });
                    }

                    if (file.ContentLength >= 50 * 1024 * 1024)
                    {
                        return Json(new { code = 1, msg = "上传视频大小不能超过50MB" });

                    }

                    string fileName = $"v_{Guid.NewGuid().ToString("N")}{fileExtension}";

                    string uploadId = MongoDBHelper.Upload(fileName, file.InputStream);
                    if (string.IsNullOrEmpty(uploadId))
                        return Json(new { code = 1, msg = "图片不存在" });
                    return Json(new { code = 0, msg = "上传成功", fileName = fileName, objectId = uploadId });
                }
                catch (Exception e)
                {
                    return Json(new { code = 1, msg = e.Message });
                }

            }

            return Json(new { code = 1, msg = "请选择图片" });

        }
    }
}