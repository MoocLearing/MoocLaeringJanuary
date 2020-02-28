using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Schedule:BaseEntity
    {
        [Display(Name = "所安排课程")]
        public long CourseId { get; set; }

        [Required(ErrorMessage = "开始时间必选")]
        [Display(Name = "课程开始时间")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "结束时间必选")]
        [Display(Name = "课程结束时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "安排备注")]
        [StringLength(100, ErrorMessage = "安排备注不得超过100字符")]
        public string Remark { get; set; }
    }
}
