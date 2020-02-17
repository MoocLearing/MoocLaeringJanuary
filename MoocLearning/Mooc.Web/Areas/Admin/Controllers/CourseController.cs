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
using Mooc.Data.Enums;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;

namespace Mooc.Web.Areas.Admin.Controllers
{
    // [CheckAdminLogin]
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
                course.Status = (int)CourseStatusEnum.未上架;
                _dataContext.Courses.Add(course);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "出错" });
        }


        public ActionResult Edit(long? id)
        {
            Course course = _dataContext.Courses.Find(id);
            if (course == null)
            {
                return null;
            }
            ViewBag.CategoryId = new SelectList(_dataContext.Categorys, "ID", "CategoryName");
            ViewBag.TeacherId = new SelectList(_dataContext.Teachers, "ID", "TeacherName");
            var courseview = AutoMapper.Mapper.Map<CourseView>(course);
            return View(courseview);


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
    }
}