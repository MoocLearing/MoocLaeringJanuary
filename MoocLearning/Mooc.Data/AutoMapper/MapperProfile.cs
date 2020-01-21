using AutoMapper;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;

namespace Mooc.Data.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();

            CreateMap<User, AdminUserViewModel>();
            CreateMap<AdminUserViewModel, User>();

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

        }
    }
}
