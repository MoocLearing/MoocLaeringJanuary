namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "CountryId", "dbo.Countries");
            DropIndex("dbo.Users", new[] { "CountryId" });
            AddColumn("dbo.Users", "CountryName", c => c.String(nullable: false));
            DropColumn("dbo.Users", "CountryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CountryId", c => c.Byte(nullable: false));
            DropColumn("dbo.Users", "CountryName");
            CreateIndex("dbo.Users", "CountryId");
            AddForeignKey("dbo.Users", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
        }
    }
}
