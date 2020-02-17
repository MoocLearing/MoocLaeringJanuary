namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimgupload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ImgGuid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ImgGuid");
        }
    }
}
