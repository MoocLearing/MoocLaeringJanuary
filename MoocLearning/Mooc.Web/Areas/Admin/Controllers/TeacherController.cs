using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TeacherController : Controller
    {
        private DataContext _dataContext;
        private SelectDepartments _departments = new SelectDepartments();


        public TeacherController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // GET: Admin/Teacher

        public ActionResult List()
        {
            return View();
        }



        public ActionResult Index()
        {

            int pagesize = 3;
            int curpage = 1;
            int totalpage = 1;

            int totalcount = _dataContext.Teachers.Count();

            if (totalcount % pagesize == 0)
            {
                totalpage = totalcount / pagesize;
            }
            else
            {
                totalpage = System.Convert.ToInt32((totalcount / pagesize) + 1);
            }


            var list = _dataContext.Teachers.Take(pagesize);
            ListTeacherPage listTeacherPage = new ListTeacherPage()
            {
                teachers = list,
                TotalCount = totalcount,
                TotalPage = totalpage,
                CurrentPage = curpage,
                PageSize = 3

            };
            ViewBag.username = Session["username"];
            return View(listTeacherPage);

        }


        public ActionResult IndexList(int currentpage)
        {
            int pagesize = 3;

            int totalpage = 1;

            int totalcount = _dataContext.Teachers.Count();

            if (totalcount % pagesize == 0)
            {
                totalpage = totalcount / pagesize;
            }
            else
            {
                totalpage = System.Convert.ToInt32((totalcount / pagesize) + 1);
            }


            var list = _dataContext.Teachers.OrderBy(p => p.ID).Skip((currentpage - 1) * pagesize).Take(pagesize);
            ListTeacherPage listTeacherPage = new ListTeacherPage()
            {
                teachers = list,
                TotalCount = totalcount,
                TotalPage = totalpage,
                CurrentPage = currentpage,
                PageSize = 3

            };
            ViewBag.username = Session["username"];
            return View("Index", listTeacherPage);
        }


        [HttpPost]

        public JsonResult GetTeacherList()
        {

            List<TeacherViewModel> teacherViewList = new List<TeacherViewModel>();
            List<Teacher> teacherList = _dataContext.Teachers.ToList();
            if (teacherList != null && teacherList.Count > 0)
            {
                teacherViewList = AutoMapper.Mapper.Map<List<Teacher>, List<TeacherViewModel>>(teacherList);
            }
            return Json(new { list = teacherViewList });
        }

        public ActionResult Create()
        {
            var viewModel = new TeacherViewModel() { ID = 0 };
            ViewBag.TeacherDepartmentList = _departments.GetDepartments();
            return View();
        }

        [HttpPost]

        public ActionResult Save(Teacher teacher)
        {
            try
            {
                _dataContext.Teachers.Add(teacher);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            catch (Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }

        //[HttpPost] 只是actionlink而没有post提交时候不能用HTTP post标签
        public ActionResult Edit(long? id)
        {
            Teacher teacher = _dataContext.Teachers.Find(id);
            if (teacher == null)
            {
                return null;
            }
            ViewBag.TeacherDepartmentList = _departments.GetDepartments();
            var teacherView = AutoMapper.Mapper.Map<Teacher, TeacherViewModel>(teacher);
            return View(teacherView);


        }

        [HttpPost]

        public ActionResult EditTeacher(TeacherViewModel teacherViewModel)
        {
            try
            {
                var model = _dataContext.Teachers.Find(teacherViewModel.ID);
                model.TeacherName = teacherViewModel.TeacherName;
                model.TeacherDepartment = teacherViewModel.TeacherDepartment;
                model.TeacherProfile = teacherViewModel.TeacherProfile;
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            catch (Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }
        }


        [HttpPost]
        public JsonResult Delete(long? id)
        {
            Teacher teacher = _dataContext.Teachers.Find(id);
            if (teacher == null)
            {
                return Json(new { code = 1, msg = "当前教师不存在" });

            }

            _dataContext.Teachers.Remove(teacher);
            _dataContext.SaveChanges();

            return Json(new { code = 0 });
        }

        [HttpPost]
        public JsonResult PageList(int pageIndex, int pageSize)
        {
            PageResult<TeacherViewModel> result = new PageResult<TeacherViewModel>() { data = new List<TeacherViewModel>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;
            var _teacher = _dataContext.Teachers;
            var list= _teacher.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            var teacherView = AutoMapper.Mapper.Map<List<TeacherViewModel>>(list);
            result.data = teacherView;
            result.Count = _teacher.Count();

            return Json(result);
        }
    }
}