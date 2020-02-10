namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Discriminator");
        }
    }
}
