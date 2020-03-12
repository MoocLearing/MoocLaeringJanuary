using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class CourseAddView
    {
        public long ID { get; set; }

        [Display(Name = "课程名称")]
        public string CourseName { get; set; }

        [Display(Name = "课程详情")]
        public string CourseDetail { get; set; }

        public string CoverPic { get; set; }
    }
}
