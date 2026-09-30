namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoTableDatecreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Videos", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "UpdatedDate");
            DropColumn("dbo.Videos", "CreatedDate");
        }
    }
}
