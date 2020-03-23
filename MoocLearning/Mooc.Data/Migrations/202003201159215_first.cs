namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Studies",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CourseId = c.Long(nullable: false),
                        ScheduleId = c.Long(nullable: false),
                        ChapterId = c.Long(nullable: false),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Studies");
        }
    }
}
