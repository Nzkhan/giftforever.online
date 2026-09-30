namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "prodid", c => c.Int(nullable: false));
            AddColumn("dbo.Reviews", "Heading", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Heading");
            DropColumn("dbo.Reviews", "prodid");
        }
    }
}
