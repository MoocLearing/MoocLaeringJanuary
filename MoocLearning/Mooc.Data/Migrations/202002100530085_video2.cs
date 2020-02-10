namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class video2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "Discriminator");
        }
    }
}
