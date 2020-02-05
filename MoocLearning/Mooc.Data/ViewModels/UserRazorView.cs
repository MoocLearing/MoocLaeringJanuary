using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class UserRazorView
    {
        [Required(ErrorMessage = "用户名必填")]
        [StringLength(100, ErrorMessage = "用户名长度不能超过100个字符")]
        [Display(Name = "用户")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码必填")]
        [Display(Name = "密码")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name ="记住我的登录状态")]
        public bool RememberMe { get; set; }


    }
}
