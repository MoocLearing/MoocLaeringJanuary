using Mooc.Data.Entities;
using Mooc.Data.Service;
using System.Collections.Generic;

namespace Mooc.Data.Service
{
    public interface IUserService:IDependency
    {
        List<User> GetList();
        int Regist(User user);
        //List<Country> GetCountries();

    }

}
