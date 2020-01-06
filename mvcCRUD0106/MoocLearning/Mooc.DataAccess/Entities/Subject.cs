using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.DataAccess.Entities
{
    public class Subject
    {
        [Key]

        public int SubId { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "SubjectName")]
        public string SubName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "SubjectDetail")]
        public string SubDetail { get; set; }

        [Display(Name = "CreatedTime")]
        public DateTime SubCreateTime { get; set; }
        //refer to price table 
        

    }
}
