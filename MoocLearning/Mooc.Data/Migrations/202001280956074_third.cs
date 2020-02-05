namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "TeacherDepartmentEnum", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "TeacherDepartmentEnum");
        }
    }
}
