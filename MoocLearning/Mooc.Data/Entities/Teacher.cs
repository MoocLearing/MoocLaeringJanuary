using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Teacher:BaseEntity
    {
        [Required(ErrorMessage = "教师名字必填")]
        [StringLength(100)]
        [Display(Name ="教师姓名")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "教师部门必填")]
        [Range(1,3)]
        [Display(Name ="教师部门")]
        public int TeacherDepartment { get; set; }

        [StringLength(500)]
        [Display(Name ="备注")]
        public string TeacherProfile { get; set; }
    }
}
