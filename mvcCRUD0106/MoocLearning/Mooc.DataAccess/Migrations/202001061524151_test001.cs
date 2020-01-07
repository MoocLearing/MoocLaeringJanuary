namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test001 : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.Subjects");
        }
    }
}
