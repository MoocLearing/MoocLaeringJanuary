namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TeacherName = c.String(nullable: false, maxLength: 100),
                        TeacherDepartment = c.Int(nullable: false),
                        TeacherProfile = c.String(maxLength: 500),
                        AddTime = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Teachers");
        }
    }
}
