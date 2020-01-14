using Mooc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.DataAccess.Dto
{
    public class UserView:User
    {

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "确认密码必填")]
        [Compare("PassWord",ErrorMessage ="确认密码和密码不匹配")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "请选择国家")]
        [Display(Name = "国家")]
        public string CountryIds { get; set; }
    }
}
