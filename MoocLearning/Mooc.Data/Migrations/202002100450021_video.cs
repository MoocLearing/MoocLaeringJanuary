namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class video : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "ChapterDetail", c => c.String());
            AddColumn("dbo.Videos", "VideoLocation", c => c.String());
            DropColumn("dbo.Chapters", "VideoDetail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chapters", "VideoDetail", c => c.String());
            DropColumn("dbo.Videos", "VideoLocation");
            DropColumn("dbo.Chapters", "ChapterDetail");
        }
    }
}
