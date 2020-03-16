using Mooc.Data.Context;
using Mooc.Data.Enums;
using Mooc.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Models
{
    //public class TeacherOption
    //{
    //    public IEnumerable<TeacherSelectOption> GetTeachers()
    //    {
    //        using (DataContext db = new DataContext())
    //        {
    //            return db.Teachers.OrderByDescending(p => p.AddTime).Select(p => new TeacherSelectOption { ID = p.ID, TeacherName = p.TeacherName });
    //        }

    //        //    List<TeacherSelectOption> list = new List<TeacherSelectOption>() {
    //        //    new TeacherSelectOption{ ID=1,TeacherName="Lily" },
    //        //    new TeacherSelectOption{ID=2,TeacherName="Tom"}
    //        //};
    //        // return list;
    //    }

    //    public IEnumerable<SelectListItem> GetTeacherSelectOptions()
    //    {
    //        var itemlist = GetTeachers().Select(r => new SelectListItem
    //        {
    //            Text = r.TeacherName,
    //            Value = r.ID.ToString(),
    //            // Selected = r.TeacherName == "中国" ? true : false
    //        });
    //        return itemlist;
    //    }


    //}

    public class SelectOptions
    {
        
        public static IEnumerable<SelectListItem> GetRoleSelectOptions()
        {
            IEnumerable<SelectListItem> enumsList = ((RoleTypeEnum[])Enum.GetValues(typeof(RoleTypeEnum))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();

            return enumsList;
        }

        public static IEnumerable<SelectListItem> GetGenderSelectOptions()
        {
            IEnumerable<SelectListItem> enumsList = ((GenderEnum[])Enum.GetValues(typeof(GenderEnum))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();

            return enumsList;
        }

        public List<SelectListItem> GetCourseSelectOptions()
        {
            using (DataContext db = new DataContext())
            {
                var list = db.Courses.OrderByDescending(p => p.AddTime).Select(r => new SelectListItem
                {
                    Text = r.CourseName,
                    Value = r.ID.ToString(),
                }).ToList();
                return list;
            }
        }

    }

    public class SelectDepartments
    {
        public IEnumerable<SelectListItem> GetDepartments()
        {
            IEnumerable<SelectListItem> enumsList = ((TeacherDepartmentEnum[])Enum.GetValues(typeof(TeacherDepartmentEnum))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();
            return enumsList;
        }
    }


    public class CategoryOption
    {
        public IEnumerable<SelectListItem> GetCategorySelectOptions()
        {
            using (DataContext db = new DataContext())
            {
                var list = db.Categorys.OrderByDescending(p => p.AddTime).Select(r => new SelectListItem
                {
                    Text = r.CategoryName,
                    Value = r.ID.ToString(),
                }).ToList();
                return list;
            }
        }
    }


    public class TeacherOption
    {
        public IEnumerable<SelectListItem> GetTeacherSelectOptions()
        {
            using (DataContext db = new DataContext())
            {
                var list = db.Teachers.OrderByDescending(p => p.AddTime).Select(r => new SelectListItem
                {
                    Text = r.TeacherName,
                    Value = r.ID.ToString(),
                }).ToList();
                return list;
            }
        }
    }


}