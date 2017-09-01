namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Creator_Id = c.String(maxLength: 128),
                        Description_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id)
                .ForeignKey("dbo.Descriptions", t => t.Description_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.Description_Id)
                .Index(t => t.Post_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "Description_Id", "dbo.Descriptions");
            DropForeignKey("dbo.Comments", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "Description_Id" });
            DropIndex("dbo.Comments", new[] { "Creator_Id" });
            DropTable("dbo.Comments");
        }
    }
}
