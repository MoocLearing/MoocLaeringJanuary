using Mooc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Models.ViewModels;

namespace Mooc.DataAccess.Service
{
    public interface IUserService:IDependency
    {
        List<User> GetList();
        int Regist(User user);
        List<Country> GetCountries();

    }

}
