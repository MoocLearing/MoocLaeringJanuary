using Mooc.Data.Entities;
using Mooc.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mooc.Data.ViewModels
{
    [NotMapped]
    public class UserViewModel: User
    {
        //[Display(Name = "确认密码")]
        //[Required(ErrorMessage = "确认密码必填")]
        //[Compare("PassWord", ErrorMessage = "确认密码和密码不匹配")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "确认密码必填")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Display(Name = "教师")]
        public string TeacherName { get; set; }

        [Display(Name = "性别")]
        public string GenderName => Enum.GetName(typeof(GenderEnum), Gender);

        [Display(Name = "角色")]
        public string RoleName => Enum.GetName(typeof(RoleTypeEnum), RoleType);
        [Display(Name = "状态")]
        public string StatusName => Enum.GetName(typeof(StatusEnum), UserState);
        [Display(Name = "创建时间")]
        public string DisplayDate =>Convert.ToDateTime( AddTime).ToString("yyyy-MM-dd");

        [Display(Name = "用户图片")]
        [Required(ErrorMessage = "图片不能为空")]
        public HttpPostedFileBase Img { get; set; }


    } 
}
