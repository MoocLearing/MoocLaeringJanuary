namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Countries", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Users", "CountryId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Users", "CountryId");
            AddForeignKey("dbo.Users", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
            DropColumn("dbo.Countries", "CountryName");
            DropColumn("dbo.Users", "CountryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CountryName", c => c.String(nullable: false));
            AddColumn("dbo.Countries", "CountryName", c => c.String(nullable: false));
            DropForeignKey("dbo.Users", "CountryId", "dbo.Countries");
            DropIndex("dbo.Users", new[] { "CountryId" });
            DropColumn("dbo.Users", "CountryId");
            DropColumn("dbo.Countries", "Name");
        }
    }
}
