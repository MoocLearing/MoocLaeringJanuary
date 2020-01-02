using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Mooctestv01.Models
{
    public class SubjectContext:DbContext
    {
        //public SubjectContext() :base(GetCon)

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public DbSet<Subject> Subjects { get; set; }

        public System.Data.Entity.DbSet<Mooctestv01.ViewModels.SubjectModel> SubjectModels { get; set; }
    }
}