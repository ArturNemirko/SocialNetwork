namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeViewModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Creator_Id", "dbo.AspNetUsers");
            AddColumn("dbo.Posts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Post_Id", c => c.Guid());
            CreateIndex("dbo.Posts", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Post_Id");
            AddForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts");
            DropIndex("dbo.AspNetUsers", new[] { "Post_Id" });
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Post_Id");
            DropColumn("dbo.Posts", "ApplicationUser_Id");
            AddForeignKey("dbo.Posts", "Creator_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
