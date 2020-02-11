using Mooc.Data.Entities;
using Mooc.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class CourseView:Course
    {
        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");
        public string TeacherName { get; set; }
        public string CategoryName { get; set; }

        public string CourseStatus => Enum.GetName(typeof(CourseStatusEnum), Status);
    }
}
