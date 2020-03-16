using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class CourseApply:BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long ScheduleId { get; set; }

    }
}
