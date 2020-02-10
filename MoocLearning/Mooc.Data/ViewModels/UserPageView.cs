using Mooc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class UserPageView:User
    {
        public string CreateDate => Convert.ToDateTime(AddTime).ToString("yyyy-MM-dd");
    }
}
