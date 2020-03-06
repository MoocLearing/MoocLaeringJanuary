namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test101 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PassWord", c => c.String(nullable: false));
            DropColumn("dbo.Users", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CategoryId", c => c.Long(nullable: false));
            AlterColumn("dbo.Users", "PassWord", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
