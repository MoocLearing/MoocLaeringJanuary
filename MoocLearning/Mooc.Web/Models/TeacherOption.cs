using Mooc.Data.Enums;
using Mooc.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Models
{
    public class TeacherOption
    {
        public List<TeacherSelectOption> GetTeachers()
        {
            List<TeacherSelectOption> list = new List<TeacherSelectOption>() {
                new TeacherSelectOption{ ID=1,TeacherName="Lily" },
                new TeacherSelectOption{ID=2,TeacherName="Tom"}
            };
            return list;
        }

        public  IEnumerable<SelectListItem> GetTeacherSelectOptions()
        {
            var itemlist = GetTeachers().AsEnumerable().Select(r => new SelectListItem
            {
                Text = r.TeacherName,
                Value = r.ID.ToString(),
                // Selected = r.TeacherName == "中国" ? true : false
            });
            return itemlist;
        }


    }

    public class SelectOptions
    {
        public static IEnumerable<SelectListItem>  GetRoleSelectOptions()
        {
            IEnumerable<SelectListItem> enumsList = ((RoleTypeEnum[])Enum.GetValues(typeof(RoleTypeEnum))).Select(c => new SelectListItem() { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();
           
            return enumsList;
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


}