namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _222 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Teachers", "TeacherDepartmentEnum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "TeacherDepartmentEnum", c => c.Int());
        }
    }
}
