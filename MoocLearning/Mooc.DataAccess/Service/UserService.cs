using Mooc.DataAccess.Context;
using Mooc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mooc.Models.ViewModels;

namespace Mooc.DataAccess.Service
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
