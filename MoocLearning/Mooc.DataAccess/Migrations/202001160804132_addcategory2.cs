namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcategory2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryTypes",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubjectCategories",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        IsDel = c.Int(nullable: false),
                        CategoryTypeId = c.Int(nullable: false),
                        Remark = c.String(maxLength: 500),
                        AddTime = c.DateTime(),
                        CategoryType_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryType_ID)
                .Index(t => t.CategoryType_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubjectCategories", "CategoryType_ID", "dbo.CategoryTypes");
            DropIndex("dbo.SubjectCategories", new[] { "CategoryType_ID" });
            DropTable("dbo.SubjectCategories");
            DropTable("dbo.CategoryTypes");
        }
    }
}
