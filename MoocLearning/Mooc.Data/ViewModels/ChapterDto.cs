using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;

namespace Mooc.Data.ViewModels
{
    public class ChapterDto
    {
        public Chapter ChapterOne { get; set; }
        public string CoursePic { get; set; }
        public long ScheduleId { get; set; }
    }
}
