using AutoMapper;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Mooc.Data.Service
{
    public class UserService : IUserService
    {

        public List<User> GetList()
        {
            using (DataContext db = new DataContext())
            {
                return db.Users.ToList();
            }
        }

        public int Regist(User user)
        {
            using (DataContext db = new DataContext())
            {
              
                db.Users.Add(user);
                return db.SaveChanges();
            }
        }
    
    }
}
