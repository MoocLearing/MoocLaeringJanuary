using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;

namespace Mooc.DataAccess.Entities
{
    public class SubjectCategory:BaseEntity
    {

        public int ParentId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "类别名称")]
        public string CategoryName { get; set; }

        public int IsDel { get; set; }

        public CategoryType CategoryType { get; set; }

        [Required(ErrorMessage = "请选择类型")]
        [Display(Name = "类型")]
        public int CategoryTypeId { get; set; }

        [StringLength(500)]
        [Display(Name = "备注")]
        public string Remark { get; set; }

    }
}
