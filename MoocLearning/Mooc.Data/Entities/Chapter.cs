using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Chapter : BaseEntity
    {
        [Required(ErrorMessage = "章节名称必填")]
        [StringLength(100, ErrorMessage = "章节名称长度不能超过100个字符")]
        [Display(Name = "章节名称")]
        public string Name { get; set; }

        [Display(Name = "章节描述")]
        public string Details { get; set; }

        public string VideoGuid { get; set; }

        //一对一关系
        // [ForeignKey(nameof(Video))]
        //  public long VideoId { get; set; }
        //  public Video Video { get; set; }

        // [ForeignKey(nameof(Course))]
        [Display(Name = "课程名称")]
        // [Required(ErrorMessage = "课程名称必填")]
        public long CourseId { get; set; }
        // public Course Course { get; set; }

        public DateTime? UpdateTime { get; set; }

    }
}
