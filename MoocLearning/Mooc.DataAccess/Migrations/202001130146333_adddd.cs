namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AddTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AddTime", c => c.DateTime(nullable: false));
        }
    }
}
