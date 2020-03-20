using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class CourseDetails : Course
    {
        public CourseDetails()
        {
            List<Chapter> chapters = new List<Chapter>();
        }
        public List<Chapter> chapters { get; set; }
        public string TeacherName { get; set; }



    }
}
