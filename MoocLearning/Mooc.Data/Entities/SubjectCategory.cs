using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;

namespace Mooc.Data.Entities
{
    public class SubjectCategory:BaseEntity
    {

        public int ParentId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public int IsDel { get; set; }

        public  int Type { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

    }
}
