namespace Mooc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcourseforeignkey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ChapterName = c.String(),
                        VideoId = c.Long(nullable: false),
                        CourseId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        VideoDetail = c.String(),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CourseName = c.String(),
                        CrouseDetail = c.String(),
                        TeacherId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        VideoName = c.String(),
                        AddTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapters", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Chapters", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropIndex("dbo.Courses", new[] { "TeacherId" });
            DropIndex("dbo.Chapters", new[] { "CourseId" });
            DropIndex("dbo.Chapters", new[] { "VideoId" });
            DropTable("dbo.Videos");
            DropTable("dbo.Courses");
            DropTable("dbo.Chapters");
        }
    }
}
