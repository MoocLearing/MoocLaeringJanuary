using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Study:BaseEntity
    {
        [Required(ErrorMessage = "用户ID必填")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "课程ID必填")]
        public long CourseId { get; set; }

        [Required(ErrorMessage = "课程安排ID必填")]
        public long ScheduleId { get; set; }

        [Required(ErrorMessage = "章节ID必填")]
        public long ChapterId { get; set; }
    }
}
