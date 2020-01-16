using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class UserViewModel: User
    {
        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "确认密码必填")]
        [Compare("PassWord", ErrorMessage = "确认密码和密码不匹配")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "请选择教师")]
        [Display(Name = "教师")]
        public string TeacherIds { get; set; }

    } 
}
