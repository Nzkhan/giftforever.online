namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productnewpropertiesadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Photo6", c => c.String());
            AddColumn("dbo.Products", "Photo7", c => c.String());
            AddColumn("dbo.Products", "Photo8", c => c.String());
            AddColumn("dbo.Products", "Photo9", c => c.String());
            AddColumn("dbo.Products", "Photo10", c => c.String());
            AddColumn("dbo.Products", "ProductDetails", c => c.String());
            AddColumn("dbo.Products", "BeneficialFor", c => c.String());
            AddColumn("dbo.Products", "ProductInformation", c => c.String());
            AddColumn("dbo.Products", "Usage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Usage");
            DropColumn("dbo.Products", "ProductInformation");
            DropColumn("dbo.Products", "BeneficialFor");
            DropColumn("dbo.Products", "ProductDetails");
            DropColumn("dbo.Products", "Photo10");
            DropColumn("dbo.Products", "Photo9");
            DropColumn("dbo.Products", "Photo8");
            DropColumn("dbo.Products", "Photo7");
            DropColumn("dbo.Products", "Photo6");
        }
    }
}
