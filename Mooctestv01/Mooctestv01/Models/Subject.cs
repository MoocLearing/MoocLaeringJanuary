using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooctestv01
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "SubjectName")]
        public string SubName { get; set; }

        [MaxLength(20)]
        [Display(Name = "Classify")]
        public string Classify { get; set; }

        [MaxLength(255)]
        [Display(Name = "SubjectDetail")]
        public string SubDetail { get; set; }

        [Display(Name = "CreateTime")]
        public DateTime AddTime { get; set; }
    }
}
