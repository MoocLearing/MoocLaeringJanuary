using AutoMapper;
using Mooc.DataAccess.Entities;
using Mooc.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mooc.DataAccess.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<Subject, SubjectViewModel>();
            CreateMap<SubjectViewModel, Subject>();
        }

        
    }
}
