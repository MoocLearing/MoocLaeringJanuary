namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddd2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "CountryId", "dbo.Countries");
            DropIndex("dbo.Users", new[] { "CountryId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "CountryId");
            AddForeignKey("dbo.Users", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
        }
    }
}
