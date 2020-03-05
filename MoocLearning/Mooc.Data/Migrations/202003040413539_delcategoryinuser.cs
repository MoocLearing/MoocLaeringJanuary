namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delcategoryinuser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CategoryId", c => c.Long(nullable: false));
        }
    }
}
