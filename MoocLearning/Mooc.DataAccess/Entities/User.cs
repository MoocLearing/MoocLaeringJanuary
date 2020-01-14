using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Mooc.Data.Entities
{
    public class User : BaseEntity
    {
       
        [Required(ErrorMessage = "用户名必填")]
        [StringLength(100, ErrorMessage = "用户名长度不能超过100个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码必填")]
        [Display(Name = "密码")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Required(ErrorMessage = "邮箱必填")]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "学号必填")]
        [Display(Name = "学号")]
        public int StudentNo { get; set; }


        [Display(Name = "教师")]
        public long TeacherId { get; set; }

        [Required(ErrorMessage = "请选择性别")]
        [Display(Name = "性别")]
        public int Gender { get; set; }


        [Display(Name = "用户状态")]
        [DefaultValue(0)]
        public int UserState { get; set; }

        [DefaultValue(3)]
        [Display(Name = "角色")]
        public int RoleType { get; set; }


    }
}
