using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Chapter:BaseEntity
    {
        public string ChapterName { get; set;}

        public string ChapterDetail { get; set; }

        //一对一关系
        [ForeignKey(nameof(Video))]
        public long VideoId { get; set; }
        public Video Video { get; set; }

        [ForeignKey(nameof(Course))]
        public long CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime UpdateTime { get; set; }

    }
}
