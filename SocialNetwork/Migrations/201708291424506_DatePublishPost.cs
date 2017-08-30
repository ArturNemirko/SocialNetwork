namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatePublishPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "DatePublish", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "DatePublish");
        }
    }
}
