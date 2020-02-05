using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{

    
    public class RazorTeacherController : Controller
    {

        private DataContext _dataContext;
        private SelectDepartments _departments = new SelectDepartments();

        public RazorTeacherController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: Admin/RazorTeacher
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
            return View("Index", listTeacherPage);
        }

        public ActionResult Create()
        {
            ViewBag.TeacherDepartmentList = _departments.GetDepartments();
            return View();
        }

        [HttpPost]
     
        public ActionResult Create(RazorTeacherView model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher
                {
                    TeacherName = model.TeacherName,
                    TeacherDepartment = model.TeacherDepartment,
                    TeacherProfile = model.TeacherProfile
                };
                
                _dataContext.Teachers.Add(teacher);
                _dataContext.SaveChanges();
                return Redirect("~/Admin/RazorTeacher/Index");

            }
            ModelState.AddModelError("", "请正确填写");
            return View();
        }

       
        public ActionResult Edit(int id)
        {
            Teacher teacher = _dataContext.Teachers.FirstOrDefault(m => m.ID == id);
            if (teacher != null)
            {
                ViewBag.TeacherDepartmentList = _departments.GetDepartments();
                TeacherViewModel model = AutoMapper.Mapper.Map<TeacherViewModel>(teacher);
                return View(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
      
        public ActionResult Edit(TeacherViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    _dataContext.Teachers.Add(model);
                }
                else
                {
                    var updateteacher = _dataContext.Teachers.Find(model.ID);

                    updateteacher.TeacherName = model.TeacherName;
                    updateteacher.TeacherDepartment = model.TeacherDepartment;
                    updateteacher.TeacherProfile = model.TeacherProfile;
                }

                _dataContext.SaveChanges();
                return Redirect("~/Admin/RazorTeacher/Index");
                
            }
            else
            {
                ModelState.AddModelError("", "请正确填写");
            }
            return View(model);
        }

        
        public ActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.Teachers.FirstOrDefault(m => m.ID == id);
            _dataContext.Teachers.Remove(teacher);
            _dataContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}