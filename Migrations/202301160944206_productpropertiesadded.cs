namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productpropertiesadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.String());
            AddColumn("dbo.Products", "Specification", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Specification");
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
