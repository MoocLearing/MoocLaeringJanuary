namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class course : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseDetail", c => c.String());
            DropColumn("dbo.Courses", "CrouseDetail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CrouseDetail", c => c.String());
            DropColumn("dbo.Courses", "CourseDetail");
        }
    }
}
