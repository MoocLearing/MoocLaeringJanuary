namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test002 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Subjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubId = c.Int(nullable: false, identity: true),
                        SubName = c.String(nullable: false, maxLength: 20),
                        SubDetail = c.String(nullable: false, maxLength: 255),
                        SubCreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubId);
            
        }
    }
}
