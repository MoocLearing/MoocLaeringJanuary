using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;
using Mooc.Data.Enums;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class TeacherViewModel:Teacher
    {
        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");

        public string TeacherDepartmentToString => Enum.GetName(typeof(TeacherDepartmentEnum), TeacherDepartment);
    }
}
