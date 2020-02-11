using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Course:BaseEntity
    {
        [Display(Name = "用户")]
        public string CourseName { get; set; }

        public string CourseDetail { get; set; }

        // [ForeignKey(nameof(Teacher))]

        [Display(Name = "课程讲师")]
        public long TeacherId { get; set; }
      //  public Teacher Teacher { get; set; }

       // [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }
       // public Category Category { get; set; }

        public int Status { get; set; }
    }
}
