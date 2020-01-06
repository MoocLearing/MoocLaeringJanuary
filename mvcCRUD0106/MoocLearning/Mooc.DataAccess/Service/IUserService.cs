using Mooc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.DataAccess.Service
{
    public interface IUserService:IDependency
    {
        List<User> GetList();
    }
}
