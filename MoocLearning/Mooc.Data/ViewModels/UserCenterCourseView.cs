﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class UserCenterCourseView
    {
        public long ScheduleId { get; set; }
        public long CourseId { get; set; }

        public string CourseName { get; set; }
        public string CourseCover { get; set; }
        public string CourseDetail { get; set; }
        public string CategoryName { get; set; }
        public string TeacherName { get; set; }

        public string StudyStatus { get; set; }



    }
}
