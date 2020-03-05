using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Enums
{
    
    public enum RoleTypeEnum
    {
        管理员 = 1,
        老师 = 2,
        学生 = 3
    }

    public enum StatusEnum
    {
        正常 = 0,
        锁定 = 1
    }

    public enum CategoryTypeEnum
    {
        FirstCategory = 1,
        SecondCategory = 2
    }

    public enum CategoryTypeNameEnum
    {
        一级分类 = 1,
        二级分类 = 2
    }

    public enum TeacherDepartmentEnum
    {   
        IT部门 = 1,

        会计部门=2,

        市场部门 = 3

    }

    public enum CourseStatusEnum
    {
        未上架=0,
        已上架=1,
        已下架=2
    }

    public enum GenderEnum
    {
        不男不女 = 0,
        男 = 1,
        女 = 2
    }

}
