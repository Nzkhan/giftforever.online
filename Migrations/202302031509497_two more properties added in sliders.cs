namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twomorepropertiesaddedinsliders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sliders", "Photo5", c => c.String());
            AddColumn("dbo.Sliders", "Photo6", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sliders", "Photo6");
            DropColumn("dbo.Sliders", "Photo5");
        }
    }
}
