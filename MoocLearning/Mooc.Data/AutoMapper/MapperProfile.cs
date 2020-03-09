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

            CreateMap<Teacher, TeacherViewModel>();
            CreateMap<TeacherViewModel, Teacher>();

            CreateMap<Category, CategoryPageView>();
            CreateMap<CategoryPageView, Category>();

            CreateMap<Course, CourseView>();
            CreateMap<CourseView, Course>();

            CreateMap<Chapter, ChapterView>();
            CreateMap<ChapterView, Chapter>();

            CreateMap<Course, CourseAddView>();
            CreateMap<CourseAddView,Course>();
        }
    }
}
