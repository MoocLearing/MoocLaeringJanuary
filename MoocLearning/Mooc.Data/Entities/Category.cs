using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Entities
{
    public class Category : BaseEntity
    {
        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, ErrorMessage = "{0}不能超过20个字符")]      
        public string CategoryName { get; set; }

        [Display(Name = "一级分类")]
        public long ParentId { get; set; }

        [Range(0, 10)]
        [Display(Name = "类别等级")]
        public int Type { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
