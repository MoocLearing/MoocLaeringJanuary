namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Phone", c => c.Int(nullable: false));
        }
    }
}
