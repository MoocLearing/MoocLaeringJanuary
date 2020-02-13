using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class ChapterView:Chapter
    {
        [Display(Name = "章节视频")]
        [Required(ErrorMessage = "章节视频不能为空")]
        public HttpPostedFileBase Video { get; set; }
        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");
        public string UpdateDate => Convert.ToDateTime(UpdateTime).ToString("yyyy-MM-dd");

        //[Display(Name="课程名称")]
        //[Required(ErrorMessage ="课程名称不能为空")]
        public string CourseName { get; set; }
    }
}
