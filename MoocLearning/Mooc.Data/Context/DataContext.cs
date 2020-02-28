using Mooc.Data.Entities;
using System.Configuration;
using System.Data.Entity;

namespace Mooc.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext() : base(GetConnectionString())
        {
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // ConfigurationManager.AppSettings[key]
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categorys { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        // public DbSet<Video> Videos { get; set; }

        //public DbSet<SubjectCategory> SubjectCategories { get; set; }

        // public DbSet<CategoryType> CategoryTypes { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
