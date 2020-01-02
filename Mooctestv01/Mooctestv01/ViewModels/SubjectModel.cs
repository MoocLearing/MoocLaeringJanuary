using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mooctestv01.ViewModels
{
    public class SubjectModel
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
    }
}