namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AddTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Users", "PassWord", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Users", "StudentNum", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "TeacherId", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserState", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "RoleType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RoleType");
            DropColumn("dbo.Users", "UserState");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "TeacherId");
            DropColumn("dbo.Users", "StudentNum");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "PassWord");
            DropColumn("dbo.Users", "UserName");
            DropColumn("dbo.Users", "AddTime");
        }
    }
}
