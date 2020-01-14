namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init001 : DbMigration
    {
        public override void Up()
        {
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
        }
    }
}
