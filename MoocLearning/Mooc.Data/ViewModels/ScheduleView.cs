using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class ScheduleView:Schedule
    {
        public string CourseName { get; set; }

        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");

        public string StartFormat => Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd");
        public string EndFormat => Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd");
    }
}
