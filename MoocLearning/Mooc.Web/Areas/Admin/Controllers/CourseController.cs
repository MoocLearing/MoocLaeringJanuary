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
    [CheckAdminLogin]
    public class CourseController : Controller
    {
        // private DataContext _dataContext = new DataContext();实例化对象
        
        private TeacherOption _option = new TeacherOption();

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
                            Status = a.Status

                        });

            var viewList = list.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            // var courseview = AutoMapper.Mapper.Map<List<CourseView>>(list);
            result.data = viewList;
            result.Count = list.Count();

            return Json(result);
        }

        public ActionResult Create()
        {
            ViewBag.TeacherList = _option.GetTeacherSelectOptions();
           // ViewBag.CategoryId = new SelectList(_dataContext.Categorys, "ID", "CategoryName");
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
        public JsonResult DeleteCourse(long? id)
        {
            return Json(new { code = 1, msg = "出错" });

        }
    }
}