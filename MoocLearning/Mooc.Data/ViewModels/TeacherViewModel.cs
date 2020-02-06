using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Data.Entities;
using Mooc.Data.Enums;

namespace Mooc.Data.ViewModels
{
    public class TeacherViewModel:Teacher
    {
        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");
    }
}
