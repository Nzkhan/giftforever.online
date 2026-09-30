namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sliderimproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sliders", "Photo", c => c.String());
            AddColumn("dbo.Sliders", "Title", c => c.String());
            AddColumn("dbo.Sliders", "SubTitle", c => c.String());
            DropColumn("dbo.Sliders", "Photo1");
            DropColumn("dbo.Sliders", "Photo2");
            DropColumn("dbo.Sliders", "Photo3");
            DropColumn("dbo.Sliders", "Photo4");
            DropColumn("dbo.Sliders", "Photo5");
            DropColumn("dbo.Sliders", "Photo6");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sliders", "Photo6", c => c.String());
            AddColumn("dbo.Sliders", "Photo5", c => c.String());
            AddColumn("dbo.Sliders", "Photo4", c => c.String());
            AddColumn("dbo.Sliders", "Photo3", c => c.String());
            AddColumn("dbo.Sliders", "Photo2", c => c.String());
            AddColumn("dbo.Sliders", "Photo1", c => c.String());
            DropColumn("dbo.Sliders", "SubTitle");
            DropColumn("dbo.Sliders", "Title");
            DropColumn("dbo.Sliders", "Photo");
        }
    }
}
