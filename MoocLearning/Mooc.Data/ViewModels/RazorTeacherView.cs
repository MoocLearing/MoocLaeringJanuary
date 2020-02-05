using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class RazorTeacherView
    {
        [Required(ErrorMessage = "教师名字必填")]
        [StringLength(100)]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "教师部门必填")]
        [Range(1, 3)]
        public int TeacherDepartment { get; set; }

        [StringLength(500)]
        public string TeacherProfile { get; set; }
    }
}
