namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                        ParentId = c.Long(nullable: false),
                        Type = c.Int(nullable: false),
                        Remark = c.String(),
                        AddTime = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        PassWord = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        StudentNo = c.Int(nullable: false),
                        TeacherId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        Gender = c.Int(nullable: false),
                        UserState = c.Int(nullable: false),
                        RoleType = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        ConfirmPassword = c.String(),
                        TeacherIds = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Teachers");
            DropTable("dbo.Categories");
        }
    }
}
