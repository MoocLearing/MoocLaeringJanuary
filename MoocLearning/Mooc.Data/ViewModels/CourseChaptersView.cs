using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class CourseChaptersView
    {
        public List<CourseAddView> CourseAddViews { get; set; }

        public List<Chapter> Chapters { get; set; }

    }
}
