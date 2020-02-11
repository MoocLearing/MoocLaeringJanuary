using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Enums
{
    public enum RoleTypeEnum
    {
        Administrator = 1,
        Teacher = 2,
        Student = 3
    }

    public enum StatusEnum
    {
        Normal = 0,
        Locked = 1
    }

    public enum CategoryTypeEnum
    {
        FirstCategory=1,
        SecondCategory=2
    }

    public enum CategoryTypeNameEnum
    {
        一级分类 = 1,
        二级分类 = 2
    }

    public enum TeacherDepartmentEnum
    {   
        ItDepartment = 1,

        AccountDepartment=2,

        MarketDepartment = 3

    }

    public enum CourseStatusEnum
    {
        未上架=0,
        已上架=1,
        已下架=2
    }

}
